using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GPLCC.Models
{
    public class Pret
    {
        [DisplayName("Numéro du pret")]
        public int Numero { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date de début")]
        public DateTime DateDebut { get; set; }
        

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date de fin")]
        public DateTime? DateFin { get; set; }
        public Membre? Membre { get; set; }
        public Livre? Livre { get; set; }


        public Pret(int numero, DateTime dateDebut, DateTime dateFin, Membre membre,Livre livre)
        {
            this.Numero = numero;
            this.DateDebut = dateDebut;
            this.DateFin = dateFin;
            this.Membre = membre;
            this.Livre = livre;
        }

        public Pret() { }


    }
}
