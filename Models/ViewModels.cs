using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Models
{
    //public class ViewModels
    //{
    //    public ViewIndiv viewind { get; set; } = null!;
    //    public ViewLegal viewleg { get; set; } = null!;
    //    public ViewClient viewcl { get; set; } = null!;
    //    public ViewEmpl viewemp { get; set; } = null!;
    //    public ViewSubc viewsubc { get; set; } = null!;
    //    public ViewProj viewprj { get; set; } = null!;
    //    public ViewEdit viewedt { get; set; } = null!;
    //    public string Target { get; set; } = null!;
    //}

    public class ViewModels
    {
        public Indiv indiv { get; set; } = null!;
        public IEnumerable<Indiv>? indivsList { get; set; } = null!;
        public Legal legal { get; set; } = null!;
        public IEnumerable<Legal>? legalsList { get; set; } = null!;
        public Employee empl { get; set; } = null!;
        public IEnumerable<Employee>? emplsList { get; set; } = null!;
        public Client client { get; set; } = null!;
        public IEnumerable<Client>? clientsList { get; set; } = null!;
        public Subcon subc { get; set; } = null!;
        public IEnumerable<Subcon>? subcsList { get; set; } = null!;
        public Project proj { get; set; } = null!;
        public IEnumerable<Project>? projsList { get; set; } = null!;
        public Edit edit { get; set; } = null!;
        public IEnumerable<Edit>? editsList { get; set; } = null!;
        [HiddenInput]
        public string Target { get; set; } = null!;

    }
}
