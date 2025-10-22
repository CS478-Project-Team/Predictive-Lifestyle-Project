using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Predictive_Lifestyle_Project.Data;
using Predictive_Lifestyle_Project.Services;
using SendingEmails;



var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


//email sending service
builder.Services.AddTransient<SendingEmails.IEmailSender, SendingEmails.EmailSender>();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        // For dev: turn off confirm if you want quicker testing
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // for Identity UI

builder.Services.Configure<PredictionServiceOptions>(
    builder.Configuration.GetSection("PredictionService"));

static IAsyncPolicy<HttpResponseMessage> RetryPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()            // 5xx, 408, network failures
        .OrResult(r => (int)r.StatusCode == 429)
        .WaitAndRetryAsync(3, attempt =>
            TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt))); // 200ms, 400ms, 800ms

builder.Services
    .AddHttpClient<IPredictApi, PredictApi>((sp, client) =>
    {
        var opts = sp.GetRequiredService<IOptions<PredictionServiceOptions>>().Value;
        client.BaseAddress = new Uri(opts.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(opts.TimeoutSeconds);
    })
    .AddPolicyHandler(RetryPolicy());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
   .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
