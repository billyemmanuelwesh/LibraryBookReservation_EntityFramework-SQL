using GPLCC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace GPLCC.Controllers
{
    public class MembresController : Controller
    {
        static string database = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bdgplcc.mdf");

        string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={database};Integrated Security=True";
        // GET: MembresController
        public ActionResult Index()
        {
            String requete = String.Format("SELECT Numero, Nom, Prenom FROM Membre");

            List<Membre> listeDesMembres = new List<Membre>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    Membre membre = new Membre();
                    membre.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);
                    membre.Nom = resultatDeRequete["Nom"].ToString();
                    membre.Prenom = resultatDeRequete["Prenom"].ToString();
                    

                    listeDesMembres.Add(membre);
                }
                con.Close();
            }
            //select
            
            return View(listeDesMembres);
        }

        // GET: MembresController/Details/5
        public ActionResult Details(int id)
        {
            //select avec where
            String requete = String.Format("SELECT Numero, Nom, Prenom FROM Membre WHERE Numero = {0}", id.ToString());

            Membre membre = new Membre();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    membre.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);
                    membre.Nom = resultatDeRequete["Nom"].ToString();
                    membre.Prenom = resultatDeRequete["Prenom"].ToString();
                }
                con.Close();
            }

            return View(membre);
        }

        // GET: MembresController/Create
        public ActionResult Create()
        {
            //vide
            return View();
        }

        // POST: MembresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                //insert into
                String requete = String.Format("INSERT INTO Membre (Numero,  Nom, Prenom) SELECT (MAX(Numero) + 1), '{0}', '{1}' FROM Membre ",
                 collection["Nom"], collection["Prenom"]);

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

        // GET: MembresController/Edit/5
        public ActionResult Edit(int id)
        {
            //select where
            String requete = String.Format("SELECT Numero, Nom, Prenom FROM Membre WHERE Numero = {0}", id.ToString());
           
            Membre membre = new Membre();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                //creer un objet reader pour recevoir le resultat de ma requete
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    membre.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);
                    membre.Nom = resultatDeRequete["Nom"].ToString();
                    membre.Prenom = resultatDeRequete["Prenom"].ToString();
                }
                con.Close();
            }

            return View(membre);
        }

        // POST: MembresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                //update
                String requete = String.Format("UPDATE Membre SET Nom = '{0}', Prenom = '{1}' WHERE Numero = {2}", 
                    collection["Nom"], collection["Prenom"], id.ToString());

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

        // GET: MembresController/Delete/5
        public ActionResult Delete(int id)
        {
            //select where
            String requete = String.Format("SELECT Numero, Nom, Prenom FROM Membre WHERE Numero = {0}", id.ToString());

            Membre membre = new Membre();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    membre.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);
                    membre.Nom = resultatDeRequete["Nom"].ToString();
                    membre.Prenom = resultatDeRequete["Prenom"].ToString();
                }
                con.Close();
            }
            ViewBag.Erreur = "";

            return View(membre);
        }

        // POST: MembresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                //delete
                String requete = String.Format("DELETE FROM Membre WHERE Numero = {0}", id.ToString());

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
                ViewBag.Erreur = "Impossible de supprimer un membre associé a un prêt";
                return View();
            }
        }
    }
}
