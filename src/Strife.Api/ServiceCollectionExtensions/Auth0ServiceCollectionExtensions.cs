﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Strife.Api.Auth;
using System.Security.Claims;

namespace Strife.Api.ServiceCollectionExtensions
{
    internal static class Auth0ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth0(this IServiceCollection services, IConfiguration configuration)
        {
            var domain = $"https://{configuration.GetValue<string>("Auth0:Domain")}/";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = configuration.GetValue<string>("Auth0:ApiIdentifier");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("test", policy => policy.Requirements.Add(new HasScopeRequirement("test:scope", domain)));
                });

                services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            });

            return services;
        }
    }
}
