using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartageDepense.Model;
using System.Windows;
using System.ComponentModel;

namespace PartageDepense.ViewModel
{
    public partial class AttribuerDepensesVM : GestionDesActivitesVM
    {
        public override void ChangementActiviteSelectionne()
        {
            // Exclusion
            if (ActiviteSelectionnee == null)
                return;

            LesParticipantsActivite = ActiviteSelectionnee.LesParticipantsActivite;
            LesDepenses = ActiviteSelectionnee.LesDepenses;
        }

        [ObservableProperty]
        private ObservableCollection<Participant>? _lesParticipantsActivite;

        partial void OnLesParticipantsActiviteChanged(ObservableCollection<Participant>? value)
        {
            if (value == null) return;
            LesParticipantsActivite = value;
        }

        [ObservableProperty]
        private Participant? _participantSelectionne;

        partial void OnParticipantSelectionneChanged(Participant? value)
        {
            if (value == null) return;
            ParticipantSelectionne = value;
        }

        [ObservableProperty]
        private DateTime? _dateDepense;

        partial void OnDateDepenseChanged(DateTime? value)
        {
            if (value == null) return;
            DateDepense = value;
        }

        [ObservableProperty]
        private decimal _montant;

        partial void OnMontantChanged(decimal value)
        {
            Montant = value;
        }

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private ObservableCollection<Depense>? _lesDepenses;

        partial void OnLesDepensesChanged(ObservableCollection<Depense>? value)
        {
            if (value == null) return;
            LesDepenses = value;
        }

        [ObservableProperty]
        private Depense? _depenseSelectionnee;

        partial void OnDepenseSelectionneeChanged(Depense? value)
        {
            if (value == null) return;
            DepenseSelectionnee = value;
        }

        public AttribuerDepensesVM(Gestionnaire gestionnaire) : base(gestionnaire)
        {
            if (ActiviteSelectionnee is not null)
            {
                _lesParticipantsActivite = ActiviteSelectionnee.LesParticipantsActivite;
                _lesDepenses = ActiviteSelectionnee.LesDepenses;

                ActiviteSelectionnee.PropertyChanged += OnPropertyChanged;
            }
            _montant = 0;
            _description = "";

        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LesParticipantsActivite))
                LesParticipantsActivite = ActiviteSelectionnee?.LesParticipantsActivite;

            if (e.PropertyName == nameof(LesDepenses))
                LesDepenses = ActiviteSelectionnee?.LesDepenses;
        }

        [RelayCommand]
        public void AjouterDepense()
        {
            // Vérification des champs obligatoires
            if (ActiviteSelectionnee == null)
            {
                MessageBox.Show("Veuillez sélectionner une activité.", "Erreur Ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (ParticipantSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un participant.", "Erreur Ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Description == null || Description.Trim().Length == 0)
            {
                MessageBox.Show("Veuillez entrer une description.", "Erreur Ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Montant <= 0)
            {
                MessageBox.Show("Veuillez entrer un montant valide supérieur à zéro.", "Erreur Ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DateDepense == null)
            {
                MessageBox.Show("Veuillez sélectionner une date pour cette dépense.", "Erreur Ajout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Si tout est validé, ajouter la dépense
            ActiviteSelectionnee.AjouterDepense(ParticipantSelectionne, Description.Trim(), Montant, DateDepense);

            OnPropertyChanged(nameof(LesDepenses));

            Description = "";
            Montant = 0;
            DateDepense = null; 
            ParticipantSelectionne = null;
        }

        [RelayCommand]
        public void SuprimmerDepense()
        {
            // Vérifier si une dépense et une activité sont sélectionnées
            if (DepenseSelectionnee == null || ActiviteSelectionnee == null)
            {
                MessageBox.Show("Veuillez sélectionner une dépense et une activité avant de procéder à la suppression.", "Erreur de suppression", MessageBoxButton.OK, MessageBoxImage.Error);
                return;  // Sortir de la méthode si les conditions ne sont pas remplies
            }

            // Demander une confirmation avant la suppression
            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette dépense ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Confirmer la suppression de la dépense
                bool isDeleted = ActiviteSelectionnee.SupprimerDepense(DepenseSelectionnee);

                if (isDeleted)
                {
                    MessageBox.Show("La dépense a été supprimée avec succès.", "Succès de suppression", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("La dépense n'a pas pu être supprimée. Veuillez réessayer.", "Erreur de suppression", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Description = "";
                Montant = 0;
                DateDepense = null;
                ParticipantSelectionne = null;
            }
            else
            {
                // Si l'utilisateur annule la suppression
                MessageBox.Show("La suppression de la dépense a été annulée.", "Annulation de suppression", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}
