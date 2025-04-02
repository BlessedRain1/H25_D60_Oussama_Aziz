using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PartageDepense.Language;
using PartageDepense.ViewModel;

namespace PartageDepense.View
{
    /// <summary>
    /// Logique d'interaction pour ThemePalette.xaml
    /// </summary>
    public partial class ThemePalette : UserControl
    {
        #region Propriété accessible dans la vue

        public string? CultureCode
        {
            set
            {
                LanguageManager.SetCulture(value);
                UpdateUI();
            }
        }

        public static readonly DependencyProperty CultureCodeProperty =
            DependencyProperty.Register("CultureCode", typeof(string), typeof(ThemePalette), new PropertyMetadata(null, new PropertyChangedCallback(OnCultureCodeChanged)));

        private static void OnCultureCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ThemePalette)d).CultureCode = (string)e.NewValue;
        }

        #endregion

        public ThemePalette()
        {
            this.DataContext = new ThemeVM();
            InitializeComponent();
            UpdateUI();
        }

        public void UpdateUI()
        {
            ThemeHead.Header = LanguageManager.GetString("Thème de l'application");
            PrimaryColor.Header = LanguageManager.GetString("Couleur primaire");
            SecondaryColor.Header = LanguageManager.GetString("Couleur secondaire");
        }
    }
}
