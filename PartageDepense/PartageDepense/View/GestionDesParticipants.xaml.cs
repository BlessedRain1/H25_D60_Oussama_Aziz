using PartageDepense.Language;
using PartageDepense.Model;
using PartageDepense.ViewModel;
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

namespace PartageDepense.View
{
    /// <summary>
    /// Logique d'interaction pour GestionDesParticipants.xaml
    /// </summary>
    public partial class GestionDesParticipants : UserControl
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
            DependencyProperty.Register("CultureCode", typeof(string), typeof(GestionDesParticipants), new PropertyMetadata(null, new PropertyChangedCallback(OnCultureCodeChanged)));

        private static void OnCultureCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GestionDesParticipants)d).CultureCode = (string)e.NewValue;
        }

        #endregion

        public GestionDesParticipants()
        {
            InitializeComponent();
            UpdateUI();
        }

        public void UpdateUI()
        {
            ParticipantsGroupBox.Header = LanguageManager.GetString("Liste des participants");
            ClientsNameLabel.Content = LanguageManager.GetString("Nom :");
            ClientsSurnameLabel.Content = LanguageManager.GetString("Prénom :");
            AddClientButton.Content = LanguageManager.GetString("Ajouter");
            DeleteClientButton.Content = LanguageManager.GetString("Supprimer");

            if (lesParticipants != null && lesParticipants.View is GridView gridView)
            {
                int count = 0;
                foreach (var column in gridView.Columns)
                {
                    switch (count++)
                    {
                        case 0:
                            column.Header = LanguageManager.GetString("Nom participant");
                            break;
                        case 1:
                            column.Header = LanguageManager.GetString("Prénom participant");
                            break;
                    }
                }
            }
        }
    }
}
