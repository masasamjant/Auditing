using Masasamjant.Auditing.Abstractions;

namespace Masasamjant.Auditing.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var auditorConfiguration = new AuditorConfiguration(Enumerable.Empty<string>(), AuditorConfiguration.DefaultAuditingTimeoutMilliseconds);
            var auditor = new MemoryAuditor(auditorConfiguration);
            auditor.Enable();
            builder.Services.AddSingleton<IAuditingEventSource>(auditor);
            builder.Services.AddSingleton<IAuditor>(auditor);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
