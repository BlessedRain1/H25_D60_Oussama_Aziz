using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartageDepense.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace PartageDepense.ViewModel
{
    public partial class GestionRemboursementsVM : GestionDesActivitesVM
    {
        public override void ChangementActiviteSelectionne()
        {
            // Exclusion
            if (ActiviteSelectionnee == null)
                return;

            ActiviteSelectionnee.CalculerRemboursement();

            ParticipantsSoldes = ActiviteSelectionnee.ParticipantsSoldes;
        }

        [ObservableProperty]
        public ObservableCollection<ParticipantSolde>? _participantsSoldes;

        [ObservableProperty]
        public ParticipantSolde? _participantSoldeSelectionne;

        partial void OnParticipantSoldeSelectionneChanged(ParticipantSolde? value)
        {
            if (value != null)
            {
                ParticipantSoldeSelectionne = value;
            }
        }

        public GestionRemboursementsVM(Gestionnaire gestionnaire) : base(gestionnaire)
        {
          
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ParticipantsSoldes))
                ParticipantsSoldes = ActiviteSelectionnee?.ParticipantsSoldes ?? new ObservableCollection<ParticipantSolde>();
            
        }


        [RelayCommand]
        public void EffectuerRemboursement()
        {
            if (ParticipantSoldeSelectionne is not null && ParticipantsSoldes is not null && ParticipantsSoldes.Contains(ParticipantSoldeSelectionne) && (ParticipantSoldeSelectionne.Etat == "non remboursé"))
            {
                ActiviteSelectionnee?.EffectuerRemboursement(ParticipantSoldeSelectionne.Participant, ParticipantSoldeSelectionne.Solde);

                OnPropertyChanged(nameof(ParticipantsSoldes));

                // changer l'état de solde du participant sélectionné
                ParticipantSoldeSelectionne = null;
            }
            else if (ParticipantSoldeSelectionne == null) { MessageBox.Show("Aucun état sélectionné", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning); }
            else { MessageBox.Show("Ce montant a déjà été remboursé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }

        [RelayCommand]
        public void RecupererMontant()
        {
            if (ParticipantSoldeSelectionne is not null && ParticipantsSoldes is not null && ParticipantsSoldes.Contains(ParticipantSoldeSelectionne) && (ParticipantSoldeSelectionne.Etat == "non récupéré"))
            {
                ActiviteSelectionnee?.RecupererSolde(ParticipantSoldeSelectionne.Participant, ParticipantSoldeSelectionne.Solde);

                OnPropertyChanged(nameof(ParticipantsSoldes));

                // changer l'état de solde du participant sélectionné
                ParticipantSoldeSelectionne = null;
            }
            else if (ParticipantSoldeSelectionne == null) { MessageBox.Show("Aucun état sélectionné", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning); }
            else { MessageBox.Show("Ce montant a déjà été récupéré.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }
    }
}
