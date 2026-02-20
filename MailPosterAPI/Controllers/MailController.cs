using MailPosterAPI.DTOs;
using MailPosterAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MailPosterAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MailController : ControllerBase
{
    private readonly IMailService _mailService;

    public MailController(IMailService mailService)
    {
        _mailService = mailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMail(
        [FromBody] SendMailRequest dto,
        [FromHeader(Name = "X-User-Id")] string userId,
        [FromHeader(Name = "X-User-Email")] string userEmail)
    {
        if (string.IsNullOrWhiteSpace(userId) ||
            string.IsNullOrWhiteSpace(userEmail))
            return BadRequest("Missing user headers.");

        var result = await _mailService.SendAsync(dto, userId, userEmail);

        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        return Ok();
    }
    [HttpGet]
    public async Task<IActionResult> GetByUser([FromQuery] string userEmail)
    {
        if (string.IsNullOrWhiteSpace(userEmail))
            return BadRequest("Email is required.");

        var emails = await _mailService.GetByUserAsync(userEmail);

        return Ok(emails);
    }
}