using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RedisCache;
using RedisCache.Data.Interface;
using RedisCache.Data.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();


builder.Services.AddDbContextPool<RedisCacheDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


string AllowOrigins = builder.Configuration["Cors"];
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigins",
               builder =>
               {
                   builder.SetIsOriginAllowed(origin => true)
                //builder.WithOrigins(AllowOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();

               });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RedisCacheDemo",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(AllowOrigins);

app.MapControllers();

app.Run();
