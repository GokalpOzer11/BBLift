using BBLift.Application.Common;
using BBLift.Application.Contracts; 
using BBLift.Application.Dtos;
using BBLift.Domain.Entities;
using BBLift.Infrastructure.Persistence.Contracts; 
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BBLift.Domain.Enums;
using BBLift.Domain.ValueObjects;

namespace BBLift.Application.Features;

public class AuthService(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    ITokenInfoRepository tokenInfoRepository,
    IValidator<SignUpRequest> signUpValidator,
    IValidator<SignInRequest> signInValidator,
    IWorkoutSessionService workoutSessionService,
    IWorkoutLogRepository workoutLogRepository

    )
    : IAuthService
{
    public async Task<Result> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await signUpValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return Result.Failure("Bu e-posta adresi ile kayıtlı bir kullanıcı zaten var.");

            var user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth,
                SecurityStamp = Guid.CreateVersion7().ToString(),
            };

            var userCreationResult = await userManager.CreateAsync(user, request.Password);

            if (!userCreationResult.Succeeded)
            {
                var errors = string.Join(", ", userCreationResult.Errors.Select(e => e.Description));
                return Result.Failure($"Kullanıcı oluşturulamadı: {errors}");
            }

            var roleAssignmentResult = await userManager.AddToRoleAsync(user, Roles.User);

            if (!roleAssignmentResult.Succeeded)
            {
                var errors = string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description));
                return Result.Failure($"Kullanıcı rolü atanamadı: {errors}");
            }
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<TokenModel>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = await signInValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result<TokenModel>.Failure(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var user = await userManager.FindByEmailAsync(request.Email);
            if(user is null)
                return Result<TokenModel>.Failure("Geçersiz e-posta veya şifre.");

            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if(!passwordValid)
                return Result<TokenModel>.Failure("Geçersiz e-posta veya şifre.");

            List<Claim> authClaims =
            [
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.CreateVersion7().ToString())
            ];

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
                authClaims.Add(new Claim("role", userRole));

            var token = tokenService.GenerateAccessToken(authClaims);
            var refreshToken = tokenService.GenerateRefreshToken();

            var tokenInfo = await tokenInfoRepository.GetAsync(predicate: x => x.UserName == user.UserName, ct: cancellationToken);
            if(tokenInfo is null)
            {
                tokenInfo = new TokenInfo
                {
                    UserName = user.UserName!,
                    Token = refreshToken,
                    ExpiredAt = DateTime.UtcNow.AddDays(7)
                };
                await tokenInfoRepository.AddAsync(tokenInfo);
            }
            else
            {
                tokenInfo.Token = refreshToken;
                tokenInfo.ExpiredAt = DateTime.UtcNow.AddDays(7);
                await tokenInfoRepository.UpdateAsync(tokenInfo);
            }
            return Result<TokenModel>.Success(new(token, refreshToken, [.. authClaims.Select(c => new ClaimModel(c.Type, c.Value))]));
        }
        catch (Exception ex)
        {
            return Result<TokenModel>.Failure("Giriş yapılırken bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result<TokenModel>> RefreshAsync(TokenModel request, CancellationToken cancellationToken = default)
    {
        try
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

            var tokenInfo = await tokenInfoRepository.GetAsync(predicate: x => x.UserName == principal.Identity!.Name, ct: cancellationToken);
            
            if(tokenInfo is null || !tokenInfo.Token.Equals(request.RefreshToken) || tokenInfo.ExpiredAt <= DateTime.UtcNow)
                return Result<TokenModel>.Failure("Geçersiz Refresh Token. Lütfen tekrar giriş yapın.");

            var newAccessToken = tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = tokenService.GenerateRefreshToken();

            tokenInfo.Token = newRefreshToken;
            tokenInfo.ExpiredAt = DateTime.UtcNow.AddDays(7);
            await tokenInfoRepository.UpdateAsync(tokenInfo);

            return Result<TokenModel>.Success(new(newAccessToken, newRefreshToken, [.. principal.Claims.Select(c => new ClaimModel(c.Type, c.Value))]));
        }
        catch (Exception ex)
        {
            return Result<TokenModel>.Failure("Token yenilenirken bir hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result> RevokeAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure("Kullanıcı bulunamadı.");

            var userName = await userManager.GetUserNameAsync(user);

            var tokenInfo = await tokenInfoRepository.GetAsync(predicate: x => x.UserName == userName, ct: cancellationToken);

            if(tokenInfo is null)
                return Result.Failure("Kullanıcıya ait aktif bir token bulunamadı.");

            tokenInfo.Token = string.Empty;
            tokenInfo.ExpiredAt = DateTime.UtcNow;
            await tokenInfoRepository.UpdateAsync(tokenInfo);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Token iptal edilirken bir hata oluştu: " + ex.Message);
        }
    }

    
   public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
{
    try
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result<UserProfileResponse>.Failure("Kullanıcı bulunamadı.");

        var sessionResult = await workoutSessionService.GetByUserIdAsync(userId, cancellationToken);

        var recentWorkouts = new List<RecentWorkoutResponse>();
        var totalVolume = 0m;

        if (sessionResult.IsSuccess && sessionResult.Data != null)
        {
            var sessions = sessionResult.Data.ToList();
            var sessionIds = sessions.Select(s => s.Id).ToList();

            var logs = await workoutLogRepository.GetListAsync(
                predicate: l => sessionIds.Contains(l.WorkoutSessionId) && l.Status != SetStatus.Warmup,
                ct: cancellationToken);

            var volumeBySession = logs
                .GroupBy(l => l.WorkoutSessionId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(l => (decimal)((l.Weight?.ToKg().Value ?? 0) * l.Reps)));

            recentWorkouts = sessions.Select(session => new RecentWorkoutResponse(
                session.Id,
                session.Name ?? string.Empty,
                session.StartTime,
                session.EndTime.HasValue
                    ? (int)(session.EndTime.Value - session.StartTime).TotalMinutes
                    : session.WorkOutDurationInSeconds / 60,
                volumeBySession.GetValueOrDefault(session.Id)
            )).ToList();

            totalVolume = recentWorkouts.Sum(w => w.TotalVolume);
        }

        var response = new UserProfileResponse(
            user.FirstName,
            user.LastName,
            user.UserName!,
            user.Email!,
            user.DateOfBirth,
            user.Weight?.Value ?? 0,
            user.Height?.Value ?? 0,
            recentWorkouts.Count,
            totalVolume,
            recentWorkouts.OrderByDescending(x => x.Date).Take(5).ToList()
        );

        return Result<UserProfileResponse>.Success(response);
    }
    catch (Exception ex)
    {
        return Result<UserProfileResponse>.Failure("Profil bilgileri alınırken hata oluştu: " + ex.Message);
    }
}

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure("Kullanıcı bulunamadı.");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            
            user.Weight = Weight.Create(request.Weight, WeightUnit.Kg); 
            user.Height = Height.Create(request.Height, HeightUnit.Cm); 

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                return Result.Failure($"Profil güncellenemedi: {errors}");
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Profil güncellenirken hata oluştu: " + ex.Message);
        }
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.NewPassword != request.ConfirmPassword)
                return Result.Failure("Yeni şifre ile doğrulama şifresi eşleşmiyor.");

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure("Kullanıcı bulunamadı.");

            var passwordResult = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            
            if (!passwordResult.Succeeded)
            {
                var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                return Result.Failure($"Şifre değiştirilemedi: {errors}");
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure("Şifre değiştirirken hata oluştu: " + ex.Message);
        }
    }
}