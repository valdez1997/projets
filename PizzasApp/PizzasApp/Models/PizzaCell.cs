using System;
using System.Collections.Generic;
using System.Text;

namespace PizzasApp.Models
{
    internal class PizzaCell
    {
        public Pizza pizza { get; set; }
        public bool isfavorite { get; set; }
        public string imagesourcefav { get { return isfavorite ? "star2.png" : "star1.png"; } }

        public PizzaCell()
        {

        }
    }
}
