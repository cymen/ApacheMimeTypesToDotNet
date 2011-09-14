using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace ApacheMimeTypesToDotNet
{
    class Program
    {
        private const string ApacheMimeTypesURL = "http://svn.apache.org/repos/asf/httpd/httpd/trunk/docs/conf/mime.types";
        private const string FileName = "ApacheMimeTypes.cs";
        private const string NameSpace = "ApacheMimeTypes";
        private const string ClassName = "Apache";
        private const string DictionaryName = "MimeTypes";
        private const char IndentChar = '\t';

        static void Main(string[] args)
        {
            string mimeTypesText = GetApacheMimeTypes(ApacheMimeTypesURL);
            if (string.IsNullOrEmpty(mimeTypesText))
            {
                Console.Write("Apache mime.types file empty!");
                Environment.Exit(0);
            }

            Dictionary<string, string> dictionary = ParseMimeTypesToDictionary(mimeTypesText);
            if (dictionary.Keys.Count == 0)
            {
                Console.WriteLine("Attempted to convert mime.types file to dictionary but no entries!");
                Environment.Exit(0);
            }

            WriteDictionaryToCsharpMimeTypesClass(dictionary);
        }

        private static string GetApacheMimeTypes(string url)
        {
            WebClient webClient = new WebClient();
            return webClient.DownloadString(url);
        }

        private static Dictionary<string, string> ParseMimeTypesToDictionary(string mimeTypesText)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            using (StringReader reader = new StringReader(mimeTypesText))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // skip commments
                    if (line.Trim().StartsWith("#"))
                        continue;

                    string[] parts = Regex.Split(line, "\\s+");
                    if (parts.Count() < 2)
                    {
                        Console.WriteLine("Skipping line: " + line);
                        continue;
                    }

                    string mimeType = parts[0];
                    foreach (string extension in parts.Skip(1))
                    {
                        if (dictionary.ContainsKey(extension))
                        {
                            Console.WriteLine("Potential duplicate for extension: " + extension);
                            continue;
                        }
                        dictionary.Add(extension, mimeType);
                    }
                }
            }

            return dictionary;
        }

        private static void WriteDictionaryToCsharpMimeTypesClass(Dictionary<string, string> dictionary)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendFormat("namespace {0}", NameSpace).AppendLine();
            sb.AppendLine("{");
            sb.Append(Indent(1))
                .AppendFormat("class {0}", ClassName)
                .AppendLine();
            sb.Append(Indent(1))
                .AppendLine("{");
            sb.Append(Indent(2))
                .AppendFormat("public static Dictionary<string, string> {0} = new Dictionary<string, string>", DictionaryName)
                .AppendLine();
            sb.Append(Indent(2))
                .AppendLine("{");
            foreach (string extension in dictionary.Keys.OrderBy(k => k))
            {
                sb.Append(Indent(3))
                    .AppendFormat("{{ \"{0}\", \"{1}\" }},", extension, dictionary[extension])
                    .AppendLine();
            }
            sb.Append(Indent(2))
                .AppendLine("};");
            sb.Append(Indent(1))
                .AppendLine("}");
            sb.AppendLine("}");

            using (TextWriter writer = File.CreateText(FileName))
            {
                writer.Write(sb.ToString());
            }
        }

        private static string Indent(int count)
        {
            return new String(IndentChar, count);
        }
    }

    
}
