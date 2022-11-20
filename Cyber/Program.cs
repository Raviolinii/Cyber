using Cyber.Data;
using Cyber.Models;
using Cyber.Services;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//builder.Services.AddDNTCaptcha(option => option.UseCookieStorageProvider().ShowThousandsSeparators(false));

IWebHostEnvironment _env = builder.Environment;

builder.Services.AddDNTCaptcha(options =>
{
    // options.UseSessionStorageProvider() // -> It doesn't rely on the server or client's times. Also it's the safest one.
    // options.UseMemoryCacheStorageProvider() // -> It relies on the server's times. It's safer than the CookieStorageProvider.
    options.UseCookieStorageProvider(SameSiteMode.Strict /* If you are using CORS, set it to `None` */) // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                                                                                        // .UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                                                                                                        // .UseDistributedSerializationProvider()

    // Don't set this line (remove it) to use the installed system's fonts (FontName = "Tahoma").
    // Or if you want to use a custom font, make sure that font is present in the wwwroot/fonts folder and also use a good and complete font!
    //.UseCustomFont(Path.Combine(_env.WebRootPath, "fonts", "IRANSans(FaNum)_Bold.ttf"))
    .AbsoluteExpiration(minutes: 7)
    .ShowThousandsSeparators(false)
    .WithNoise(pixelsDensity: 25, linesCount: 3)
    .WithEncryptionKey("6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe")
    .InputNames(// This is optional. Change it if you don't like the default names.
        new DNTCaptchaComponent
        {
            CaptchaHiddenInputName = "DNT_CaptchaText",
            CaptchaHiddenTokenName = "DNT_CaptchaToken",
            CaptchaInputName = "DNT_CaptchaInputText"
        })
    .Identifier("dnt_Captcha")// This is optional. Change it if you don't like its default name.
    ;
});



builder.Services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromSeconds(10));
builder.Services.AddAuthentication()
    .Services.ConfigureApplicationCookie(options =>
    {
//LAB2 automatyczne wylogowanie po 15 sekundach
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromSeconds(420); // <-
    });

//LAB2 maksymalna liczba prób logowania (do przetestowania bo baza posz³a siê chrzaniæ)
builder.Services.AddDefaultIdentity<UserModel>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    // Lockout settings
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3; // <-
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserAndRolesManager, UserAndRolesManager>();
builder.Services.AddScoped<UserManager<UserModel>>();
builder.Services.AddSingleton<ApplicationDbInitializer>();

builder.Services.AddControllersWithViews().AddRazorPagesOptions(options =>
{
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//pattern: "{controller=Login}/{action=Register}/{id?}");
app.MapRazorPages();
app.Services.GetService<ApplicationDbInitializer>();
app.Run();
