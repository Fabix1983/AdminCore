using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdminCore.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Runtime.ConstrainedExecution;

namespace AdminCore.Controllers
{
    public class CompareController : Controller
    {
        private readonly ILogger<CompareController> _logger;
        private readonly IConfiguration _configuration;

        public CompareController(ILogger<CompareController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            int anno = Int32.Parse(DateTime.Now.ToString("yyyy"));
            int mese = Int32.Parse(DateTime.Now.ToString("MM"));

            var connString = _configuration.GetConnectionString("Default");

            DataTable dtMese0 = new DataTable();
            DataTable dtMese1 = new DataTable();
            DataTable dtMese2 = new DataTable();
            DataTable dtMese3 = new DataTable();
            DataTable dtMese4 = new DataTable();
            DataTable dtMese5 = new DataTable();


            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EXEC spAggregatoSpese_Mese @Anno, @Mese", connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtMese0);

                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati dtMese0";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            using (var connection = GetConnection(connString))
            {
                using (var cmd = new SqlCommand("SELECT ID, Descrizione FROM tblPeriodi WHERE  Mese = @Mese AND Anno = @Anno", connection))
                {
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewBag.RifPeriodo0 = (int)reader["ID"];
                            ViewBag.Descrizione0 = reader["Descrizione"].ToString();
                        }
                    }
                }
            }

            if (mese == 1)
            {
                mese = 12;
                anno = anno - 1;
            }
            else {
                mese = mese - 1;    
            }

            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EXEC spAggregatoSpese_Mese @Anno, @Mese", connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtMese1);

                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati dtMese0";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            using (var connection = GetConnection(connString))
            {
                using (var cmd = new SqlCommand("SELECT ID, Descrizione FROM tblPeriodi WHERE  Mese = @Mese AND Anno = @Anno", connection))
                {
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewBag.RifPeriodo1 = (int)reader["ID"];
                            ViewBag.Descrizione1 = reader["Descrizione"].ToString();
                        }
                    }
                }
            }

            if (mese == 1)
            {
                mese = 12;
                anno = anno - 1;
            }
            else
            {
                mese = mese - 1;
            }

            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EXEC spAggregatoSpese_Mese @Anno, @Mese", connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtMese2);

                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati dtMese0";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            using (var connection = GetConnection(connString))
            {
                using (var cmd = new SqlCommand("SELECT ID, Descrizione FROM tblPeriodi WHERE  Mese = @Mese AND Anno = @Anno", connection))
                {
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewBag.RifPeriodo2 = (int)reader["ID"];
                            ViewBag.Descrizione2 = reader["Descrizione"].ToString();
                        }
                    }
                }
            }

            if (mese == 1)
            {
                mese = 12;
                anno = anno - 1;
            }
            else
            {
                mese = mese - 1;
            }

            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EXEC spAggregatoSpese_Mese @Anno, @Mese", connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtMese3);

                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati dtMese0";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            using (var connection = GetConnection(connString))
            {
                using (var cmd = new SqlCommand("SELECT ID, Descrizione FROM tblPeriodi WHERE  Mese = @Mese AND Anno = @Anno", connection))
                {
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewBag.RifPeriodo3 = (int)reader["ID"];
                            ViewBag.Descrizione3 = reader["Descrizione"].ToString();
                        }
                    }
                }
            }

            if (mese == 1)
            {
                mese = 12;
                anno = anno - 1;
            }
            else
            {
                mese = mese - 1;
            }

            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EXEC spAggregatoSpese_Mese @Anno, @Mese", connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtMese4);

                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati dtMese0";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            using (var connection = GetConnection(connString))
            {
                using (var cmd = new SqlCommand("SELECT ID, Descrizione FROM tblPeriodi WHERE  Mese = @Mese AND Anno = @Anno", connection))
                {
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewBag.RifPeriodo4 = (int)reader["ID"];
                            ViewBag.Descrizione4 = reader["Descrizione"].ToString();
                        }
                    }
                }
            }

            if (mese == 1)
            {
                mese = 12;
                anno = anno - 1;
            }
            else
            {
                mese = mese - 1;
            }

            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EXEC spAggregatoSpese_Mese @Anno, @Mese", connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtMese5);

                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati dtMese0";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            using (var connection = GetConnection(connString))
            {
                using (var cmd = new SqlCommand("SELECT ID, Descrizione FROM tblPeriodi WHERE  Mese = @Mese AND Anno = @Anno", connection))
                {
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewBag.RifPeriodo5 = (int)reader["ID"];
                            ViewBag.Descrizione5 = reader["Descrizione"].ToString();
                        }
                    }
                }
            }

            var model = new CompareModel();
            model.DtMese0 = dtMese0; //recupera Attivita del mese oppure in corso
            model.DtMese1 = dtMese1; //recupera Attivita del mese - 1
            model.DtMese2 = dtMese2; //recupera Attivita del mese - 2
            model.DtMese3 = dtMese3; //recupera Attivita del mese - 3
            model.DtMese4 = dtMese4; //recupera Attivita del mese - 4
            model.DtMese5 = dtMese5; //recupera Attivita del mese - 5

            return View(model);
        }

        public IActionResult Error(String Description = null)
        {
            ViewBag.Descrizione = Description;
            return View();
        }

        public static SqlConnection GetConnection(String ConnStr)
        {
            SqlConnection connection = new SqlConnection(ConnStr);
            connection.Open();
            return connection;
        }
    }
}
