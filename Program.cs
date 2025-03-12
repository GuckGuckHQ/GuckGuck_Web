using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
builder.Services.AddHostedService<TimedCleanupService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();


app.MapGet("/image/{id}", (string id) =>
{
    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", id);
    var filePath = Path.Combine(directoryPath, $"image.png");

    if (!File.Exists(filePath))
    {
        return Results.NotFound();
    }

    return Results.File(filePath, "image/png");
});

app.MapPost("/image", async ([FromForm] ImageRequest request) =>
{
    var image = request.Image;
    var id = request.Id;
    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", id);
    Directory.CreateDirectory(directoryPath);
    var filePath = Path.Combine(directoryPath, $"image.png");

    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
    {
        await image.CopyToAsync(stream);
    }

    return Results.Redirect("/");
}).DisableAntiforgery();


app.Run();



public class ImageRequest
{
    public string Id { get; set; }
    public IFormFile Image { get; set; }
}

