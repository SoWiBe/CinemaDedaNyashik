using System.ComponentModel.DataAnnotations;

namespace Back.Api.Models.Core;

/// <summary>
/// Базовая модель
/// </summary>
public abstract class BaseModel
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    [Key]
    public long Id { get; set; }
}