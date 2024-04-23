using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Edit
    {
        //[Column(TypeName = "tinyint")]
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[HiddenInput]
        //[DisplayFormat(DataFormatString = "EDT-{0:d3}", ApplyFormatInEditMode = true)]
        //public byte Id { get; set; }

        //[Column("Проект", TypeName = "tinyint")]
        //[DisplayName("Проект")]
        //[Key]
        //[Required(ErrorMessage = "Пожалуйста, укажите ID проекта")]
        //public byte ProjId {  get; set; }

        [Column("Причина")]
        [MaxLength(50)]
        [DisplayName("Причина")]
        public string? Reason { get; set; }
        [Column("Описание", TypeName = "text")]
        [DisplayName("Описание")]
        [UIHint("MultilineText")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
        [Column("Изменение бюджета", TypeName = "money")]
        [DisplayName("Изменение бюджета")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)]
        public double? EditedBudget { get; set; }
        [Column("Время", TypeName = "DateTime2")]
        [DisplayName("Время")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Пожалуйста, укажите дату правок")]
        [HiddenInput]
        public DateTime EditDate { get; set; } = DateTime.Now;

        [Column("Правки проекта", TypeName = "text")]
        [DisplayName("Правки проекта")]
        [HiddenInput]
        //[UIHint("MultilineText")]
        [DataType(DataType.MultilineText)]
        public string? Total { get; set; }
        [DisplayName("Проект")]
        [Column("Проект", Order = 0, TypeName = "tinyint")]
        [Key]
        [Required(ErrorMessage = "Пожалуйста, укажите ID проекта")]
        public byte Id { get; set; }
        [ForeignKey("Id")]
        public Project Project { get; set; }
        
        
    }
    //public class ViewEdit
    //{
    //    public Edit edit { get; set; } = null!;
    //    public IEnumerable<Edit>? editsList { get; set; } = null!;
    //}
}
