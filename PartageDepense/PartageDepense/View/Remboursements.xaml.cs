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
using PartageDepense.Model;
using PartageDepense.ViewModel;

namespace PartageDepense.View
{
    /// <summary>
    /// Logique d'interaction pour Remboursements.xaml
    /// </summary>
    public partial class Remboursements : UserControl
    {

        public string? CultureCode
        {
            set
            {
                LanguageManager.SetCulture(value);
                UpdateUI();
            }
        }

        public Activite? ActiviteSelectionnee
        {
            get
            {
                // Exclusions
                if (DataContext == null) return null;
                if (DataContext is not GestionDesActivitesVM) return null;

                return ((GestionDesActivitesVM)DataContext).ActiviteSelectionnee;
            }

            set
            {
                // Exclusions
                if (DataContext == null) return;
                if (DataContext is not GestionDesActivitesVM) return;

                ((GestionDesActivitesVM)DataContext).ActiviteSelectionnee = value;
            }
        }

        public static readonly DependencyProperty ActiviteProperty =
                    DependencyProperty.Register("ActiviteSelectionnee", typeof(Activite), typeof(Remboursements), new PropertyMetadata(null, OnActiviteSelectionneeChanged));

        private static void OnActiviteSelectionneeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Remboursements)d;
            if (control.DataContext is GestionDesActivitesVM gestionDesActivitesVM)
            {
                gestionDesActivitesVM.ActiviteSelectionnee = (Activite?)e.NewValue;
            }
        }

        public Remboursements()
        {
            InitializeComponent();
            UpdateUI();
        }

        public void UpdateUI()
        {
            AccountState.Header = LanguageManager.GetString("États des comptes");
            Participants.Header = LanguageManager.GetString("Participants");
            Solde.Header = LanguageManager.GetString("Solde");
            SoldeState.Header = LanguageManager.GetString("État Du Solde");
            OperationParticipant.Header = LanguageManager.GetString("Opérations Du Participants");
            Refund.Content = LanguageManager.GetString("Rembourser");
            GetBack.Content = LanguageManager.GetString("Récupérer");
        }
    }
}
