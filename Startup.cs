using KindergartenManagementSystem.Core.Helpers;
using KindergartenManagementSystem.Core.Services;
using KindergartenManagementSystem.Extensions;
using KindergartenManagementSystem.Infrastructure.Data;
using KindergartenManagementSystem.Infrastructure.Services;
using KindergartenManagementSystem.Infrastructure.Services.Background;
using KindergartenManagementSystem.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KindergartenManagementSystem
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
            services.AddMvc().AddNewtonsoftJson();

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IMapper, ApplicationMapper>();

            services.AddScoped<IUserStore<InfrastructureUser>, InfrastructureUserStore>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ISignInManager, SignInManager>();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailService, EmailService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<InfrastructureDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("KindergartenCS")));

            services.AddIdentity<InfrastructureUser, InfrastructureRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                    options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
                    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                })
                .AddRoles<InfrastructureRole>()
                .AddEntityFrameworkStores<InfrastructureDbContext>()
                .AddDefaultTokenProviders();

            services.ApplyConfigurationToExtensions();
            services.AddJwtAuthentication();

            services.AddHostedService<ScheduleRemoveExpiredJwt>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                endpoints.MapRazorPages();
            });
        }
    }
}
