using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ApiDebts.Src.DAO;
using ApiDebts.Src.API;

//dotnet ef migrations add Migration6
//dotnet ef database update
//dotnet watch run

namespace ApiDebts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });

            services.AddControllers();

            // Handle DataTime culture
            services.AddControllersWithViews()
               .AddJsonOptions(options =>
               {
                   options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
               });

            //SAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Test API WS",
                    Version = "1.0"
                });

                c.OperationFilter<AddHeaderOperationFilter>();
            });

            var connectionString = Configuration["DbContextSettings:ConnectionString"];
            services.AddDbContext<DebtsContext>(opts => opts.UseNpgsql(connectionString));

            // Add custon header attributes
            AppDomain.CurrentDomain
                   .GetAssemblies()
                   .SelectMany(assembly => assembly.GetTypes())
                   .Where(type => type.IsSubclassOf(typeof(BaseAttribute)) && !type.IsAbstract)
                   .ToList()
                   .ForEach((attribute) => services.AddScoped(attribute));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //CORS
            app.UseCors("CorsPolicy");

            //app.UseHttpsRedirection();

            //Handel datetime culture
            var defaultCulture = new CultureInfo("es-AR");
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo>() { defaultCulture },
                SupportedUICultures = new List<CultureInfo>() { defaultCulture },
            });

            //Exception handler
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                var result = exception.Message;
                context.Response.ContentType = "text/play";
                await context.Response.WriteAsync(result);
            }));

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "");
            });
        }
    }
}

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var data = reader.GetString();
        return DateTime.Parse(data);
    }


    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        string jsonDateTimeFormat = DateTime.SpecifyKind(value, DateTimeKind.Local).ToString("o", System.Globalization.CultureInfo.CurrentCulture);
        writer.WriteStringValue(jsonDateTimeFormat);
    }
}

