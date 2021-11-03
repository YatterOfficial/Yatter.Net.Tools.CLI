using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Threading.Tasks;
using Yatter.Security.Cryptography;

namespace Yatter.Net.Tools.CLI.Yatter
{
    public static class Cryptography
    {
        public static async Task<int> Run(string[] args, string currentDirectory)
        {

            RootCommand rootCommand = new RootCommand(
              description: "A Content Management Tool for using Yatter Object Notation (YON). Standard usage: yatter [command] [options].");

            Command command = new Command("cryptography", "RSA Public and Private key encryption, using only .NET Code from System.Security.Cryptography to create keys, to both encrypt and decrypt using those keys, and to retrieve public keys from DNS TXT records, and public key servers. Standard usage: yatter cryptography [options]");
            rootCommand.Add(command);

            var createkeypair = new Option<bool>("-c");
            createkeypair.AddAlias("--createkeypair");
            createkeypair.Description = "The cryptography command's switch to indicate that it will create an RSA Public and Private Key Pair, in Base64 format, outputting the private key to a filepath indicated by the switch -P (--privatekeyfilename), and outputting the public key to a filepath indicated by the switch -p (--publickeyfilename); optional. Path specified can be to a file in the current directory, relative to the current directory, or absolute.";
            command.AddOption(createkeypair);

            var publickeyfilepath = new Option<string>("-p");
            publickeyfilepath.AddAlias("--publickeyfilename");
            publickeyfilepath.Description = "The cryptography command's filename switch for the file that contains, or will contain, the public key in Base64 format; optional. Path can be to a file in the current directory, relative to the current directory, or absolute.";
            command.AddOption(publickeyfilepath);

            var privatekeyfilepath = new Option<string>("-P");
            privatekeyfilepath.AddAlias("--privatekeyfilename");
            privatekeyfilepath.Description = "The cryptography command's filename switch for the file that contains, or will contain, the private key in Base64 format; optional. Path can be to a file in the current directory, relative to the current directory, or absolute.";
            command.AddOption(privatekeyfilepath);

            var encrypt = new Option<bool>("-e");
            encrypt.AddAlias("--encrypt");
            encrypt.Description = "The cryptography command's switch to indicate that it will encrypt text indicated by the switch -t (--textinput), or encrypt text that is the contents of the file indicated by the switch -f (--fileinput), using the Base64 key indicated by the switch -k (--key), or using the Base64 key in the contents of the file indicated by the switch -a (--anykeyfilename), outputting the result to the console, or to the filename -o (--output); optional. Paths specified by -a (--anykeyfilename) or -o (--output) can be to files in the current directory, relative to the current directory, or absolute.";
            command.AddOption(encrypt);

            var decrypt = new Option<bool>("-d");
            decrypt.AddAlias("--decrypt");
            decrypt.Description = "The cryptography command's switch to indicate that it will decrypt text indicated by the switch -t (--textinput), or decrypt text that is the contents of the file indicated by the switch -f (--fileinput), using either the Base64 public or private key indicated by the switch -k (--key), or using the Base64 key in the contents of the file indicated by the switch -a (--anykeyfilename), outputting the result to the console, or to the filename -o (--output); optional. Paths specified by -a (--anykeyfilename) or -o (--output) can be to files in the current directory, relative to the current directory, or absolute.";
            command.AddOption(decrypt);

            var textinput = new Option<string>("-t");
            textinput.AddAlias("--textinput");
            textinput.Description = "The cryptography command's switch to indicate text that will be encrypted in conjunction with the use of the switch -e (--encrypt); optional";
            command.AddOption(textinput);

            var fileinput = new Option<string>("-f");
            fileinput.AddAlias("--fileinput");
            fileinput.Description = "The cryptography command's switch to indicate text in the contents of a file that will be encrypted in conjunction with the use of the switch -e (--encrypt); optional. Path specified by -f (--fileinput) can be to a file in the current directory, relative to the current directory, or absolute.";
            command.AddOption(fileinput);

            var key = new Option<string>("-k");
            key.AddAlias("--key");
            key.Description = "The cryptography command's switch to indicate the Base64 public or private key that will be used in conjunction with the use of the switch -e (--encrypt), or the switch -d (--decrypt), to encrypt, or decrypt, respectively; optional.";
            command.AddOption(key);

            var getpublickey = new Option<bool>("-g");
            getpublickey.AddAlias("--getpublickey");
            getpublickey.Description = "The cryptography command's switch to indicate that it will use an HttpClient to get the public key in Base64 from a DNS TXT Record indicated by the switch -n (--dnstxtkey), at a URL indicated by the switch -u (--url), or to get it from a public key server at a URL indicated by the switch -u (--url), and requiring a key id indicated by the switch -i (--id),  where a querystring appended to that url is in the format ?id=[id] and [id] is Base64Encoded; optional. When using -u (--url) in conjunction with -i (--id), do not append the querystring, the command appends this internally.";
            command.AddOption(getpublickey);

            var url = new Option<bool>("-u");
            url.AddAlias("--url");
            url.Description = "The cryptography command's switch to indicate the URL that will be used in conjunction with -g (getpublickey).";
            command.AddOption(url);


            var anykeyfilename = new Option<string>("-a");
            anykeyfilename.AddAlias("--anykeyfilename");
            anykeyfilename.Description = "The cryptography command's switch to indicate the Base64 public or private key in the contents of the indicated filename that will be used in conjunction with the use of the switch -e (--encrypt), or the switch -d (--decrypt), to encrypt, or decrypt, respectively; optional.";
            command.AddOption(anykeyfilename);

            var dnstxtkey = new Option<string>("-n");
            dnstxtkey.AddAlias("--dnstxtkey");
            dnstxtkey.Description = "The cryptography command's switch to indicate the DNS TXT key that is used in conjunction with -g (--getpublickey) from a URL indicated by the switch -u (--url); optional. Cannot be used with -i (--id).";
            command.AddOption(dnstxtkey);

            var id = new Option<string>("-i");
            id.AddAlias("--id");
            id.Description = "The cryptography command's switch to indicate the id that will be used in conjunction with the switch -g (--getpublickey); optional. Cannot be used with -n (--dnstxtkey).";
            command.AddOption(id);

            var tokenheaderkey = new Option<string>("-y");
            tokenheaderkey.AddAlias("--tokenheaderkey");
            tokenheaderkey.Description = "The cryptography command's switch to indicate the header key, if required, of a -u (--url) that will be used in conjunction with the switch -i (--id); optional.";
            command.AddOption(tokenheaderkey);

            var tokenheadervalue = new Option<string>("-z");
            tokenheadervalue.AddAlias("--tokenheadervalue");
            tokenheadervalue.Description = "The cryptography command's switch to indicate the header value, if required, of a -u (--url) that will be used in conjunction with the switch -i (--id); optional.";
            command.AddOption(tokenheadervalue);

            var output = new Option<string>("-o");
            output.AddAlias("--output");
            output.Description = "The cryptography command's switch to indicate that output is to be placed as the contents of the file indicated; optional, if not specified, output will be written to console. Path can be to a file in the current directory, relative to the current directory, or absolute.";
            command.AddOption(output);

            var verbose = new Option<bool>("--verbose");
            verbose.AddAlias("-v");
            verbose.Description = "Instructs the CLI to make verbose CLI comments as it executes.";
            command.AddOption(verbose);

            command.Handler = CommandHandler.Create<ParseResult>(async (result) =>
            {
                await RunActions(args, currentDirectory);
            });

            var commandLineBuilder = new CommandLineBuilder(rootCommand);
            commandLineBuilder.UseMiddleware(async (context, next) =>
            {
                if (context.ParseResult.Tokens[0].Value.ToLower().Equals("cryptography"))
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

        private static async Task<int> RunActions(string[] args, string currentDirectory)
        {
            int isError = 0;
            var messages = new List<string>();

            bool createkeypair = false;
            string publickeyfilename = string.Empty;
            string privatekeyfilename = string.Empty;
            bool encrypt = false;
            bool decrypt = false;
            string textinput = string.Empty;
            string fileinput = string.Empty;
            string key = string.Empty;
            bool getpublickey = false;
            string url = string.Empty;
            string anykeyfilename = string.Empty;
            string dnstxtkey = string.Empty;
            string id = string.Empty;
            string tokenheaderkey = string.Empty;
            string tokenheadervalue = string.Empty;
            string output = string.Empty;
            bool verbose = false;

            for (int x = 0; x < args.Length; x++)
            {
                if (args[x].Equals("-c") || args[x].Equals("--createkeypair"))
                {
                    createkeypair = true;

                    ConsoleWriteVerboseBooleanCommandLineAssignment(args, isError, createkeypair, verbose, x);
                }

                if (args[x].Equals("-p") || args[x].Equals("--publickeyfilename"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref publickeyfilename, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, publickeyfilename, verbose, x);
                }

                if (args[x].Equals("-P") || args[x].Equals("--privatekeyfilename"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref privatekeyfilename, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, privatekeyfilename, verbose, x);
                }

                if (args[x].Equals("-e") || args[x].Equals("--encrypt"))
                {
                    encrypt = true;

                    ConsoleWriteVerboseBooleanCommandLineAssignment(args, isError, encrypt, verbose, x);
                }

                if (args[x].Equals("-d") || args[x].Equals("--decrypt"))
                {
                    decrypt = true;

                    ConsoleWriteVerboseBooleanCommandLineAssignment(args, isError, decrypt, verbose, x);
                }

                if (args[x].Equals("-t") || args[x].Equals("--textinput"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref textinput, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, textinput, verbose, x);
                }

                if (args[x].Equals("-f") || args[x].Equals("--fileinput"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref fileinput, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, fileinput, verbose, x);
                }

                if (args[x].Equals("-k") || args[x].Equals("--key"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref key, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, key, verbose, x);
                }

                if (args[x].Equals("-g") || args[x].Equals("--getpublickey"))
                {
                    getpublickey = true;

                    ConsoleWriteVerboseBooleanCommandLineAssignment(args, isError, getpublickey, verbose, x);
                }

                if (args[x].Equals("-u") || args[x].Equals("--url"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref url, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, url, verbose, x);
                }

                if (args[x].Equals("-a") || args[x].Equals("--anykeyfilename"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref anykeyfilename, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, anykeyfilename, verbose, x);
                }

                if (args[x].Equals("-n") || args[x].Equals("--dnstxtkey"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref dnstxtkey, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, dnstxtkey, verbose, x);
                }

                if (args[x].Equals("-i") || args[x].Equals("--id"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref id, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, id, verbose, x);
                }

                if (args[x].Equals("-y") || args[x].Equals("--tokenheaderkey"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref tokenheaderkey, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, tokenheaderkey, verbose, x);
                }

                if (args[x].Equals("-z") || args[x].Equals("--tokenheadervalue"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref tokenheadervalue, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, tokenheadervalue, verbose, x);
                }

                if (args[x].Equals("-o") || args[x].Equals("--output"))
                {
                    AssignValueIfFollowsOrExit(args, ref isError, messages, ref output, x);

                    ConsoleWriteVerboseStringCommandLineAssignment(args, isError, output, verbose, x);
                }

                if (args[x].Equals("-v") || args[x].Equals("--verbose"))
                {
                    verbose = true;

                    Console.WriteLine(); // Commence verbose after a blank line
                    ConsoleWriteVerboseBooleanCommandLineAssignment(args, isError, verbose, verbose, x);
                }
            }

            if(createkeypair)
            {
                if(string.IsNullOrEmpty(publickeyfilename)||string.IsNullOrEmpty(privatekeyfilename))
                {
                    isError = 1;
                    messages.Add($"Exiting: both -p (--publickeyfilename) and -P (--privatekeyfilename) must be specified.");
                }

                if(encrypt||decrypt||getpublickey)
                {
                    isError = 1;
                    messages.Add("Exiting: none of -e (--encrypt), -d (--decrypt), or -g (--getpublickey), may be specified with -c (--createkeypair).");
                }
            }

            if(encrypt)
            {
                isError = EnforceEncryptDecryptStringStates(isError, messages, textinput, fileinput, key, anykeyfilename);

                if (createkeypair || decrypt || getpublickey)
                {
                    isError = 1;
                    messages.Add("Exiting: none of -c (--createkeypair), -d (--decrypt), or -g (--getpublickey), may be specified with -e (--encrypt).");
                }
            }

            if (decrypt)
            {
                isError = EnforceEncryptDecryptStringStates(isError, messages, textinput, fileinput, key, anykeyfilename);

                if (createkeypair || encrypt || getpublickey)
                {
                    isError = 1;
                    messages.Add("Exiting: none of -c (--createkeypair), -e (--encrypt), or -g (--getpublickey), may be specified with -d (--decrypt).");
                }
            }

            if (getpublickey)
            {
                if(string.IsNullOrEmpty(url))
                {
                    isError = 1;
                    messages.Add("Exiting: -u (--url) must be specified when using -g (--getpublickey).");
                }

                if (!string.IsNullOrEmpty(dnstxtkey) && !string.IsNullOrEmpty(id))
                {
                    isError = 1;
                    messages.Add("Exiting: only one of -n (--dnstxtkey) and -i (--id) may be specified.");
                }

                if (string.IsNullOrEmpty(dnstxtkey) && string.IsNullOrEmpty(id))
                {
                    isError = 1;
                    messages.Add("Exiting: one of -n (--dnstxtkey) and -i (--id) must be specified.");
                }

                if (!string.IsNullOrEmpty(tokenheaderkey) && string.IsNullOrEmpty(tokenheadervalue))
                {
                    isError = 1;
                    messages.Add("Exiting: -z (--tokenheadervalue) must be specified when -y (--tokenheaderkey) is specified.");
                }

                if (string.IsNullOrEmpty(tokenheaderkey) && !string.IsNullOrEmpty(tokenheadervalue))
                {
                    isError = 1;
                    messages.Add("Exiting: -y (--tokenheaderkey) must be specified when -z (--tokenheadervalue) is specified.");
                }


                if (createkeypair || encrypt || decrypt)
                {
                    isError = 1;
                    messages.Add("Exiting: none of -c (--createkeypair), -e (--encrypt), or -d (--decrypt), may be specified with -g (--getpublickey).");
                }
            }

            if (isError == 0)
            {
                if (createkeypair)
                {
                    var cryptographyKeyManager = new CryptographyKeyManager();

                    cryptographyKeyManager.CreateKeySet();
                   
                    var privateKeyBytes = cryptographyKeyManager.ExportRSAPrivateKeyBytes();
                    var publicKeyBytes = cryptographyKeyManager.ExportRSAPublicKeyBytes();

                    var privateKeyBytesAsBase64 = Convert.ToBase64String(privateKeyBytes);
                    var publicKeyBytesAsBase64 = Convert.ToBase64String(publicKeyBytes);

                    if(verbose)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine();
                        Console.WriteLine($"Public Key Base64: {publicKeyBytesAsBase64}");
                        Console.WriteLine();
                        Console.WriteLine($"Private Key Base64: {privateKeyBytesAsBase64}");
                        Console.WriteLine();
                        Console.ResetColor();
                    }

                    await System.IO.File.WriteAllTextAsync(Path.Combine(currentDirectory, publickeyfilename), publicKeyBytesAsBase64);

                    if(verbose)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"Public Key written to: {Path.Combine(currentDirectory, publickeyfilename)}");
                        Console.ResetColor();
                    }

                    await System.IO.File.WriteAllTextAsync(Path.Combine(currentDirectory, privatekeyfilename), privateKeyBytesAsBase64);

                    if(verbose)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"Private Key written to: {Path.Combine(currentDirectory, privatekeyfilename)}");
                        Console.ResetColor();
                    }

                }
                else if (encrypt)
                {

                }
                else if (decrypt)
                {

                }
                else if (getpublickey)
                {

                }
                else
                {
                    isError = 1;
                    messages.Add($"Exiting: Either -c (--createkeypair), -e (--encrypt), -d (--decrypt), or -g (--getpublickey), must be specified.");
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

        private static void ConsoleWriteVerboseBooleanCommandLineAssignment(string[] args, int isError, bool createkeypair, bool verbose, int x)
        {
            if (isError!=1 && verbose)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Command Line Assignment: {args[x]} is {createkeypair}");
                Console.ResetColor();
            }
        }

