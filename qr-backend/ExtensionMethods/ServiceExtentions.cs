using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using qr_backend.Filters;
using qr_backend.Repository;
using qr_backend.Services.Cache;
using qrbackend.Api.Repository;
using qrbackend.Api.Services.Authentication.Security;
using qrbackend.Api.Services.Authentication.Token;
using qrbackend.Api.Services.BrokerHelper;
using qrBackend.Api.Services.Authentication;
using System;
using System.Text;
using System.Threading.Tasks;

namespace qr_backend.ExtensionMethods
{
    public static class ServiceExtentions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IBroker, Broker>();
            services.AddTransient<ISecurity, Security>();
            services.AddTransient<IToken, Token>();
            services.AddTransient<IToken, Token>();
            services.AddTransient(typeof(IAuth<>), typeof(Auth<>));
            services.AddTransient(typeof(IRepositoryAuthentication<>), typeof(RepositoryAuthentication<>));
            services.AddTransient(typeof(IRepository<>), typeof(RepositoryMQ<>));
            services.AddSingleton<ICache, Cache>();
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
                //Aqui se agrega al header Token-Expired cuando este expire, y asi el client-side, usa el refresh token
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
