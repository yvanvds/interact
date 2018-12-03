using Microsoft.CSharp;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;

namespace ScriptCompiler
{
	/// <summary>
	/// When hosted in a separate AppDomain, provides a mechanism for loading 
	/// plugin assemblies and instantiating objects within them.
	/// </summary>
	//[SecurityPermission(SecurityAction.Demand, Infrastructure = true)]
	internal sealed class PluginLoader : MarshalByRefObject, IDisposable
	{
		private object[] constructorArgs;
		private Sponsor<TextWriter> mLog;

		/// <summary>
		/// Gets or sets the directory containing the assemblies.
		/// </summary>
		private string PluginDir { get; set; }
		/// <summary>
		/// Gets or sets the collection of assemblies that have been loaded.
		/// </summary>
		private List<Assembly> Assemblies { get; set; }
		/// <summary>
		/// Gets or sets the collection of constructors for various interface types.
		/// </summary>
		private Dictionary<Type, LinkedList<ConstructorInfo>> ConstructorCache { get; set; }
		/// <summary>
		/// Gets or sets the TextWriter to use for logging.
		/// </summary>
		/// 

		private List<ParseError> errors = new List<ParseError>();
		public string Errors()
		{
			var arr = new JArray();
			foreach(var error in errors)
			{
				arr.Add(error.AsJson());
			}
			return arr.ToString();
		}

		public TextWriter Log
		{
			get
			{
				return (mLog != null) ? mLog.Instance : null;
			}
			set
			{
				mLog = (value != null) ? new Sponsor<TextWriter>(value) : null;
			}
		}

		private CSharpCodeProvider provider;
		private CompilerParameters parameters;


		/// <summary>
		/// Initialises a new instance of the PluginLoader class.
		/// </summary>
		public PluginLoader()
		{
			Log = TextWriter.Null;
			ConstructorCache = new Dictionary<Type, LinkedList<ConstructorInfo>>();
			Assemblies = new List<Assembly>();

			provider = new CSharpCodeProvider();
			parameters = new CompilerParameters();
			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;

			List<string> assemblies = new List<string>();
			assemblies.Add("System.dll");
			assemblies.Add("netstandard.dll");
			assemblies.Add("ScriptInterface.dll");

			parameters.ReferencedAssemblies.AddRange(assemblies.ToArray());
		}

		/// <summary>
		/// Finalizer.
		/// </summary>
		~PluginLoader()
		{
			Dispose(false);
		}

		public bool Init(string[] filesToCompile)
		{
			Uninit();
			errors.Clear();

			CompilerResults results = provider.CompileAssemblyFromFile(parameters, filesToCompile);
			if (results.Errors.HasErrors)
			{
				
				for(int i = 0; i < results.Errors.Count; i++)
				{
					errors.Add(new ParseError(results.Errors[i]));
				}
				return false;
			}

			Assemblies.Add(results.CompiledAssembly);
			return true;
		}

		/// <summary>
		/// Loads plugin assemblies into the application domain and populates the collection of plugins.
		/// </summary>
		/// <param name="pluginDir"></param>
		/// <param name="disabledPlugins"></param>
		public void Init(string pluginDir)
		{
			Uninit();

			PluginDir = pluginDir;

			foreach (string dllFile in Directory.GetFiles(PluginDir, "*.dll"))
			{
				try
				{
					Assembly asm = Assembly.LoadFile(dllFile);
					Log.WriteLine("Loaded assembly {0}.", asm.GetName().Name);

					// TODO: restrict assemblies loaded based on digital signature, 
					// implementing a required interface, DRM, etc

					Assemblies.Add(asm);
				}
				catch (ReflectionTypeLoadException rex)
				{
					Log.WriteLine("Plugin {0} failed to load.", Path.GetFileName(dllFile));
					foreach (Exception ex in rex.LoaderExceptions)
					{
						Log.WriteLine("\t{0}: {1}", ex.GetType().Name, ex.Message);
					}
				}
				catch (BadImageFormatException)
				{
					// ignore, this simply means the DLL was not a .NET assembly
					Log.WriteLine("Plugin {0} is not a valid assembly.", Path.GetFileName(dllFile));
				}
				catch (Exception ex)
				{
					Log.WriteLine("Plugin {0} failed to load.", Path.GetFileName(dllFile));
					Log.WriteLine("\t{0}: {1}", ex.GetType().Name, ex.Message);
				}
			}
		}

		/// <summary>
		/// Clears all plugin assemblies and type info.
		/// </summary>
		public void Uninit()
		{
			Assemblies.Clear();
			ConstructorCache.Clear();
		}

