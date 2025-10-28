using Microsoft.EntityFrameworkCore;
using ispk.data;
using ispk.models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=ispk_db.db"));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/test", () => test());

app.MapGet("/users", async (AppDbContext db) => await getUsers(db));

app.Run();

String test(){
    String test = "Testingtong the testilion!";
    Console.WriteLine(test);
    return test;
}

async Task<List<User>> getUsers(AppDbContext db){
    return await db.Users.ToListAsync();
}
