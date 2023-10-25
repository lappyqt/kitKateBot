using kitKateBot.Domain.Enums;

namespace kitKateBot.Domain.Entities;

public class AuthorizationHistory : BaseEntity
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = string.Empty;
    public long ExpiresIn { get; set; }
    public string ProviderName { get; set; } = string.Empty;
    public AuthAction AuthAction { get; set; }
    public DateTime AuthorizedAt { get; set; }
}