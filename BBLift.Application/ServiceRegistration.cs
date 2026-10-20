using System.Reflection;
using BBLift.Application.Contracts;
using BBLift.Application.Features;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BBLift.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        services.AddValidatorsFromAssembly(assembly);
        
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IFavoriteExerciseService, FavoriteExerciseService>();
        services.AddScoped<IMembershipPackageService, MembershipPackageService>();
        services.AddScoped<IMuscleGroupService, MuscleGroupService>();
        services.AddScoped<IRoutineService, RoutineService>();
        services.AddScoped<IRoutineExerciseService, RoutineExerciseService>();
        services.AddScoped<IWorkoutLogService, WorkoutLogService>();
        services.AddScoped<IWorkoutSessionService, WorkoutSessionService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICartItemService, CartItemService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }
}