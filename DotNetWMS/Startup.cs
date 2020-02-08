using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
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

namespace DotNetWMS
{
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
            services.AddControllersWithViews();
            services.AddDbContext<DotNetWMSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DotNetWMSContext")));
            services.AddIdentity<WMSIdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<DotNetWMSContext>();
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
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
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("pl") };
                options.DefaultRequestCulture = new RequestCulture("en", "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var defaultCulture = new CultureInfo("en-US");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };
            app.UseRequestLocalization(localizationOptions);
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
