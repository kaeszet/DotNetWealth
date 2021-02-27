using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace DotNetWMS
{
    /// <summary>
    /// Class responsible for application configuration
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">Parameter that enables adding and configuring services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Necessary service for the MVC model
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            //Entity Framework Core service
            services.AddDbContext<DotNetWMSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DotNetWMSContext")));
            //Identity Core service
            services.AddIdentity<WMSIdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
                    .AddEntityFrameworkStores<DotNetWMSContext>()
                    //The option activates a security token provider related to the framework
                    .AddDefaultTokenProviders()
                    //Add custom error describer
                    .AddErrorDescriber<CustomIdentityErrorDescriber>();
            //Adds localization with directory path
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            //Adds MVC model and Polish localization of error messages
            services.AddMvc(options =>
            {
                var F = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                var L = F.Create("ModelBindingMessages", "DotNetWMS");
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) => L["Wprowadzona wartoœæ '{0}' jest nieprawid³owa!", x]);
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) => L["W polu '{0}' musi znajdowaæ siê liczba!", x]);
                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((x) => L["Wartoœæ dla w³aœciwoœci '{0}' nie zosta³a przekazana.", x]);
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => L["Wartoœæ '{0}' nie jest prawid³owa dla {1}.", x, y]);
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => L["Wartoœæ jest wymagana."]);
                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) => L["Podana wartoœæ jest nieprawid³owa dla {0}.", x]);
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((x) => L["Wartoœæ 'null' jest nieprawid³owa dla tego formularza!", x]);
                options.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddDataAnnotationsLocalization()
            .AddViewLocalization();
            //Adds the units presentation characteristic for Polish culture
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var culture = CultureInfo.CreateSpecificCulture("pl-PL");
                var dateFormat = new DateTimeFormatInfo { ShortDatePattern = "dd.MM.yyyy", LongDatePattern = "dd.MM.yyyy hh:mm:ss tt" };
                culture.DateTimeFormat = dateFormat;
                var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("pl"), culture };
                options.DefaultRequestCulture = new RequestCulture(culture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            //Adds the path for the page with the error message due to lack of privileges
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
            //Adds cookie policy for GDPR requirements
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential 
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

        }
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Parameter for configuring application elements</param>
        /// <param name="env">Parameter for configuring WebHost environment elements</param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            var defaultCulture = new CultureInfo("pl-PL");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };
            app.UseRequestLocalization(localizationOptions);
            loggerFactory.AddLog4Net();
            //If you are working in development mode, view the detailed page with the error generated by the framework. Otherwise - enter the path to the custom page for the user
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "userpanel",
                    pattern: "UserPanel/{controller=Employees}/{action=Index}/{id?}"
                    );
            });
            
        }
    }
}
