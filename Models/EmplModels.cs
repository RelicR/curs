namespace WebApplication3.Models
{
    public class EmplModels
    {
        public Indiv? Indiv { get; set; }
        public Employee? Employee { get; set; }
        public List<Project>? ProjsList { get; set; }
        public List<Edit>? EditsList { get; set; }
        public List<Subcon>? SubconsList { get; set; }
        public Person User { get; set; } = null!;
    }
}
