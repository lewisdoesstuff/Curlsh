using System;
using System.Collections;
using System.Collections.Generic;
using CommandLine;
using System.Net;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
namespace Curlsh
{
    class Options
    {
        [Option('a', "args", Required = false, HelpText = "Passes given arguments to script")]
        public string Curl { get; set; }
        [Option('d', "dontrun", Required = false, HelpText = "Just run script analysis, don't run. You'll still be prompted if this isn't passed.")]
        public bool DontRun { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    var get = args[0];
                    var script = Fetch(get);

                    Console.WriteLine(script);
                    Urls(script);
                    Repos(script);
                    Make(script);
                    Remove(script);
                    if (o.DontRun) return;
                    Console.WriteLine("\n");
                    Console.WriteLine("Exit Code: " + Run(script, o.Curl));

                });

        }

        static void Urls(string script)
        {
            var urls = Parsing.Match(script,@"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*");
            
            Console.WriteLine("Script references {0} URLs.", urls.Length);
            if (urls.Length == 0) return;
            Console.WriteLine("URLs: ");

            foreach (var match in urls)
            { 
                Console.WriteLine(match);
            }
        }

        static void Repos(string script)
        {
            var repos = Parsing.Match(script, ".* >> /etc/apt/sources.list.*", "[\t]");
            
            Console.WriteLine("Script adds {0} APT repositories.", repos.Length);
            if (repos.Length == 0) return;
            Console.WriteLine("Lines:");
            
            foreach (var match in repos)
            {
                Console.WriteLine(match);
            }
        }

        static void Remove(string script)
        {
            var removed = Parsing.Match(script, "(?<! (?:.*)?)rm .*|(?<! (?:.*)?)rmdir .*", "[\t]");
            
            Console.WriteLine("Script removes {0} files", removed.Length );
            if (removed.Length == 0) return;
            Console.WriteLine("Lines:");

            foreach (var match in removed)
            {
                Console.WriteLine(match);
            }
        }

        static void Make(string script)
        {
            var mk = Parsing.Match(script, "(?<! (?:.*)?)mkdir .*|(?<! (?:.*)?)touch .*", "[\t]");
            
            Console.WriteLine("Script creates {0} files", mk.Length );
            if (mk.Length == 0) return;
            Console.WriteLine("Lines:");

            foreach (var match in mk)
            {
                Console.WriteLine(match);
            }
        }
        static string Fetch(string url)
        {
            string script = null;
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "GET";
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                script = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();


                if (response != null)
                    response.Close();

            }
            
            return script;
        }

        static int Run(string script, string curlArgs = null)
        {

            if (!string.IsNullOrEmpty(curlArgs))
            {
                var curlSep = curlArgs.Split(' ');
                curlArgs = curlSep.Aggregate<string, string>(null, (current, arg) => current + arg.Insert(0, " -"));
            }

            while (true)
            {
                Console.WriteLine("Would you like to run the script? y/n");
                var response = Console.ReadLine();
                if (response == "y")
                {
                    break;
                } 
                if (response == "n")
                    Environment.Exit(0);

            }
            return Functions.Shell(script,curlArgs);
        }
        
    }
}
