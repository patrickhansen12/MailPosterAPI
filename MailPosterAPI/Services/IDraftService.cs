using MailPosterAPI.DTOs;

namespace MailPosterAPI.Services;

public interface IDraftService
{
    Task<DraftResponseDto> CreateAsync(CreateDraftRequestDto dto, string userEmail);
    Task<List<DraftResponseDto>> GetByUserAsync(string userEmail);
    Task<DraftResponseDto?> UpdateAsync(Guid id, UpdateDraftRequestDto dto, string userEmail);
    Task<bool> DeleteAsync(Guid id, string userEmail);
}