using Blog.Api;
using Blog.Api.Services;
using Blog.Api.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<BlogDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

//register developer defined services
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", new() { Title = "BlogApi", Version = "v1" });
    c.OperationFilter<CustomHeaderSwaggerAttribute>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
