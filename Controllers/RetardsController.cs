using GPLCC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace GPLCC.Controllers
{

    public class RetardsController : Controller
    {
        static string database = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bdgplcc.mdf");

        string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={database};Integrated Security=True";
       // GET: RetardsController
        public ActionResult Index()
        {


            //PURGER LA TABLE DES RETARD

            try
            {
                //delete
                String requeteDelete = "DELETE FROM Retard";

                //creer une connection vers ma bd
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //preparer une commande sql abvec ma requete et ma connection
                    SqlCommand cmd = new SqlCommand(requeteDelete, con);
                    con.Open();
                    //executer ma commade (ma requete)
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch
            {
                ViewBag.Erreur = "Impossible de supprimer cet enregistrement";
                return View();
            }

            //AJOUTER LES RETARD ACTUELS

            try
            {
                //insert
                String requeteCreate = "INSERT INTO Retard (NumPret,NbJoursRetard) (SELECT Numero,DATEDIFF(DAY,DateFin,GETDATE()) AS NbJours " +
                    "FROM Pret WHERE DateFin < GETDATE())";

                //creer une connection vers ma bd
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //preparer une commande sql abvec ma requete et ma connection
                    SqlCommand cmd = new SqlCommand(requeteCreate, con);
                    con.Open();
                    //executer ma commade (ma requete)
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch
            {
                ViewBag.Erreur = "Impossible de generer les retards";
                return View();
            }

            String requete = String.Format("SELECT NbJoursRetard, NumPret, Nom, Prenom, Titre FROM Retard  INNER JOIN Pret ON Pret.Numero = Retard.NumPret" +
                " INNER JOIN Membre ON Pret.NumMembre = Membre.Numero" + " INNER JOIN Livre  ON Pret.NumLivre = Livre.Numero");


            List<Retard> listeDesRetards = new List<Retard>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    Retard retard = new Retard();
                    Pret pret = new Pret();
                    Membre membre = new Membre();
                    Livre livre = new Livre();

                    membre.Numero = Convert.ToInt32(resultatDeRequete["NumPret"]);
                    membre.Nom = resultatDeRequete["Nom"].ToString();
                    membre.Prenom = resultatDeRequete["Prenom"].ToString();

                    livre.Titre = resultatDeRequete["Titre"].ToString();
                    retard.NbJoursRetard = Convert.ToInt32(resultatDeRequete["NbJoursRetard"]);

                    pret.Membre = membre;
                    pret.Livre = livre;
                    retard.pretLie = pret;

                    listeDesRetards.Add(retard);
                }
                con.Close();
            }
            return View(listeDesRetards);
        }

    }
}
