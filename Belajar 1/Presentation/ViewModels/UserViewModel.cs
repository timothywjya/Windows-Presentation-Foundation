using System;
using System.Collections.ObjectModel;
using Belajar_1.Domain.Entities;
using Belajar_1.Helpers;
using Belajar_1.UseCases.Users;

namespace Belajar_1.Presentation.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IUserService _userService;

        public ObservableCollection<User> Users { get; } = new();

        private User? _selectedUser;
        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (!SetField(ref _selectedUser, value)) return;

                if (value != null)
                {
                    UserId = value.UserId;
                    Username = value.Username;
                    Email = value.Email;
                    LevelText = value.UserLevel.ToString();
                    Password = string.Empty;
                    IsUserIdEditable = false;
                    RequestClearPasswordBox?.Invoke();
                }
            }
        }

        private string _userId = string.Empty;
        public string UserId
        {
            get => _userId;
            set => SetField(ref _userId, value);
        }

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetField(ref _username, value);
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        /// <summary>
        /// PasswordBox.Password tidak bisa di-binding langsung (dibatasi WPF
        /// demi keamanan), jadi code-behind FormUser meng-copy nilainya ke
        /// sini lewat event PasswordChanged. Selebihnya properti ini
        /// diperlakukan seperti properti ViewModel biasa.
        /// </summary>
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        private string _levelText = string.Empty;
        public string LevelText
        {
            get => _levelText;
            set => SetField(ref _levelText, value);
        }

        private bool _isUserIdEditable = true;
        public bool IsUserIdEditable
        {
            get => _isUserIdEditable;
            set => SetField(ref _isUserIdEditable, value);
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        /// <summary>
        /// View wajib subscribe ini untuk mengosongkan PasswordBox, karena
        /// ViewModel tidak boleh (dan tidak bisa) menyentuh elemen UI secara langsung.
        /// </summary>
        public event Action? RequestClearPasswordBox;

        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand ClearCommand { get; }

        public UserViewModel(IUserService userService)
        {
            _userService = userService;

            AddCommand = new RelayCommand(_ => AddUser());
            UpdateCommand = new RelayCommand(_ => UpdateUser());
            ClearCommand = new RelayCommand(_ => Clear());

            LoadUsers();
        }

        public void LoadUsers()
        {
            Users.Clear();
            foreach (User user in _userService.GetAllUsers())
                Users.Add(user);
        }

        private void AddUser()
        {
            if (!TryParseLevel(out int level)) return;

            if (_userService.CreateUser(UserId, Username, Password, Email, level, out string error))
            {
                LoadUsers();
                Clear();
            }
            else
            {
                ErrorMessage = error;
            }
        }

        private void UpdateUser()
        {
            if (!TryParseLevel(out int level)) return;

            if (_userService.UpdateUser(UserId, Username, Password, Email, level, out string error))
            {
                LoadUsers();
                Clear();
            }
            else
            {
                ErrorMessage = error;
            }
        }

        /// <summary>
        /// Dipanggil oleh code-behind SETELAH user mengonfirmasi dialog
        /// "Hapus User ini?" — konfirmasi dialog itu sendiri sengaja dibiarkan
        /// jadi tanggung jawab View, bukan logika bisnis ViewModel.
        /// </summary>
        public bool DeleteSelected(out string error)
        {
            if (_userService.DeleteUser(UserId, out error))
            {
                LoadUsers();
                Clear();
                return true;
            }

            ErrorMessage = error;
            return false;
        }

        public void Clear()
        {
            SelectedUser = null;
            UserId = string.Empty;
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            LevelText = string.Empty;
            IsUserIdEditable = true;
            ErrorMessage = string.Empty;
            RequestClearPasswordBox?.Invoke();
        }

        private bool TryParseLevel(out int level)
        {
            if (int.TryParse(LevelText, out level))
            {
                ErrorMessage = string.Empty;
                return true;
            }

            ErrorMessage = "User level harus berupa angka.";
            return false;
        }
    }
}
