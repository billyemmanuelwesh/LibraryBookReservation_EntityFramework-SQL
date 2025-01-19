using GPLCC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace GPLCC.Controllers
{
    public class LivresController : Controller
    {
        static string database = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bdgplcc.mdf");

        string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={database};Integrated Security=True";


        public String[] MesCategories = new String[] { "","Enfant(-14 ans)", "Adulte(+18 ans)", "Jeunesse (+14 ans)" };

// GET: LivreController
        public ActionResult Index()
        {
            String requete = String.Format("SELECT Numero, Titre, Auteur, Categories FROM Livre");
            ViewBag.MesCategories = MesCategories; // pour afficher : Enfant, Adulte, Jenesse à la place de 1, 2, 3 ect...
            List<Livre> listeDesLivres = new List<Livre>(); //on veut retourner une liste de livre et leur info
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {   
                    // creation des livres que contiendra notre liste de livre
                    Livre livre = new Livre();
                    livre.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);
                    livre.Titre = resultatDeRequete["Titre"].ToString();
                    livre.Auteur = resultatDeRequete["Auteur"].ToString();
                    livre.Categorie = Convert.ToInt32(resultatDeRequete["Categories"]) ;

                    listeDesLivres.Add(livre);
                }
                con.Close();
            }
            return View(listeDesLivres);
        }

        // GET: LivreController/Details/5
        public ActionResult Details(int id)
        {
            //select avec where
            String requete = String.Format("SELECT Numero, Titre, Auteur, Categories FROM Livre WHERE Numero = {0}", id.ToString());

            Livre livre= new Livre(); //on veut retourner un livre et ses infos
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    livre.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);
                    livre.Titre = resultatDeRequete["Titre"].ToString();
                    livre.Auteur = resultatDeRequete["Auteur"].ToString();
                    livre.Categorie = Convert.ToInt32(resultatDeRequete["Categories"]);
                }
                con.Close();
            }
            return View(livre);
        }

        // GET: LivreController/Create
        public ActionResult Create()
        {
            // vide
            ViewBag.MesCategories = MesCategories;

            return View();
        }

        // POST: LivreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {

            try
            {
                //insert into
                String requete = String.Format("INSERT INTO Livre(Numero,Titre,Auteur,Categories) SELECT (MAX(Numero) + 1),'{0}', '{1}',{2} FROM Livre ",
                   collection["Titre"], collection["Auteur"], collection["Categorie"]);

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
                ViewBag.MesCategories = MesCategories;
                return View();
            }
        }

        // GET: LivreController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.MesCategories = MesCategories;
            //select avec where
            String requete = String.Format("SELECT Numero, Titre, Auteur, Categories FROM Livre WHERE Numero = {0}", id.ToString());

            Livre livre = new Livre();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {

                    livre.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);
                    livre.Titre = resultatDeRequete["Titre"].ToString();
                    livre.Auteur = resultatDeRequete["Auteur"].ToString();
                    livre.Categorie = Convert.ToInt32(resultatDeRequete["Categories"]);

                }
                con.Close();
            }
            return View(livre);
        }

        // POST: LivreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                //update
                String requete = String.Format("UPDATE Livre SET Titre = '{0}', Auteur = '{1}', Categories = '{2}' WHERE Numero = {3}",
                    collection["Titre"], collection["Auteur"], collection["Categorie"], id.ToString());

                //creer une connection vers ma bd
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //preparer une commande sql avvec ma requete et ma connection
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

        // GET: LivreController/Delete/5
        public ActionResult Delete(int id)
        {
            //select avec where
            String requete = String.Format("SELECT Numero, Titre, Auteur, Categories FROM Livre WHERE Numero = {0}", id.ToString());

            Livre livre = new Livre();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(requete, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader resultatDeRequete = cmd.ExecuteReader();

                while (resultatDeRequete.Read())
                {
                    livre.Numero = Convert.ToInt32(resultatDeRequete["Numero"]);
                    livre.Titre = resultatDeRequete["Titre"].ToString();
                    livre.Auteur = resultatDeRequete["Auteur"].ToString();
                    livre.Categorie = Convert.ToInt32(resultatDeRequete["Categories"]);

                }
                con.Close();
            }
            return View(livre);
        }

        // POST: LivreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                //delete
                String requete = String.Format("DELETE FROM Livre WHERE Numero = {0}", id.ToString());

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
    }
}
