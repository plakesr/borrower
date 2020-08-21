// <copyright file="Startup.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal
{
    using System.Globalization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Nortridge.BorrowerPortal.Core;
    using Nortridge.BorrowerPortal.Core.Auth;
    using Nortridge.BorrowerPortal.Core.Contacts;
    using Nortridge.BorrowerPortal.Core.Loans;
    using Nortridge.BorrowerPortal.Core.Infrastructure;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Configuration;
    using Nortridge.BorrowerPortal.Core.Infrastructure.ModelBinders;
    using Nortridge.BorrowerPortal.Core.UseCases;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddSession(options => options.Cookie.IsEssential = true);

            services.AddMemoryCache();

            services
                .AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    options.Filters.Add(new AuthorizeFilter(policy));

                    CommonModelBinderProviderRegister.Providers.Iter(provider =>
                        options.ModelBinderProviders.Insert(0, provider));
                })
                .AddRazorPagesOptions(options => options.Conventions
                    .AddPageRoute("/Index", "/Balances"))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization()
                .AddSessionStateTempDataProvider();

            services.AddHttpContextAccessor();

            var appConfig = this.Configuration.Get<AppSettingsConfig>();
            services
                .AddInfrastructure(
                    appConfig.Authentication,
                    appConfig.NlsApi,
                    appConfig.AutomatedPayment,
                    appConfig.UserAuthentication,
                    appConfig.SmtpServer,
                    appConfig.Payix)
                .AddCore()
                .AddAuth(appConfig.Authentication)
                .AddLoans()
                .AddContacts()
                .AddUseCases();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net("log4net.config", watch: true);

            var supportedCultures = new[]
            {
                new CultureInfo("en-US")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),

                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,

                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseErrorHandling(env);

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc();
        }
    }
}
