using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    [Index(nameof(ClientId), IsUnique = false)]
    [Index(nameof(EmployeeId), IsUnique = false)]
    [Index(nameof(SubconId), IsUnique = false)]
    public class Project
    {
        [Column(TypeName = "tinyint")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        //[HiddenInput]
        //[DisplayFormat(DataFormatString = "PRT-{0:d3}", ApplyFormatInEditMode = true)]
        public byte Id { get; set; }
        [Column("Тип")]
        [MaxLength(30)]
        [DisplayName("Тип")]
        [Required(ErrorMessage = "Пожалуйста, укажите тип проекта")]
        public string Type { get; set; }
        [Column("Адрес")]
        [MaxLength(50)]
        [DisplayName("Адрес")]
        [Required(ErrorMessage = "Пожалуйста, укажите адрес расположения здания")]
        public string Address { get; set; }
        [Column("Дата начала", TypeName = "DateTime2")]
        [DisplayName("Дата начала")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Пожалуйста, укажите дату начала работы с проектом")]
        public DateTime StartDate { get; set; } = DateTime.Today;
        [Column("Комментарий", TypeName = "text")]
        [DisplayName("Комментарий")]
        public string? Description { get; set; }
        [Column("Бюджет", TypeName = "money")]
        [DisplayName("Бюджет")]
        [Required(ErrorMessage = "Пожалуйста, укажите бюджет проекта")]
        public double Budget { get; set; }
        [Column("Наличие правок", TypeName = "bit")]
        [DisplayName("Наличие правок")]
        [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]
        [HiddenInput]
        public bool IsEdit { get; set; } = false;
        [Column("Заказчик")]
        [DisplayName("Заказчик")]
        [MaxLength(7)]
        [RegularExpression(@"(CLT-){1}\d{1,3}", ErrorMessage = "ID должен иметь формат CLT-xxx")]
        [Required(ErrorMessage = "Пожалуйста, укажите ID заказчика")]
        public string ClientUID { get; set; }
        [Column("Архитектор")]
        [DisplayName("Архитектор")]
        [MaxLength(7)]
        [RegularExpression(@"(EMP-){1}\d{1,3}", ErrorMessage = "ID должен иметь формат EMP-xxx")]
        [Required(ErrorMessage = "Пожалуйста, укажите ID ответственного за проект")]
        public string ArchUID { get; set; }
        [Column("Подрядчик")]
        [DisplayName("Подрядчик")]
        [MaxLength(7)]
        [RegularExpression(@"(SUB-){1}\d{1,3}", ErrorMessage = "UID должен иметь формат SUB-xxx")]
        [Required(ErrorMessage = "Пожалуйста, укажите ID подрядчика")]
        public string SubcUID { get; set; }
        [Column("Гос. заказ", TypeName = "bit")]
        [DisplayName("Гос. заказ")]
        public bool Gos { get; set; } = false;

        public Client? Client { get; set; }
        [ScaffoldColumn(false)]
        [ForeignKey("ClientId")]
        public byte? ClientId { get; set; } = null!;
        public Employee? Employee { get; set; }
        [ScaffoldColumn(false)]
        [ForeignKey("EmployeeId")]
        public byte? EmployeeId { get; set; }
        public Subcon? Subcon { get; set; }
        [ScaffoldColumn(false)]
        [ForeignKey("SubconId")]
        public byte? SubconId { get; set; }

        public Edit Edit { get; set; }
    }
    //public class ViewProj
    //{
    //    public Project proj { get; set; } = null!;
    //    public IEnumerable<Project>? projsList { get; set; } = null!;
    //}
}
