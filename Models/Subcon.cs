using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Subcon
    {
        [Column(TypeName = "tinyint")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [HiddenInput]
        [DisplayFormat(DataFormatString = "SUB-{0:d3}", ApplyFormatInEditMode = true)]
        public byte Id { get; set; }
        [Column("UID")]
        [DisplayName("UID")]
        [MaxLength(7)]
        [RegularExpression(@"(IND-|LEG-){1}\d{1,3}", ErrorMessage = "UID должен иметь формат IND-xxx или LEG-xxx")]
        [Required(ErrorMessage = "Пожалуйста, укажите UID юр. или физ. лица")]
        public string UID { get; set; }
        [Column("Сфера деятельности")]
        [MaxLength(50)]
        [DisplayName("Сфера деятельности")]
        [Required(ErrorMessage = "Пожалуйста, укажите сферу деятельности")]
        public string Area { get; set; }
        [Column("Район")]
        [MaxLength(50)]
        [DisplayName("Район")]
        [Required(ErrorMessage = "Пожалуйста, укажите район деятельности")]
        public string Zone { get; set; }
        [Column("Договор #", TypeName = "smallint")]
        [DisplayName("Договор #")]
        [Range(1, 29999, ErrorMessage = "Номер договора может быть больше 1 и меньше 30000")]
        [DisplayFormat(DataFormatString = "{0:d4}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Пожалуйста, укажите номер договора")]
        public short Contract { get; set; }

        public Indiv Indiv { get; set; }
        [ScaffoldColumn(false)]
        [ForeignKey("Party"), Column(Order = 0)]
        public byte? IndivId { get; set; }
        public Legal Legal { get; set; }
        [ScaffoldColumn(false)]
        [ForeignKey("Party"), Column(Order = 1)]
        public byte? LegalId { get; set; }

        //public Project Project { get; set; }
        public List<Project>? Project { get; set; }
    }
    //public class ViewSubc
    //{
    //    public Subcon subc { get; set; } = null!;
    //    public IEnumerable<Subcon>? subcsList { get; set; } = null!;
    //}
}
