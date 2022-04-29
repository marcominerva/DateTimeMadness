using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TinyHelpers.Json.Serialization;

var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
jsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());

var invoice = new Invoice(DateTime.Today, 42);
var json = JsonSerializer.Serialize(invoice, jsonSerializerOptions);

var httpClient = new HttpClient();
var response = await httpClient.PostAsJsonAsync("https://localhost:7239/api/Invoices", invoice, jsonSerializerOptions);

Console.ReadLine();

public record class Invoice(
    [property: JsonConverter(typeof(ShortDateConverter))]
    DateTime Date,
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