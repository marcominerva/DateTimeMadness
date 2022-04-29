using System.ComponentModel;
using ClientContext;
using DateTimeMadness.Controllers;
using DateTimeMadness.DataAccessLayer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using TinyHelpers.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
    {
        TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddClientContextOperationFilter();

    options.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date"
    });

    options.MapType<TimeOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "time",
        Example = new OpenApiString(TimeOnly.FromDateTime(DateTime.Now).ToString("HH:mm:ss"))
    });
});

builder.Services.AddClientContextAccessor();

builder.Services.AddSqlServer<DataContext>(builder.Configuration.GetConnectionString("SqlConnection"));

var app = builder.Build();
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseClientContext();

app.UseAuthorization();

app.MapControllers();

app.Run();
