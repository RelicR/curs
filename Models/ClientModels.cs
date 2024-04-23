namespace WebApplication3.Models
{
    public class ClientModels
    {
        public Indiv? Indiv { get; set; }
        public Legal? Legal { get; set; }
        public Client? Client { get; set; }
        public List<Project>? ProjsList { get; set; }
        public List<Edit>? EditsList { get; set; }
        public Person User { get; set; }
    }
}
