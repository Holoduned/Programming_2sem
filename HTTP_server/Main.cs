﻿using Npgsql;

namespace Http_Server
{
    public static class Programm
    {
              private static bool _appIsRunning = true;
                static void Main(string[] args)
                {
                    using (var server=new HttpServer())
                    {
                        server.Start();
                        while (_appIsRunning)
                            Handler(Console.ReadLine()?.ToLower(),server);
                    }
                }
                static void Handler(string command, HttpServer server)
                {
                    switch (command)
                    {
                        case "stop": server.Stop(); break;
                        case "restart": server.Start(); server.Stop(); break;
                        case "start": server.Start(); break;
                        case "status": Console.WriteLine(server.Status.ToString()); break;
                        case "exit": _appIsRunning = false; break;
                    }
                }

    }
}
