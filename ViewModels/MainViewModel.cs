using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Zvonilka.Models;
using Zvonilka.Services;

namespace Zvonilka.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly UserSettingsService _userSettingsService;
        private string _messageText = string.Empty;
        private User? _currentUser;

        public ObservableCollection<Message> Messages { get; }
        public ObservableCollection<User> OnlineUsers { get; }
        public ObservableCollection<Room> AvailableRooms { get; }

        public string MessageText
        {
            get => _messageText;
            set
            {
                if (_messageText != value)
                {
                    _messageText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CurrentUserName => _currentUser?.Username ?? "User";
        public string? CurrentUserAvatar => _currentUser?.AvatarPath;

        public ICommand SendMessageCommand { get; }
        public ICommand SendFileCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        public MainViewModel()
        {
            _userSettingsService = new UserSettingsService();
            Messages = new ObservableCollection<Message>();
            OnlineUsers = new ObservableCollection<User>();
            AvailableRooms = new ObservableCollection<Room>();
            
            // Initialize current user with saved settings
            _currentUser = new User(
                _userSettingsService.UserName, 
                _userSettingsService.UserAvatarPath
            );
            
            // Add current user to online users
            OnlineUsers.Add(_currentUser);
            
            // Add some sample rooms
            AvailableRooms.Add(new Room("General", 5, false));
            AvailableRooms.Add(new Room("Voice Chat", 3, false));
            AvailableRooms.Add(new Room("Private Room", 2, true));
            
            // Initialize commands
            SendMessageCommand = new RelayCommand(SendMessage);
            SendFileCommand = new RelayCommand(SendFile);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            
            // Subscribe to settings changes
            _userSettingsService.PropertyChanged += OnUserSettingsChanged;
        }

        private void OnUserSettingsChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_userSettingsService.UserName))
            {
                if (_currentUser != null)
                {
                    _currentUser.Username = _userSettingsService.UserName;
                    OnPropertyChanged(nameof(CurrentUserName));
                }
            }
            else if (e.PropertyName == nameof(_userSettingsService.UserAvatarPath))
            {
                if (_currentUser != null)
                {
                    _currentUser.AvatarPath = _userSettingsService.UserAvatarPath;
                    OnPropertyChanged(nameof(CurrentUserAvatar));
                }
            }
        }

        private void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(MessageText))
            {
                var message = new Message(
                    _currentUser.Username, 
                    MessageText, 
                    _currentUser.AvatarPath
                );
                
                Messages.Add(message);
                MessageText = string.Empty;
            }
        }

        private void SendFile()
        {
            // Placeholder for file sending functionality
            System.Windows.MessageBox.Show("File sending functionality coming soon!");
        }

        private void OpenSettings()
        {
            var settingsWindow = new SettingsWindow(new AudioService());
            settingsWindow.ShowDialog();
        }

        public void Dispose()
        {
            _userSettingsService.PropertyChanged -= OnUserSettingsChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}