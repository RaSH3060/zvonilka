using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Zvonilka
{
    public partial class CreateRoomWindow : Window, INotifyPropertyChanged
    {
        private string _roomName = string.Empty;
        private string _password = string.Empty;
        private int _maxMembers = 10;
        private bool _isPrivate = false;
        private bool _enableVoice = true;
        private bool _enableText = true;
        private bool _allowFiles = true;
        private bool _isModerated = false;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public CreateRoomWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        
        public string RoomName
        {
            get => _roomName;
            set
            {
                if (_roomName != value)
                {
                    _roomName = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public int MaxMembers
        {
            get => _maxMembers;
            set
            {
                if (_maxMembers != value)
                {
                    _maxMembers = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsPrivateRoom
        {
            get => _isPrivate;
            set
            {
                if (_isPrivate != value)
                {
                    _isPrivate = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsPublicRoom
        {
            get => !_isPrivate;
            set
            {
                if (!_isPrivate != value)
                {
                    _isPrivate = !value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool EnableVoice
        {
            get => _enableVoice;
            set
            {
                if (_enableVoice != value)
                {
                    _enableVoice = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool EnableText
        {
            get => _enableText;
            set
            {
                if (_enableText != value)
                {
                    _enableText = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool AllowFiles
        {
            get => _allowFiles;
            set
            {
                if (_allowFiles != value)
                {
                    _allowFiles = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsModerated
        {
            get => _isModerated;
            set
            {
                if (_isModerated != value)
                {
                    _isModerated = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RoomName))
            {
                MessageBox.Show("Please enter a room name", "Validation Error", 
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Get password from PasswordBox
            Password = PasswordBox.Password;

            DialogResult = true;
            Close();
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}