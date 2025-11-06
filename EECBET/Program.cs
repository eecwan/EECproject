using Microsoft.EntityFrameworkCore;
using EECBET.Data;
using EECBET.Services;

var builder = WebApplication.CreateBuilder(args);

// 把資料庫交給EF Core :ApplicationDbContext ,然後資料庫在Neon
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// 註冊服務
builder.Services.AddSingleton<DrawHistoryService>();
builder.Services.AddSingleton<DrawService>();

// 加入 Session 支援
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session 逾時時間
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 啟用 Session（必須在 UseRouting 之後，UseEndpoints 之前）
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
