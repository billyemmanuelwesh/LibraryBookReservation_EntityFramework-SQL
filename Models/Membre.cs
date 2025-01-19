using System.ComponentModel.DataAnnotations;

namespace GPLCC.Models
{
    public class Membre
    {

        [Display(Name = "Numéro du membre")]
        public int Numero { get; set; }
        public String Nom { get; set; }

        [Display(Name ="Prénom")]
        public string Prenom { get; set;}



        public Membre(int numero, String nom, String prenom)
        {
            this.Numero = numero;
            this.Nom = nom;
            this.Prenom = prenom;
        }

        public Membre() { }






    }
}
