namespace Belajar_1.Model
{
    public class Product
    {
        public string PRD_PRDCD { get; set; }
        public string PRD_NAMA { get; set; }
        public string PRD_MERK { get; set; }
        public string PRD_FLAVOUR { get; set; }
        public string PRD_KEMASAN { get; set; }
        public string PRD_SIZE { get; set; }
        public string PRD_DESKRIPSI { get; set; }
        public string PRD_UNIT { get; set; } = "PCS";
        public int PRD_FRAC { get; set; } = 1;
        public decimal PRD_HRGJUAL { get; set; }
        public decimal PRD_HRGJUAL2 { get; set; } 
    }
}