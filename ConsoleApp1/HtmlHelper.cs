using System;
using System.IO;
using System.Text.Json;

namespace ConsoleApp1
{
    public class HtmlHelper
    {
        private static HtmlHelper _instance;
        public static HtmlHelper Instance => _instance ??= new HtmlHelper();

        public string[] HtmlTags { get; private set; }
        public string[] HtmlVoidTags { get; private set; }

        private HtmlHelper()
        {
            string allTagsJson = File.ReadAllText("HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(allTagsJson);

            string selfClosingJson = File.ReadAllText("HtmlVoidTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(selfClosingJson);
        }

        public bool IsSelfClosing(string tag) =>
            Array.Exists(HtmlVoidTags, t => t.Equals(tag, StringComparison.OrdinalIgnoreCase));

        public bool IsValidTag(string tag) =>
            Array.Exists(HtmlTags, t => t.Equals(tag, StringComparison.OrdinalIgnoreCase));
    }
}
