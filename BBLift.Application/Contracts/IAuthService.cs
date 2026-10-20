using BBLift.Application.Common;
using BBLift.Application.Dtos;

namespace BBLift.Application.Contracts;

public interface IAuthService
{
    Task<Result> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
    Task<Result<TokenModel>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
    Task<Result<TokenModel>> RefreshAsync(TokenModel request, CancellationToken cancellationToken = default);
    Task<Result> RevokeAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
    
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken cancellationToken = default);
}