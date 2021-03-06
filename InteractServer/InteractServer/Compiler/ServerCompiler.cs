﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace InteractServer.Compiler
{
	public class ServerCompiler
	{
		ScriptCompiler.Compiler compiler = null;
		ServerCommunicator server = null;

		public ServerCompiler()
		{
			compiler = new ScriptCompiler.Compiler(new EventHandler(OnLoad), new EventHandler(OnUnload));
		}

		public bool Compile(string[] files)
		{
			try
			{
				bool result = compiler.CreateAssembly(files);
				if (result == false)
				{
					Log.Log.Handle.AddEntry("Scripts are not loaded");
				}
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry(e.Message);
			}
			CodeEditor.ErrorList.Handle.Populate(compiler.Errors());
			return !CodeEditor.ErrorList.Handle.ContainsErrors();
		}

		public bool HasScriptInterface()
		{
			return compiler.HasScriptInterface();
		}

		public void Run()
		{
			if (server == null)
			{
				server = new ServerCommunicator();
			}
			Osc.Tree.ServerScripts.Endpoints.Clear();

			var result = compiler.Run(server);
			if (result != string.Empty)
			{
				Log.Log.Handle.AddEntry("Server Script Error: " + result);
			}
		}

		public void InvokeOsc(string endpoint, object[] args)
		{
			try
			{
				string result = compiler.InvokeOsc(endpoint, args);
				if (result != string.Empty)
				{
					Log.Log.Handle.AddEntry("Server Script Error: " + result);
				}
			}
			catch (System.Runtime.Remoting.RemotingException e)
			{
				Log.Log.Handle.AddEntry("Server Script InvokeOsc error: " + e.Message);
			}

		}

		public void OnProjectStart()
		{
			try
			{
				string result = compiler.OnProjectStart();
				if (result != string.Empty)
				{
					Log.Log.Handle.AddEntry("Server Script Error on Project Start: " + result);
				}
			} catch(Exception e)
			{
				Log.Log.Handle.AddEntry("Server Script Error on Project Start: " + e.Message);
			}
		}

		public void OnProjectStop()
		{
			try
			{
				string result = compiler.OnProjectStop();
				if (result != string.Empty)
				{
					Log.Log.Handle.AddEntry("Server Script Error on Project Stop: " + result);
				}
			}
			catch (Exception e)
			{
				Log.Log.Handle.AddEntry("Server Script Error on Project Stop: " + e.Message);
			}
		}

		public object OscValueOverride(string methodName, object[] args)
		{
			return compiler.OscValueOverride(methodName, args);
		}

		public void StopAssembly()
		{
			compiler.Stop();
			Osc.Tree.ServerScripts.Endpoints.Clear();
		}

		~ServerCompiler()
		{
			StopAssembly();
		}

		public void OnLoad(object sender, EventArgs e)
		{
			Log.Log.Handle.AddEntry("Server Scripts Loaded");
		}

		public void OnUnload(object sender, EventArgs e)
		{
			Log.Log.Handle.AddEntry("Server Scripts Unloaded");
		}
	}
}
