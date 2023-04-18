using System.Text.Json;
using Refit;

namespace YandexDiskWebApi.Controllers
{
    public interface IYandexDiskImageClient
    {
        [Get("/v1/disk/resources?path={fullPath}")]
        Task<object> GetYandexDiskInfo(string fullPath, [Header("Authorization")] string authorization);
    }
}