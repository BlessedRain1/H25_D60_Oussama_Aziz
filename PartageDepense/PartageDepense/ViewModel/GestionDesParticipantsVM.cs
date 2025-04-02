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
using System.ComponentModel;
using PartageDepense.View;

namespace PartageDepense.ViewModel
{
    public partial class GestionDesParticipantsVM : GestionnaireVM
    {

        [ObservableProperty]
        private string? _nom, _prenom;

        [ObservableProperty]
        private ObservableCollection<Participant>? _lesParticipants;
        partial void OnLesParticipantsChanged(ObservableCollection<Participant>? value)
        {
            if (LesParticipants != value)
            {
                LesParticipants = value;
            }
        }

        [ObservableProperty]
        private Participant? _participantSelectionne;

        partial void OnParticipantSelectionneChanged(Participant? value)
        {
            _participantSelectionne = value;
            if (_participantSelectionne != null)
            {
                Nom = _participantSelectionne.Nom;
                Prenom = _participantSelectionne.Prenom;
            }
        }

        public GestionDesParticipantsVM(Gestionnaire gestionnaire) : base(gestionnaire)
        {
            LesParticipants = gestionnaire?.LesParticipants ?? new ObservableCollection<Participant>();
        }

        [RelayCommand]
        public void AjouterParticipant()
        {
            if (string.IsNullOrEmpty(Nom) || string.IsNullOrEmpty(Prenom))
                MessageBox.Show("Veuillez remplir tous les champs vides.", "Erreur d'ajout du participant", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (_gestionnaire.AjouterParticipant(Nom, Prenom))
            {
                Nom = "";
                Prenom = "";
            }
            else
            {
                MessageBox.Show("Ce participant existe déjà. Veuillez changer les champs nom et prenom", "Erreur d'ajout du participant", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public void SupprimerParticipant()
        {
            if (ParticipantSelectionne is null) return;
            bool result = _gestionnaire.SupprimerParticipant(ParticipantSelectionne);
            //confirmer le participant avant suppression
            if (ParticipantSelectionne != null && result)
            {
                MessageBox.Show("Participant Supprimé avec succès.", "Succès Suppression", MessageBoxButton.OK, MessageBoxImage.Information);
                Nom = "";
                Prenom = "";
            }
            else if (ParticipantSelectionne != null && !result)
            {
                MessageBox.Show("Le participant sélectionné à un solde dans une activité. Veuillez régulariser son solde avant de le supprimer.", "Erreur de suppression du participant", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner le participant à supprimer.", "Erreur de suppression du participant", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
