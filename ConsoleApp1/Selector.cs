using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector FromString(string query)
        {
            Selector root = null;
            Selector current = null;

            var parts = query.Split(' ');
            foreach (var part in parts)
            {
                var sel = new Selector();
                string temp = part;

                var idMatch = Regex.Match(temp, "#(\\w+)");
                if (idMatch.Success)
                {
                    sel.Id = idMatch.Groups[1].Value;
                    temp = temp.Replace(idMatch.Value, "");
                }

                var classMatches = Regex.Matches(temp, "\\.(\\w+)");
                foreach (Match m in classMatches)
                {
                    sel.Classes.Add(m.Groups[1].Value);
                    temp = temp.Replace(m.Value, "");
                }

                if (!string.IsNullOrEmpty(temp))
                    sel.TagName = temp;

                if (root == null)
                    root = sel;
                else
                {
                    current.Child = sel;
                    sel.Parent = current;
                }

                current = sel;
            }

            return root;
        }
    }
}
