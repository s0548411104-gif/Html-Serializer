using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public static class HtmlSerializer
    {
        private static HashSet<string> HtmlTags;
        private static HashSet<string> SelfClosingTags;

        private static void LoadJsonFiles()
        {
            if (HtmlTags != null && SelfClosingTags != null)
                return;

            string tagsJson = File.ReadAllText("HTMLTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(tagsJson)
                       .ToHashSet(StringComparer.OrdinalIgnoreCase);

            string voidTagsJson = File.ReadAllText("HTMLVoidTags.json");
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(voidTagsJson)
                               .ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        public static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public static HtmlElement ParseHtml(string html)
        {
            LoadJsonFiles(); 

            html = Regex.Replace(html, @"\r?\n|\t", "");
            var regex = new Regex(@"<([^>]+)>");
            var matches = regex.Matches(html);

            HtmlElement root = new HtmlElement { Name = "root" };
            HtmlElement current = root;

            foreach (Match match in matches)
            {
                string tagContent = match.Groups[1].Value.Trim();

                if (tagContent.StartsWith("/"))
                {
                    if (current.Parent != null)
                        current = current.Parent;
                    continue;
                }

                var parts = tagContent.Split(' ', 2);
                string tagName = parts[0];
                string attrString = parts.Length > 1 ? parts[1] : "";

                if (!HtmlTags.Contains(tagName))
                    continue;

                var element = new HtmlElement { Name = tagName, Parent = current };

                var attrMatches = Regex.Matches(attrString, @"([\w-]+)\s*=\s*""(.*?)""");
                foreach (Match attr in attrMatches)
                {
                    element.Attributes[attr.Groups[1].Value] = attr.Groups[2].Value;
                    if (attr.Groups[1].Value == "id")
                        element.Id = attr.Groups[2].Value;
                    if (attr.Groups[1].Value == "class")
                        element.Classes = attr.Groups[2].Value.Split(' ').ToList();
                }

                current.Children.Add(element);

                if (!SelfClosingTags.Contains(tagName) && !tagContent.EndsWith("/"))
                    current = element;
            }

            return root;
        }
    }
}
