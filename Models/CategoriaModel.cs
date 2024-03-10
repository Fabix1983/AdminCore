using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AdminCore.Models
{
    public class CategoriaModel
    {
        [Display(Name = "Tipo Attivita")]
        public int RifTipoAttivita { get; set; }
        public List<TipoAttivitaModel> TipoAttivita { get; set; } = new List<TipoAttivitaModel>();
        public DataTable DtGenerico { get; set; }
    }
}
