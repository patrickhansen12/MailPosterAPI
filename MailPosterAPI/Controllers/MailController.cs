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
        [FromBody] SendMailRequest request,
        [FromHeader(Name = "X-User-Email")] string senderEmail)
    {
        await _mailService.SendAsync(request, senderEmail);
        return Ok();
    }
}