		public bool InjectCommunicator(Scripts.ICommunicator communicator)
		{
			foreach (var asm in Assemblies)
			{
				Type t = asm.GetType("Scripts.Main");
				if (t != null)
				{
					Type b = t.BaseType;
					MethodInfo info = b.GetMethod("InjectCommunicator");
					info.Invoke(null, new object[] { communicator });
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Returns a sequence of instances of types that implement a 
		/// particular interface. Any instances that are MarshalByRefObject 
		/// must be sponsored to prevent disconnection.
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <returns></returns>
		public IEnumerable<TInterface> GetImplementations<TInterface>()
		{
			LinkedList<TInterface> instances = new LinkedList<TInterface>();

			foreach (ConstructorInfo constructor in GetConstructors<TInterface>())
			{
				instances.AddLast(CreateInstance<TInterface>(constructor));
			}

			return instances;
		}

		/// <summary>
		/// Returns the name of the assembly that owns the specified instance 
		/// of a particular interface. (If you try to obtain the assembly using 
		/// Object.GetType(), you will get MarshalByRefObject.)
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public AssemblyName GetOwningAssembly(object instance)
		{
			Type type = instance.GetType();
			return type.Assembly.GetName();
		}

		/// <summary>
		/// Returns the name of the type of the specified instance of a 
		/// particular interface. (If you try to obtain the type using 
		/// Object.GetType(), you will get MarshalByRefObject.)
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public string GetTypeName(object instance)
		{
			Type type = instance.GetType();
			return type.FullName;
		}

		/// <summary>
		/// Returns the first implementation of a particular interface type. 
		/// Default implementations are not favoured.
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <returns></returns>
		public TInterface GetImplementation<TInterface>()
		{
			return GetImplementations<TInterface>().FirstOrDefault();
		}

		/// <summary>
		/// Returns the constructors for implementations of a particular interface 
		/// type. Constructor info is cached after the initial crawl.
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <returns></returns>
		private IEnumerable<ConstructorInfo> GetConstructors<TInterface>()
		{
			if (ConstructorCache.ContainsKey(typeof(TInterface)))
			{
				return ConstructorCache[typeof(TInterface)];
			}
			else
			{
				LinkedList<ConstructorInfo> constructors = new LinkedList<ConstructorInfo>();

				foreach (Assembly asm in Assemblies)
				{
					foreach (Type type in asm.GetTypes())
					{
						if (type.IsClass && !type.IsAbstract)
						{
							if (type.GetInterfaces().Contains(typeof(TInterface)))
							{
								if(constructorArgs == null)
								{
									ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
									constructors.AddLast(constructor);
								} else
								{
									Type[] types = new Type[constructorArgs.Length];
									for(int i= 0; i < constructorArgs.Length; i++)
									{
										types[i] = constructorArgs[i].GetType(); 
									}
									ConstructorInfo constructor = type.GetConstructor(types);
									constructors.AddLast(constructor);
								}
								
							}
						}
					}
				}

				ConstructorCache[typeof(TInterface)] = constructors;
				return constructors;
			}
		}

		/// <summary>
		/// Returns instances of all implementations of a particular interface 
		/// type in the specified assembly.
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="assembly"></param>
		/// <returns></returns>
		private IEnumerable<TInterface> GetImplementations<TInterface>(Assembly assembly)
		{
			List<TInterface> instances = new List<TInterface>();

			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsClass && !type.IsAbstract)
				{
					if (type.GetInterfaces().Contains(typeof(TInterface)))
					{
						TInterface instance = default(TInterface);
						ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
						instance = CreateInstance<TInterface>(constructor);
						if (instance != null) instances.Add(instance);
					}
				}
			}

			return instances;
		}

		/// <summary>
		/// Invokes the specified constructor to create an instance of an 
		/// interface type.
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="constructor"></param>
		/// <returns></returns>
		private TInterface CreateInstance<TInterface>(ConstructorInfo constructor)
		{
			TInterface instance = default(TInterface);

			try
			{
				instance = (TInterface)constructor.Invoke(constructorArgs);
			}
			catch (Exception ex)
			{
				Log.WriteLine(
					"Unable to instantiate type {0} in plugin {1}",
					constructor.ReflectedType.FullName,
					Path.GetFileName(constructor.ReflectedType.Assembly.Location)
				);
				Log.WriteLine("\t{0}: {1}", ex.GetType().Name, ex.Message);
			}

			return instance;
		}

		/// <summary>
		/// Gets the first implementation of a particular interface type in 
		/// the specified assembly. Default implementations are not favoured.
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="assembly"></param>
		/// <returns></returns>
		private TInterface GetImplementation<TInterface>(Assembly assembly)
		{
			return GetImplementations<TInterface>(assembly).FirstOrDefault();
		}

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				Uninit();
				if (mLog != null) mLog.Dispose();
			}
		}

		#endregion
	}
}
