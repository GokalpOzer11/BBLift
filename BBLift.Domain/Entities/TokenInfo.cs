using BBLift.Domain.Common;

namespace BBLift.Domain.Entities;

public class TokenInfo : EntityBase
{
    public string UserName { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime ExpiredAt { get; set; }
}