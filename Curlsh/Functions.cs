using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Curlsh
{
    public class Functions
    {
        public static int Shell(string command, string args)
        {
            var arguments = string.Format(command + " " + args);
            var processInfo = new ProcessStartInfo()
            {
                FileName = "sh",
                Arguments = "-c " + arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true

            };

            Process process = Process.Start(processInfo);   // Start that process.
            while (!process.StandardOutput.EndOfStream)
            {
                string result = process.StandardOutput.ReadLine();
                Console.WriteLine(result);

            }
            while (!process.StandardError.EndOfStream)
            {
                string result = process.StandardError.ReadLine();
                Console.WriteLine(result);

            }
            process.WaitForExit();
            return 1;
        }
    }
}