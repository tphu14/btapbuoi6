﻿using Microsoft.EntityFrameworkCore;
using WebApplication17.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext trước khi gọi Build()
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBookRepository, EFBookRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Cấu hình HSTS cho môi trường sản xuất
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Đảm bảo thứ tự middleware đúng
app.UseRouting();
app.UseAuthorization();

// Cấu hình các route cho controller
app.MapControllerRoute(
    name: "categoryBooks",
    pattern: "Book/ByCategory/{categoryId}",
    defaults: new { controller = "Book", action = "ByCategory" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=Index}/{id?}");


app.Run();
