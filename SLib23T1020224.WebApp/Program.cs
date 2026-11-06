using SLib23T1020224.BusinessLayer;
using SLib23T1020224.DataLayer.Interfaces;
using SLib23T1020224.DataLayer.SQLServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Tầng dữ liệu
// Đăng ký db Helper
builder.Services.AddScoped<DbHelper>();

// Tầng dữ liệu

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IMemberTypeRepository, MemberTypeRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();

// Tầng nghiệp vụ
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<LanguageService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<MemberTypeService>();
builder.Services.AddScoped<PublisherService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();