using System.ComponentModel.DataAnnotations;

namespace DnDemo.Models
{
    public class Fighter
    {
        [Required]
        public string Name {get;set;}
    }
}