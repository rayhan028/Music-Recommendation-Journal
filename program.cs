using MoodTunes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<MoodService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mood}/{action=Index}/{id?}"
);

app.MapGet("/export/json", (MoodService service) =>
{
    var data = service.GetAllEntries();
    return Results.Json(data);
});

app.Run();
