using MailPosterAPI.DTOs;
using MailPosterAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MailPosterAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DraftController : ControllerBase
{
    private readonly IDraftService _draftService;

    public DraftController(IDraftService draftService)
    {
        _draftService = draftService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDraftRequestDto dto,
        [FromHeader(Name = "X-User-Email")] string userEmail)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
            return BadRequest("Missing user email header.");

        var result = await _draftService.CreateAsync(dto, userEmail);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByUser(
        [FromHeader(Name = "X-User-Email")] string userEmail)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
            return BadRequest("Missing user email header.");

        var drafts = await _draftService.GetByUserAsync(userEmail);
        return Ok(drafts);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDraftRequestDto dto,
        [FromHeader(Name = "X-User-Email")] string userEmail)
    {
        var result = await _draftService.UpdateAsync(id, dto, userEmail);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromHeader(Name = "X-User-Email")] string userEmail)
    {
        var success = await _draftService.DeleteAsync(id, userEmail);

        if (!success)
            return NotFound();

        return NoContent();
    }
}