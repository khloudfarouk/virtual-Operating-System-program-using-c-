using System;
using static System.Console;
using System.IO;

namespace OS_Project_fat_file
{
    class Program
    {

        static Directory MoveToDir(string path)
        {
            string [] names=path.Split('\\');
            names[0] = names[0].ToUpper();
           // WriteLine(names[0].Length);
           // WriteLine(Program.root.dir_name.ToString().Length);
           // WriteLine(Program.root.dir_name.ToString());
            //for (int i = 0; i < names.Length; i++)
            //{
            //    WriteLine(names[i]);
            //}

            if (names[0] == new string(Program.root.dir_name))
            {
                Directory rot = Program.root;
                for (int i = 1; i < names.Length; i++)
                {                                                      //في الفنكشن التانية اللي زيها بتاعت الفايل نادى على قراءة الديركتوري لكن هنا لا ؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟؟  
                    int index = rot.searchDirectory(names[i]);
                    if (index != -1)
                    {
                        DirectoryEntry entry = rot.Dir_Files[index];
                        string l = new string(entry.dir_name);
                        Directory dir = new Directory(l, entry.dir_attr, entry.dir_FirstCluster[0], entry.dir_FileSize[0], rot);

                        rot = dir;
                    }
                    else
                    {
                        WriteLine("This path dosn't exist");
                    }
                }
                return rot;  
            }
            else
            {
                WriteLine("This path isn't valid (Wrong root name)");
                return null;
            }
        }

        static FileEntry MoveToFile(string path)
        {
            int fileindex = path.LastIndexOf('\\');   //  h:/vhgh/hjhjh/jk.txt
            string filename = path.Substring(fileindex+1);


            string path1 = path.Substring(0, fileindex);

            Directory lastDir=MoveToDir(path1);
            if (lastDir!=null)
            {
                lastDir.readDirectory();
                int index = lastDir.searchDirectory(filename);
                if (index!=-1)
                {
                    DirectoryEntry entry = lastDir.Dir_Files[index];
                    FileEntry file = new FileEntry(entry.dir_name.ToString(), entry.dir_attr, entry.dir_FirstCluster[0], entry.dir_FileSize[0], lastDir);
                    return file;
                }
                else
                {
                    WriteLine("This path dosn't exist");
                    
                }
            }
            return null;

        }
        
        static void Help(string s)
        {
            if (s == "help")
            {
                WriteLine("cd - Change the current default directory to . If the argument is not present, report the current directory. If the directory does not exist an appropriate error should be reported");
                WriteLine("cls - Clear the screen");
                WriteLine("dir - List the contents of directory");
                WriteLine("quit - Quit the shell");
                WriteLine("copy - Copies one or more files to another location");
                WriteLine("del - Deletes one or more files");
                WriteLine("help -Provides Help information for commands");
                WriteLine("md - Creates a directory");
                WriteLine("rd - Removes a directory");
                WriteLine("rename -  Renames a file");
                WriteLine("type - Displays the contents of a text file");
                WriteLine("import – import text file(s) from your computer");
                WriteLine("export – export text file(s) to your computer");
            }
            else
            {
                string m = "";
                for (int i = 4; i < s.Length; i++)
                {
                    if (s[i] != ' ')
                    {
                        m = s.Substring(i);
                        break;
                    }
                }

                if (m == "cd")
                {
                    WriteLine("Change the current default directory to a new one.");
                    WriteLine("If the argument is not present, report the current directory.");
                    WriteLine("If the directory does not exist an appropriate error should be reported");
                }

                else if (m == "cls")
                {
                    WriteLine("Clear the screen");
                }

                else if (m == "dir")
                {
                    WriteLine("Displays a list of files and subdirectories in a directory.");
                }

                else if (m == "rd")
                {
                    WriteLine("Removes a directory");
                }

                else if (m == "rename")
                {
                    WriteLine("Renames a file");
                }

                else if (m == "type")
                {
                    WriteLine("Displays the contents of a text file");
                }

                else if (m == "import")
                {
                    WriteLine("import text file(s) from your computer");
                }

                else if (m == "export")
                {
                    WriteLine("export text file(s) to your computer");
                }

                else if (m == "quit")
                {
                    WriteLine("Quit the shell");
                }

                else if (m == "copy")
                {
                    WriteLine("Copies one or more files to another location");
                }

                else if (m == "del")
                {
                    WriteLine("Deletes one or more files");
                }

                else
                {
                    WriteLine("There is a mistake in the command, check it");
                }
            }

        }

