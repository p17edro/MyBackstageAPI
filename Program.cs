using System.Reflection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS
builder.Services.AddCors(options =>
{
    //options.AddPolicy("AllowBackstage", policy =>
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        //policy.WithOrigins("http://localhost:3000") // Allow Backstage to access the API 
        policy.AllowAnyOrigin() // Allow any origin to access the API
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//builder.Services.AddHttpContextAccessor();

// Add XML comments file
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My Backstage API",
        Version = "v1",
        Description = "A REST API integrated with Backstage and NSwag."//
    });

    // Add servers dynamically
    c.AddServer(new OpenApiServer
    {
        //Url = string.Empty,
        Url = $"{builder.Configuration["Swagger:BaseUrl"] ?? "http://localhost:5036"}",
        Description = "Dynamic server URL"
    });

    // Use a custom operation filter to dynamically set the server URL
    //c.OperationFilter<DynamicServerOperationFilter>();

    // Manually specify OpenAPI version to ensure compatibility
    //c.UseOneOfForPolymorphism();
    //c.SupportNonNullableReferenceTypes();

    // Include XML comments
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddOpenApiDocument(); // NSwag integration

// Register the custom operation filter
//builder.Services.AddSingleton<DynamicServerOperationFilter>();

var app = builder.Build();

// 🟢 Apply CORS policy
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Backstage API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.Run();
