using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Predictive_Lifestyle_Project.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// --- PredictionService: bind options + resilient HttpClient ---
builder.Services.Configure<PredictionServiceOptions>(
    builder.Configuration.GetSection("PredictionService"));

static IAsyncPolicy<HttpResponseMessage> RetryPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()      // 5xx, 408, network failures
        .OrResult(r => (int)r.StatusCode == 429)
        .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)));

builder.Services.AddHttpClient<IPredictApi, PredictApi>((sp, client) =>
{
    var opts = sp.GetRequiredService<IOptions<PredictionServiceOptions>>().Value;
    client.BaseAddress = new Uri(opts.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(opts.TimeoutSeconds);
})
.AddPolicyHandler(RetryPolicy());   // Polly

var app = builder.Build();

// --- Pipeline (your existing code) ---
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
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();