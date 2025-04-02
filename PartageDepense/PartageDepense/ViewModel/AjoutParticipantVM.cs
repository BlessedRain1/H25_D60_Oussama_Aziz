using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartageDepense.Model;
using System.Windows;

namespace PartageDepense.ViewModel
{
    public partial class AjoutParticipantVM : GestionDesActivitesVM
    {
        public override void ChangementActiviteSelectionne()
        {
            // Exclusion
            if (ActiviteSelectionnee == null)
                return;

            LesParticipantsActivite = ActiviteSelectionnee?.LesParticipantsActivite ?? new ObservableCollection<Participant>();
        }

        [ObservableProperty]
        private ObservableCollection<Participant>? _lesParticipants;

        partial void OnLesParticipantsChanged(ObservableCollection<Participant>? value)
        {
            if (value == null) return;
            LesParticipants = value;
        }

        [ObservableProperty]
        private Participant? _participantSelectionne;

        partial void OnParticipantSelectionneChanged(Participant? value)
        {
            if (value == null) return;
            ParticipantSelectionne = value;
            ParticipantSelectionneActivite = null;
        }

        [ObservableProperty]
        private ObservableCollection<Participant>? _lesParticipantsActivite;

        partial void OnLesParticipantsActiviteChanged(ObservableCollection<Participant>? value)
        {
            if (value == null) return;
            _lesParticipantsActivite = value;
        }

        [ObservableProperty]
        private Participant? _participantSelectionneActivite;

        partial void OnParticipantSelectionneActiviteChanged(Participant? value)
        {
            if (value == null) return;
            ParticipantSelectionneActivite = value;
            ParticipantSelectionne = null;
        }

        public AjoutParticipantVM(Gestionnaire gestionnaire) : base(gestionnaire)
        {
            LesParticipants = gestionnaire.LesParticipants ?? new ObservableCollection<Participant>();

            if (ActiviteSelectionnee != null)
                LesParticipantsActivite = ActiviteSelectionnee.LesParticipantsActivite;
        }

        [RelayCommand]
        public void AjouterParticipant()
        {
            // Vérifier si aucun participant n'est sélectionné
            if (ParticipantSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un participant avant de l'ajouter.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                ParticipantSelectionne = null;
                return; // Arrêter l'exécution de la méthode si aucun participant n'est sélectionné
            }

            // Vérifier si le participant est déjà dans la liste des participants de l'activité
            if (LesParticipantsActivite?.Contains(ParticipantSelectionne) ?? true)
            {
                MessageBox.Show("Ce participant est déjà inscrit à cette activité. Veuillez en choisir un autre.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                ParticipantSelectionne = null;
                return; // Arrêter l'exécution si le participant est déjà dans la liste
            }

            // Ajouter le participant à l'activité
            ActiviteSelectionnee?.AjouterParticipant(ParticipantSelectionne);

            // Réinitialiser la sélection du participant
            ParticipantSelectionne = null;
        }

        [RelayCommand]
        public void SupprimerParticipant()
        {
            // Vérifier si un participant ou une activité n'est pas sélectionné
            if (ParticipantSelectionneActivite == null || ActiviteSelectionnee == null)
            {
                MessageBox.Show("Veuillez sélectionner un participant avant de procéder.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Arrêter l'exécution de la méthode si un participant ou une activité n'est pas sélectionné
            }

            // Confirmer la suppression du participant avant de procéder
            var result = MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer {ParticipantSelectionneActivite.Nom} {ParticipantSelectionneActivite.Prenom} de cette activité ?",
                "Confirmation de suppression",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            // Si l'utilisateur confirme, procéder à la suppression
            if (result == MessageBoxResult.Yes)
            {
                ActiviteSelectionnee.SupprimerParticipant(ParticipantSelectionneActivite);
                ParticipantSelectionneActivite = null; // Réinitialiser la sélection du participant
                MessageBox.Show("Le participant a été supprimé avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Réinitialiser la sélection du participant
                ParticipantSelectionne = null;
            }
        }

        // Méthode pour gérer l'ajout d'un participant par drag-and-drop
        public void AjouterParticipantParDragDrop(Participant participant)
        {
            // Vérifier si le participant est déjà dans la liste des participants de l'activité
            if (LesParticipantsActivite?.Contains(participant) ?? true)
            {
                ParticipantSelectionne = null;
                MessageBox.Show("Ce participant est déjà inscrit à cette activité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Ajouter le participant à la liste des participants de l'activité
            ActiviteSelectionnee?.AjouterParticipant(participant);

            ParticipantSelectionne = null;
        }
    }
}
