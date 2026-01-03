using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Zvonilka.Properties;

namespace Zvonilka.Services
{
    public class UserSettingsService : INotifyPropertyChanged
    {
        private readonly Settings _settings;

        public UserSettingsService()
        {
            _settings = Settings.Default;
            LoadSettings();
        }

        public string UserName
        {
            get => _settings.UserName;
            set
            {
                if (_settings.UserName != value)
                {
                    _settings.UserName = value;
                    _settings.Save();
                    OnPropertyChanged();
                }
            }
        }

        public string? UserAvatarPath
        {
            get => _settings.UserAvatarPath;
            set
            {
                if (_settings.UserAvatarPath != value)
                {
                    _settings.UserAvatarPath = value;
                    _settings.Save();
                    OnPropertyChanged();
                }
            }
        }

        public void SetUserAvatar(string? avatarPath)
        {
            UserAvatarPath = avatarPath;
        }

        public void SetUserName(string name)
        {
            UserName = name;
        }

        private void LoadSettings()
        {
            // Settings are automatically loaded from the .settings file
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}