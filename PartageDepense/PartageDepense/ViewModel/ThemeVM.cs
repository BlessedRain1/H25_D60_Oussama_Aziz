using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PartageDepense.View;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace PartageDepense.ViewModel
{
    /// <summary>
    /// Classe provenement de MaterialDesign3Demo et adaptée par Patrick Simard
    /// </summary>
    internal class ThemeVM : ViewModelBase
    {

        #region Propriétés pour la sélection de couleurs de Material Design
        public IEnumerable<Swatch> Swatches { get; }

        private Swatch? _couleurPrimaireMaterialDesign;
        public Swatch? CouleurPrimaireMaterialDesign
        {
            get => _couleurPrimaireMaterialDesign;
            set
            {
                if (SetProperty(ref _couleurPrimaireMaterialDesign, value))
                {
                    if (CouleurPrimaireMaterialDesign != null)
                    {
                        ModifyTheme(theme => theme.SetPrimaryColor(CouleurPrimaireMaterialDesign.ExemplarHue.Color));
                    }
                }
            }
        }

        private Swatch? _couleurSecondaireMaterialDesign;
        public Swatch? CouleurSecondaireMaterialDesign
        {
            get => _couleurSecondaireMaterialDesign;
            set
            {
                if (SetProperty(ref _couleurSecondaireMaterialDesign, value))
                {
                    if (CouleurSecondaireMaterialDesign != null)
                    {
                        if (CouleurSecondaireMaterialDesign.SecondaryExemplarHue != null)
                            ModifyTheme(theme => theme.SetSecondaryColor(CouleurSecondaireMaterialDesign.SecondaryExemplarHue.Color));
                        else
                            ModifyTheme(theme => theme.SetSecondaryColor(CouleurSecondaireMaterialDesign.ExemplarHue.Color));
                    }
                }
            }
        }

        #endregion

        #region Propriétés pour la sélection d'une couleur Visual Studio à l'aide d'un combobox simple
        public IEnumerable<Couleurs> Couleurs => Enum.GetValues(typeof(Couleurs)).Cast<Couleurs>();

        private Couleurs? _couleurPrimaire;
        public Couleurs? CouleurPrimaire
        {
            get => _couleurPrimaire;
            set
            {
                if (SetProperty(ref _couleurPrimaire, value))
                {
                    if (CouleurPrimaire != null)
                        ModifyTheme(theme => theme.SetPrimaryColor(Couleur.GetColorFromCouleurs((Couleurs)CouleurPrimaire)));
                }

            }
        }

        private Couleurs? _couleurSecondaire;
        public Couleurs? CouleurSecondaire
        {
            get => _couleurSecondaire;
            set
            {
                if (SetProperty(ref _couleurSecondaire, value))
                {
                    if (CouleurSecondaire != null)
                        ModifyTheme(theme => theme.SetSecondaryColor(Couleur.GetColorFromCouleurs((Couleurs)CouleurSecondaire)));
                }
            }
        }

        #endregion

        #region Propriétés et fonctions pour la sélection d'une couleur Visual Studio à l'aide d'une boîte d'autosuggestion
        private string? _couleurPrimaireAutoSuggestion;
        public string? CouleurPrimaireAutoSuggestion
        {
            get => _couleurPrimaireAutoSuggestion;
            set
            {
                if (SetProperty(ref _couleurPrimaireAutoSuggestion, value) &&
                    _originalAutoSuggestBoxSuggestions != null && value != null)
                {
                    IEnumerable<KeyValuePair<string, Color>> searchResult = _originalAutoSuggestBoxSuggestions.Where(x => IsMatch(x.Key, value));
                    AutoSuggestBoxSuggestions = new ObservableCollection<KeyValuePair<string, Color>>(searchResult);
                    if(searchResult.Count() >= 1)
                        ModifyTheme(theme => theme.SetPrimaryColor(((KeyValuePair<string, Color>)searchResult.ElementAt(0)).Value));
                }
            }
        }

        private ObservableCollection<KeyValuePair<string, Color>>? _autoSuggestBoxSuggestions;
        private readonly List<KeyValuePair<string, Color>>? _originalAutoSuggestBoxSuggestions;

        public ObservableCollection<KeyValuePair<string, Color>>? AutoSuggestBoxSuggestions
        {
            get => _autoSuggestBoxSuggestions;
            set => SetProperty(ref _autoSuggestBoxSuggestions, value);
        }

        private static bool IsMatch(string item, string currentText)
        {
#if NET6_0_OR_GREATER
            return item.Contains(currentText, StringComparison.OrdinalIgnoreCase);
#else
        return item.IndexOf(currentText, StringComparison.OrdinalIgnoreCase) >= 0;
#endif
        }

        private static IEnumerable<KeyValuePair<string, Color>> GetColors()
        {
            return typeof(Colors)
                .GetProperties()
                .Where(prop =>
                    typeof(Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop =>
                    new KeyValuePair<string, Color>(prop.Name, (Color)prop.GetValue(null)!));
        }
        #endregion

        #region Propriétés pour la palette de couleurs

        public bool CouleurPrimaireSelectionne {  get; set; }

        private Color? _selectedColor;
        public Color? SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged();

                    if (SelectedColor == null) return;
                    if(CouleurPrimaireSelectionne)
                        ModifyTheme(theme => theme.SetPrimaryColor((Color)SelectedColor));
                    else
                        ModifyTheme(theme => theme.SetSecondaryColor((Color)SelectedColor));
                }
            }
        }

        #endregion

        public ThemeVM()
        {
            // Récupère les couleurs de Material Design
            Swatches = new SwatchesProvider().Swatches;

            // Récupère les couleurs de Visual Studio pour l'autosuggestion
            _originalAutoSuggestBoxSuggestions = new List<KeyValuePair<string, Color>>(GetColors());

            CouleurPrimaireSelectionne = true;

            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();

            IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark;

            if (theme is Theme internalTheme)
            {
                _isColorAdjusted = internalTheme.ColorAdjustment is not null;

                var colorAdjustment = internalTheme.ColorAdjustment ?? new ColorAdjustment();
                _desiredContrastRatio = colorAdjustment.DesiredContrastRatio;
                _contrastValue = colorAdjustment.Contrast;
                _colorSelectionValue = colorAdjustment.Colors;
            }

            if (paletteHelper.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e) =>
                {
                    IsDarkTheme = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
                };
            }
        }

        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? BaseTheme.Dark : BaseTheme.Light));
                }
            }
        }

        private bool _isColorAdjusted;
        public bool IsColorAdjusted
        {
            get => _isColorAdjusted;
            set
            {
                if (SetProperty(ref _isColorAdjusted, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme)
                        {
                            internalTheme.ColorAdjustment = value
                                ? new ColorAdjustment
                                {
                                    DesiredContrastRatio = DesiredContrastRatio,
                                    Contrast = ContrastValue,
                                    Colors = ColorSelectionValue
                                }
                                : null;
                        }
                    });
                }
            }
        }

        private float _desiredContrastRatio = 4.5f;
        public float DesiredContrastRatio
        {
            get => _desiredContrastRatio;
            set
            {
                if (SetProperty(ref _desiredContrastRatio, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                            internalTheme.ColorAdjustment.DesiredContrastRatio = value;
                    });
                }
            }
        }

        public IEnumerable<Contrast> ContrastValues => Enum.GetValues(typeof(Contrast)).Cast<Contrast>();

        private Contrast _contrastValue;
        public Contrast ContrastValue
        {
            get => _contrastValue;
            set
            {
                if (SetProperty(ref _contrastValue, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                            internalTheme.ColorAdjustment.Contrast = value;
                    });
                }
            }
        }

        public IEnumerable<ColorSelection> ColorSelectionValues => Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>();

        private ColorSelection _colorSelectionValue;
        public ColorSelection ColorSelectionValue
        {
            get => _colorSelectionValue;
            set
            {
                if (SetProperty(ref _colorSelectionValue, value))
                {
                    ModifyTheme(theme =>
                    {
                        if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                            internalTheme.ColorAdjustment.Colors = value;
                    });
                }
            }
        }

        private static void ModifyTheme(Action<Theme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            Theme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}