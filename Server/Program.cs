using Microsoft.EntityFrameworkCore;
using TrainingSocialMedia.Server.Data;



var builder = WebApplication.CreateBuilder(args);
ConfigureServices();
var app = builder.Build();
MigrateDb();
ConfigurePipeline();
app.Run();



void ConfigureServices()
{
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
}

void ConfigurePipeline()
{
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
}

void MigrateDb()
{
    using var dbContext = app.Services.GetRequiredService<TrainingSocialMediaDbContext>();
    dbContext.Database.Migrate();
}