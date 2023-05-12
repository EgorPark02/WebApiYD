using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace YandexDiskWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class YandexDiskController : ControllerBase
{
    private readonly IYandexDiskImageClient _client;
    private readonly string _token;

    public YandexDiskController(IYandexDiskImageClient client, IConfiguration cfg)
    {
        _client = client;
        _token = cfg.GetValue<string>("YandexToken");
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var test = await _client.GetYandexDiskInfo("/", _token);
        return Ok(test.ToString());
    }

    [HttpGet("Images")]
    public async Task<ActionResult> GetImages()
    {
        var images = await _client.GetYandexDiskImages("/image", _token);
        return Ok(images.ToString());
    }
}