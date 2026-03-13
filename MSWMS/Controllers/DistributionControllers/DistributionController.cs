using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MSWMS.Entities.Distributions;
using MSWMS.Hubs;
using MSWMS.Infrastructure.Authorization;
using MSWMS.Interfaces;
using MSWMS.Models.DTO.Requests;
using MSWMS.Models.DTO.Responses.Distributions;
using MSWMS.Models.DTO.Soap.Responses;
using MSWMS.Repositories.Interfaces;
using MSWMS.Services.Interfaces;

namespace MSWMS.Controllers;

[Route("api/distributions")]
[ApiController]
public class DistributionController : ControllerBase
{
    private readonly IDistributionService _distributionService;
    private readonly IDistributionDocumentRepository _distributionDocumentRepository;
    private readonly IAsyncRepository<DistributionScan> _distributionScanRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHubContext<ScanHub> _hubContext;

    public DistributionController(
        IDistributionService distributionService,
        IDistributionDocumentRepository distributionDocumentRepository,
        IAsyncRepository<DistributionScan> distributionScanRepository,
        IUserRepository userRepository,
        IHubContext<ScanHub> hubContext)
    {
        _distributionService = distributionService;
        _distributionDocumentRepository = distributionDocumentRepository;
        _distributionScanRepository = distributionScanRepository;
        _userRepository = userRepository;
        _hubContext = hubContext;
    }

    // POST: /api/distributions
    [HttpPost]
    public async Task<ActionResult<DistributionDto>> CreateDistribution([FromBody] CreateDistributionRequest request)
    {
        var distribution = await _distributionService.CreateDistributionAsync(request);
        var dto = await _distributionService.GetDistributionDtoByIdAsync(distribution.Id);
        return CreatedAtAction(nameof(GetDistributionById), new { id = distribution.Id }, dto);
    }

    // GET: /api/distributions/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DistributionDto>> GetDistributionById([FromRoute] int id)
    {
        var distribution = await _distributionService.GetDistributionDtoByIdAsync(id);
        if (distribution is null)
        {
            return NotFound();
        }

        return distribution;
    }

