using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;


namespace MainFile
{
    class Program
    {
        static void Main(string[] args)
        {
            var ans = "";       // Any command you give will be stored inside this variable for future references.
            var curpath = Directory.GetCurrentDirectory(); // Current folder.
            var loop = true;    // Variable that validates the loop. If the user types "quit" it'll set to false and close the program.
            
            void WriteColor(String text = "", 
                            ConsoleColor fColor = ConsoleColor.Gray, 
                            ConsoleColor bColor = ConsoleColor.Black,
                            Boolean maintain = false)
            {
                /*
                This function works the same as the Console.Write() command, but you can change both background and foreground colors.
                Notice that it will always go back to normal after the Console.Write() command if the maintain argument is set to false.
                You can also change the read argument to true, turning the Console.Write() command into a Console.ReadLine() command.
                */
                
                Console.ForegroundColor = fColor;
                Console.BackgroundColor = bColor;
                Console.Write(text);
                if (!maintain)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            
            var help = new SortedDictionary<string, string> {
                
                /*
                This dictionary contains descriptions regarding every command in the program.
                
                You need to make sure the key is the same as in the cmdict, since it will search the key in the help dictionary
                using keys from cmdict dictionary as a reference.
                */
                
                {"cd", "Accesses the given location."},
                {"del", "Deletes the given file or directory."},
                {"help", "Opens up documentation regarding every command."},
                {"listf", "Shows a list containing every folder and directory present in the current location."},
                {"newdir", "Creates a new directory in the current location."},
                {"newf", "Creates a new file in the current location"},
                {"quit", "Quits the program."},
                
            };
            
            
            Console.Clear();
            var cmdict = new SortedDictionary<string, Action>
            {
                /*
                ** COMMAND DICTIONARY **
                
                Every function in the program is stored here.
                Its key is assigned a string value which is the command you want the user to enter.
                Its value receives an action which will execute any commands you have in mind.
                Every action must have a 'hlp' variable which receives a string with the documentation regarding your command.
                */
                
                {"quit", () => {
                    loop = false;
                }},
                
                {"help", () => {
                }},
                
                {"cd", () => {
                    WriteColor("Enter directory: ", fColor: ConsoleColor.Blue);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var dir = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    
                    if (dir == "..")
                    {
                        var dirs = curpath.Split(@"\");
                        var len = dirs.Length-2;
                        
                        curpath = "";
                        foreach(var element in dirs)
                        {
                            if (element != dirs[len])
                            {
                                curpath += String.Format(@"{0}\", element);
                            }
                        }
                        if (curpath[curpath.Length-1] == curpath[curpath.Length-2])
                        {
                            StringBuilder npath = new StringBuilder(curpath);
                            npath.Length -= 1;
                            curpath = npath.ToString();
                        }
                        Console.Clear();
                    }
                    else
                    {
                        if (Directory.Exists(curpath+dir) && (dir != ""))
                        {
                            Console.Clear();
                            curpath = String.Format(@"{0}{1}\", curpath, dir);
                        }
                        else
                        {
                            Console.Clear();
                            WriteColor("Could not find such path.\n\n", fColor:ConsoleColor.Red);
                        }
                    }
                }},
                {"newf", () => {
                    WriteColor("Enter file name: ", fColor:ConsoleColor.Blue);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var filename = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Clear();
                    try
                    {
                        var path = curpath+filename;
                        
                        FileStream nf = File.Create(curpath+filename);
                        nf.Close();
                        WriteColor(String.Format("Successfully created '{0}'\n\n", filename), fColor:ConsoleColor.Green);
                    }
                    catch
                    {
                        WriteColor(String.Format("Something went wrong when creating '{0}'.\n\n", filename), fColor:ConsoleColor.Red);
                    }
                }},
                {"listf", () => {
                    Console.Clear();
                    Console.Write("\n");
                    
                    var dirs = Directory.GetDirectories(curpath);
                    var files = Directory.GetFiles(curpath);
                    
                    Console.Write("    ");
                    WriteColor(" FILES ", fColor:ConsoleColor.Black, bColor:ConsoleColor.Magenta);
                    Console.Write(@" \\ ");
                    WriteColor(" DIRECTORIES \n\n", fColor:ConsoleColor.Black, bColor:ConsoleColor.Cyan);
                    
                    foreach(var element in files)
                    {
                        var tempel = element.Split(@"\");
                        
                        WriteColor(String.Format("    {0}\n", tempel[tempel.Length-1]), fColor:ConsoleColor.Magenta);
                    }
                    foreach(var element in dirs)
                    {
                        var tempel = element.Split(@"\");
                        
                        WriteColor(String.Format("    {0}\n", tempel[tempel.Length-1]), fColor:ConsoleColor.Cyan);
                    }
                    Console.Write("\n");
                }},
                {"newdir", () => {
                    WriteColor("Enter the directory name: ", fColor:ConsoleColor.Blue);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var dir = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    
                    try
                    {
                        Directory.CreateDirectory(curpath+dir);
                        Console.Clear();
                        WriteColor(String.Format("Successfully created directory '{0}'.\n\n", dir), fColor:ConsoleColor.Green);
                    }
                    catch
                    {
                        Console.Clear();
                        WriteColor("Something went wrong when creating your directory.\n\n", fColor:ConsoleColor.Red);
                    }
                }},
                {"del", () => {
                    
                    WriteColor("Enter file to delete: ", fColor:ConsoleColor.Blue);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var file = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (Directory.Exists(curpath+file) || File.Exists(curpath+file))
                    {
                        while (true)
                        {
                            WriteColor(String.Format("Are you sure you want to delete '{0}'? (Y/N)\n", file), fColor:ConsoleColor.Red);
                            
                            var ans = Console.ReadKey().Key.ToString();
                            
                            if (ans == "Y")
                            {
                                if (Directory.Exists(curpath+file))
                                {
                                    Directory.Delete(curpath+file, true);
                                    Console.Clear();
                                    WriteColor("Successfully deleted given directory.\n\n", fColor:ConsoleColor.Green);
                                }
                                else if (File.Exists(curpath+file))
                                {
                                    File.Delete(curpath+file);
                                    Console.Clear();
                                    WriteColor("Successfully deleted given file.\n\n", fColor:ConsoleColor.Green);
                                }
                                break;
                            }
                            else if (ans == "N")
                            {
                                Console.Clear();
                                break;
                            }
                            Console.Clear();
                            WriteColor("Given answer not recognized.\n\n", fColor:ConsoleColor.Red);
                        }
                    }
                    else
                    {
                        WriteColor("Could not find given directory.", fColor:ConsoleColor.Red);
                    }
                }},
                {"open", () => {
                    try
                    {
                        WriteColor("Enter file path or name with extension: ", fColor:ConsoleColor.Blue);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        var toopen = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.Gray;
                        if (toopen == null) { toopen = ""; }
                        
                        var pcss = new Process();
                        pcss.StartInfo = new ProcessStartInfo(curpath+toopen)
                        {
                            UseShellExecute=true
                        };
                        pcss.Start();
                        pcss.Close();
                        Console.Clear();
                        WriteColor(String.Format("Successfully opened '{0}'.\n\n", toopen), fColor:ConsoleColor.Green);
                    }
                    catch
                    {
                        WriteColor("Something went wrong when opening your file.\n\n", fColor:ConsoleColor.Red);
                    }
                }},
            };
            
            
            
            
            while (loop) // MAIN LOOP
            {
                WriteColor(String.Format("{0}\n\n", curpath), fColor:ConsoleColor.Green);
                WriteColor("$ ", fColor: ConsoleColor.Blue);
                Console.ForegroundColor = ConsoleColor.Yellow;
                ans = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Gray;
                
                if (ans == null) { ans = ""; } else { ans = ans.ToLower(); }
                
                try
                {
                    /*
                    
                    HELP COMMAND
                    
                    */
                    
                    if (ans == "help")
                    {
                        /*
                        Making a function outside the Main function requires an object reference,
                        which is not possible since changing from "var" to "Dictionary<>" results in possible null references.
                        That is the reason why the help command is not listed on the dictionary.
                        */
                        
                        var keys = cmdict.Keys;
                        var max = 0;
                        
                        foreach(var element in keys)
                        {
                            if (element.Length > max)
                            {
                                max = element.Length;
                            }
                        }
                        
                        Console.Clear();
                        WriteColor("\n COMMAND / FUNCTION \n\n", fColor:ConsoleColor.Black, bColor:ConsoleColor.Blue);
                        foreach(var position in cmdict)
                        {
                            string helpstring = "{0," + Convert.ToString(max) + "}";
                            WriteColor(String.Format(helpstring, position.Key), fColor: ConsoleColor.Blue);
                            try
                            {
                                Console.Write(" ## {0}\n", help[Convert.ToString(position.Key)]);
                            }
                            catch
                            {
                                Console.Write(" ## Description unavailable\n");
                            }
                        }
                        Console.Write("\n");
                    }
                    else
                    {
                        cmdict[ans]();
                    }
                }
                catch
                {
                    Console.Clear();
                    WriteColor(" ** ERROR ** ", bColor: ConsoleColor.Red, fColor: ConsoleColor.Black);
                    WriteColor(String.Format(" '{0}' is not recognized as a command within the program.\n\n", ans), fColor: ConsoleColor.Red);
                }
            }
            Console.Clear();
        }
    }
}