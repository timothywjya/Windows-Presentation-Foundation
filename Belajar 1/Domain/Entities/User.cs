namespace Belajar_1.Domain.Entities
{
    /// <summary>
    /// Domain entity untuk User. Sengaja dibuat "polos" (tidak tahu-menahu
    /// soal MySQL, WPF, atau hashing) sesuai aturan Clean Architecture:
    /// layer Domain tidak boleh bergantung pada layer lain.
    /// </summary>
    public class User
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Menyimpan hash password, BUKAN plain text. Saat menampilkan daftar
        /// user (GetAll), field ini sengaja tidak diisi oleh repository agar
        /// tidak pernah bocor ke UI.
        /// </summary>
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int UserLevel { get; set; }
    }
}
