using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Refit;
using YandexDiskWebApi.Controllers;
using YandexDiskWebApi.Extensions;

namespace YandexDiskWebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAppAuth(Configuration);
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImageService", Version = "v1" });
        });

        // services.AddTransient<IImageService, Services.ImageService>();

        SetupRefit(services);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageService v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private void SetupRefit(IServiceCollection services)
    {
        var refitSettings = GetRefitSettings();

        var yandexApiUrl = Configuration.GetSection("YandexApiUrl").Value;

        services.TryAddTransient(
            ImplementationFactory<IYandexDiskImageClient>(refitSettings, yandexApiUrl));
    }

    private RefitSettings GetRefitSettings()
    {
        return new()
        {
            ContentSerializer = new NewtonsoftJsonContentSerializer(
                new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                })
        };
    }

    private static Func<IServiceProvider, T> ImplementationFactory<T>(
        RefitSettings refitSettings,
        string uriAddress)
    {
        return _ => RestService.For<T>(new HttpClient
        {
            BaseAddress = new Uri(uriAddress)
        }, refitSettings);
    }
}