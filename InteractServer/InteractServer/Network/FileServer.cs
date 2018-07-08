using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InteractServer.Network
{
	public class FileServer
	{
		private Thread thread;
		private HttpListener listener;

		public FileServer()
		{
			thread = new Thread(this.listen);
			thread.IsBackground = true;
			thread.Start();
		}

		public void Stop()
		{
			thread.Abort();
			listener.Stop();
		}

		private void listen()
		{
			listener = new HttpListener();
			listener.Prefixes.Add("http://*:11235/");
			listener.Start();

			while (true)
			{
				try
				{
					HttpListenerContext context = listener.GetContext();
					process(context);
				} catch (Exception e)
				{
					Global.Log.AddEntry("Webserver Error: " + e.Message);
				}
			}
		}

		private void process(HttpListenerContext context)
		{
			string fileID = context.Request.Url.Query;
			string path = context.Request.Url.AbsolutePath;

			// remove / from path
			path = path.Remove(0, 1);
			string answer = string.Empty;

			if(path.Equals(Global.ProjectManager.Current.ProjectID().ToString()))
			{
				answer = Global.ProjectManager.Current.SerializeProjectContents();
			} else
			{
				answer = Global.ProjectManager.Current.SerializeResource(new Guid(path));
			}

			if(answer != string.Empty)
			{
				try
				{
					context.Response.ContentType = "application/octet-stream";
					context.Response.ContentLength64 = answer.Length;
					StreamWriter writer = new StreamWriter(context.Response.OutputStream);
					writer.Write(answer);
					writer.Flush();
					context.Response.OutputStream.Flush();
					Global.Log.AddEntry("Fileserver: Sending resource " + path + " to " + context.Request.RemoteEndPoint.Address.ToString());
				} catch (Exception e)
				{
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					Global.Log.AddEntry("Fileserver Error: " + e.Message);
				}
				
			} else
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				context.Response.OutputStream.Close();
			}
		}
	}
}
