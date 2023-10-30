using System.Reflection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.Areas.Identity;
using TrainingSocialMedia.Entities;
using TrainingSocialMedia.Interfaces;
using TrainingSocialMedia.Presenters;
using TrainingSocialMedia.Repositories;
using TrainingSocialMedia.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbServer = builder.Environment.IsDevelopment() ? "localhost" : "db";
var dbPassword = builder.Configuration["TRAINING_SOCIAL_MEDIA_DB_PASSWORD"];
if (string.IsNullOrEmpty(dbPassword))
    throw new Exception("DB password is not set");
var dbConnectionString = $"server={dbServer};uid=root;pwd={dbPassword};database=TrainingSocialMedia";
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString),
        mySqlOptions => { mySqlOptions.CommandTimeout(10); });
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IPostPresenter, PostPresenter>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

var app = builder.Build();

// Migrate the database
using (var scope = app.Services.CreateScope())
{
    using (var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();