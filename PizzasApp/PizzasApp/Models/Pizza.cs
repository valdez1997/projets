using PizzasApp.extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzasApp.Models
{
    internal class Pizza
    {
        public string nom { get; set; }
        public string Titre { get{ return nom.PremiereLettreMajuscule(); }  }
        public string imageurl { get; set; }
        public int prix { get; set; }
        public string [] ingredients { get; set; }
        public string prixEuro { get { return prix + "€"; } }

        public string ingredientstr { get { return string.Join("; ",ingredients ); } }
        public Pizza()
        {
            

        }


    }
}
