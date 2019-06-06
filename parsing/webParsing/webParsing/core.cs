using System;
using System.Net;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
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

        public List<Event> Run(int year, int month, int page)
        {
            var result = new List<Event>();
            for (int i = 1; i <= page; i++)
            {
                foreach (var link in MainPageLoad(WebDownload(year, month, i)))
                {
                    result.Add(getPage(link));
                }
                Writer(result);
            }
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
            var tel = pos.SelectSingleNode("//div[@id='telDiv']/input").Attributes["value"].Value.Trim().Split(' ');


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