    // DELETE: /api/distributions/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDistribution([FromRoute] int id)
    {
        var deleted = await _distributionService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    // PATCH: /api/distributions/{id}/note
    [HttpPatch("{id:int}/note")]
    public async Task<ActionResult<DistributionDto>> UpdateDistributionNote(
        [FromRoute] int id,
        [FromBody] UpdateDistributionNoteRequest request)
    {
        var distribution = await _distributionService.UpdateNoteAsync(id, request.Note ?? string.Empty);
        if (distribution is null)
        {
            return NotFound();
        }

        var dto = await _distributionService.GetDistributionDtoByIdAsync(distribution.Id);
        return dto is null ? NotFound() : Ok(dto);
    }

    // POST: /api/distributions/{id}/documents
    [HttpPost("{id:int}/documents")]
    public async Task<ActionResult<IReadOnlyList<DistributionDocumentDto>>> AddDocuments(
        [FromRoute] int id,
        [FromBody] List<CreateDistributionDocumentRequest> documents)
    {
        if (documents.Count == 0)
        {
            return BadRequest("Documents list is empty.");
        }

        foreach (var document in documents)
        {
            document.DistributionId = id;
            document.OriginId = 1; // User.Location
        }

        await _distributionService.AddDocumentsAsync(documents);
        var result = await _distributionService.GetDocumentsDtoAsync(id);
        return Ok(result);
    }

    // GET: /api/distributions/{id}/documents
    [HttpGet("{id:int}/documents")]
    public async Task<ActionResult<IReadOnlyList<DistributionDocumentDto>>> GetDocuments([FromRoute] int id)
    {
        var distribution = await _distributionService.GetDistributionDtoByIdAsync(id);
        if (distribution is null)
        {
            return NotFound();
        }

        var documents = await _distributionService.GetDocumentsDtoAsync(id);
        return Ok(documents);
    }

    // GET: /api/distributions/{id}/documents/{docId}
    [HttpGet("{id:int}/documents/{docId:int}")]
    public async Task<ActionResult<DistributionDocumentDto>> GetDocumentById([FromRoute] int id, [FromRoute] int docId)
    {
        var document = await _distributionService.GetDocumentDtoByIdAsync(id, docId);
        if (document is null)
        {
            return NotFound();
        }

        return Ok(document);
    }

    // GET: /api/distributions/{id}/documents/items
    [HttpGet("{id:int}/documents/items")]
    public async Task<ActionResult<IReadOnlyList<DistributionItemDto>>> GetDistributionItems([FromRoute] int id)
    {
        var distribution = await _distributionService.GetDistributionDtoByIdAsync(id);
        if (distribution is null)
        {
            return NotFound();
        }

        var items = await _distributionService.GetDistributionItemsDtoAsync(id);
        return Ok(items);
    }

    // GET: /api/distributions/{id}/documents/{docId}/items
    [HttpGet("{id:int}/documents/{docId:int}/items")]
    public async Task<ActionResult<IReadOnlyList<DistributionItemDto>>> GetDocumentItems(
        [FromRoute] int id,
        [FromRoute] int docId)
    {
        var document = await _distributionService.GetDocumentDtoByIdAsync(id, docId);
        if (document is null)
        {
            return NotFound();
        }

        var items = await _distributionService.GetDistributionItemsDtoAsync(id);
        var result = items.Where(i => i.DocumentId == docId).ToList();
        return Ok(result);
    }

    // GET: /api/distributions/{id}/documents/{docId}/items/{itemId}
    [HttpGet("{id:int}/documents/{docId:int}/items/{itemId:int}")]
    public async Task<ActionResult<DistributionItemDto>> GetDocumentItemById(
        [FromRoute] int id,
        [FromRoute] int docId,
        [FromRoute] int itemId)
    {
        var document = await _distributionService.GetDocumentDtoByIdAsync(id, docId);
        if (document is null)
        {
            return NotFound();
        }

        var items = await _distributionService.GetDistributionItemsDtoAsync(id);
        var item = items.FirstOrDefault(i => i.DocumentId == docId && i.Id == itemId);
        return item is null ? NotFound() : Ok(item);
    }

    // DELETE: /api/distributions/{id}/documents/{docId}
    [HttpDelete("{id:int}/documents/{docId:int}")]
    public async Task<IActionResult> DeleteDocument([FromRoute] int id, [FromRoute] int docId)
    {
        var removed = await _distributionService.RemoveDocumentAsync(id, docId);
        return removed ? NoContent() : NotFound();
    }

    // PATCH: /api/distributions/{id}/documents/{docId}
    [HttpPatch("{id:int}/documents/{docId:int}")]
    public async Task<ActionResult<DistributionDocumentDto>> UpdateDocument(
        [FromRoute] int id,
        [FromRoute] int docId,
        [FromBody] UpdateDistributionDocumentRequest request)
    {
        var document = await _distributionDocumentRepository.GetByDistributionAndDocumentIdAsync(id, docId);
        if (document is null)
        {
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(request.DocumentNumber))
        {
            document.DocumentNumber = request.DocumentNumber;
        }

        if (!string.IsNullOrWhiteSpace(request.OrderNumber))
        {
            document.OrderNumber = request.OrderNumber;
        }

        if (request.OriginId.HasValue)
        {
            document.OriginId = request.OriginId.Value;
        }

        /*if (request.DestinationId.HasValue)
        {
            document.DestinationId = request.DestinationId.Value;
        }*/

        await _distributionDocumentRepository.UpdateAsync(document);
        var dto = await _distributionService.GetDocumentDtoByIdAsync(id, docId);
        return dto is null ? NotFound() : Ok(dto);
    }

    // GET: /api/distributions/{id}/scans
    [HttpGet("{id:int}/scans")]
    public async Task<ActionResult<IReadOnlyList<DistributionScanDto>>> GetScans([FromRoute] int id)
    {
        var distribution = await _distributionService.GetDistributionDtoByIdAsync(id);
        if (distribution is null)
        {
            return NotFound();
        }

        var scans = await _distributionService.GetScansDtoAsync(id);
        return Ok(scans);
    }

    // GET: /api/distributions/{id}/scans/{scanId}
    [HttpGet("{id:int}/scans/{scanId:int}")]
    public async Task<ActionResult<DistributionScanDto>> GetScanById([FromRoute] int id, [FromRoute] int scanId)
    {
        var scan = await _distributionService.GetScanDtoByIdAsync(id, scanId);
        if (scan is null)
        {
            return NotFound();
        }

        return Ok(scan);
    }

    // POST: /api/distributions/{id}/scans
    [HttpPost("{id:int}/scans")]
    public async Task<ActionResult<DistributionScanDto>> ProceedScan(
        [FromRoute] int id,
        [FromBody] DistributionScanRequest request)
    {
        request.DistributionId = id;
        var scan = await _distributionService.ProceedScanAsync(request);
        return Ok(scan);
    }

    // DELETE: /api/distributions/{id}/scans/{scanId}
    [HttpDelete("{id:int}/scans/{scanId:int}")]
    public async Task<IActionResult> DeleteScan([FromRoute] int id, [FromRoute] int scanId)
    {
        var scan = await _distributionScanRepository.GetByIdAsync(scanId);
        if (scan is null || scan.DistributionId != id)
        {
            return NotFound();
        }

        await _distributionScanRepository.DeleteAsync(scanId);
        return NoContent();
    }
}
