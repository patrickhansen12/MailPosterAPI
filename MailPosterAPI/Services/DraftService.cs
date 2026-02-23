using MailPosterAPI.Data;
using MailPosterAPI.DTOs;
using MailPosterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MailPosterAPI.Services;

public class DraftService : IDraftService
{
    private readonly ApplicationDbContext _context;

    public DraftService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DraftResponseDto> CreateAsync(CreateDraftRequestDto dto, string userEmail)
    {
        var draft = new Draft
        {
            Id = Guid.NewGuid(),
            UserEmail = userEmail,
            RecipientEmail = dto.RecipientEmail,
            Subject = dto.Subject,
            Body = dto.Body,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Drafts.Add(draft);
        await _context.SaveChangesAsync();

        return MapToResponse(draft);
    }

    public async Task<List<DraftResponseDto>> GetByUserAsync(string userEmail)
    {
        return await _context.Drafts
            .Where(d => d.UserEmail == userEmail)
            .OrderByDescending(d => d.UpdatedAt)
            .Select(d => MapToResponse(d))
            .ToListAsync();
    }

    public async Task<DraftResponseDto?> UpdateAsync(Guid id, UpdateDraftRequestDto dto, string userEmail)
    {
        var draft = await _context.Drafts
            .FirstOrDefaultAsync(d => d.Id == id && d.UserEmail == userEmail);

        if (draft == null)
            return null;

        draft.RecipientEmail = dto.RecipientEmail;
        draft.Subject = dto.Subject;
        draft.Body = dto.Body;
        draft.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponse(draft);
    }

    public async Task<bool> DeleteAsync(Guid id, string userEmail)
    {
        var draft = await _context.Drafts
            .FirstOrDefaultAsync(d => d.Id == id && d.UserEmail == userEmail);

        if (draft == null)
            return false;

        _context.Drafts.Remove(draft);
        await _context.SaveChangesAsync();

        return true;
    }

    private static DraftResponseDto MapToResponse(Draft draft) =>
        new()
        {
            Id = draft.Id,
            RecipientEmail = draft.RecipientEmail,
            Subject = draft.Subject,
            Body = draft.Body,
            CreatedAt = draft.CreatedAt,
            UpdatedAt = draft.UpdatedAt
        };
}