        private static void ConsoleWriteVerboseStringCommandLineAssignment(string[] args, int isError, string value, bool verbose, int x)
        {
            if (isError!=1 && verbose)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Command Line Assignment: {args[x]} is [{value}]");
                Console.ResetColor();
            }
        }

        private static int EnforceEncryptDecryptStringStates(int isError, List<string> messages, string textinput, string fileinput, string key, string anykeyfilename)
        {
            if (!string.IsNullOrEmpty(textinput) && !string.IsNullOrEmpty(fileinput))
            {
                isError = 1;
                messages.Add("Exiting: only one of -t (--textinput) and -f (--fileinput) may be specified.");
            }

            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(anykeyfilename))
            {
                isError = 1;
                messages.Add("Exiting: only one of -k (--key) and -a (--anykeyfilename) may be specified.");
            }

            if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(anykeyfilename))
            {
                isError = 1;
                messages.Add("Exiting: one of -k (--key) and -a (--anykeyfilename) must be specified.");
            }

            return isError;
        }

        private static void AssignValueIfFollowsOrExit(string[] args, ref int isError, List<string> messages, ref string publickeyfilename, int x)
        {
            if (x + 1 < args.Length)
            {
                if (args[x + 1] != "-c" &&
                    args[x + 1] != "-createkeypair" &&
                    args[x + 1] != "-p" &&
                    args[x + 1] != "--publickeyfilename" &&
                    args[x + 1] != "-P" &&
                    args[x + 1] != "--privatekeyfilename" &&
                    args[x + 1] != "-e" &&
                    args[x + 1] != "--encrypt" &&
                    args[x + 1] != "-d" &&
                    args[x + 1] != "--decrypt" &&
                    args[x + 1] != "-t" &&
                    args[x + 1] != "--textinput" &&
                    args[x + 1] != "-f" &&
                    args[x + 1] != "--fileinput" &&
                    args[x + 1] != "-k" &&
                    args[x + 1] != "--key" &&
                    args[x + 1] != "-g" &&
                    args[x + 1] != "--getpublickey" &&
                    args[x + 1] != "-a" &&
                    args[x + 1] != "--anykeyfilename" &&
                    args[x + 1] != "-n" &&
                    args[x + 1] != "--dnstxtkey" &&
                    args[x + 1] != "-i" &&
                    args[x + 1] != "--id" &&
                    args[x + 1] != "-y" &&
                    args[x + 1] != "--tokenheaderkey" &&
                    args[x + 1] != "-z" &&
                    args[x + 1] != "--tokenheadervalue" &&
                    args[x + 1] != "-o" &&
                    args[x + 1] != "--output"

                    )
                {
                    publickeyfilename = args[x + 1];
                }
                else
                {
                    isError = 1;
                    messages.Add($"Exiting: {args[x]} specified but it's value does not follow it.");
                }
            }
        }
    }
}
