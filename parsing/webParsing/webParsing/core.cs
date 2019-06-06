using System;
using System.Net;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web;
namespace webParsing
{
    public class core
    {
        string MakeLink(int year, int month, int page)
        {
            return $"http://www.horangi.kr/festival.asp?nowyear={year}&nowmonth={month}&nowpage={page}";
        }

        string WebDownload(int year, int month, int page)
        {
            return new WebClient().DownloadString(MakeLink(year,month, page));
        }

        public void Convert()
        {

            string data = new StreamReader("/Users/ak/Desktop/result.json").ReadToEnd();
            JArray a = JArray.Parse(data);
            // 한국 일본 중국 러시아 미국 캐나다
            List<string> coun = new List<string> { "한국", "일본", "중국", "러시아", "미국", "캐나다" };
            Random rand = new Random();

            JsonWriter writer = new JsonTextWriter(new StreamWriter("/Users/ak/Desktop/conv.json"));

            writer.Formatting = Formatting.None;

            writer.WriteStartArray();
            foreach (var e in a.Children())
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Title");
                writer.WriteValue(e["Title"]);

                writer.WritePropertyName("Image");
                writer.WriteValue(e["Image"]);

                writer.WritePropertyName("EventTime");
                writer.WriteValue(e["EventTime"]);

                writer.WritePropertyName("Homapage");
                writer.WriteValue(e["Homapage"]);

                writer.WritePropertyName("Location");
                if (e["Location"] != null)
                {
                    writer.WriteValue(HttpUtility.HtmlDecode(e["Location"].ToString()));
                }
                else
                {
                    writer.WriteValue("");
                }

                writer.WritePropertyName("Position");
                if (e["Position"] != null)
                {
                    writer.WriteValue(HttpUtility.HtmlDecode(e["Position"].ToString()));
                }
                else
                {
                    writer.WriteValue("");
                }
                writer.WritePropertyName("PhoneNumber");
                writer.WriteValue(e["PhoneNumber"]);

                writer.WritePropertyName("StartDay");
                writer.WriteValue(e["StartDay"]);

                writer.WritePropertyName("EndDay");
                writer.WriteValue(e["EndDay"]);

                writer.WritePropertyName("Description");
                if (e["Description"] != null)
                {
                    writer.WriteValue(HttpUtility.HtmlDecode(e["Description"].ToString()));
                }
                else
                {
                    writer.WriteValue("");
                }


                writer.WritePropertyName("Country");
                writer.WriteValue(coun[rand.Next(0, 6) % 6]);

                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.Close();

        }

        void Writer(List<Event> list)
        {

            JsonWriter writer = new JsonTextWriter(new StreamWriter("/Users/ak/Desktop/result.json"));

            writer.Formatting = Formatting.Indented;

            writer.WriteStartArray();
            foreach (var e in list)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(nameof(e.Title));
                writer.WriteValue(e.Title);

                writer.WritePropertyName(nameof(e.Image));
                writer.WriteValue(e.Image);

                writer.WritePropertyName(nameof(e.EventTime));
                writer.WriteValue(e.EventTime);

                writer.WritePropertyName(nameof(e.Homapage));
                writer.WriteValue(e.Homapage);

                writer.WritePropertyName(nameof(e.Location));
                writer.WriteValue(e.Location);

                writer.WritePropertyName(nameof(e.Position));
                writer.WriteValue(e.Position);

                writer.WritePropertyName(nameof(e.PhoneNumber));
                writer.WriteValue(e.PhoneNumber);

                writer.WritePropertyName(nameof(e.StartDay));
                writer.WriteValue(e.StartDay);

                writer.WritePropertyName(nameof(e.EndDay));
                writer.WriteValue(e.EndDay);


                writer.WriteEndObject();
            }
            writer.WriteEndArray();

            writer.Close();
        }

        int GetFestCount(string link)
        {
            var p = new WebClient().DownloadString(link);
            var count = Regex.Split(p, "festsu = ")[1].Split(';')[0];

            return (int)Math.Ceiling(double.Parse(count) / 10.0);
        }

        public List<Event> Run(int years, int yeare)
        {
            var result = new List<Event>();

            for (int year = years; year <= 2019; year++)
            {
                for (int m = 1; m <= 12; m++)
                {
                    int p = GetFestCount(MakeLink(year, m, 1));
                    for (int i = 1; i <= p; i++)
                    {
                        foreach (var link in MainPageLoad(WebDownload(year, m, i)))
                        {
                            result.Add(getPage(link));
                        }
                        Console.WriteLine($"{i} page");
                    }
                    Console.WriteLine($"{year} | {m}");
                }
            }

            Writer(result);
            return result;
        }

        List<string> MainPageLoad(string page)
        {
            var data = new List<string>();

            var result = Regex.Split(page, "festival_info");

            for (int i = 1; i < result.Length; i++)
            {
                data.Add($"http://www.horangi.kr/festival_info{result[i].Split('\'')[0]}");
            }
            return data;
        }

        Event getPage(string link)
        {

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(new WebClient().DownloadString(link));
            var pos = document.DocumentNode.SelectSingleNode("//body");

            var l = pos.SelectNodes("//ul[@id='workingInfo']/li/p");


            var result = new Event();

            try
            {
                result.Title = pos.SelectSingleNode("//div[@id='festTitle']").InnerText.Trim();
            }
            catch { }

            try
            {
                result.Image = pos.SelectSingleNode("//div[@id='imgDiv']/img").Attributes["src"].Value.Trim();
            }
            catch { }
            try
            {
                result.Description = pos.SelectSingleNode("//div[@id='descDiv']").InnerText.Trim();
            }
            catch { }
            try
            {
                result.Location = pos.SelectSingleNode("//div[@id='addrDiv']").InnerText.Trim();
            }
            catch { }

            try
            {
                result.Position = l[1].InnerText.Trim();
            }
            catch { }
            try
            {
                result.EventTime = l[3].InnerText.Trim();
            }
            catch { }
            try
            {
                var tel = pos.SelectSingleNode("//div[@id='telDiv']/input").Attributes["value"].Value.Trim().Split(' ');
                result.PhoneNumber = tel[tel.Length - 1];
            }
            catch { }
            try
            {
                result.Location = pos.SelectSingleNode("//div[@id='addrDiv']").InnerText.Trim();
            }
            catch { }
            try
            {
                result.Homapage = pos.SelectSingleNode("//div[@id='webDiv']/div/a").InnerText.Trim();
            }
            catch { }
            try
            {
                var day = l[0].InnerText.Trim().Split('~');
                result.StartDay = day[0].Trim();
                result.EndDay = day[1].Trim();
            }
            catch { }

            return result;
        }
    }
}
