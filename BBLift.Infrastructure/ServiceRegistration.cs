using BBLift.Infrastructure.Persistence.Contracts;
using BBLift.Infrastructure.Persistence.Data;
using BBLift.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BBLift.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options => 
        {
            options.UseSqlServer(configuration.GetConnectionString("DbConn"));
        });
        
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IEquipmentRepository, EquipmentRepository>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IFavoriteExerciseRepository, FavoriteExerciseRepository>();
        services.AddScoped<IMembershipPackageRepository, MembershipPackageRepository>();
        services.AddScoped<IMuscleGroupRepository, MuscleGroupRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRoutineExerciseRepository, RoutineExerciseRepository>();
        services.AddScoped<IRoutineRepository, RoutineRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ITokenInfoRepository, TokenInfoRepository>();
        services.AddScoped<IWorkoutLogRepository, WorkoutLogRepository>();
        services.AddScoped<IWorkoutSessionRepository, WorkoutSessionRepository>();

        return services;
    }
}