using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace PartageDepense.Language
{
    public static class LanguageManager
    {
        private static ResourceManager _resourceManager = new ResourceManager("PartageDepense.Language.Resource", typeof(LanguageManager).Assembly);
        private static CultureInfo _currentCulture = CultureInfo.CurrentUICulture;

        public static string GetString(string? key)
        {
            // Exclusions
            if (key == null) return "#ERREUR : Clé nulle";
            if (_currentCulture == null) return "#ERREUR : Culture nulle";

            // Récupération de la chaîne selon la langue sélectionnée
            string retour = _resourceManager.GetString(key, _currentCulture) ?? $"#ERREUR : [{key}]";
            return retour;
        }

        public static void SetCulture(string? cultureCode)
        {
            if (string.IsNullOrWhiteSpace(cultureCode))
                _currentCulture = CultureInfo.CurrentUICulture;
            else
                _currentCulture = new CultureInfo(cultureCode);
        }
    }
}
