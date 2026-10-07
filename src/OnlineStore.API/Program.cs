using OnlineStore.BLL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessLogic(builder.Configuration.GetConnectionString("StoreDb")!);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "OnlineStore API"));
}

app.MapControllers();

app.Run();
