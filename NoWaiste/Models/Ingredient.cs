using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoWaiste.Models
{
    public class Ingredient
    {
        public string? name { get; set; }
        public float amount { get; set; }
    }

    public class SpoonacularResponse
    {
        public List<Ingredient>? ingredients { get; set; }
    }
}
