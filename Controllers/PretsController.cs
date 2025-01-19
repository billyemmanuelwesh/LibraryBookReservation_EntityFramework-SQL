using GPLCC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace GPLCC.Controllers
{
    public class PretsController : Controller
    {
        static string database = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bdgplcc.mdf");

        string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={database};Integrated Security=True";
        // GET: PretsController
        public ActionResult Index()
        {
            String requete = String.Format("Select *, Pret.Numero AS NumeroPret, Livre.Numero AS NumeroLivre, Membre.Numero AS NumeroMembre FROM Pret INNER JOIN Membre ON Pret.NumMembre = Membre.Numero " +
                " INNER JOIN Livre ON Pret.NumLivre = Livre.Numero");

            List<Pret> listeDesPrets = new List<Pret>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {

                    Pret pret = new Pret();
                    pret.DateDebut = Convert.ToDateTime(resultatDeRequete["DateDebut"]);
                    if (resultatDeRequete["DateFin"] != DBNull.Value)
                    {
                        pret.DateFin = Convert.ToDateTime(resultatDeRequete["DateFin"]);
                    }

                    Livre livre = new Livre();
                    livre.Numero = Convert.ToInt32(resultatDeRequete["NumeroLivre"]);
                    livre.Titre = resultatDeRequete["Titre"].ToString();
                    livre.Auteur = resultatDeRequete["Auteur"].ToString();
                    livre.Categorie = Convert.ToInt32(resultatDeRequete["Categories"]);

                    Membre membre = new Membre();
                    membre.Numero = Convert.ToInt32(resultatDeRequete["NumeroMembre"]);
                    membre.Nom = resultatDeRequete["Nom"].ToString();
                    membre.Prenom = resultatDeRequete["Prenom"].ToString();

                    pret.Livre = livre;
                    pret.Membre = membre;
                    pret.Numero = Convert.ToInt32(resultatDeRequete["NumeroPret"]);



                    listeDesPrets.Add(pret);
                }
                con.Close();
                return View(listeDesPrets);
            }
        }

        // GET: PretsController/Details/5
        public ActionResult Details(int id)
        {
            //select avec where
            String requete = String.Format("Select *, Pret.Numero AS NumeroPret, Livre.Numero AS NumeroLivre, Membre.Numero AS NumeroMembre FROM Pret INNER JOIN Membre ON Pret.NumMembre = Membre.Numero " +
        " INNER JOIN Livre ON Pret.NumLivre = Livre.Numero WHERE Pret.Numero = {0}", id.ToString());

            Pret pret = new Pret();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    pret.DateDebut = Convert.ToDateTime(resultatDeRequete["DateDebut"]);
                    if (resultatDeRequete["DateFin"] != DBNull.Value)
                    {
                        pret.DateFin = Convert.ToDateTime(resultatDeRequete["DateFin"]);
                    }

                    Livre livre = new Livre();
                    livre.Numero = Convert.ToInt32(resultatDeRequete["NumeroLivre"]);
                    livre.Titre = resultatDeRequete["Titre"].ToString();
                    livre.Auteur = resultatDeRequete["Auteur"].ToString();
                    livre.Categorie = Convert.ToInt32(resultatDeRequete["Categories"]);

                    Membre membre = new Membre();
                    membre.Numero = Convert.ToInt32(resultatDeRequete["NumeroMembre"]);
                    membre.Nom = resultatDeRequete["Nom"].ToString();
                    membre.Prenom = resultatDeRequete["Prenom"].ToString();

                    pret.Livre = livre;
                    pret.Membre = membre;
                    pret.Numero = Convert.ToInt32(resultatDeRequete["NumeroPret"]);


                }
                con.Close();
            }
            return View(pret);
        }

        // GET: PretsController/Create
        public ActionResult Create()
        {
            String requeteNumeroPret = "SELECT (MAX(Numero) + 1) AS NumeroSuivant FROM Pret";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requeteNumeroPret, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();
                resultatDeRequete.Read();
                ViewBag.ProchainNumero = (Int32)resultatDeRequete.GetInt32("NumeroSuivant");
              
                con.Close();
            }

            // RECUPERATION DES MEMBRES POUR ALIMENTER UNE ZONE DEROULANTE
            String requete2 = "SELECT  Numero, Titre FROM Livre l " +
                "WHERE NOT EXISTS ( SELECT p.Numero FROM Pret p WHERE p.NumLivre = l.Numero) ";

            List<SelectListItem> listeDesLivres = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete2, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    SelectListItem itemDeListe = new SelectListItem();
                    itemDeListe.Text = resultatDeRequete["Titre"].ToString();
                    itemDeListe.Value = resultatDeRequete["Numero"].ToString();
                    listeDesLivres.Add(itemDeListe);
                }
                con.Close();
            }
            // RECUPERATION DES MEMBRES POUR ALIMENTER UNE ZONE DEROULANTE ( LIMITE A 3 EMPRUNT )
            String requete = String.Format("SELECT Numero, Nom, Prenom FROM Membre m " +
                " WHERE ((SELECT Count(*) FROM Pret p WHERE m.Numero = p.NumMembre) < 3)");

            List<SelectListItem> listeDesMembres = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    SelectListItem itemDeListe = new SelectListItem();
                    itemDeListe.Text = resultatDeRequete["Prenom"].ToString() + " " + resultatDeRequete["Nom"].ToString();
                    itemDeListe.Value = resultatDeRequete["Numero"].ToString();
                    listeDesMembres.Add(itemDeListe);
                }
                con.Close();
            }

            ViewBag.Membres = listeDesMembres;
            ViewBag.Livres = listeDesLivres;

            return View();
        }

        // POST: PretsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                DateTime DateRetour = Convert.ToDateTime(collection["DateDebut"]);
                DateRetour = DateRetour.AddDays(7);
                //insert into
                String requete = String.Format("INSERT INTO Pret (Numero, NumMembre, NumLivre, Datedebut, DateFin) VALUES ({0}, {1}, {2}, '{3}', '{4}') ",
                    collection["Numero"], collection["Membre.Numero"], collection["Livre.Numero"], Convert.ToDateTime(collection["DateDebut"]).ToString("u"), DateRetour.ToString("u"));
                
                //creer une connection vers ma bd
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //preparer une commande sql abvec ma requete et ma connection
                    SqlCommand cmd = new SqlCommand(requete, con);
                    con.Open();
                    //executer ma commade (ma requete)
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PretsController/Edit/5
        public ActionResult Edit(int id)
        {
            //select avec where
            String requete = String.Format("Select DateDebut,DateFin, Numero FROM Pret  WHERE Numero = {0}", id.ToString());

            Pret pret = new Pret();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                //creer un objet reader pour recevoir le resultat de ma requete
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    pret.DateDebut = Convert.ToDateTime(resultatDeRequete["DateDebut"]);

                    if (resultatDeRequete["DateFin"] != DBNull.Value)
                    {
                        pret.DateFin = Convert.ToDateTime(resultatDeRequete["DateFin"]);
                    }

                    pret.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);


                }
                con.Close();
            }
            return View(pret);
        }

        // POST: PretsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                String requete; // selon que la DateFin soit vide ou pas
                if (collection["DateFin"] != String.Empty)
                {
                    requete = String.Format("UPDATE Pret SET DateDebut = '{0}', DateFin = '{1}' WHERE Numero = {2}",
                    Convert.ToDateTime(collection["DateDebut"]).ToString("u"), Convert.ToDateTime(collection["DateFin"]).ToString("u"), id.ToString());
                }
                else
                {
                    requete = String.Format("UPDATE Pret SET DateDebut = '{0}', DateFin = NULL WHERE Numero = {1}",
                        Convert.ToDateTime(collection["DateDebut"]).ToString("u"), id.ToString());
                }
                //update
              

                //creer une connection vers ma bd
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //preparer une commande sql abvec ma requete et ma connection
                    SqlCommand cmd = new SqlCommand(requete, con);
                    con.Open();
                    //executer ma commade (ma requete)
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PretsController/Delete/5
        public ActionResult Delete(int id)
        {
            //select where
            String requete = String.Format("SELECT Numero, DateDebut, DateFin, NumMembre, NumLivre FROM Pret WHERE Numero = {0}", id.ToString());

            Pret pret = new Pret();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    pret.DateDebut = Convert.ToDateTime(resultatDeRequete["DateDebut"]);

                    if (resultatDeRequete["DateFin"] != DBNull.Value)
                    {
                        pret.DateFin = Convert.ToDateTime(resultatDeRequete["DateFin"]);
                    }

                    pret.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);
                    
                }
                con.Close();
            }
            ViewBag.Erreur = "";
            return View(pret);
        }

        // POST: PretsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                //delete
                String requete = String.Format("DELETE FROM Pret WHERE Numero = {0}", id.ToString());

                //creer une connection vers ma bd
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //preparer une commande sql abvec ma requete et ma connection
                    SqlCommand cmd = new SqlCommand(requete, con);
                    con.Open();
                    //executer ma commade (ma requete)
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Erreur = "Impossible de supprimer cet enregistrement";
                return View();
            }
        }
    }
}
