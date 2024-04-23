using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Person
    {
        [DisplayName("Роль")]
        public List<string> RolesList { get; } = new List<string> { "Администратор", "Клиент", "Архитектор" };
        public string? Role { get; set; }
        [RegularExpression(@"((CLT-|EMP-){1}\d{1,3})|(admin)", ErrorMessage = "UID должен быть формата CLT-xxx или EMP-xxx")]
        public string? UID { get; set; }
        public byte? Id { get; set; }
    }
}
