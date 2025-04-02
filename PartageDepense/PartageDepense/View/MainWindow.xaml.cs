using PartageDepense.Model;
using PartageDepense.Language;
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
using System.Windows.Shapes;
using System.Globalization;

namespace PartageDepense.View
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;

            Gestionnaire gestionnaire = new Gestionnaire();
            Sauvegarde s = new Sauvegarde();
            s.ChargerSauvegarde(gestionnaire);

            this.DataContext = new GestionnaireVM(gestionnaire);
            GestionParticipantPage.DataContext = new GestionDesParticipantsVM(gestionnaire);
            GestionActivitePage.DataContext = new GestionDesActivitesVM(gestionnaire);

            if (GestionActivitePage.DataContext != null)
                GestionActivitePage.InitComponents();

        }

        private void GestionDesParticipants(object sender, RoutedEventArgs e)
        {
            // Affiche la page des clients
            GestionParticipantPage.Visibility = Visibility.Visible;
            GestionActivitePage.Visibility = Visibility.Hidden;
        }

        // Gère le clic sur le bouton "Activités"
        private void GestionDesActivites(object sender, RoutedEventArgs e)
        {
            // Affiche la page des calculs
            GestionParticipantPage.Visibility = Visibility.Hidden;
            GestionActivitePage.Visibility = Visibility.Visible;
        }

        private void UpdateLanguage()
        {
            var selectedItem = LanguageSelector.SelectedItem as System.Windows.Controls.ComboBoxItem;
            if (selectedItem != null)
            {
                string? cultureCode = selectedItem.Tag.ToString();
                LanguageManager.SetCulture(cultureCode);
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            Titre.Text = LanguageManager.GetString("Partage des dépenses : Finance Canada");

            //
            if (TabItemParticipant != null)
            {
                var headerP = TabItemParticipant.Header as StackPanel;
                var textBlock = headerP?.Children.OfType<TextBlock>().FirstOrDefault();
                if (textBlock != null)
                {
                    textBlock.Text = LanguageManager.GetString("Participant"); 
                }
            }

            if (TabItemActivite != null)
            {
                var headerA = TabItemActivite.Header as StackPanel;
                var textBlock = headerA?.Children.OfType<TextBlock>().FirstOrDefault();
                if (textBlock != null)
                {
                    textBlock.Text = LanguageManager.GetString("Activités"); 
                }
            }

            
            if (TabItemPalette != null)
            {
                var headerPal = TabItemPalette.Header as StackPanel;
                var textBlock = headerPal?.Children.OfType<TextBlock>().FirstOrDefault();
                if (textBlock != null)
                {
                    textBlock.Text = LanguageManager.GetString("Palette"); 
                }
            }
        }

        private async void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Code que vous souhaitez exécuter lors de la fermeture de l'application
            var result = MessageBox.Show("Êtes-vous sûr de vouloir quitter ?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // Annuler la fermeture si l'utilisateur choisit "Non"
            }
            else
            {
                // Effectuer la sauvegarde
                GestionnaireVM gestionnaireVM = (GestionnaireVM)this.DataContext;
                Gestionnaire gestionnaire = gestionnaireVM._gestionnaire;

                Sauvegarde s = new Sauvegarde();
                await s.EffectuerSauvegardeAsync(gestionnaire);

                // Vous pouvez mettre un message de confirmation si nécessaire
                MessageBox.Show("Sauvegarde effectuée avec succès.","Sauvegarde automatique", MessageBoxButton.OK,MessageBoxImage.Asterisk);
            }
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLanguage();
        }
    }
}
