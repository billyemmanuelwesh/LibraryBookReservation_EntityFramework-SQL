using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GPLCC.Models
{
    public class Livre
    {

        [Display(Name = "Numéro du livre")]
        public int Numero { get; set; } 
        public String Titre { get; set; }
        public String Auteur { get; set;}


        [Display(Name = "Catégorie")]
        public int Categorie { get; set; }

        public Livre(int numero, string titre, string auteur, int categorie)
        {
            this.Numero = numero;
            this.Titre = titre;
            this.Auteur = auteur;
            this.Categorie = categorie;
        }

        public Livre() { }  








    }
}
