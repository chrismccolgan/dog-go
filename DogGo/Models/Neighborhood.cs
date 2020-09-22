using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Neighborhood
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
