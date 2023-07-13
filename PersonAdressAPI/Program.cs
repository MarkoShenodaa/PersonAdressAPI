global using PersonAdressAPI.Services.Pepole;
global using PersonAdressAPI.JWT_Handeler;
using PersonAdressAPI.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.Configure<Tokens>(builder.Configuration.GetSection("Tokens"));

builder.Services.AddSingleton<IPerson, PepoleService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

// custom jwt auth middleware
app.UseMiddleware<JwtHandeler>();

app.MapControllers();

//app.Run("https://localhost:3000");
app.Run();