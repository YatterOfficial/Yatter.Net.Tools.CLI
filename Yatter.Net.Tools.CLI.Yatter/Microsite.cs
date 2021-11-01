﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yatter.UI.ListBuilder.Serialization.Archives;

namespace Yatter.Net.Tools.CLI.Yatter
{
    public enum PackingType
    {
        notspecified,
        yatra,
        yatrz
    }
    public static class Microsite
    {
        public static async Task<int> Run(string[] args, string currentDirectory)
        {
            int isError = 0;
            var messages = new List<string>();

            bool pack = false;
            bool yatra = false;
            bool yatrz = false;
            bool customfilename = false;
            bool root = false;
            string rootpath = string.Empty;
            string filename = string.Empty;
            bool verbose = false;

            for (int x = 0; x < args.Length; x++)
            {
                if (args[x].Equals("-p") || args[x].Equals("--pack"))
                {
                    pack = true;
                }

                if (args[x].Equals("-v") || args[x].Equals("--verbose"))
                {
                    verbose = true;
                }

                if (args[x].Equals("-a") || args[x].Equals("--yatra"))
                {
                    yatra = true;
                }

                if (args[x].Equals("-z") || args[x].Equals("--yatrz"))
                {
                    yatrz = true;
                }

                if (args[x].Equals("-r") || args[x].Equals("--root"))
                {
                    root = true;

                    if (x + 1 < args.Length)
                    {
                        if (args[x + 1] != "-p" &&
                            args[x + 1] != "-pack" &&
                            args[x + 1] != "-a" &&
                            args[x + 1] != "--yatra" &&
                            args[x + 1] != "-z" &&
                            args[x + 1] != "--yatrz" &&
                            args[x + 1] != "-r" &&
                            args[x + 1] != "--root" &&
                            args[x + 1] != "-f" &&
                            args[x + 1] != "--filename"
                            )
                        {
                            rootpath = args[x + 1];
                        }
                        else
                        {
                            isError = 1;
                            messages.Add($"Exiting: {args[x]} specified but no root path follows it");
                        }
                    }
                    else
                    {
                        isError = 1;
                        messages.Add($"Exiting: {args[x]} specified but no root path follows it");
                    }
                }

                if (args[x].Equals("-f") || args[x].Equals("--filename"))
                {
                    customfilename = true;

                    if (x + 1 < args.Length)
                    {
                        if(args[x + 1]!="-p"&&
                            args[x + 1] != "-pack" &&
                            args[x + 1] != "-a" &&
                            args[x + 1] != "--yatra" &&
                            args[x + 1] != "-z" &&
                            args[x + 1] != "--yatrz" &&
                            args[x + 1] != "-r" &&
                            args[x + 1] != "--root" &&
                            args[x + 1] != "-f" &&
                            args[x + 1] != "--filename"
                            )
                        {
                            filename = args[x + 1];
                        }
                        else
                        {
                            isError = 1;
                            messages.Add($"Exiting: {args[x]} specified but no filename follows it");
                        }
                    }
                    else
                    {
                        isError = 1;
                        messages.Add($"Exiting: {args[x]} specified but no filename follows it");
                     }
                }
            }

            if (!string.IsNullOrEmpty(filename))
            {
                var extension = filename.Substring(filename.Length - 5);

                if(!extension.Equals("yatra")&&!extension.Equals("yatrz"))
                {
                    isError = 1;
                    messages.Add($"Exiting: -f (--filename) extension can only be one of .yatra or .yatrz");
                }

                if (extension.Equals("yatrz"))
                {
                    if (yatra)
                    {
                        isError = 1;
                        messages.Add($"Exiting: -a (--yatra) specified but filename has .yatrz extension for -z (--yatrz)");
                    }
                    else
                    {
                        yatrz = true;
                    }
                }

                if (extension.Equals("yatra"))
                {
                    if (yatrz)
                    {
                        isError = 1;
                        messages.Add($"Exiting: -z (--yatrz) specified but filename has .yatra extension for -a (--yatra)");
                    }
                    else
                    {
                        yatra = true;
                    }
                }
            }

            if(string.IsNullOrEmpty(rootpath))
            {
                isError = 1;
                messages.Add($"Exiting: You must specify a -r (--root) path where the archive can be unpacked ... or specify -r null");
            }

            if (yatra && yatrz)
            {
                isError = 1;
                messages.Add($"Exiting: You cannot specify both of -a (-yatra) and -z (yatrz)");
            }

            if (isError == 0)
            {
                if (pack)
                {
                    if (yatra)
                    {
                        isError = await Pack(PackingType.yatra, filename, rootpath, currentDirectory, verbose);
                    }

                    if (yatrz)
                    {
                        isError = await Pack(PackingType.yatrz, filename, rootpath, currentDirectory, verbose);
                    }
                }
                else
                {
                    isError = 1;
                    messages.Add($"Exiting as -p (--pack) was not specified");
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

        private static async Task<int> Pack(PackingType packingType, string filename, string rootPath, string currentDirectory, bool verbose)
        {
            int response = 0;
            if(packingType==PackingType.yatra)
            {
                if (verbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Proceeding to pack lightweight archive with file extension .yatra");
                    Console.ResetColor();
                }
                response = await PackLightweight(filename, rootPath, currentDirectory, verbose);
            }
            else if(packingType==PackingType.yatrz)
            {
                if (verbose)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Proceeding to pack zip archive with file extension .yatrz");
                    Console.ResetColor();
                }
                response = PackZip(filename, rootPath, currentDirectory, verbose);
            }
            else
            {
                response = 1;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exiting, attempted to pack without proper pack-type.");
                Console.ResetColor();
            }

            return response;
        }

        private static async Task<int> PackLightweight(string filename, string rootPath, string currentDirectory, bool verbose)
        {
            int response = 0;

            var magazine = new Magazine() { PathRoot = rootPath };

            List<string> rawPaths = new List<string>();
            List<string> filenames = new List<string>();
            List<string> yattercache = new List<string>();


            string[] files = Directory.GetFiles(currentDirectory, "*.yatr");

            Console.ForegroundColor = ConsoleColor.Blue;
            if (verbose)
            {
                Console.WriteLine("Searching for .yatr files in current directory:");
            }
            foreach (var file in files)
            {
                if (verbose)
                {
                    Console.WriteLine(file);
                }
                rawPaths.Add(file);
                filenames.Add(file.Substring(currentDirectory.Length+1));
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            if (verbose)
            {
                Console.WriteLine("Revising .yatr files in current directory:");
            }
            foreach (var file in filenames)
            {
                if(verbose)
                {
                    Console.WriteLine(file);
                }
            }
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (verbose)
            {
                Console.WriteLine("Reading .yatr files to yatter-cache:");
            }
            foreach(var raw in rawPaths)
            {
                var content = System.IO.File.ReadAllText(raw);
                yattercache.Add(content);
                if (verbose)
                {
                    Console.WriteLine($"File {raw} :");
                    Console.WriteLine(content);
                }
            }
            Console.ResetColor();

            string[] filenameArray = filenames.ToArray();
            string[] yatterCacheArray = yattercache.ToArray();

            magazine.Documents = new List<Document>();

            for(int d = 0; d < filenameArray.Length; d++)
            {
                magazine.Documents.Add(new Document { Base64Content = Base64Encode(yatterCacheArray[d]), Path = filenameArray[d] });
            }

            var magazineJson = JsonConvert.SerializeObject(magazine, Formatting.Indented);

            await System.IO.File.WriteAllTextAsync(Path.Combine(currentDirectory, filename), magazineJson, System.Text.Encoding.UTF8);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Wrote each of ");

            foreach(var file in filenames)
            {
                Console.Write($"[{file}]");
            }
            Console.WriteLine($" as Base64 in a respective Document of Type {typeof(Document)}, thereso describing each one's respective path, then archived each of those Documents in a single Magazine of Type {typeof(Magazine)} and saved that Magazine in Yatter Object Notation (YON) format - a subset of JSON - in file {Path.Combine(currentDirectory, filename)}");

            Console.ResetColor();

            return response;
        }

        private static int PackZip(string filename, string rootPath, string currentDirectory, bool verbose)
        {
            int response = 0;

            return response;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
