using System.ComponentModel.DataAnnotations;

namespace Overflow.Services.QuestionsService.Models;

public class Tag
{
    [MaxLength(36)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(255)]
    public required string Slug { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}
