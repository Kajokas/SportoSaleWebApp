using Microsoft.EntityFrameworkCore;
using ispk.data;
using ispk.models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=ispk_db.db"));

builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.MapGet("/test", () => test());

app.Run();

String test(){
    String test = "Testingtong the testilion!";
    Console.WriteLine(test);
    return test;
}
