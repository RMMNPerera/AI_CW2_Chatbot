using ExploreSrilanka.DbLayer.Models;
using ExploreSrilanka.DbLayer.Repositories;
using ExploreSrilanka.ServiceLayer.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ChatBotContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DevConnection")));

// Db layer dependecy injection
builder.Services.AddScoped<IChatBotRepo, ChatBotRepo>();

// Service layer dependecy injection
builder.Services.AddScoped<IChatBotService, ChatBotService>();

builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true; // Required to allow session storage
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ChatBot}/{action=Index}/{id?}");

app.Run();
