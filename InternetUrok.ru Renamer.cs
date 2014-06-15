// Developed by Riketta https://github.com/Riketta/ rowneg@bk.ru


using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace InternetUrok.ru_Renamer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                return;

            try
            {
                string Path = Directory.GetCurrentDirectory() + "\\";
                StreamReader Reader = new StreamReader(args[0]);
                string A;
                for (int i = 1; ((A = Reader.ReadLine()) != null); i++)
                {
                    string[] AA = A.Split('|');
                    string Name = Path + i + ". " + AA[1].Trim().Replace("<sup>", "^").Replace("</sup>", "").Replace(":", " деленное на ").Replace("/", " деленное на ").Replace('*', 'x').Replace("?", "").Replace('\"', '\'') + ".mp4";

                    for (int j = 2; j <= 4; j++)
                    {
                        string[] AAA = AA[j].Replace('/', '\\').Split('\\');
                        string FName = Path + AAA[AAA.Length - 1].Trim();
                        if (File.Exists(FName))
                            try
                            {
                                File.Move(Path + AAA[AAA.Length - 1].Trim(), Name);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                Console.WriteLine(Name);
                            }
                    }
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Все переименовано");
            Console.ReadLine();
        }
    }
}
