using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Employee
    {
        [Column(TypeName = "tinyint")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput]
        [DisplayFormat(DataFormatString = "EMP-{0:d3}", ApplyFormatInEditMode = true)]
        public byte Id { get; set; }
        [Column("UID")]
        [DisplayName("UID")]
        [MaxLength(7)]
        [RegularExpression(@"(IND-){1}\d{1,3}", ErrorMessage = "UID должен иметь формат IND-xxx")]
        [Required(ErrorMessage = "Пожалуйста, укажите UID физ. лица")]
        public string UID { get; set; }
        [Column("Сфера деятельности")]
        [MaxLength(50)]
        [DisplayName("Сфера деятельности")]
        [Required(ErrorMessage = "Пожалуйста, укажите сферу деятельности")]
        public string Area { get; set; }
        [Column("Проекты", TypeName = "nvarchar(max)")]
        [DisplayName("Проекты")]
        [RegularExpression(@"(\b\d+\b )*(\b\d+\b)", ErrorMessage = "Номера проектов перечисляются через пробел")]
        [HiddenInput]
        public string? Projects { get; set; }
        [Column("Зарплата", TypeName = "money")]
        [DisplayName("Зарплата")]
        [RegularExpression(@"(\b\d+\b)", ErrorMessage = "Поле допускает только цифры")]
        [Required(ErrorMessage = "Пожалуйста, укажите размер зарплаты")]
        public double Salary { get; set; }
        [Column("Отпуск", TypeName = "bit")]
        [DisplayName("Отпуск")]
        public bool Vac { get; set; } = false;
        [Column("Отпуск начало", TypeName = "DateTime2")]
        [DisplayName("Отпуск начало")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? VacStart { get; set; }
        [Column("Отпуск конец", TypeName = "DateTime2")]
        [DisplayName("Отпуск конец")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? VacEnd { get; set; }

        public Indiv Indiv { get; set; }
        [ScaffoldColumn(false)]
        [ForeignKey("Party"), Column(Order = 0)]
        public byte? IndivId { get; set; }

        //public Project Project { get; set; }
        public List<Project>? Project { get; set; }
    }
    //public class ViewEmpl
    //{
    //    public Employee empl { get; set; } = null!;
    //    public IEnumerable<Employee>? emplsList { get; set; } = null!;
    //}
}
