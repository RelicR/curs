using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Indiv
    {
        [Column(TypeName = "tinyint")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        //[HiddenInput]
        [DisplayFormat(DataFormatString = "IND-{0:d3}", ApplyFormatInEditMode = true)]
        public byte Id { get; set; }
        [Column("Тип", TypeName = "bit")]
        [DisplayName("ИП")]
        public bool Type { get; set; } = false;
        [Column("Фамилия")]
        [MaxLength(25)]
        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Пожалуйста, укажите фамилию")]
        public string Surname { get; set; }
        [Column("Имя")]
        [MaxLength(25)]
        [DisplayName("Имя")]
        [Required(ErrorMessage = "Пожалуйста, укажите имя")]
        public string Name { get; set; }
        [Column("Отчество")]
        [MaxLength(25)]
        [DisplayName("Отчество")]
        public string? Midname { get; set; }
        [Column("Телефон")]
        [MaxLength(12)]
        [DisplayName("Телефон")]
        [RegularExpression(@"[+]\d{11}", ErrorMessage = "Номер телефона должен иметь формат +7xxxxxxxxxx")]
        [Required(ErrorMessage = "Пожалуйста, укажите телефон")]
        public string Phone { get; set; }
        [Column("ИНН")]
        [MaxLength(12)]
        [DisplayName("ИНН")]
        [RegularExpression(@"\d{12}", ErrorMessage = "ИНН должен иметь формат 12-ти цифр без разделителей")]
        [Required(ErrorMessage = "Пожалуйста, укажите ИНН")]
        public string INN { get; set; }

        public Client Client { get; set; }
    }
    //public class ViewIndiv
    //{
    //    public Indiv indiv { get; set; } = null!;
    //    public IEnumerable<Indiv>? indivsList { get; set; } = null!;
    //}
}
