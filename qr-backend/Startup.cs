using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using qr_backend.ExtensionMethods;
using qr_backend.Filters;
using qrbackend.Api.Services.BrokerHelper;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections;
using System.Collections.Generic;

namespace qr_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureServices();
            services.ConfigureCors();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelFilter));
                options.Filters.Add(typeof(AuthenticationFilter));

            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .ConfigureApiBehaviorOptions(o =>
           {
               o.InvalidModelStateResponseFactory = context =>
               {
                   return new ValidationFailedResult(context.ModelState);
               };
           });
            services.Configure<InfoBroker>(Configuration.GetSection("MQConnection"));
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(
                    string.Format(@"{0}\SwaggerPaymeApi.XML",
                    System.AppDomain.CurrentDomain.BaseDirectory));
                c.IgnoreObsoleteProperties();
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Info
                {
                    Version = "V1",
                    Title = "Paymen API",
                    Description = "API Con los servicios del proyecto Payme",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Payme Project", Email = "payme@bhdleon.do.com", Url = "www.bhdleon.com.do" }
                });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {
                        "Bearer", new string[] { }
                    }
                });
            });

            services.AddMemoryCache();
            services.ConfigureAuthentication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
                // app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseCors(options =>
           {
               options
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin();
           });
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Paymen API");
            });


        }
    }
}
