using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;

namespace TatamiArena.App.Services
{
    public class LocalizationService
    {
        private static LocalizationService _instance;
        public static LocalizationService Instance => _instance ??= new LocalizationService();

        private string _currentLanguage;
        private ResourceMap _resourceMap;
        private ResourceContext _resourceContext;

        public event EventHandler LanguageChanged;

        private LocalizationService()
        {
            // Initialize with system language or default to Polish
            _currentLanguage = "pl-PL";
            _resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
            _resourceContext = ResourceManager.Current.DefaultContext;
            _resourceContext.QualifierValues["Language"] = _currentLanguage;
        }

        public void SetLanguage(string languageCode)
        {
            if (_currentLanguage != languageCode)
            {
                _currentLanguage = languageCode;
                _resourceContext.QualifierValues["Language"] = languageCode;
                LanguageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string GetCurrentLanguage()
        {
            return _currentLanguage;
        }

        public string GetString(string key)
        {
            try
            {
                return _resourceMap.GetValue(key, _resourceContext).ValueAsString;
            }
            catch
            {
                return key; // Return the key if the string is not found
            }
        }

        public static Dictionary<string, string> AvailableLanguages => new()
        {
            { "en-US", "English" },
            { "pl-PL", "Polski" }
        };
    }
} 