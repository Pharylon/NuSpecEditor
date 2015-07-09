using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NuSpecEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || args.Any(x => new[] { "--help", "-h", "-help" }.Contains(x)))
                {
                    Console.WriteLine("Valid arguments are -v for Version and -n for Notes or -r for Recursive, or -h to see this again");
                    //args = new[] { "-r", "-v 2.0", "-n", "Version 2.0/Catapult" };
                }
                else
                {
                    var arguments = GetArguments(args);
                    var myDir = Directory.GetCurrentDirectory();
                    var myFiles = Directory.GetFiles(myDir, "*.nuspec", arguments.SeachOption);
                    if (myFiles.Length > 0)
                    {
                        foreach (var myFile in myFiles)
                        {
                            var xml = XDocument.Load(myFile);
                            xml.Declaration.Encoding = "utf-8";
                            if (arguments.Version != null)
                            {
                                xml.Root.Descendants("version").First().Value = arguments.Version;
                            }
                            if (arguments.Notes != null)
                            {
                                xml.Root.Descendants("releaseNotes").First().Value = arguments.Notes;
                            }
                            xml.Save(myFile);
                        }
                        Console.WriteLine("Updated {0} Nuget files.", myFiles.Length);
                    }
                    else
                    {
                        Console.WriteLine("Could not find any nuget files to update. Use the -r switch to search recursively.");
                    }

                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static Arguments GetArguments(string[] args)
        {
            var validArgs = new[] { "-v", "-n", "-r", "-h" };
            foreach (var s in args)
            {
                if (s.StartsWith("-") && !validArgs.Contains(s))
                {
                    throw new Exception(s + " is not a valid argument");
                }
            }
            var a = new Arguments();
            a.Version = GetArgValue(args, "-v");
            a.Notes = GetArgValue(args, "-n");
            a.SeachOption = GetArg(args, "-r") ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return a;
        }

        private static string GetArgValue(string[] args, string argSwitch)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == argSwitch && args.Length > i + 1)
                {
                    return args[i + 1];
                }   
            }
            return null;
        }

        private static bool GetArg(string[] args, string argSwitch)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToLower() == argSwitch)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool ArgIsValid(string a)
        {
            return !String.IsNullOrWhiteSpace(a) && !a.StartsWith("-");
        }

        class Arguments
        {
            internal Arguments()
            {
                SeachOption = SearchOption.TopDirectoryOnly;
            }

            public string Version { get; set; }
            public string Notes { get; set; }
            public SearchOption SeachOption { get; set; }
        }
    }
}
