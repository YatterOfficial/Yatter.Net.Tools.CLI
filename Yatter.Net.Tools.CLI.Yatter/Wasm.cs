using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DnsClient;
using Newtonsoft.Json;
using Yatter.Security.Cryptography;

namespace Yatter.Net.Tools.CLI.Yatter
{
    public static class WasmSerializer
    {
        public static async Task<int> Run(string[] args, string currentDirectory, bool silent)
        {

            RootCommand rootCommand = new RootCommand(
              description: "A WASM packing tool that packs all of the following four files in the current directory into a single file: index.html, index.wasm, index.data, index.js");

            Command command = new Command("wasm", "The wasmdiv command packs all of the following four files in the current directory into a single file: index.html, index.wasm, index.data, index.js. Standard usage: yatter wasmdiv [options]");
            rootCommand.Add(command);

            var pack = new Option<bool>("-p");
            pack.AddAlias("--pack");
            pack.Description = "The wasm command's packing switch, optional, however one of -p (--pack) or -u (--unpack) must be specified; instructs the CLI to pack index.html index.wasm, index.data, and index.js, into a single file.";
            pack.IsRequired = false;
            command.AddOption(pack);

            var unpack = new Option<bool>("--unpack");
            unpack.AddAlias("-u");
            unpack.Description = "The wasm command's unpacking switch, optional, however one of -p (--pack) or -u (--unpack) must be specified;  instructs the CLI to unpack WASM that is in a single file, into the following four files: index.html, index.wasm, index.data, and index.js.";
            command.AddOption(unpack);

            var filename = new Option<string>("--file");
            filename.AddAlias("-f");
            filename.Description = "The wasm command's filename switch, optional, the input or output filename, depending upon whether packing (-p (--pack)) or unpacking (-u (--unpack).";
            command.AddOption(filename);

            var ratio = new Option<double>("--ratio");
            ratio.AddAlias("-r");
            ratio.Description = "The wasm command's Height-to-Width Ratio switch expressed as a double, optional, defaults to -1 (fullscreen), 0 (zero) specifies no visible context.";
            command.AddOption(ratio);

            var width = new Option<int>("--width");
            width.AddAlias("-w");
            width.Description = "The wasm command's default width switch expressed as an int.";
            command.AddOption(width);

            var height = new Option<int>("--height");
            height.AddAlias("-h");
            height.Description = "The wasm command's default height switch expressed as an int.";
            command.AddOption(height);

            var verbose = new Option<bool>("--verbose");
            verbose.AddAlias("-v");
            verbose.Description = "Instructs the CLI to make verbose CLI comments as it executes.";
            command.AddOption(verbose);


            command.Handler = CommandHandler.Create<ParseResult>(async (result) =>
            {
                await RunActions(args, currentDirectory, silent);
            });

            var commandLineBuilder = new CommandLineBuilder(rootCommand);
            commandLineBuilder.UseMiddleware(async (context, next) =>
            {
                if (context.ParseResult.Tokens[0].Value.ToLower().Equals("wasm"))
                {
                    await command.Handler.InvokeAsync(context);
                }
                else
                {
                    await next(context);
                }
            });

            commandLineBuilder.UseDefaults();

            var parser = commandLineBuilder.Build();

            return await parser.InvokeAsync(args);
        }

