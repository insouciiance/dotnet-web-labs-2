using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Orderly.Application.Entities;
using Orderly.Application.Identity;
using Orderly.Infrastructure.Extensions;
using Orderly.WebAPI.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Orderly.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        builder.Services.AddAutoMapper(typeof(IEntity<>));

        builder.Services.AddEfContext(builder.Configuration.GetConnectionString("DefaultConnection")!);
        builder.Services.AddEfRepository<Ticket, Guid>();
        builder.Services.AddEfRepository<AppUser, Guid>();

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new()
                {
                    ValidIssuer = builder.Configuration[IdentityConstants.CONFIG_SECTION_ISSUER],
                    ValidAudience = builder.Configuration[IdentityConstants.CONFIG_SECTION_AUDIENCE],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[IdentityConstants.CONFIG_SECTION_KEY]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddJwtGenerator();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}
