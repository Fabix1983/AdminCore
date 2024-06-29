using System.Data;

namespace AdminCore.Models
{
    public class CompareModel
    {
        public DataTable DtMese0 { get; set; } //Dati del mese in corso
        public DataTable DtMese1 { get; set; } //Dati del mese - 1
        public DataTable DtMese2 { get; set; } //Dati del mese - 2 
        public DataTable DtMese3 { get; set; } //Dati del mese - 3 
        public DataTable DtMese4 { get; set; } //Dati del mese - 4
        public DataTable DtMese5 { get; set; } //Dati del mese - 5

    }
}
