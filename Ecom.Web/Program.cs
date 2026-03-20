using Ecom.Application.Authentication;
using Ecom.Application.Product;
using Ecom.Application.Product.Services;
using Ecom.Application.User;
using Ecom.Web.Common.Auth;
using Ecom.Web.Common.AuthCookie;
using Ecom.Web.Common.Config;
using Ecom.Web.Common.HeaderHandler;
using Ecom.Web.Shared.Interfaces;

var builder = WebApplication.CreateBuilder(args);
Console.OutputEncoding = System.Text.Encoding.UTF8;
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
// Đăng ký IHttpClientFactory
builder.Services.AddMemoryCache(options =>
{
    // Giới hạn tổng số lượng item hoặc dung lượng
    options.SizeLimit = 1000;

    // Tần suất quét để dọn dẹp các item hết hạn (mặc định 1 phút)
    options.ExpirationScanFrequency = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthenticationExtensions(builder.Configuration); //Authentication
builder.Services.AddScoped<AuthTokenCookie>(); // cấu hình cookie lưu token
builder.Services.AddTransient<AuthenticationHeaderHandler>(); // cấu hình kiểm tra token 401
builder.Services.AddApplicationHeadeHandler(builder.Configuration); // cấu hình header handler


//cấu hình appsetting
builder.Services.AddConfigAppSetting(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();
//
builder.Services.AddScoped<ICacheService, CacheService>();


// application DI
builder.Services.AddApplicationAuthenticationDependencyInjection(builder.Configuration);
builder.Services.AddApplicationUserDependencyInjection(builder.Configuration);
builder.Services.AddApplicationProductDependencyInjection(builder.Configuration);

// 1. Thêm dịch vụ nén
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// 2. Sử dụng Middleware nén (đặt trước StaticFiles)
app.UseResponseCompression();
// Cấu hình trong Program.cs
app.UseStaticFiles(new StaticFileOptions {
    OnPrepareResponse = ctx =>
    {
        // 3,888,000 giây = 45 ngày
        const int durationInSeconds = 3888000;

        // Chỉ cache cho thư mục chứa hình ảnh
        if (ctx.Context.Request.Path.Value != null &&
        ctx.Context.Request.Path.Value.Contains("https://zhlneinpjgzpjbpozskh.supabase.co"))
        {
            ctx.Context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.CacheControl] =
                $"public,max-age={durationInSeconds}";
        }
    }
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession(); // Phải nằm trước Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
