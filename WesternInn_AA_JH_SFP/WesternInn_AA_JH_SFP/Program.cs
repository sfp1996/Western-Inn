using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WesternInn_AA_JH_SFP.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=WesternInn.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>()
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

//create user roles (adminstrators and guests)
using (var scope = app.Services.CreateScope())
{
    //get the service providers
    var services = scope.ServiceProvider;
    try
    {
        var serviceProvider = services.GetRequiredService<IServiceProvider>();
        //get the Config object for the appsettings.json file 
        var configuration = services.GetRequiredService<IConfiguration>();
        //create the roles
        SeedRoles.CreateRoles(serviceProvider, configuration).Wait();
    }
    catch (Exception exception)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "An error occurred while creating roles");
    }
}

app.Run();
