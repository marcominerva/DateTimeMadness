using System.Net.Mime;
using ClientContext;
using DateTimeMadness.DataAccessLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DateTimeMadness.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : ControllerBase
{
    private readonly IClientContextAccessor clientContextAccessor;

    public SessionsController(IClientContextAccessor clientContextAccessor)
    {
        this.clientContextAccessor = clientContextAccessor;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Session>>> Get([FromServices] DataContext dataContext)
    {
        var sessions = await dataContext.Sessions.OrderBy(s => s.StartTime)
            .Select(s => new Session(s.Id, s.Name, s.StartTime, s.ProposedDate)).ToListAsync();

        return sessions;
    }

    [HttpGet("pdf")]
    [Produces(MediaTypeNames.Application.Pdf)]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPdf([FromServices] DataContext dataContext)
    {
        var sessions = await dataContext.Sessions.OrderBy(s => s.StartTime)
            .ToListAsync();

        var output = new MemoryStream();
        var document = new Document(PageSize.A4);
        var writer = PdfWriter.GetInstance(document, output);

        document.Open();

        var table = new PdfPTable(2);

        var cell = new PdfPCell(new Phrase("Sessions"))
        {
            Colspan = 2,
            HorizontalAlignment = 1 //0=Left, 1=Centre, 2=Right
        };

        table.AddCell(cell);

        foreach (var session in sessions)
        {
            table.AddCell(session.Name);

            var clientDateTime = TimeZoneInfo.ConvertTimeFromUtc(session.StartTime, clientContextAccessor.ClientContext.TimeZone);
            table.AddCell(clientDateTime.ToString("dd/MM/yyyy HH:mm"));
        }

        document.Add(table);

        document.Close();
        writer.Close();

        output.Position = 0;
        return File(output, MediaTypeNames.Application.Pdf, "Sessions.pdf");
    }
}

public record class Session(Guid Id, string Name, DateTime StartTime, DateTime ProposedDate);