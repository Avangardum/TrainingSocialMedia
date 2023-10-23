using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.Server.Data;
using Microsoft.AspNetCore.Identity;

// Create the builder
var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var dbPassword = builder.Configuration["DB_PASSWORD"];
if (string.IsNullOrEmpty(dbPassword))
    throw new Exception("DB password is not set");
var dbConnectionString = $"server=db;uid=root;pwd={dbPassword};database=TrainingSocialMedia";
builder.Services.AddDbContext<TrainingSocialMediaDbContext>(options =>
{
    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString), mySqlOptions =>
    {
        mySqlOptions.CommandTimeout(10);
    });
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<TrainingSocialMediaDbContext>();

// Build the app
var app = builder.Build();

// Migrate the database
using (var dbContext = app.Services.GetRequiredService<TrainingSocialMediaDbContext>())
{
    dbContext.Database.Migrate();
}

// Configure the middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Run the app
app.Run();