namespace Belajar_1.Domain.Entities
{
    /// <summary>
    /// Domain entity untuk Produk. Sama seperti User.cs: sengaja "polos",
    /// tidak tahu-menahu soal MySQL/WPF, dan nama propertinya bukan nama
    /// kolom SQL (PRD_PRDCD, dst) supaya Domain tidak bergantung pada
    /// detail penyimpanan data.
    /// </summary>
    public class Product
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public string Merk { get; set; } = string.Empty;
        public string Flavour { get; set; } = string.Empty;
        public string Kemasan { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Deskripsi { get; set; } = string.Empty;
        public string Unit { get; set; } = "PCS";
        public int Frac { get; set; } = 1;
        public decimal HargaJual { get; set; }
        public decimal HargaJual2 { get; set; }
    }
}
