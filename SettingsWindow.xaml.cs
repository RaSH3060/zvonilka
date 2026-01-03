using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Zvonilka.Services;

namespace Zvonilka
{
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        private readonly AudioService _audioService;

        // Backing fields with proper initialization
        private string? _selectedInputDevice = null;
        private string? _selectedOutputDevice = null;
        private double _microphoneVolume = 50.0;
        private double _microphoneBoost = 0.0;
        private double _outputVolume = 50.0;
        private bool _noiseSuppressionEnabled = true;
        private double _noiseThreshold = 30.0;
        private bool _automaticGainControl = true;
        private bool _echoCancellation = true;
        private int _defaultPort = 7777;
        private double _connectionTimeout = 30.0;
        private bool _autoReconnect = true;
        private string? _voiceQuality = "High (16kHz, 32kbps)";
        private bool _enableQoS = true;
        private string? _selectedTheme = "Dark";
        private double _uiScaling = 100.0;
        private bool _smoothScrolling = true;
        private bool _showNotifications = true;
        private bool _typingIndicators = true;
        private bool _showOnlineStatus = true;
        private bool _rememberChatHistory = true;
        private bool _autoStart = false;
        private string _pushToTalkKey = "Ctrl + V";
        private string _toggleMuteKey = "Ctrl + M";
        private string _toggleDeafenKey = "Ctrl + D";
        private string _userName = "User";
        private string? _userAvatarPath = null;

        public event PropertyChangedEventHandler? PropertyChanged;

        public SettingsWindow(AudioService audioService)
        {
            _audioService = audioService;
            InitializeComponent();
            DataContext = this;
            
            // Initialize commands
            SelectAvatarCommand = new RelayCommand(SelectAvatar);
            ResetAvatarCommand = new RelayCommand(ResetAvatar);
            
            Loaded += (s, e) => LoadSettings(); // Defer loading until UI is ready
        }

