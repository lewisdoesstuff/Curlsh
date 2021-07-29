using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;

namespace Curlsh
{
    public class Parsing
    {

        

        public static string[] Match(string script, string pattern, string removePattern = null)
        {
            Console.WriteLine();
            var regex = new Regex(pattern);
            var matches = regex.Matches(script).Select(match => match.Value).ToArray();

            if (!string.IsNullOrEmpty(removePattern))
            {
                var i = 0;
                foreach (var match in matches)
                {
                    matches[i] = Regex.Replace(match, removePattern, "");
                    i++;
                }
            }
            return matches;
        }
    }
}