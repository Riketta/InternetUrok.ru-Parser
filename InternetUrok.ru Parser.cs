// Developed by Riketta https://github.com/Riketta/ rowneg@bk.ru


using System;
using System.Collections.Generic;
using System.Text;

using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace InternetUrok.ru_Parser
{
    class Program
    {

        static string PageReader(string URI)
        {
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(URI);
            Request.AllowAutoRedirect = false;
            Request.Timeout = 20000; // 20 сек.
            Request.Method = "GET";
            Request.Referer = "http://google.com";
            Request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            Stream Stream = Response.GetResponseStream();
            using (StreamReader Reader = new StreamReader(Stream, Encoding.GetEncoding(Response.CharacterSet)))
            {
                return Reader.ReadToEnd();
            }
        }

        static void Main()
        {
            Console.Title = "InternetUrok.ru Parser";
            Console.WriteLine("InternetUrok.ru");
            string Base = "http://interneturok.ru - автор Riketta (rowneg@bk.ru) - 2014";

            Console.WriteLine("Введите конец ссылки после \"http://interneturok.ru\" включая \"/\"\n(например: \"/ru/school/physics/10-klass\"):");
            string URL_End = Console.ReadLine();
            string URI = Base + URL_End;

            HtmlNodeCollection Nodes = null;
            string Title = "";
            try
            {
                HtmlDocument HTML = new HtmlDocument();
                HTML.LoadHtml(PageReader(URI));

                Title = HTML.DocumentNode.SelectSingleNode("//title").InnerText.Replace(" - InternetUrok.ru", "");

                Nodes = HTML.DocumentNode.SelectNodes("//a[@rel='canonical']");
            }
            catch (Exception ex)
            {
                ExLog(ex);
            }


            int StartFrom = 0;
            Console.WriteLine("Желаете ли докачать базу (если нет - ничего не вводите)?\nВведите последний номер базы:");
            string SStartFrom = Console.ReadLine();
            if (!string.IsNullOrEmpty(SStartFrom)) // Если ввели цифру
                StartFrom = int.Parse(SStartFrom);
            else // Если не ввели - удаляем файл
                File.Delete(Title + ".txt");

            for (int i = StartFrom; i < Nodes.Count; i++)
            {
                bool IsComplete = false;
                while (!IsComplete)
                try
                {
                    string FullURI = Base + Nodes[i].Attributes["href"].Value; //.Replace("?seconds=0", "");
                    string Page = PageReader(FullURI);

                    Regex Regex = new Regex("<script> var lessonID = \"(\\w+)\" </script>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match Match = Regex.Match(Page);
                    if (Match.Success)
                    {
                        string XML_Sorce = PageReader("http://interneturok.ru/lessons/" + Match.Groups[1].ToString() + ".xml");
                        XmlDocument XML = new XmlDocument();
                        XML.LoadXml(XML_Sorce);

                        using (StreamWriter Writer = new StreamWriter(Title + ".txt", true))
                        {
                            string Name = XML.SelectSingleNode("/VIDEOCATEGORY").Attributes["Name"].Value;

                            string H270 = "";
                            try
                            {
                                H270 = XML.SelectSingleNode("//quality[@value='270']").Attributes["file"].Value;
                            }
                            catch { }

                            string H360 = "";
                            try
                            {
                                H360 = XML.SelectSingleNode("//quality[@value='360']").Attributes["file"].Value;
                            }
                            catch { }

                            string H480 = "";
                            try
                            {
                                H480 = XML.SelectSingleNode("//quality[@value='480']").Attributes["file"].Value;
                            }
                            catch { }

                            string Line = i + 1 + "/" + Nodes.Count + " | " + Name + " | " + H270 + " | " + H360 + " | " + H480;

                            Console.WriteLine(Line);
                            Writer.WriteLine(Line);
                            IsComplete = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExLog(ex);
                }
            }


            Console.WriteLine("\nЗагружено!");
            Console.ReadLine();
        }

        static void ExLog(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n================\n" + ex + "================\n");
            Console.ResetColor();
        }
    }
}