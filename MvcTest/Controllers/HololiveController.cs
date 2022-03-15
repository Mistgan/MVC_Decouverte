using System.Net;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MvcTest.Controllers;

public class HololiveController : Controller
{
    string[] ignore = new[] {"free", "chat", "schedule", "weekly", "pretend"};
    /* TODO
            Compresser la liste des live => dropdown
            Faire la meme pour twitch et vshojo
     */
    
    public IActionResult HololiveIndex()
    {
        var live = GetLive();
        var upcoming = GetUpcoming();
        var ended = GetEnded();
        ViewData["Live"] = live.Result;
        ViewData["Upcoming"] = upcoming.Result;
        ViewData["Ended"] = ended.Result;
        return View();
    }
    
    // GET: hololive/
    public async Task<JToken?> GetAll()
    {
        var client = new HttpClient();
        var uri = new Uri("https://api.holotools.app/v1/live");
        var response = await client.GetAsync(uri);
        string textResult = await response.Content.ReadAsStringAsync();
        JObject json = JObject.Parse(textResult);
        return json;
    }
    
    // GET: hololive/live
    public async Task<JToken> GetLive()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var data = await GetAll();
        JContainer live = (JContainer) data["live"];
        foreach (var token in live)
        {
            token["title"] = (JToken) Translate(token["title"].ToString());
            
            Console.WriteLine("TYPE : " + token.GetType());
            if (ignore.Any(token["title"].ToString().ToLower().Contains))
            {
                //Console.WriteLine("TO BE REMOVE : " + token);
                
            }
            
        }
        return live;
    }
    
    // GET: hololive/upcoming
    public async Task<JToken?> GetUpcoming()
    {
        var data = await GetAll();
        var upcoming = data["upcoming"];

        return upcoming;
    }
    
    // GET: hololive/ended
    public async Task<JToken?> GetEnded()
    {
        var data = await GetAll();
        var ended = data["ended"];

        return ended;
    }
    
    
    // Translate to 
    public string Translate(string input)
    {
        HttpClient client = new HttpClient();
        var from = "auto";
        var to = "en";
        var text = HttpUtility.UrlEncode(input);
        var url = String.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", from, to, text);
        var request = client.GetStringAsync(url).Result;
        var jsonData = JsonConvert.DeserializeObject(request);
        var lang = ((JArray) jsonData).Last.Last.Last;
        if (lang.ToString() == "en")
        {
            return input;
        }
        var cr = ((JArray) jsonData).First.First.First.ToString().Replace("]", "】");
        var cl = cr.Replace("[", "【");
        return cl;
    }
}