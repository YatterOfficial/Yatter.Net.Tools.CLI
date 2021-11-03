// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Yatter.Net.Tools.CLI.Yatter;

namespace Yatter.Net.Tools.CLI
{
    class Program
    {

        static async Task<int> Main(string[] args)
        {
            bool silent = false;
            for (int x = 0; x < args.Length; x++)
            {
                if(args[x].Equals("-s")||args[x].Equals("--silent"))
                {
                    silent = true;
                }
            }
            string versionString = "0.0.7";

            string currentExecutionModule = string.Empty;

            List<string> primaryarguments = new List<string>();
            primaryarguments.Add("microsite");
            primaryarguments.Add("cryptography");

            if (!silent)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Executing yatter ");

                for (int x = 0; x < args.Length; x++)
                {
                    Console.Write($"{args[x]} ");
                }

                Console.WriteLine($"in {System.Environment.CurrentDirectory}");
                Console.ResetColor();
            }

            int response = 0;

            if (args.Length == 0)
            {
                response = ArgsZero.Run(versionString);
            }
            else if(args[0].Equals("microsite"))
            {
                currentExecutionModule = "microsite";

                response = await Microsite.Run(args, System.Environment.CurrentDirectory);
            }
            else if (args[0].Equals("cryptography"))
            {
                currentExecutionModule = "cryptography";

                response = await Cryptography.Run(args, System.Environment.CurrentDirectory, silent);
            }
            else
            {
                response = 1;

                if (!silent)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Exiting, first <argument> in yatter <argument> ... was not one of ");

                    foreach (var primary in primaryarguments)
                    {
                        Console.Write($"[{primary}]");
                    }
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.WriteLine("Try:");
                    Console.WriteLine(" yatter [enter]");
                    Console.WriteLine(" yatter microsite --help [enter]");
                    Console.WriteLine(" yatter cryptography --help [enter]");
                    Console.WriteLine();
                }
            }

            if (!silent)
            {
                if (response == 1)
                {
                    Console.WriteLine("Failed.");
                }
                else
                {
                    Console.WriteLine($"Finished executing yatter {currentExecutionModule}");
                }
            }


            if (!silent)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Nuget Package Yatter.Net.Tools.CLI::{versionString}, released as open-source under MIT Licence as ");
                Console.WriteLine($"'Yatter Content-Management CLI (dotnet tool)', Copyright (C) Anthony Harrison 2021");
                Console.ResetColor();
            }

            return response;
        }
    }
}