using System.Text.Json;
using Refit;

namespace YandexDiskWebApi.Controllers
{
    public interface IYandexDiskImageClient
    {
        [Get("/v1/disk/resources?path={fullPath}")]
        Task<object> GetYandexDiskInfo(string fullPath, [Header("Authorization")] string authorization);
        
        [Get("/v1/disk/resources/files?path={fullPath}&media_type=image&fields=items.name,items.preview")]
        Task<object> GetYandexDiskImages(string fullPath, [Header("Authorization")] string authorization);
    }
}