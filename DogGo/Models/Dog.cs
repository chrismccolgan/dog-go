using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }

        [MaxLength(35)]
        [Required(ErrorMessage = "Hmmm... You should really add a Name...")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Owner Name")]
        public int OwnerId { get; set; }

        [Required]
        public string Breed { get; set; }

        public string Notes { get; set; }

        [DisplayName("Picture")]
        public string ImageUrl { get; set; }

        public Owner Owner { get; set; }
    }
}
