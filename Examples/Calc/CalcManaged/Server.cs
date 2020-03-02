﻿using System;
using System.Threading;
using IPC.Bond.Managed;

namespace Calc.Managed
{
    internal static class Server
    {
        public static void Run(string address)
        {
            Console.WriteLine($"Hosting server at {address}");

            using (var transport = new Transport<Request, Response>())
            using (var serversAccessor = transport.AcceptServers(address, (inMemory, outMemory) => new Service().Invoke))
            {
                serversAccessor.Error += (sender, args) => Console.WriteLine($"IPC: {args.Exception.Message}");

                serversAccessor.Connected += (sender, args) =>
                    Console.WriteLine($"Connected: {args.Component.InputMemory.Name} -> {args.Component.OutputMemory.Name}");

                serversAccessor.Disconnected += (sender, args) =>
                    Console.WriteLine($"Disconnected: {args.Component.InputMemory.Name} -> {args.Component.OutputMemory.Name}");

                Console.WriteLine("Press Ctrl+C to exit.");

                using (var exit = new ManualResetEvent(false))
                {
                    Console.CancelKeyPress += (sender, args) => { args.Cancel = true; exit.Set(); };
                    exit.WaitOne();
                }

                Console.WriteLine("Exiting...");

                foreach (var server in serversAccessor.Servers) // Just to trigger Disconnected events
                {
                    server.Dispose();
                }
            }
        }
    }
}
