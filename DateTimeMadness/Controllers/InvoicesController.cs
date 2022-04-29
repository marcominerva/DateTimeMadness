using System.ComponentModel;
using System.Globalization;
using ClientContext;
using Microsoft.AspNetCore.Mvc;

namespace DateTimeMadness.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly IClientContextAccessor clientContextAccessor;

    public InvoicesController(IClientContextAccessor clientContextAccessor)
    {
        this.clientContextAccessor = clientContextAccessor;
    }

    [HttpGet]
    public IActionResult Search(DateOnly date)
        => NoContent();

    [HttpPost]
    public IActionResult Save(Invoice invoce)
        => NoContent();
}

public record class Invoice(
    DateOnly Date,
    double Total);

//public class ShortDateTimeConverter : JsonConverter<DateTime>
//{
//    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        var value = reader.GetString();
//        var result = DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal).Date;
//        return result;
//    }

//    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) 
//        => writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
//}

public class DateOnlyTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        if (sourceType == typeof(string))
        {
            return true;
        }

        return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            return true;
        }

        return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string input)
        {
            return DateOnly.Parse(input);
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is DateOnly dateOnly)
        {
            return dateOnly.ToString();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}