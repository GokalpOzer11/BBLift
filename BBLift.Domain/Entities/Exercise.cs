using BBLift.Domain.Common;
using BBLift.Domain.Enums;

namespace BBLift.Domain.Entities;

public class Exercise : EntityBase
{
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? GifUrl { get; set; }

    public Guid MuscleGroupId { get; set; }
    public MuscleGroup? MuscleGroup { get; set; }
    
    public Guid EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }
    
    public DifficultyLevel Difficulty { get; set; }
    
    public ICollection<FavoriteExercise> FavoritedByUsers { get; set; } = new List<FavoriteExercise>();
}
