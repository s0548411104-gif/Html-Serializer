using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class HtmlLoader
    {
        public static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
