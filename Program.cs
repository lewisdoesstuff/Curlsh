using System;
using CommandLine;
using System.Net;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
namespace Curlsh
{
    class Options
    {

    }

    class Program
    {
        static void Main(string[] args)
        {
            var get = args[0];
            var script = Fetch(get);
            
            Console.WriteLine(script);
            Urls(script);
            Repos(script);
            Make(script);
            Remove(script);


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
        
        
    }
}
