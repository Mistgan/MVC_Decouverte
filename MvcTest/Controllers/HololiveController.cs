using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MvcTest.Controllers;

public class HololiveController : Controller
{
    /* TODO
            Retirer les free chat 
            Mettre en noir le fond
            Compresser la liste des live => dropdown
            Faire la meme pour twitch et vshoja
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
    public async Task<IEnumerable<JToken>> GetLive()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var data = await GetAll();
        var live = data["live"];
        return live.AsEnumerable();
    }
    
    // GET: hololive/upcoming
    public async Task<JToken?> GetUpcoming()
    {
        var data = await GetAll();
        var live = data["upcoming"];
        return live;
    }
    
    // GET: hololive/ended
    public async Task<JToken?> GetEnded()
    {
        var data = await GetAll();
        var live = data["ended"];
        return live;
    }
}