using System;

namespace ConsoleApp1
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string url = "https://example.com";
            string html = await HtmlLoader.Load(url);

            var root = HtmlParser.Parse(html);

            var selector = Selector.FromString("div");
            var results = root.Query(selector);

            foreach (var el in results)
                Console.WriteLine(el);

            Console.ReadLine();
        }
    }
}
