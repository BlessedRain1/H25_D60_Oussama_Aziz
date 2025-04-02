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
    /// Logique d'interaction pour GestionDesActivites.xaml
    /// </summary>
    public partial class GestionDesActivites : UserControl
    {
        #region Propriété accessible dans la vue

        public string? CultureCode
        {
            get
            {
                return (string?)GetValue(CultureCodeProperty);
            }

            set
            {
                LanguageManager.SetCulture(value);
                UpdateUI();
            }
        }

        public static readonly DependencyProperty CultureCodeProperty =
            DependencyProperty.Register("CultureCode", typeof(string), typeof(GestionDesActivites), new PropertyMetadata(null, new PropertyChangedCallback(OnCultureCodeChanged)));

        private static void OnCultureCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GestionDesActivites)d).CultureCode = (string)e.NewValue;
            ((GestionDesActivites)d).AttribuerDepenseTab.CultureCode = ((GestionDesActivites)d).CultureCode;
            ((GestionDesActivites)d).RemboursementsTab.CultureCode = ((GestionDesActivites)d).CultureCode;
            ((GestionDesActivites)d).AjouterParticipantTab.CultureCode = ((GestionDesActivites)d).CultureCode;
        }

        #endregion


        public GestionDesActivites()
        {
            InitializeComponent();
            UpdateUI();
        }
        /// <summary>
        /// Méthode pour attriuber le dataContext correspondant aux users controls de la vue GestionDesActivités
        /// seulement si le dataContext de la fenêtre mère est déjà défini.
        /// </summary>
        public void InitComponents()
        {
            if (this.DataContext != null && this.DataContext is GestionDesActivitesVM gestionDesActivitesVM)
            {
                Gestionnaire gestionnaire = gestionDesActivitesVM._gestio;

                AttribuerDepenseTab.DataContext = new AttribuerDepensesVM(gestionnaire);
                RemboursementsTab.DataContext = new GestionRemboursementsVM(gestionnaire);
                AjouterParticipantTab.DataContext = new AjoutParticipantVM(gestionnaire);
            }

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl)
            {
                RemboursementsTab.Visibility = Visibility.Hidden;
                AttribuerDepenseTab.Visibility = Visibility.Hidden;
                AjouterParticipantTab.Visibility = Visibility.Hidden;

                TabItem selectedTab = tabControl?.SelectedItem as TabItem ?? new TabItem();
                if (selectedTab != null) 
                {
                    switch (selectedTab.Header.ToString())
                    {
                        //Original Code
                        /*
                        case "Remboursements":
                            RemboursementsTab.Visibility = Visibility.Visible;
                            GestionRemboursementsVM gestioRem = (GestionRemboursementsVM)RemboursementsTab.DataContext;
                            //gestioRem.ChangementActiviteSelectionne();
                            break;
                        case "Attribuer des dépenses":
                            AttribuerDepenseTab.Visibility = Visibility.Visible;
                            break;
                        case "Ajouter des Participants":
                            AjouterParticipantTab.Visibility = Visibility.Visible;
                            break;
                        */
                        case "Refunds":
                        case "Remboursements":
                            RemboursementsTab.Visibility = Visibility.Visible;
                            break;
                        case "Assign Expenses":
                        case "Attribuer Des Dépenses":
                            AttribuerDepenseTab.Visibility = Visibility.Visible;
                            break;
                        case "Add Participants":
                        case "Ajouter Des Participants":
                            AjouterParticipantTab.Visibility = Visibility.Visible;
                            break;
                    }
                }
            }
        }

        private void UpdateUI()
        {
            ActivityList.Header = LanguageManager.GetString("Liste d'activités");
            OperationActivity.Header = LanguageManager.GetString("Opérations dans l'activité");
            NewActivity.Text = LanguageManager.GetString("Nouvelle Activité");
            DeleteActivityButton.Content = LanguageManager.GetString("Supprimer Activité");
            Accept.Content = LanguageManager.GetString("Accepter");
            Cancel.Content = LanguageManager.GetString("ANNULER");
            sauvegardeButton.Content = LanguageManager.GetString("Sauvegarder");
            ActivityNameColumn.Header = LanguageManager.GetString("Nom Activité");
            AddActivity.Text = LanguageManager.GetString("Ajouter une activité");

            if (AssignExpenses != null)
            {
                if (AssignExpenses.Header != null)
                {
                    AssignExpenses.Header = LanguageManager.GetString("Attribuer des dépenses");
                }
            }

            if (Refund != null)
            {
                if (Refund.Header != null)
                {
                    Refund.Header = LanguageManager.GetString("Remboursements");
                }
            }

            if (AddParticipants != null)
            {
                if (AddParticipants.Header != null)
                {
                    AddParticipants.Header = LanguageManager.GetString("Ajouter des participants");
                }
            }

        }
    }
}
