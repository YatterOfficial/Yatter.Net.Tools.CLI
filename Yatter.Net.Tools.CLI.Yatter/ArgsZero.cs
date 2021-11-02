using System;
namespace Yatter.Net.Tools.CLI.Yatter
{
    public static class ArgsZero
    {
        public static int Run(string versionString)
        {
            Console.WriteLine("Yatter Content-Management CLI (dotnet tool)");
            Console.WriteLine($"yatter v{versionString}");
            Console.WriteLine("Author: Anthony Harrison");
            Console.WriteLine("-------------");
            Console.WriteLine("\nUsage:");
            Console.WriteLine("  yatter [command] [options]");
            Console.WriteLine();
            Console.WriteLine("Available Commands:");
            Console.WriteLine();
            Console.WriteLine(" microsite");
            Console.WriteLine("  yatter microsite [options]");
            Console.WriteLine("  yatter microsite --help");
            Console.WriteLine();
            Console.WriteLine("-------------");

            return 0;
        }
    }
}
