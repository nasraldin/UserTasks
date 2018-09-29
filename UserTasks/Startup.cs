using AspNet.Security.OpenIdConnect.Primitives;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using UserTasks.Authorization;
using UserTasks.Core;
using UserTasks.Core.Authorization;
using UserTasks.Core.Authorization.Interfaces;
using UserTasks.Core.Data;
using UserTasks.Core.Data.Interfaces;
using UserTasks.Core.Entities;
using UserTasks.Core.Uow;
using UserTasks.Core.Uow.Interfaces;
using UserTasks.Helpers;

namespace UserTasks
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
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"],
                    b => b.MigrationsAssembly("UserTasks"));
                options.UseOpenIddict();
            });

            // add identity
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity options and password complexity
            services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;

                //// Password settings
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = false;

                //// Lockout settings
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //options.Lockout.MaxFailedAccessAttempts = 10;

                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            // Register the OpenIddict services
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore().UseDbContext<AppDbContext>();
                })
                .AddServer(options =>
                {
                    options.UseMvc();
                    options.EnableTokenEndpoint("/connect/token");
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    options.AcceptAnonymousClients();
                    options.DisableHttpsRequirement(); // Note: Comment this out in production
                    options.RegisterScopes(
                        OpenIdConnectConstants.Scopes.OpenId,
                        OpenIdConnectConstants.Scopes.Email,
                        OpenIdConnectConstants.Scopes.Phone,
                        OpenIdConnectConstants.Scopes.Profile,
                        OpenIdConnectConstants.Scopes.OfflineAccess,
                        OpenIddictConstants.Scopes.Roles);

                    // options.UseRollingTokens(); //Uncomment to renew refresh tokens on every refreshToken request
                    // Note: to use JWT access tokens instead of the default encrypted format, the following lines are required:
                    // options.UseJsonWebTokens();
                })
                .AddValidation(); //Only compatible with the default token format. For JWT tokens, use the Microsoft JWT bearer handler.

            // Add cors
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            //Todo: Using DataAnnotations for validation until Swashbuckle supports FluentValidation
            //services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            //.AddJsonOptions(opts =>
            //{
            //    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //});

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.ViewAllUsers, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewUsers));

                options.AddPolicy(Policies.ManageAllUsers, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageUsers));

                options.AddPolicy(Policies.ViewAllRoles, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ViewRoles));

                options.AddPolicy(Policies.ViewRoleByRoleName, policy => policy.Requirements.Add(new ViewRoleAuthRequirement()));

                options.AddPolicy(Policies.ManageAllRoles, policy => policy.RequireClaim(CustomClaimTypes.Permission, ApplicationPermissions.ManageRoles));

                options.AddPolicy(Policies.AssignAllowedRoles, policy => policy.Requirements.Add(new AuthorizationRequirement()));
            });

            // Initialize Auto Mapper Profile
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            // Repositories
            services.AddScoped<IUnitOfWork, HttpUnitOfWork>();
            services.AddScoped<IAccountManager, AccountManager>();

            // Auth Handlers
            services.AddSingleton<IAuthorizationHandler, ViewUserAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ManageUserAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ViewRoleAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, AssignRolesAuthorizationHandler>();

            // DB Creation and Seeding
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Warning);
            //loggerFactory.AddFile(Configuration.GetSection("Logging"));

            Utilities.ConfigureLogger(loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                //{
                //    serviceScope.ServiceProvider.GetService<AppDbContext>().Database.Migrate();
                //    serviceScope.ServiceProvider.GetService<AppDbContext>().Database.EnsureCreated();
                //}
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //Configure Cors
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");

                    // Increase the timeout if angular app is taking longer to startup
                    //spa.Options.StartupTimeout = TimeSpan.FromSeconds(60);

                    // Use this instead to use the angular cli server
                    // spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
