// Developed by Riketta https://github.com/Riketta/ rowneg@bk.ru


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InternetUrok.ru_DMLinks
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                try
                {
                    Console.WriteLine("Выберите разрешение видео:");
                    Console.WriteLine("1. 270"); // 2 (3)
                    Console.WriteLine("2. 360"); // 3 (4)
                    Console.WriteLine("3. 480"); // 4 (5)
                    int Mode = int.Parse(Console.ReadLine()) + 1;
                    Console.Clear();

                    StreamReader Reader = new StreamReader(args[0]);
                    StreamWriter Writer = new StreamWriter(args[0].Remove(args[0].Length - 4) + " DM.txt");
                    string A;
                    while ((A = Reader.ReadLine()) != null)
                    {
                        string[] AA = A.Split('|');

                        string Out = "";
                        // 2 3 4 | 2 1 0
                        for (int i = 0; i < 4 - Mode && Out == ""; i++)
                            Out = AA[Mode + i].Trim();

                        if (Out != "")
                        {
                            Console.WriteLine(Out);
                            Writer.WriteLine(Out);
                        }
                    }
                    Reader.Close();
                    Writer.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Console.ReadLine();
            }
        }
    }
}
