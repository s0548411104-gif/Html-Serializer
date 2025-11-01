using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public class HtmlParser
    {
        public static HtmlElement Parse(string html)
        {
            var helper = HtmlHelper.Instance;
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

                if (!helper.IsValidTag(tagName))
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

                if (!helper.IsSelfClosing(tagName) && !tagContent.EndsWith("/"))
                    current = element;
            }
            return root;
        }
    }
}