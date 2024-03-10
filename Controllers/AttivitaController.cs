using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdminCore.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Runtime.ConstrainedExecution;

namespace AdminCore.Controllers
{
    public class AttivitaController : Controller
    {
        private readonly ILogger<AttivitaController> _logger;
        private readonly IConfiguration _configuration;

        public AttivitaController(ILogger<AttivitaController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Base(int anno = 0, int mese = 0, String? result = null, String? filtro = null, String? order = null)
        {
            ViewBag.GiornoOdierno = DateTime.Now.ToString("dd");
            ViewBag.MeseAttuale = "NO";
            ViewBag.Bilancio = (decimal)0.00;
            ViewBag.Entrate = (decimal)0.00;
            ViewBag.Uscite = (decimal)0.00;

            String mesestringa;
            if (mese == 1 || mese == 2 || mese == 3 || mese == 4 || mese == 5 || mese == 6 || mese == 7 || mese == 8 || mese == 9)
            {
                mesestringa = "0" + mese.ToString();
            }
            else
            {
                mesestringa = mese.ToString();
            }

            if ( ( DateTime.Now.ToString("yyyy") == anno.ToString()  && DateTime.Now.ToString("MM") == mesestringa ) || (mese == 0) )
            {
                ViewBag.MeseAttuale = "SI";
            }

            if (result != null)
            {
                ViewBag.Result = result;
            }
            else
            {
                ViewBag.Result = "";
            }

            if (anno == 0)
            {
                //Mese corrente
                anno = Int32.Parse(DateTime.Now.ToString("yyyy"));
                mese = Int32.Parse(DateTime.Now.ToString("MM"));
                ViewBag.Mese = mese;
                ViewBag.Anno = anno;
            }
            else
            {
                // Mese passato in input
                ViewBag.Mese = mese;
                ViewBag.Anno = anno;
            }

            var connString = _configuration.GetConnectionString("Default");

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
                            ViewBag.RifPeriodo = (int)reader["ID"];
                            ViewBag.Descrizione = reader["Descrizione"].ToString();
                        }
                    }
                }
            }

            using (var connection = GetConnection(connString))
            {
                using (var cmdb = new SqlCommand("SELECT " +
                                                "   'Entrate' AS Tipo, " +
                                                "   SUM(Att.Costo) AS Tot " +
                                                "FROM " +
                                                "   tblAttivita ATT " +
                                                "   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo " +
                                                "   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita " +
                                                "WHERE " +
                                                "   Per.Anno = @Anno AND Per.Mese = @Mese " +
                                                "   AND Att.Costo > 0 " +
                                                "GROUP BY " +
                                                "   ATT.RifPeriodo " +
                                                "UNION " +
                                                "SELECT " +
                                                "   'Uscite' AS Tipo, " +
                                                "   SUM(Att.Costo) AS Tot " +
                                                "FROM " +
                                                "   tblAttivita ATT " +
                                                "   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo " +
                                                "   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita " +
                                                "WHERE " +
                                                "   Per.Anno = @Anno AND Per.Mese = @Mese " +
                                                "   AND Att.Costo < 0 " +
                                                "GROUP BY " +
                                                "   ATT.RifPeriodo " +
                                                "UNION " +
                                                "SELECT " +
                                                "   'Bilancio' AS Tipo, " +
                                                "   SUM(Att.Costo) AS Tot " +
                                                "FROM " +
                                                "   tblAttivita ATT " +
                                                "   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo " +
                                                "   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita " +
                                                "WHERE " +
                                                "   Per.Anno = @Anno AND Per.Mese = @Mese " +
                                                "GROUP BY " +
                                                "   ATT.RifPeriodo", connection))
                {
                    cmdb.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmdb.Parameters["@Anno"].Value = anno;
                    cmdb.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmdb.Parameters["@Mese"].Value = mese;
                    using (SqlDataReader reader = cmdb.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["Tipo"].ToString() == "Bilancio")
                            {
                                ViewBag.Bilancio = (decimal)reader["Tot"];
                            }
                            if (reader["Tipo"].ToString() == "Entrate")
                            {
                                ViewBag.Entrate = (decimal)reader["Tot"];
                            }
                            if (reader["Tipo"].ToString() == "Uscite")
                            {
                                ViewBag.Uscite = (decimal)reader["Tot"];
                            }

                        }
                    }
                }
            }

            DataTable dt = new DataTable();
            using (var connection = GetConnection(connString))
            {
                try
                {
                    String sSQL_Select = "SELECT " +
                                        "   ATT.ID, " +
                                        "   ATT.Giorno, " +
                                        "   ISNULL(ATT.Dettagli, '-') AS Dettagli, " +
                                        "   ATT.Costo, " +
                                        "   Per.Anno, " +
                                        "   Per.Mese, " +
                                        "   Per.Descrizione, " +
                                        "   TP.TipoAttivita, " +
                                        "   Tp.ColoreAzione, " +
                                        "   Tp.ColoreHTML, " +
                                        "   Tp.Tipologia " +
                                        "FROM " +
                                        "   tblAttivita ATT " +
                                        "   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo " +
                                        "   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita " +
                                        "WHERE " +
                                        "   Per.Anno = @Anno AND Per.Mese = @Mese ";

                    String FiltroSQL = "";
                    if (filtro != null)
                    {
                        FiltroSQL = " AND TP.TipoAttivita = @TipoAttivita ";
                        ViewBag.Result = "Filtro Richiesto: " + filtro.ToString();
                    }

                    if (order != null)
                    {
                        sSQL_Select = sSQL_Select + " ORDER BY ATT.Costo ";
                        ViewBag.Result = "Ordinamento Richiesto: " + order.ToString();
                    }
                    else
                    {
                        sSQL_Select = sSQL_Select + FiltroSQL + " ORDER BY ATT.Giorno ";
                    }

                    SqlCommand cmd = new SqlCommand(sSQL_Select, connection);
                    cmd.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd.Parameters["@Anno"].Value = anno;
                    cmd.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd.Parameters["@Mese"].Value = mese;
                    if (filtro != null)
                    {
                        cmd.Parameters.Add("@TipoAttivita", SqlDbType.VarChar, 50);
                        cmd.Parameters["@TipoAttivita"].Value = filtro.ToString();
                    }
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati 1";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            DataTable dtAgg = new DataTable();
            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd2 = new SqlCommand(
                                        "SELECT " +
                                        "   ATT.RifPeriodo, " +
                                        "   TP.TipoAttivita, " +
                                        "   TP.ColoreHTML, " +
                                        "   SUM(ATT.Costo) AS TotAtt " +
                                        "FROM " +
                                        "   tblAttivita ATT " +
                                        "   INNER JOIN tblPeriodi Per ON Per.ID = ATT.RifPeriodo " +
                                        "   INNER JOIN tblTipoAttivita TP ON TP.ID = ATT.RifTipoAttivita " +
                                        "WHERE " +
                                        "   Per.Anno = @Anno AND Per.Mese = @Mese " +
                                        "GROUP BY " +
                                        "   ATT.RifPeriodo, TP.TipoAttivita, TP.ColoreHTML " +
                                        "ORDER BY " +
                                        "    SUM(ATT.Costo)", connection);
                    cmd2.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd2.Parameters["@Anno"].Value = anno;
                    cmd2.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd2.Parameters["@Mese"].Value = mese;
                    SqlDataAdapter adp2 = new SqlDataAdapter(cmd2);
                    adp2.Fill(dtAgg);

                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati 2";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            DataTable dtAggG = new DataTable();
            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd3 = new SqlCommand("EXEC spAggregatiPerGiorno @Anno, @Mese", connection);
                    cmd3.Parameters.Add("@Anno", SqlDbType.Int, 4);
                    cmd3.Parameters["@Anno"].Value = anno;
                    cmd3.Parameters.Add("@Mese", SqlDbType.Int, 4);
                    cmd3.Parameters["@Mese"].Value = mese;
                    SqlDataAdapter adp3 = new SqlDataAdapter(cmd3);
                    adp3.Fill(dtAggG);

                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati 3";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            var model = new AttivitaModel();
            model.TipoAttivita.AddRange(GetTipoAttivita()); //recupera Attivita Tipo
            model.Dt = dt; //recupera Attivita del mese Scelto oppure in corso
            model.DtAgg = dtAgg; //recupera aggregato Attivita del mese Scelto oppure in corso
            model.DtAggGiorno = dtAggG; //recupera aggregato Attivita del mese Scelto oppure in corso ragruppato per giorno
            model.Anno = anno;
            model.Mese = mese;

            ViewBag.annoPrec = anno;
            ViewBag.mesePrec = mese - 1;
            ViewBag.annoSucc = anno;
            ViewBag.meseSucc = mese + 1;

            if (mese == 12)
            {
                ViewBag.annoPrec = anno;
                ViewBag.mesePrec = mese - 1;
                ViewBag.annoSucc = anno + 1;
                ViewBag.meseSucc = 1;
            }
            if (mese == 1)
            {
                ViewBag.annoPrec = anno - 1;
                ViewBag.mesePrec = 12;
                ViewBag.annoSucc = anno;
                ViewBag.meseSucc = mese + 1;
            }

            return View(model);
        }

        private IEnumerable<TipoAttivitaModel> GetTipoAttivita()
        {
            var connString = _configuration.GetConnectionString("Default");
            var list = new List<TipoAttivitaModel>();
            using (var connection = GetConnection(connString))
            {
                var cmd = new SqlCommand("SELECT ID, Tipologia + ' - '+ TipoAttivita AS Tipo FROM tblTipoAttivita ORDER BY Tipologia", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TipoAttivitaModel { Id = (int)reader["ID"], TipoAttivita = (string)reader["Tipo"] });
                    }
                }
            }
            return list;
        }

        [HttpPost]
        public IActionResult BaseInsert(AttivitaModel model)
        {
            //Salva dati
            var connString = _configuration.GetConnectionString("Default");
            using (var connection = GetConnection(connString))
            {
                try
                {
                    var command = new SqlCommand("EXEC spAttivita_Nuova @Giorno, @RifPeriodo, @RiftipoAttivita, @Dettagli, @Costo", connection);
                    command.Parameters.Add("@Giorno", SqlDbType.Int, 4);
                    command.Parameters["@Giorno"].Value = model.Giorno;
                    command.Parameters.Add("@RifPeriodo", SqlDbType.Int, 4);
                    command.Parameters["@RifPeriodo"].Value = model.RifPeriodo;
                    command.Parameters.Add("@RiftipoAttivita", SqlDbType.Int, 4);
                    command.Parameters["@RiftipoAttivita"].Value = model.RifTipoAttivita;
                    command.Parameters.Add("@Dettagli", SqlDbType.VarChar, 250);
                    String sDettagli = "";
                    if (model.Dettagli == "" || model.Dettagli is null)
                    {
                        sDettagli = "-";
                    }
                    else
                    {
                        sDettagli = model.Dettagli;
                    }
                    command.Parameters["@Dettagli"].Value = sDettagli;
                    command.Parameters.Add("@Costo", SqlDbType.Money, 8);
                    command.Parameters["@Costo"].Value = model.Costo;
                    command.ExecuteNonQuery();
                    return RedirectToAction(nameof(Base), new { model.Anno, model.Mese, result = "Attività inserita correttamente!" });
                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Eccezione rilevata in inserimento attività " + e;
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }
        }

        public IActionResult Cancella(int id, int mese, int anno)
        {
            //Salva dati
            var connString = _configuration.GetConnectionString("Default");
            using (var connection = GetConnection(connString))
            {
                try
                {
                    var command = new SqlCommand("DELETE FROM tblAttivita WHERE ID = @iRifAtt", connection);
                    command.Parameters.Add("iRifAtt", SqlDbType.Int, 4);
                    command.Parameters["iRifAtt"].Value = (Int32)id;
                    int iRecordDelete = 0;
                    iRecordDelete = command.ExecuteNonQuery();
                    if (iRecordDelete == 1)
                    {
                        return RedirectToAction(nameof(Base), new { anno = anno, mese = mese, result = "Attività eliminata correttamente!" });
                    }
                    else
                    {
                        String Errore;
                        Errore = "Eccezione rilevata in cancellazione attività (record eliminati: " + iRecordDelete.ToString() + ")";
                        return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                    }
                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Eccezione rilevata in cancellazione attività";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }
        }

        public IActionResult GoPeriodo(String descrizione)
        {
            descrizione = descrizione.Replace(" ", "-");
            //Formato Descrizione: Gennaio-2021    
            if (descrizione != "" && descrizione != null && descrizione.Contains("-") == true)
            {
                int Anno;
                int Mese = 0;
                string[] words = descrizione.Split("-");
                String MeseLungo = "";
                MeseLungo = words[0];
                if (MeseLungo.ToLower() == "gennaio")
                {
                    Mese = 1;
                }
                if (MeseLungo.ToLower() == "febbraio")
                {
                    Mese = 2;
                }
                if (MeseLungo.ToLower() == "marzo")
                {
                    Mese = 3;
                }
                if (MeseLungo.ToLower() == "aprile")
                {
                    Mese = 4;
                }
                if (MeseLungo.ToLower() == "maggio")
                {
                    Mese = 5;
                }
                if (MeseLungo.ToLower() == "giugno")
                {
                    Mese = 6;
                }
                if (MeseLungo.ToLower() == "luglio")
                {
                    Mese = 7;
                }
                if (MeseLungo.ToLower() == "agosto")
                {
                    Mese = 8;
                }
                if (MeseLungo.ToLower() == "settembre")
                {
                    Mese = 9;
                }
                if (MeseLungo.ToLower() == "ottobre")
                {
                    Mese = 10;
                }
                if (MeseLungo.ToLower() == "novembre")
                {
                    Mese = 11;
                }
                if (MeseLungo.ToLower() == "dicembre")
                {
                    Mese = 12;
                }

                Anno = Int32.Parse(words[1]);
                return RedirectToAction(nameof(Base), new { Anno, Mese });
            }
            else
            {
                String Errore;
                Errore = "Formato Descrizione Periodo non valido";
                return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
            }
        }

        public IActionResult Trend(int id = 100000, String desc = "Da sempre", String descrizione = "")
        {

            if (descrizione != "")
            {
                return RedirectToAction(nameof(GoPeriodo), new { descrizione = descrizione.ToString() });
            }

            DataTable dt = new DataTable();
            var connString = _configuration.GetConnectionString("Default");
            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EXEC spTrend @Top", connection);
                    cmd.Parameters.Add("@Top", SqlDbType.Int, 4);
                    cmd.Parameters["@Top"].Value = id;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);

                    try
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            Decimal TotBilancio = 0;
                            while (dr.Read())
                            {
                                TotBilancio += (Decimal)dr["Bilancio"];
                            }
                            ViewBag.BilancioPeriodo = Math.Round(TotBilancio, 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        String Errore;
                        Errore = "Errore nel recupero dei Dati 4";
                        return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                    }
                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati 5";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            var model = new GenericModel();
            ViewBag.DescrizionePeriodo = desc.ToString();
            model.DtGenerico = dt; //recupera Attivita del mese Scelto oppure in corso
            return View(model);
        }

        public IActionResult Categoria(int rifCategoria = 1, String descrizione = "")
        {
            if (descrizione != "")
            {
                return RedirectToAction(nameof(GoPeriodo), new { descrizione = descrizione.ToString() });
            }

            var connString = _configuration.GetConnectionString("Default");
            using (var connection = GetConnection(connString))
            {
                using (var cmd = new SqlCommand("SELECT Tipologia, TipoAttivita, ColoreHTML FROM tblTipoAttivita WHERE ID = @iTipoAtt", connection))
                {
                    cmd.Parameters.Add("@iTipoAtt", SqlDbType.Int, 4);
                    cmd.Parameters["@iTipoAtt"].Value = rifCategoria;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewBag.Descrizione = "[" + reader["Tipologia"].ToString() + "] " + reader["TipoAttivita"].ToString();
                            ViewBag.Colore = reader["ColoreHTML"].ToString();
                        }
                    }
                }
            }

            DataTable dt = new DataTable();
            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EXEC spCategoriaDiSpesaPerMese @RifCat", connection);
                    cmd.Parameters.Add("@RifCat", SqlDbType.Int, 4);
                    cmd.Parameters["@RifCat"].Value = rifCategoria;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Decimal PercMedia = 0;
                        int numCont = 0;
                        while (reader.Read())
                        {
                            numCont = numCont + 1;
                            ViewBag.Media = (decimal)reader["Media"];
                            PercMedia = PercMedia + (decimal)reader["PercentualeSulTotale"];
                        }
                        ViewBag.PercMedia = Math.Round((PercMedia / numCont), 2);
                    }
                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati 6";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            var modelIn = new CategoriaModel();
            modelIn.DtGenerico = dt; //recupera Attivita del mese Scelto oppure in corso
            modelIn.TipoAttivita.AddRange(GetTipoAttivita()); //recupera Attivita Tipo
            return View(modelIn);
        }

        public IActionResult Media()
        {
            DataTable dt = new DataTable();
            var connString = _configuration.GetConnectionString("Default");
            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EXEC spMedie", connection);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt);
                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati 7";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            using (var connection = GetConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spMedia_Mensile", connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewBag.SpesaMediaMensile = (decimal)reader["SpesaMediaMensile"];
                        }
                        reader.NextResult();
                        while (reader.Read())
                        {
                            ViewBag.EntrateMediaMensile = (decimal)reader["EntrateMediaMensile"];
                        }
                    }
                }
                catch (Exception e)
                {
                    String Errore;
                    Errore = "Errore nel recupero dei Dati 8";
                    return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                }
            }

            var model = new GenericModel();
            model.DtGenerico = dt;
            return View(model);
        }

        public IActionResult Ricerca(String cerca = null)
        {
            DataTable dt = new DataTable();
            if (cerca != "" && !String.IsNullOrEmpty(cerca))
            {
                ViewBag.Descrizione = cerca.ToString();
                var connString = _configuration.GetConnectionString("Default");
                using (var connection = GetConnection(connString))
                {
                    try
                    {
                        SqlCommand cmdUno = new SqlCommand("EXEC spCercaAttivitaPerParola @Cerca", connection);
                        cmdUno.Parameters.Add("@Cerca", SqlDbType.VarChar, 150);
                        cmdUno.Parameters["@Cerca"].Value = cerca.ToString().ToUpper();
                        SqlDataAdapter adp = new SqlDataAdapter(cmdUno);
                        adp.Fill(dt);

                    }
                    catch (Exception e)
                    {
                        String Errore;
                        Errore = "Errore nel recupero dei Dati 9";
                        return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                    }

                    try
                    {
                        SqlCommand cmd = new SqlCommand("EXEC spCercaAttivitaPerParola_Aggregato @Cerca", connection);
                        cmd.Parameters.Add("@Cerca", SqlDbType.VarChar, 150);
                        cmd.Parameters["@Cerca"].Value = cerca.ToString().ToUpper();
                        SqlDataReader dr = cmd.ExecuteReader();
                        String sDataServizi = "";
                        String sLabelsServizi = "";
                        String sColorsServizi = "";

                        if (dr.HasRows)
                        {
                            int counter = 0;
                            while (dr.Read())
                            {
                                if (counter == 0)
                                {
                                    sDataServizi += "[\"";
                                    sLabelsServizi += "[\"";
                                    sColorsServizi += "[\"";
                                }
                                else
                                {
                                    sDataServizi += ",\"";
                                    sLabelsServizi += ",\"";
                                    sColorsServizi += ",\"";
                                }
                                sDataServizi += ((int)dr["Costo"]).ToString() + "\"";
                                sLabelsServizi += dr["TipoAttivita"].ToString() + "\"";
                                sColorsServizi += dr["ColoreHTML"].ToString() + "\"";

                                counter++;
                            }
                            sDataServizi += "]";
                            sLabelsServizi += "]";
                            sColorsServizi += "]";
                        }

                        ViewBag.DataServizi = sDataServizi;
                        ViewBag.LabelsServizi = sLabelsServizi;
                        ViewBag.ColorsServizi = sColorsServizi;
                    }
                    catch (Exception ex)
                    {
                        String Errore;
                        Errore = "Errore nel recupero dei Dati 10";
                        return RedirectToAction(nameof(Error), new { Description = Errore.ToString() });
                    }
                }
            }
            else
            {
                ViewBag.Descrizione = "Non hai indicato alcuna parola da cercare!";
            }

            var model = new CercaModel();
            model.DtCerca = dt; //recupera Attivita del in base a cosa cercato
            return View(model);
        }

        public IActionResult License()
        {
            return View();
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
