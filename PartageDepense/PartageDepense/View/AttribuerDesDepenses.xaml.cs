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
using static MaterialDesignThemes.Wpf.Theme;

namespace PartageDepense.View
{
    /// <summary>
    /// Logique d'interaction pour AttribuerDesDepenses.xaml
    /// </summary>
    public partial class AttribuerDesDepenses : UserControl
    {
        #region Propriété accessible dans la vue

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
                    DependencyProperty.Register("ActiviteSelectionnee", typeof(Activite), typeof(AttribuerDesDepenses), new PropertyMetadata(null, OnActiviteSelectionneeChanged));

        private static void OnActiviteSelectionneeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AttribuerDesDepenses)d;
            if (control.DataContext is GestionDesActivitesVM gestionDesActivitesVM)
            {
                gestionDesActivitesVM.ActiviteSelectionnee = (Activite?)e.NewValue;
            }
        }

        #endregion

        public string? CultureCode
        {
            set
            {
                LanguageManager.SetCulture(value);
                UpdateUI();
            }
        }

        public AttribuerDesDepenses()
        {
            InitializeComponent();
            UpdateUI();
        }

        #region Méthodes pour les comportements des boîtes de texte
        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            Comportements.TextBox_SelectionContenu(sender, e);
        }

        private void TextBox_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            Comportements.PreventionSuppressionSeparateurDecimale(sender, e);
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            Comportements.ValidationFormatReel(sender, e);
        }
        #endregion

        public void UpdateUI()
        {
            RegisterAExpense.Header = LanguageManager.GetString("Enregistrer une dépense");
            Description.Text = LanguageManager.GetString("Description :");
            Description2.Header = LanguageManager.GetString("Description");
            Amount.Text = LanguageManager.GetString("Montant :");
            Amount2.Header = LanguageManager.GetString("Montant");
            Date.Text = LanguageManager.GetString("Date :");
            Date2.Header = LanguageManager.GetString("Date");
            ExpensesList.Header = LanguageManager.GetString("Liste des dépenses");
            participants.Header = LanguageManager.GetString("Participants");
            DeleteExpense.Content = LanguageManager.GetString("Supprimer Une Dépense");
            AddToActivity.Content = LanguageManager.GetString("Ajouter à l'activité");

        }
    }
}
