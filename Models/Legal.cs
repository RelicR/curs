using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Legal
    {
        [Column(TypeName = "tinyint")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        //[HiddenInput]
        [DisplayFormat(DataFormatString = "LEG-{0:d3}", ApplyFormatInEditMode = true)]
        public byte Id {  get; set; }
        [Column("Тип", TypeName = "bit")]
        [DisplayName("Некоммерческое")]
        public bool Type { get; set; } = false;
        [Column("Наименование")]
        [MaxLength(50)]
        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Пожалуйста, укажите наименование")]
        public string Name { get; set; }
        [Column("Контакное лицо")]
        [MaxLength(50)]
        [DisplayName("Контакное лицо")]
        [Required(ErrorMessage = "Пожалуйста, укажите ФИО контактного лица")]
        public string ConPers {  get; set; }
        [Column("Телефон")]
        [MaxLength(12)]
        [DisplayName("Телефон")]
        [RegularExpression(@"[+]\d{11}", ErrorMessage = "Номер телефона должен иметь формат +7xxxxxxxxxx")]
        [Required(ErrorMessage = "Пожалуйста, укажите телефон контактного лица")]
        public string Phone { get; set; }
        [Column("ИНН")]
        [MaxLength(12)]
        [DisplayName("ИНН")]
        [RegularExpression(@"\d{12}", ErrorMessage = "ИНН должен иметь формат 12-ти цифр без разделителей")]
        [Required(ErrorMessage = "Пожалуйста, укажите ИНН")]
        public string INN { get; set; }
        [Column("ОГРН")]
        [MaxLength(15)]
        [DisplayName("ОГРН")]
        [RegularExpression(@"\d{15}", ErrorMessage = "ОГРН должен иметь формат 15-ти цифр без разделителей")]
        [Required(ErrorMessage = "Пожалуйста, укажите ОГРН")]
        public string OGRN { get; set; }

        public Client Client { get; set; }
    }
    //public class ViewLegal
    //{
    //    public Legal legal { get; set; } = null!;
    //    public IEnumerable<Legal>? legalsList { get; set; } = null!;
    //}
}