        private static async Task<int> RunActions(string[] args, string currentDirectory, bool silent)
        {
            int isError = 0;
            var messages = new List<string>();

            bool pack = false;
            bool unpack = false;
            string filename = string.Empty;
            double ratio = 0;
            int width = 0;
            int height = 0;
            bool verbose = false;

            for (int x = 0; x < args.Length; x++)
            {
                if (args[x].Equals("-p") || args[x].Equals("--pack"))
                {
                    pack = true;
                }

                if (args[x].Equals("-u") || args[x].Equals("--unpack"))
                {
                    unpack = true;
                }

                if (args[x].Equals("-v") || args[x].Equals("--verbose"))
                {
                    verbose = true;
                }

                if (args[x].Equals("-f") || args[x].Equals("--file"))
                {
                    AssignStringValueIfFollowsOrExit(args, ref isError, messages, ref filename, x);

                    Utilities.ConsoleWriteVerboseStringCommandLineAssignment(args, isError, filename, verbose, silent, x);
                }

                if (args[x].Equals("-r") || args[x].Equals("--ration"))
                {
                    AssignDoubleValueIfFollowsOrExit(args, ref isError, messages, ref ratio, x);

                    Utilities.ConsoleWriteVerboseStringCommandLineAssignment(args, isError, filename, verbose, silent, x);
                }

                if (args[x].Equals("-w") || args[x].Equals("--width"))
                {
                    AssignInt32ValueIfFollowsOrExit(args, ref isError, messages, ref width, x);

                    Utilities.ConsoleWriteVerboseStringCommandLineAssignment(args, isError, filename, verbose, silent, x);
                }

                if (args[x].Equals("-h") || args[x].Equals("--height"))
                {
                    AssignInt32ValueIfFollowsOrExit(args, ref isError, messages, ref height, x);

                    Utilities.ConsoleWriteVerboseStringCommandLineAssignment(args, isError, filename, verbose, silent, x);
                }
            }

            if (!string.IsNullOrEmpty(filename))
            {
            }



            if (isError == 0)
            {
                if (pack)
                {
                    isError = await Pack(filename, currentDirectory, ratio, width, height, verbose, silent);
                }
                else if (unpack)
                {
                    isError = await Unpack(filename, currentDirectory, verbose, silent);
                }
                else
                {
                    isError = 1;
                    messages.Add($"Exiting: Either -p (--pack) or -u (--unpack) must be specified.");
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
            Console.ResetColor();

            return isError;
        }

        private static string indexFile = string.Empty;
        private static byte[]? wasmFile;
        private static byte[]? dataFile;
        private static string javascriptFile = string.Empty;

        private async static Task<int> Pack(string filename, string currentDirectory, double heightWidthRatio, int width, int height, bool verbose, bool silent)
        {
            int response = 0;
            try
            {
                indexFile = await File.ReadAllTextAsync(Path.Combine(currentDirectory, "index.html"));
                wasmFile = await File.ReadAllBytesAsync(Path.Combine(currentDirectory, "index.wasm"));
                dataFile = await File.ReadAllBytesAsync(Path.Combine(currentDirectory, "index.data"));
                javascriptFile = await File.ReadAllTextAsync(Path.Combine(currentDirectory, "index.js"));
            }
            catch(Exception ex)
            {
                return 1;
            }
            if(string.IsNullOrEmpty(indexFile)|| string.IsNullOrEmpty(javascriptFile)||wasmFile==null||wasmFile.Length==0||dataFile==null||dataFile.Length==0)
            {
                return 1;
            }

            var wasmBase64 = Convert.ToBase64String(wasmFile);
            var dataBase64 = Convert.ToBase64String(dataFile);

            var wasm = new Models.WasmSerialisation();
            wasm.IndexBase64 = Utilities.Base64Encode(indexFile);
            wasm.JavascriptBase64 = Utilities.Base64Encode(javascriptFile);
            wasm.WasmBase64 = wasmBase64;
            wasm.DataBase64 = dataBase64;
            wasm.HeightWidthRatio = heightWidthRatio;
            wasm.Width = width;
            wasm.Height = height;

            var serialised = JsonConvert.SerializeObject(wasm, Formatting.Indented);

            try
            {
                if (string.IsNullOrEmpty(filename))
                {
                    await File.WriteAllTextAsync(Path.Combine(currentDirectory, "index.yatrt"), serialised);
                }
                else
                {
                    await File.WriteAllTextAsync(Path.Combine(currentDirectory, filename), serialised);
                }
            }
            catch(Exception ex)
            {
                return 1;
            }

            return response;
        }

        private static Task<int> Unpack(string filename, string currentDirectory, bool verbose, bool silent)
        {
            throw new NotImplementedException();
        }

        private static void AssignStringValueIfFollowsOrExit(string[] args, ref int isError, List<string> messages, ref string key, int x)
        {
            if (x + 1 < args.Length)
            {
                if (Filter(args, x))
                {
                    key = args[x + 1];
                }
                else
                {
                    isError = 1;
                    messages.Add($"Exiting: {args[x]} specified but it's value does not follow it.");
                }
            }
        }

        private static void AssignDoubleValueIfFollowsOrExit(string[] args, ref int isError, List<string> messages, ref double key, int x)
        {
            if (x + 1 < args.Length)
            {
                if (Filter(args, x))
                {
                    key = Convert.ToDouble(args[x + 1]);
                }
                else
                {
                    isError = 1;
                    messages.Add($"Exiting: {args[x]} specified but it's value does not follow it.");
                }
            }
        }

        private static void AssignInt32ValueIfFollowsOrExit(string[] args, ref int isError, List<string> messages, ref int key, int x)
        {
            if (x + 1 < args.Length)
            {
                if (Filter(args, x))
                {
                    key = Convert.ToInt32(args[x + 1]);
                }
                else
                {
                    isError = 1;
                    messages.Add($"Exiting: {args[x]} specified but it's value does not follow it.");
                }
            }
        }

        private static bool Filter(string[] args, int x)
        {
            return args[x + 1] != "-p" &&
                                args[x + 1] != "--pack" &&
                                args[x + 1] != "-u" &&
                                args[x + 1] != "--unpack" &&
                                args[x + 1] != "-f" &&
                                args[x + 1] != "--filename" &&
                                args[x + 1] != "-r" &&
                                args[x + 1] != "--ratio" &&
                                args[x + 1] != "-w" &&
                                args[x + 1] != "--width" &&
                                args[x + 1] != "-h" &&
                                args[x + 1] != "--height" &&
                                args[x + 1] != "-v" &&
                                args[x + 1] != "--verbose" &&
                                args[x + 1] != "-s" &&
                                args[x + 1] != "--silent";
        }
    }
}
