
using System;

namespace Services
{
    public static class GlobalSettings
    {
        public static Action OnChange;
        private static bool _music = true;

        public static bool Music
        {
            get => _music;
            set
            {
                if (_music == value) return;
                _music = value;
                OnChange?.Invoke();
            }
        }

        public static bool SFX
        {
            get => _sfx;
            set
            {
                if (_sfx == value) return;
                _sfx = value;
                OnChange?.Invoke();
            }
        }

        private static bool _sfx = true;

        public static string Language
        {
            get => _language;
            set
            {
                if (_language == value) return;
                _language = value;
                OnChange?.Invoke();
            }
        }

        private static string _language;
    }
}