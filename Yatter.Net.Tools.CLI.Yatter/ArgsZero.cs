using System;
namespace Yatter.Net.Tools.CLI.Yatter
{
    public static class ArgsZero
    {
        public static int Run(string versionString)
        {
            Console.WriteLine($"yatter v{versionString}");
            Console.WriteLine("-------------");
            Console.WriteLine("\nUsage:");
            Console.WriteLine("  yatter pack");

            return 0;
        }
    }
}
