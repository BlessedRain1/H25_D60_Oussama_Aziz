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
    /// Logique d'interaction pour AjouterDesParticipants.xaml
    /// </summary>
    public partial class AjouterDesParticipants : UserControl
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
                    DependencyProperty.Register("ActiviteSelectionnee", typeof(Activite), typeof(AjouterDesParticipants), new PropertyMetadata(null, OnActiviteSelectionneeChanged));

        private static void OnActiviteSelectionneeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AjouterDesParticipants)d;
            if (control.DataContext is GestionDesActivitesVM gestionDesActivitesVM)
            {
                gestionDesActivitesVM.ActiviteSelectionnee = (Activite?)e.NewValue;
            }
        }

        public AjouterDesParticipants()
        {
            InitializeComponent();
            UpdateUI();
        }

        // Gérer le début du drag de façon plus fluide et controlée
        // (lorsque l'utilisateur commence à glisser intentionnellement)

        #region version du drag modifiée

        private Point _startPoint;

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null); // Stocke la position initiale de la souris
        }

        private void ListView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var listView = (System.Windows.Controls.ListView)sender;
                var selectedItem = listView.SelectedItem as Participant;

                // Vérifie si la souris a bougé suffisamment pour initier un drag
                Point currentPosition = e.GetPosition(null);
                if (selectedItem != null && IsDragGesture(_startPoint, currentPosition))
                {
                    DragDrop.DoDragDrop(listView, selectedItem, DragDropEffects.Move);
                    e.Handled = true;
                }
            }
        }

        private bool IsDragGesture(Point start, Point current)
        {
            return (Math.Abs(current.X - start.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(current.Y - start.Y) > SystemParameters.MinimumVerticalDragDistance);
        }
        #endregion

        // Gérer le drag (glisser) des éléments dans la ListView "Participants disponibles"
        private void ListView_DragOver(object sender, DragEventArgs e)
        {
            // Vérifiez que l'élément déplacé est un Participant
            if (e.Data.GetDataPresent(typeof(Participant)))
            {
                e.Effects = DragDropEffects.Move; // Permet le déplacement
            }
            else
            {
                e.Effects = DragDropEffects.None; // Aucun effet si les données sont incorrectes
            }
        }

        // Gérer l'événement Drop (déposer) pour déplacer l'élément dans "Participants de l'activité"
        private void ListView_Drop(object sender, DragEventArgs e)
        {
            var viewModel = (AjoutParticipantVM)this.DataContext; // Accéder au ViewModel

            // Vérifier si le participant peut être récupéré
            if (e.Data.GetDataPresent(typeof(Participant)))
            {
                var participant = (Participant)e.Data.GetData(typeof(Participant));

                // Appeler la méthode du ViewModel pour ajouter le participant
                viewModel.AjouterParticipantParDragDrop(participant);
            }
        }

        public void UpdateUI()
        {
            ParticipantActivity.Header = LanguageManager.GetString("Participants De l'Activité");
            AvalaibleParticipants.Header = LanguageManager.GetString("Participants Disponibles");
            ParticipantsFirstNames.Header = LanguageManager.GetString("Noms Participants");
            ParticipantsLastNames.Header = LanguageManager.GetString("Prénoms Participants");
            ParticipantsFirstNames2.Header = LanguageManager.GetString("Noms Participants");
            ParticipantsLastNames2.Header = LanguageManager.GetString("Prénoms Participants");
            AddParticipant.Content = LanguageManager.GetString("Ajouter Participant");
            WithdrawParticipant.Content = LanguageManager.GetString("Retirer Participant");
        }
    }
}
