using System.ComponentModel.DataAnnotations;
using Back.Api.Models.Core;

namespace Back.Api.Models;

/// <summary>
/// Модель для представления фильма или сериала
/// </summary>
public class MediaContent : BaseModel
{
    /// <summary>
    /// Название
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
    /// <summary>
    /// Описание
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    /// <summary>
    /// Рейтинг
    /// </summary>
    public decimal? Rating { get; set; }
    /// <summary>
    /// Жанр(ы)
    /// </summary>
    public string? Genres { get; set; }
}