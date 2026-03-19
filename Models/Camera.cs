using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RtspViewer.Models;

[Table("MonitoringPost", Schema = "public")]
public class MonitoringPost
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    [Column("Name")]
    public string Name { get; set; } = string.Empty;

    [Column("Description")]
    public string? Description { get; set; }

    [Column("Longitude")]
    [Range(-180.0, 180.0, ErrorMessage = "Долгота должна быть от -180 до 180")]
    public double? Longitude { get; set; }

    [Column("Latitude")]
    [Range(-90.0, 90.0, ErrorMessage = "Широта должна быть от -90 до 90")]
    public double? Latitude { get; set; }

    [Column("IsMobile")]
    public bool IsMobile { get; set; } = false;

    [Column("IsActive")]
    public bool IsActive { get; set; } = true;

    [Column("CreatedAt")]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("UpdatedAt")]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    // Навигационное свойство
    public virtual ICollection<Camera> Cameras { get; set; } = new List<Camera>();
}

[Table("Snapshots", Schema = "public")]
public class Snapshot
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CameraId { get; set; }

    [ForeignKey("CameraId")]
    public virtual Camera? Camera { get; set; }

    [Required]
    public string FilePath { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

[Table("Cameras", Schema = "public")]
public class Camera
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Название камеры")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Место установки")]
    public string? InstallationLocation { get; set; }

    [Display(Name = "Запрос API")]
    public string? ApiRequest { get; set; }

    [Required]
    [Display(Name = "Ссылка на видеопоток")]
    public string RtspUrl { get; set; } = string.Empty;

    [Display(Name = "Серийный номер")]
    public string? SerialNumber { get; set; }

    [Display(Name = "Логин")]
    public string? Username { get; set; }

    [Display(Name = "Пароль")]
    public string? Password { get; set; }

    [Display(Name = "URL-адрес")]
    public string? WebUrl { get; set; }

    [Display(Name = "Пикет")]
    public string? Picket { get; set; }

    [Display(Name = "Id комплекса в системе FDA")]
    public string? FdaId { get; set; }

    [Display(Name = "Интервал опроса, сек")]
    public int PollingInterval { get; set; } = 10;

    [Display(Name = "Пост мониторинга")]
    public int? MonitoringPostId { get; set; }

    [ForeignKey("MonitoringPostId")]
    public virtual MonitoringPost? MonitoringPost { get; set; }

    [Display(Name = "Поддержка PTZ")]
    public bool IsPtzSupported { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
