namespace Belajar_1.Domain.Entities
{
    public class Supplier
    {
        public string Kode { get; set; } = string.Empty;
        public string Nama { get; set; } = string.Empty;
        public string Singkatan { get; set; } = string.Empty;
        public string Telepon { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
