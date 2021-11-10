using System;
namespace Yatter.Net.Tools.CLI.Yatter
{
    public static class Utilities
    {
        public static void ConsoleWriteVerboseBooleanCommandLineAssignment(string[] args, int isError, bool createkeypair, bool verbose, bool silent, int x)
        {
            if (isError != 1 && verbose && !silent)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Command Line Assignment: {args[x]} is {createkeypair}");
                Console.ResetColor();
            }
        }

        public static void ConsoleWriteVerboseStringCommandLineAssignment(string[] args, int isError, string value, bool verbose, bool silent, int x)
        {
            if (isError != 1 && verbose && !silent)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Command Line Assignment: {args[x]} is [{value}]");
                Console.ResetColor();
            }
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
