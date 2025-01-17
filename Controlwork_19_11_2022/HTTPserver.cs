﻿using System.Net;
using System.Text;

namespace Controlwork_19_11_2022;

public enum ServerStatus{Start,Stop};
public class HttpServer : IDisposable
{
    public ServerStatus Status = ServerStatus.Stop;
    private ServerSettings _serverSetting=new ServerSettings();
    private HttpListener _listener;
  
    public HttpServer() 
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://localhost:"+ _serverSetting.Port + "/");
    }
    public void Start()
    {
        _listener.Start();
        Status = ServerStatus.Start;
        Receive();
    }

    public void Stop()
    {
        _listener.Stop();
        Status = ServerStatus.Stop;
    }

    private void Receive()
    {
        _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
    }

    private void ListenerCallback(IAsyncResult result)
    {
        if(_listener.IsListening)
        {
            var _httpContext = _listener.EndGetContext(result);
            HttpListenerRequest request = _httpContext.Request;
            HttpListenerResponse response = _httpContext.Response;
            byte[] buffer = new byte[] { };
            string st;
            if(Directory.Exists(_serverSetting.Path))
            {
                buffer = FileManager.getFile(_httpContext.Request.RawUrl.Replace("%20", " "), _serverSetting);
                string stringbyte = System.Text.Encoding.UTF8.GetString(buffer);
                stringbyte = StringCut.Cut(stringbyte);
                buffer = Encoding.UTF8.GetBytes(stringbyte);

                if (buffer==null)
                {
                    response.Headers.Set("Content-Type", "text/plain");
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    string err = "404 - not found";
                    buffer = Encoding.UTF8.GetBytes(err);
                }
            }
            else
            {
                string err = $"Directory "+ _serverSetting.Path + " not found";
                buffer = Encoding.UTF8.GetBytes(err);
            }
            Stream output = response.OutputStream;
            output.Write(buffer,0,buffer.Length);
            output.Close();
            Receive();
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}