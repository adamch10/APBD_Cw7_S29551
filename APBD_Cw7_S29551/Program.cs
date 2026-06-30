using APBD_Cw7_S29551.Data;
using APBD_Cw7_S29551.Middleware;
using APBD_Cw7_S29551.Repositories;
using APBD_Cw7_S29551.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Rejestracja DI
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Globalna obsługa błędów
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    // Wymóg z zadania: użytkownik ma zobaczyć normalny błąd, a nie stack trace w trybie produkcyjnym. 
    // Do testów odpalamy po prostu UseExceptionHandler.
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// Rejestracja własnego middleware
app.UseMiddleware<RequestTimingMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tickets}/{action=Index}/{id?}");

app.Run();