        static void quit(string s)
        {
            if (s == "quit")
            {
                System.Environment.Exit(0);
                // break;
            }
            else
            {
                WriteLine("There is i mistake in the command");
            }
        }

        static void cls(string s)
        {
            if (s=="cls")
            {
                Clear();
            }
            else
            {
                WriteLine("There is i mistake in the command");
            }
        }

        static void cd(string s)
        {
            if (s == "cd")
            {
                string char_to_Str = new string(Program.current.dir_name);
                WriteLine($"{char_to_Str}");
                //WriteLine(Environment.CurrentDirectory);
            }
            else if (s == "cd .")
            {
                string char_to_Str = new string(Program.current.dir_name);
                WriteLine($"{char_to_Str}");
            }
            else if (s == "cd ..")
            {
                if (Program.current.parent != null)
                {
                    Program.current = Program.current.parent;
                }
            }
            else
            {
                string m = "";
                for (int i = 2; i < s.Length; i++)
                {
                    if (s[i] == ' ')
                    {
                        m = s.Substring(i+1);
                      //  WriteLine(m);
                        break;
                    }
                }

                if (m.Contains('\\'))
                {
                    if (m[0]=='.'&& m[1] == '.')
                    {
                        string [] ss = m.Split("\\");
                        for (int i = 0; i < ss.Length; i++)
                        {
                            if (Program.current.parent!=null)
                            {
                                Program.current = Program.current.parent;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    Directory d=MoveToDir(m);
                    if (d!=null)
                    {
                        Program.current = d;
                    }
                }
                else
                {
                    int index = Program.current.searchDirectory(m);
                    if (index != -1)
                    {
                        string str = new string(Program.current.Dir_Files[index].dir_name);
                        Directory d = new Directory(str, Program.current.Dir_Files[index].dir_attr, Program.current.Dir_Files[index].dir_FirstCluster[0], Program.current.Dir_Files[index].dir_FirstCluster[0], Program.current);
                        Program.current = d;
                    }
                    else
                    {
                        WriteLine("This path dosn't exist!!");
                    }

                }
                

            }
        }

        static void dir(string s)
        {
            if (s == "dir")
            {
                string cha = new string(Program.current.dir_name);
                WriteLine($"Directory of {cha}");

                int numd = 0, numf = 0, fsize = 0, dFreeSize;
                if (Program.current!=root)
                {
                    WriteLine($"<DIR> . {cha}");
                    cha = new string(Program.current.parent.dir_name);
                    WriteLine($"<DIR> .. {cha}");
                }
               // Program.current.readDirectory();
                for (int i = 0; i < Program.current.Dir_Files.Count; i++)
                {
                    string char_to_Str = new string(Program.current.Dir_Files[i].dir_name);

                    if (Program.current.Dir_Files[i].dir_attr == 0x0)
                    {
                        WriteLine($"                  {char_to_Str}   {Program.current.Dir_Files[i].dir_FileSize[0]}");
                        numf++;
                        fsize += Program.current.Dir_Files[i].dir_FileSize[0];
                    }
                    else
                    {
                        WriteLine($"      < DIR >     {char_to_Str}   {Program.current.Dir_Files[i].dir_FileSize[0]}");
                        numd++;

                    }
                }
                dFreeSize = Mini_Fat.getFreeSpace();
                WriteLine($"   {numf} file(s)  {fsize} bytes");
                WriteLine($"   {numd} dir(s)   {dFreeSize} free bytes");
            }
            else
            {
                string m = "";
                for (int i = 2; i < s.Length; i++)
                {
                    if (s[i] == ' ')
                    {
                        m = s.Substring(i+1);
                       // WriteLine(m);
                        break;
                    }
                }
                //if (m.Contains("\\") && m.Contains(".txt"))
                //{
                //    for (int i = m.Length-1; i > 0; i--)
                //    {
                //        if (m[i]=='\\')
                //        {
                //            m = s.Substring(i + 1);
                //            // WriteLine(m);
                //            break;
                //        }
                //    }
                //    Directory d = MoveToDir(m);
                //    if (d != null)
                //    {
                //        string cha = new string(d.dir_name);
                //        WriteLine($"Directory of {cha}");

                //        int numd = 0, numf = 0, fsize = 0, dFreeSize;
                //        if (d.Dir_Files != null)
                //        {
                //            for (int i = 0; i < d.Dir_Files.Count; i++)
                //            {
                //                string char_to_Str = new string(d.Dir_Files[i].dir_name);

                //                if (d.Dir_Files[i].dir_attr == 0x0)
                //                {
                //                    WriteLine($"                  {d.Dir_Files[i].dir_FileSize[0]}    {char_to_Str}");
                //                    numf++;
                //                    fsize += d.Dir_Files[i].dir_FileSize[0];
                //                }
                //                else
                //                {
                //                    WriteLine($"      < DIR >     {char_to_Str}   {d.Dir_Files[i].dir_FileSize[0]}");
                //                    numd++;

                //                }
                //            }
                //        }
                //        else
                //        {
                //            WriteLine($"{cha} is empty");
                //        }

                //    }
                //}
                if (m.Contains("\\"))
                {
                    Directory d = MoveToDir(m);
                    if (d != null)
                    {
                       // d.readDirectory();
                        string cha = new string(d.dir_name);
                        WriteLine($"Directory of {cha}");

                        int numd = 0, numf = 0, fsize = 0, dFreeSize;

                        if (d.Dir_Files!=null)
                        {
                            for (int i = 0; i < d.Dir_Files.Count; i++)
                            {
                                string char_to_Str = new string(d.Dir_Files[i].dir_name);

                                if (d.Dir_Files[i].dir_attr == 0x0)
                                {
                                    WriteLine($"                  {d.Dir_Files[i].dir_FileSize[0]}    {char_to_Str}");
                                    numf++;
                                    fsize += d.Dir_Files[i].dir_FileSize[0];
                                }
                                else
                                {
                                    WriteLine($"      < DIR >     {char_to_Str}   {d.Dir_Files[i].dir_FileSize[0]}");
                                    numd++;

                                }
                            }
                        }
                        else
                        {
                            WriteLine($"{cha} is empty");
                        }
                       
                    }
                }
                else if (m.Contains(".txt"))
                {
                    int index = Program.current.searchDirectory(m);
                    if (index != -1)
                    {
                        //Program.current.readDirectory();
                        string cha = new string(Program.current.dir_name);
                        WriteLine($"Directory of {cha}");

                        int numd = 0, numf = 0, fsize = 0, dFreeSize;
                        for (int i = 0; i < Program.current.Dir_Files.Count; i++)
                        {
                            string char_to_Str = new string(Program.current.Dir_Files[i].dir_name);

                            if (Program.current.Dir_Files[i].dir_attr == 0x0)
                            {
                                WriteLine($"                  {Program.current.Dir_Files[i].dir_FileSize[0]}    {char_to_Str}");
                                numf++;
                                fsize += Program.current.Dir_Files[i].dir_FileSize[0];
                            }
                            else
                            {
                                WriteLine($"      < DIR >     {char_to_Str}   {Program.current.Dir_Files[i].dir_FileSize[0]}");
                                numd++;

                            }
                        }
                    }
                    else
                    {
                        WriteLine("this file dosen't exist!");
                    }
                }
            }

        }

        static void md(string s)
        {
            string m = "";
            for (int i = 2; i < s.Length; i++)
            {
                if (s[i] == ' ')
                {
                    m = s.Substring(i + 1);
                    // WriteLine(m);
                    break;
                }
            }
            if (s == "md")
            {
                WriteLine("There is a mistake in the command");
            }
            else
            {
                if (!m.Contains("\\"))
                {
                   
                    if (Program.current.searchDirectory(m) == -1)
                    {
                        DirectoryEntry newDir = new DirectoryEntry(m, 0x10, 0, 0);
                        if (Program.current.canAddEntry(newDir) == true)
                        {
                            Program.current.AddEntry(newDir);
                        }
                        else
                        {
                            WriteLine("There is no space");
                        }
                    }
                    else
                    {
                        WriteLine("This directory exists");
                    }

                }
                //else
                //{
                //    Directory d = MoveToDir(m);
                //    if (d != null)
                //    {
                //        if (d.searchDirectory(m) == -1)
                //        {
                //            for (int i = m.Length - 1; i > 0; i--)
                //            {
                //                if (m[i] == '\\')
                //                {
                //                    m = s.Substring(i + 1);
                //                    // WriteLine(m);
                //                    break;
                //                }
                //            }
                //            DirectoryEntry newDir = new DirectoryEntry(m , 0x10, 0, 0);
                //            if (d.canAddEntry(newDir) == true)
                //            {
                //                d.AddEntry(newDir);
                //            }
                //            else
                //            {
                //                WriteLine("There is no space");
                //            }
                //        }
                //        else
                //        {
                //            WriteLine("This directory exists");
                //        }
                //    }

                //}
            }

        }

        static void rd(string s)
        {
            string m = "";
            if (s == "rd")
            {
                WriteLine("There is a mistake in the command");
            }
            else
            {
                for (int i = 2; i < s.Length; i++)
                {
                    if (s[i] != ' ')
                    {
                        m = s.Substring(i);
                        break;
                    }
                }
                int index = Program.current.searchDirectory(m);
                if (index != -1)
                {
                    string str = new string(Program.current.Dir_Files[index].dir_name);
                    Directory d = new Directory(str, Program.current.Dir_Files[index].dir_attr, Program.current.Dir_Files[index].dir_FirstCluster[0], Program.current.Dir_Files[index].dir_FirstCluster[0], Program.current);
                    //if (d.dir_FirstCluster[0] == 0)
                    //{
                    WriteLine("If you really want to delete this file enter yes");
                    string confirm = ReadLine();
                    if (confirm == "yes")
                    {
                        //Program.current.removeEntry(d);
                        d.deleteDireactory();
                    }
                    //}
                    //else
                    //{
                    //    WriteLine("Can't delete a full directory");
                    //}


                }
                else
                {
                    WriteLine("This directory dosn't exist");
                }

            }
        }

        static void import(string s)    
        {
            string m = "";
            if (s == "import")
            {
                WriteLine("There is a mistake in the command");
            }
            else
            {
                for (int i = 6; i < s.Length; i++)
                {
                    if (s[i] != ' ')
                    {
                        m = s.Substring(i);
                        break;
                    }
                }

                if (File.Exists(m))
                {
                    int name_start = m.LastIndexOf("\\");

                    string content = File.ReadAllText(m);
                    int size = content.Length;

                    string name;
                    name = m.Substring(name_start + 1);

                    int index = Program.current.searchDirectory(name);
                    int frist;
                    if (size > 0)
                    {
                        frist = Mini_Fat.GetEmptyCluster();
                    }
                    else
                    {
                        frist = 0;
                    }
                    if (index == -1)
                    {
                        FileEntry file = new FileEntry(name, 0x0, frist, size, Program.current, content);
                        file.writeFileContent();
                       // DirectoryEntry dir = new DirectoryEntry(name, 0x0, frist, size);
                        Program.current.Dir_Files.Add(file.getfileEntry());
                        Program.current.WriteDirectory();



                    }
                    else
                    {
                        WriteLine("This file already exists");
                    }

                }
                else
                {
                    WriteLine("This file dosen't exist");
                }
            }
        }

        static void type(string s)
        {
            string m = "";
            if (s == "type")
            {
                WriteLine("There is a mistake in the command");
            }
            else
            {
                for (int i = 4; i < s.Length; i++)
                {
                    if (s[i] != ' ')
                    {
                        m = s.Substring(i);
                        break;
                    }
                }

                int index = Program.current.searchDirectory(m);
                if (index != -1)
                {
                    int first = Program.current.Dir_Files[index].dir_FirstCluster[0];
                    int size = Program.current.Dir_Files[index].dir_FileSize[0];
                    string content = "";
                    FileEntry file = new FileEntry(m, 0x0, first, size, Program.current, content);
                    
                    WriteLine(m);

                     //file.readFileContent();
                    file.printContent();

                }
                else
                {
                    WriteLine("This file dosn't exist");
                }
            }
        }

        static void export(string s)
        {
            string m = "";
            if (s == "export")
            {
                WriteLine("There is a mistake in the command");
            }
            else
            {
                for (int i = 6; i < s.Length; i++)
                {
                    if (s[i] != ' ')
                    {
                        m = s.Substring(i);
                        break;
                    }
                }
                string[] ss = m.Split(" ");
                string source = ss[0];
                string distination = ss[1];

                int index = Program.current.searchDirectory(source);
                if (index != -1)
                {
                    if (System.IO.Directory.Exists(distination))
                    {
                        int first = Program.current.Dir_Files[index].dir_FirstCluster[0];
                        int size = Program.current.Dir_Files[index].dir_FileSize[0];
                        string content = "";
                        FileEntry file = new FileEntry(m, 0x0, first, size, Program.current, content);
                        // file.readFileContent();

                        StreamWriter strem = new StreamWriter((distination + "\\" + source));
                        strem.Write(file.content);
                        strem.Flush();
                        strem.Close();

                    }
                    else
                    {
                        WriteLine("This path specified dosn't exist in the Computer Disk");
                    }
                }
                else
                {
                    WriteLine("This file specified dosn't exist in the Vertual Disk");
                }
            }
        }

        static void rename(string s)
        {
            string m = "";
            if (s == "rename")
            {
                WriteLine("There is a mistake in the command");
            }
            else
            {
                for (int i = 6; i < s.Length; i++)
                {
                    if (s[i] != ' ')
                    {
                        m = s.Substring(i);
                        break;
                    }
                }

                string[] ss = m.Split(" ");
                string oldName = ss[0];
                string newName = ss[1];

                int index = Program.current.searchDirectory(oldName);
                if (index != -1)
                {
                    if (Program.current.searchDirectory(newName) == -1)
                    {
                        Program.current.Dir_Files[index].dir_name = newName.ToCharArray();

                        //DirectoryEntry dir = Program.current.Dir_Files[index];
                        //    dir.dir_name = newName.ToCharArray();
                        //Program.current.removeEntry(dir);
                        //Program.current.AddEntry(dir);
                        Program.current.WriteDirectory();
                    }
                    else
                    {
                        WriteLine("This new name dose exist in the Vertual Disk");
                    }
                }
                else
                {
                    WriteLine("This file specified dosn't exist in the Vertual Disk");
                }
            }
        }

        public static void del(string s)
        {
            string m = "";
            if (s == "del")
            {
                WriteLine("There is a mistake in the command");
            }
            else
            {
                for (int i = 3; i < s.Length; i++)
                {
                    if (s[i] != ' ')
                    {
                        m = s.Substring(i);
                        break;
                    }
                }

                int index = Program.current.searchDirectory(m);
                if (index != -1)
                {
                    if (Program.current.Dir_Files[index].dir_attr == 0x0)//file
                    {
                        
                        //string content = null;
                        int first = Program.current.Dir_Files[index].dir_FirstCluster[0];
                        int size = Program.current.Dir_Files[index].dir_FileSize[0];
                        DirectoryEntry file = new DirectoryEntry(m, 0x0, first, size);
                        Program.current.removeEntry(file);

                        //file.deleteFile();//??????????????????????????????????????????????????????????????????????????????????????????????????????

                    }

                }
                else
                {
                    Console.WriteLine("This file dosen't exist");
                }
            }
        }
      
        public static void copy(string s)
        {
            string m = "";
            if (s == "copy")
            {
                WriteLine("There is a mistake in the command");
            }
            else
            {
                for (int i = 4; i < s.Length; i++)
                {
                    if (s[i] != ' ')
                    {
                        m = s.Substring(i);
                        break;
                    }
                }

                string[] ss = m.Split(" ");
                string source = ss[0];
                string distination = ss[1];


                int index = Program.current.searchDirectory(source);
                if (index != -1)
                {
                    int start_index = distination.LastIndexOf("\\");
                    string name = distination.Substring(start_index + 1);
                    int index_destenation = Program.current.searchDirectory(name);

                    if (index_destenation == -1)
                    {
                        //DirectoryEntry dir = Program.current.Dir_Files[index];
                        //dir.dir_name = name.ToCharArray();
                        //Program.current.removeEntry(dir);
                        //Program.current.AddEntry(dir);

                        if (distination != Program.current.dir_name.ToString())
                        {
                            int f_c = Program.current.Dir_Files[index].dir_FirstCluster[0];
                            int f_size = Program.current.Dir_Files[index].dir_FileSize[0];
                            DirectoryEntry entry = new DirectoryEntry(source, 0x0, f_c, f_size);
                            Directory dir = new Directory(distination, 0x10, 0,0, Program.current.parent);
                            dir.Dir_Files.Add(entry);
                           

                        }
                        else Console.WriteLine("This distination specified dosn't exist in the Vertual Disk");
                    }
                }
                else
                {
                    WriteLine("This source specified dosn't exist in the Vertual Disk");
                }

              
            }
        }



        static public Directory current = new Directory();                                                            //???????????????????اعملي كونستركتور
       // static public FileEntry currentFile = new FileEntry();
        static public string currentPath = "fat_file.txt";
        static public Directory root = new Directory("H:", 0x10, 5, 0);
        static void Main(string[] args)
        {
            string s;
            string[] ss;

            VertualDisk.initialize(currentPath);
            //Mini_Fat.WriteFat();

           //Directory root = new Directory("H:",0x10,5,0);       
           // Mini_Fat.PrepareFat();
            // Mini_Fat.print();

            root.readDirectory();

            Program.current = root;

          
           

            do
            {
                // WriteLine("Enter your command");
                string char_to_Str =new string(Program.current.dir_name);
                Write($"{char_to_Str}\\>>>>>");
                s = ReadLine();
                if (s != "")
                {
                    if (s[0] == ' ')
                    {
                        for (int i = 0; i < s.Length; i++)
                        {
                            if (s[i] != ' ')
                            {
                                s = s.Substring(i);
                                break;
                            }
                        }
                    }

                    if (s[s.Length - 1] == ' ')
                    {
                        for (int i = (s.Length - 1); i >= 0; i--)
                        {
                            if (s[i] != ' ')
                            {
                                s = s.Substring(0, (i + 1));
                                break;
                            }
                        }
                    }

                    s = s.ToLower();

                    ss = s.Split(' ');

                    if (ss[0] == "quit")
                    {
                        quit(s);
                    }

                    else if (ss[0] == "cls")
                    {
                        cls(s);
                    }

                    else if (ss[0] == "help")
                    {
                        Help(s);
                    }

                    else if (ss[0] == "cd")
                    {
                        cd(s);
                    }

                    else if (ss[0] == "dir")
                    {
                        dir(s);
                    }

                    else if (ss[0] == "rd")
                    {
                        rd(s);
                    }
                    
                    else if (ss[0] == "md")
                    {
                        md(s);
                    }

                    else if (ss[0] == "import")
                    {
                        import(s);
                    }

                    else if (ss[0] == "type")
                    {
                        type(s);
                    }

                    else if (ss[0] == "export")
                    {
                        export(s);
                    }

                    else if (ss[0] == "rename")
                    {
                        rename(s);
                    } 
                    
                    else if (ss[0] == "del")
                    {
                        del(s);
                    }

                    else if (ss[0] == "copy")
                    {
                        copy(s);
                    }

                    else
                    {
                        WriteLine("There is a mistake in the command, check it");
                    }
                }
            } while (true);

     
           
            ReadKey();
        }
    }
}

