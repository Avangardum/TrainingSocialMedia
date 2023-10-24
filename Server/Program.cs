using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.Server.Data;

// Create the builder
var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var dbServer = builder.Environment.IsDevelopment() ? "localhost" : "db";
var dbPassword = builder.Configuration["TRAINING_SOCIAL_MEDIA_DB_PASSWORD"];
if (string.IsNullOrEmpty(dbPassword))
    throw new Exception("DB password is not set");
var dbConnectionString = $"server={dbServer};uid=root;pwd={dbPassword};database=TrainingSocialMedia";
builder.Services.AddDbContext<TrainingSocialMediaDbContext>(options =>
{
    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString),
        mySqlOptions => { mySqlOptions.CommandTimeout(10); });
});

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<TrainingSocialMediaDbContext>();

// Build the app
var app = builder.Build();

// Migrate the database
using (var scope = app.Services.CreateScope())
{
    using (var dbContext = scope.ServiceProvider.GetRequiredService<TrainingSocialMediaDbContext>())
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
        }
    }
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
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Run the app
app.Run();