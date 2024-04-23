using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Client
    {
        [Column(TypeName = "tinyint")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        //[HiddenInput]
        [DisplayFormat(DataFormatString = "CLT-{0:d3}", ApplyFormatInEditMode = true)]
        public byte Id { get; set; }
        [Column("UID")]
        [DisplayName("UID")]
        [MaxLength(7)]
        [RegularExpression(@"(IND-|LEG-){1}\d{1,3}", ErrorMessage = "UID должен иметь формат IND-xxx или LEG-xxx")]
        [Required(ErrorMessage = "Пожалуйста, укажите UID клиента")]
        public string UID { get; set; }
        [Column("Заказы", TypeName = "nvarchar(max)")]
        [DisplayName("Заказы")]
        [RegularExpression(@"(\b\d+\b )*(\b\d+\b)", ErrorMessage = "Номера заказов перечисляются через пробел")]
        [HiddenInput]
        public string? Orders { get; set; }

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
    //public class ViewClient
    //{
    //    public Client client { get; set; } = null!;
    //    public IEnumerable<Client>? clientsList { get; set; } = null!;
    //}
}
