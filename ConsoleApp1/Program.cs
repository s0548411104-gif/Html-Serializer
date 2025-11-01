using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter website URL:");
            string url = Console.ReadLine();

            Console.WriteLine("Enter HTML tag name to count:");
            string tagName = Console.ReadLine();

            try
            {
                string html = await HtmlSerializer.Load(url);

                var root = HtmlSerializer.ParseHtml(html);

                var selector = Selector.FromString(tagName);

                var results = root.Query(selector);

                Console.WriteLine($"Found {results.Count} elements of type '{tagName}' on the website {url}.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
