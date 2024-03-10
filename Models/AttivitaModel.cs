using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AdminCore.Models
{
    public class AttivitaModel
    {
        public int Id { get; set; }
        [Display(Name = "Giorno")]
        [Required]
        public int Giorno { get; set; }
        public int RifPeriodo { get; set; }
        [Display(Name = "Tipo Attivita")]
        public int RifTipoAttivita { get; set; }
        public string Dettagli { get; set; }
        [Display(Name = "Valore")]
        [Required]
        public decimal Costo { get; set; }
        public int Anno { get; set; }
        public int Mese { get; set; }

        public List<PeriodiModel> Periodi { get; set; } = new List<PeriodiModel>();
        public List<TipoAttivitaModel> TipoAttivita { get; set; } = new List<TipoAttivitaModel>();

        public DataTable Dt { get; set; } //Dati del mese in questione
        public DataTable DtAgg { get; set; } //Dati aggregati per tipo di spesa del mese in questione
        public DataTable DtAggGiorno { get; set; } //Dati aggregati per tipo di spesa del mese in questione e per giorno
    }

    public partial class PeriodiModel
    {
        public int Id { get; set; }
        public int Mese { get; set; }
        public int Anno { get; set; }
        public string Descrizione { get; set; }
    }

    public partial class TipoAttivitaModel
    {
        public int Id { get; set; }
        public string TipoAttivita { get; set; }
        public string ColoreHtml { get; set; }
        public string Tipologia { get; set; }
        public string ColoreAzione { get; set; }
    }

    public partial class CercaModel
    {
        public int Id { get; set; }
        public string cerca { get; set; }
        public DataTable DtCerca { get; set; } //Dati del mese in questione
    }
}