        private void LoadSettings()
        {
            try
            {
                // Load audio devices first
                var inputDevices = _audioService.GetAudioInputDevices();
                var outputDevices = _audioService.GetAudioOutputDevices();

                AudioInputDevices.Clear();
                AudioInputDevices.AddRange(inputDevices);
                AudioOutputDevices.Clear();
                AudioOutputDevices.AddRange(outputDevices);

                // Load persisted settings
                var settings = Properties.Settings.Default;

                // Audio settings
                MicrophoneVolume = settings.MicrophoneVolume;
                OutputVolume = settings.OutputVolume;
                NoiseSuppressionEnabled = settings.NoiseSuppressionEnabled;
                NoiseThreshold = settings.NoiseThreshold * 100; // Convert stored fraction to percentage

                // Network settings
                DefaultPort = settings.DefaultPort;

                // Apply device selections after devices are populated
                if (!string.IsNullOrEmpty(settings.LastInputDevice) &&
                    inputDevices.Contains(settings.LastInputDevice))
                {
                    SelectedInputDevice = settings.LastInputDevice;
                }
                else if (inputDevices.Count > 0)
                {
                    SelectedInputDevice = inputDevices[0];
                }

                if (!string.IsNullOrEmpty(settings.LastOutputDevice) &&
                    outputDevices.Contains(settings.LastOutputDevice))
                {
                    SelectedOutputDevice = settings.LastOutputDevice;
                }
                else if (outputDevices.Count > 0)
                {
                    SelectedOutputDevice = outputDevices[0];
                }

                // Apply audio processing settings
                _audioService.SetNoiseSuppression(NoiseSuppressionEnabled);
                _audioService.SetNoiseThreshold((float)(NoiseThreshold / 100.0));
                
                // Load user profile settings
                UserName = settings.UserName;
                UserAvatarPath = settings.UserAvatarPath;
            }
            catch (Exception ex)
            {
                // Fallback to defaults on error
                MessageBox.Show($"Settings load error: {ex.Message}", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SelectAvatar()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                UserAvatarPath = openFileDialog.FileName;
            }
        }

        private void ResetAvatar()
        {
            UserAvatarPath = null;
        }

        public System.Collections.Generic.List<string> AudioInputDevices { get; } = new();
        public System.Collections.Generic.List<string> AudioOutputDevices { get; } = new();

        // User Profile Properties
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    Properties.Settings.Default.UserName = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged();
                }
            }
        }

        public string? UserAvatarPath
        {
            get => _userAvatarPath;
            set
            {
                if (_userAvatarPath != value)
                {
                    _userAvatarPath = value;
                    Properties.Settings.Default.UserAvatarPath = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged();
                }
            }
        }

        // Commands
        public System.Windows.Input.ICommand SelectAvatarCommand { get; private set; }
        public System.Windows.Input.ICommand ResetAvatarCommand { get; private set; }

        // Optimized property setters with validation and side effects
        public string? SelectedInputDevice
        {
            get => _selectedInputDevice;
            set
            {
                if (_selectedInputDevice != value && !string.IsNullOrEmpty(value))
                {
                    _selectedInputDevice = value;
                    _audioService.SetMicrophone(value);
                    Properties.Settings.Default.LastInputDevice = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged();
                }
            }
        }

        public string? SelectedOutputDevice
        {
            get => _selectedOutputDevice;
            set
            {
                if (_selectedOutputDevice != value && !string.IsNullOrEmpty(value))
                {
                    _selectedOutputDevice = value;
                    _audioService.SetHeadphones(value);
                    Properties.Settings.Default.LastOutputDevice = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged();
                }
            }
        }

        public double MicrophoneVolume
        {
            get => _microphoneVolume;
            set
            {
                value = Math.Clamp(value, 0, 100);
                if (!_microphoneVolume.Equals(value))
                {
                    _microphoneVolume = value;
                    Properties.Settings.Default.MicrophoneVolume = (float)value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged();
                }
            }
        }

        public double MicrophoneBoost
        {
            get => _microphoneBoost;
            set
            {
                value = Math.Clamp(value, 0, 20); // Reasonable boost limit
                if (!_microphoneBoost.Equals(value))
                {
                    _microphoneBoost = value;
                    OnPropertyChanged();
                }
            }
        }

        public double OutputVolume
        {
            get => _outputVolume;
            set
            {
                value = Math.Clamp(value, 0, 100);
                if (!_outputVolume.Equals(value))
                {
                    _outputVolume = value;
                    Properties.Settings.Default.OutputVolume = (float)value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged();
                }
            }
        }

        public bool NoiseSuppressionEnabled
        {
            get => _noiseSuppressionEnabled;
            set
            {
                if (_noiseSuppressionEnabled != value)
                {
                    _noiseSuppressionEnabled = value;
                    _audioService.SetNoiseSuppression(value);
                    Properties.Settings.Default.NoiseSuppressionEnabled = value;
                    Properties.Settings.Default.Save();
                    OnPropertyChanged();
                }
            }
        }

        public double NoiseThreshold
        {
            get => _noiseThreshold;
            set
            {
                value = Math.Clamp(value, 0, 100);
                if (!_noiseThreshold.Equals(value))
                {
                    _noiseThreshold = value;
                    _audioService.SetNoiseThreshold((float)(value / 100.0));
                    Properties.Settings.Default.NoiseThreshold = (float)(value / 100.0);
                    Properties.Settings.Default.Save();
                    OnPropertyChanged();
                }
            }
        }

        // Other properties remain unchanged but with added validation where appropriate
        // (Validation logic added only where critical for stability)
        public bool AutomaticGainControl
        {
            get => _automaticGainControl;
            set => SetProperty(ref _automaticGainControl, value);
        }

        public bool EchoCancellation
        {
            get => _echoCancellation;
            set => SetProperty(ref _echoCancellation, value);
        }

        public int DefaultPort
        {
            get => _defaultPort;
            set
            {
                value = Math.Clamp(value, 1024, 65535); // Valid port range
                if (SetProperty(ref _defaultPort, value))
                {
                    Properties.Settings.Default.DefaultPort = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public double ConnectionTimeout
        {
            get => _connectionTimeout;
            set
            {
                value = Math.Clamp(value, 5, 300); // 5s min, 5min max
                if (SetProperty(ref _connectionTimeout, value))
                {
                    // Note: ConnectionTimeout not in settings, could add if needed
                }
            }
        }

        // ... (Other properties maintain identical signatures but use SetProperty pattern)
        // Example implementation for remaining properties:
        public bool AutoReconnect
        {
            get => _autoReconnect;
            set => SetProperty(ref _autoReconnect, value);
        }

        // Helper method to reduce boilerplate
        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ApplySettings();
            DialogResult = true;
            Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e) => ApplySettings();

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ApplySettings()
        {
            var settings = Properties.Settings.Default;

            // Persist critical audio settings
            settings.MicrophoneVolume = (float)MicrophoneVolume;
            settings.OutputVolume = (float)OutputVolume;
            settings.NoiseSuppressionEnabled = NoiseSuppressionEnabled;
            settings.NoiseThreshold = (float)(NoiseThreshold / 100.0); // Store as fraction

            // Persist device selections
            settings.LastInputDevice = SelectedInputDevice;
            settings.LastOutputDevice = SelectedOutputDevice;

            // Persist network settings
            settings.DefaultPort = DefaultPort;

            settings.Save();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}