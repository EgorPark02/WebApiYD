using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using YandexDiskWebApi.Models;

namespace YandexDiskWebApi.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddAppAuth(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<AuthOptions>(cfg.GetSection(AuthOptions.Security));

            var authOpts = new AuthOptions();
            cfg.GetSection(AuthOptions.Security).Bind(authOpts);
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = authOpts.Issuer,
                        ValidAudience = authOpts.Audience,
                        IssuerSigningKey = authOpts.GetSymmetricSecurityKey()
                    };
                });

            return services;
        }
    }
}