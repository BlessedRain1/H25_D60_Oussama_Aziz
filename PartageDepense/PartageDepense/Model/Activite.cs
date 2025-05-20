using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PartageDepense.Model
{
    public class Activite : ObservableObject
    {
        private bool _donneesModifiees = true;

        public string NomActivite { get; private set; }

        public ObservableCollection<Participant>? LesParticipantsActivite { get; private set; }

        public ObservableCollection<Depense>? LesDepenses { get; private set; }

        public ObservableCollection<Remboursement>? LesRemboursements { get; private set; }

        private ObservableCollection<ParticipantSolde> _participantsSoldes;

        public ObservableCollection<ParticipantSolde> ParticipantsSoldes
        {
            get
            {
                // Avant de retourner la liste, on calcule les remboursements
                //CalculerRemboursement();
                return _participantsSoldes;
            }
        }



        public Activite(string nom)
        {
            NomActivite = nom;
            LesParticipantsActivite = new ObservableCollection<Participant>();
            LesDepenses = new ObservableCollection<Depense>();
            LesRemboursements = new ObservableCollection<Remboursement>();
            _participantsSoldes = new ObservableCollection<ParticipantSolde>();
        }

        public Activite(string nom, ObservableCollection<Participant> participants, ObservableCollection<Depense> depenses, ObservableCollection<Remboursement> remboursements, ObservableCollection<ParticipantSolde> participantsSoldes)
        {
            NomActivite = nom;
            LesParticipantsActivite = participants;
            LesDepenses = depenses;
            LesRemboursements = remboursements;
            _participantsSoldes = participantsSoldes;
        }

        public bool AjouterParticipant(Participant participant)
        {
            // Exclusions
            if (participant == null) return false;
            if (LesParticipantsActivite is not null && LesParticipantsActivite.Contains(participant)) return false;

            //Ajout
            if (LesParticipantsActivite is not null && !LesParticipantsActivite.Contains(participant)) LesParticipantsActivite.Add(participant);

            _donneesModifiees = true;
            CalculerRemboursement();

            OnPropertyChanged(nameof(LesParticipantsActivite));

            return true;
        }
        public bool SupprimerParticipant(Participant participant)
        {
            // Exclusions
            if (participant == null) return false;
            if (LesParticipantsActivite is not null && !LesParticipantsActivite.Contains(participant)) return false;

            //Ajout
            if (LesParticipantsActivite is not null && LesParticipantsActivite.Contains(participant)) LesParticipantsActivite.Remove(participant);

            _donneesModifiees = true;
            CalculerRemboursement();

            OnPropertyChanged(nameof(LesParticipantsActivite));

            return true;
        }
        public bool AjouterDepense(Participant participant, string description, decimal montant, DateTime? date)
        {
            if (LesParticipantsActivite is not null && LesParticipantsActivite.Contains(participant))
            {
                LesDepenses?.Add(new Depense(participant, description, montant, date));
                _donneesModifiees = true;
                CalculerRemboursement();

                return true;
            }
            return false;
        }

        public bool EffectuerRemboursement(Participant participant, decimal montant)
        {
            // Vérification de la validité du participant
            if (LesParticipantsActivite?.Contains(participant) != true)
                return false;

            // Ajout du remboursement à l'historique
            LesRemboursements?.Add(new Remboursement(montant, participant));

            // Récupération et suppression de l'ancien solde
            var ancienSolde = _participantsSoldes.FirstOrDefault(p => p.Participant == participant);
            if (ancienSolde == null) return false;

            _participantsSoldes.Remove(ancienSolde);

            // Calcul du nouveau solde après remboursement
            decimal montantAbsolu = Math.Abs(montant);
            decimal nouveauSolde = ancienSolde.Solde + montantAbsolu;

            // Mise à jour du solde si non nul
            if (nouveauSolde != 0)
            {
                string statut = nouveauSolde > 0 ? "non récupéré" : "non remboursé";
                _participantsSoldes.Add(new ParticipantSolde(participant, nouveauSolde, statut));
            }

            // Répartition du montant entre les créditeurs
            var creditrices = _participantsSoldes
                .Where(p => p.Solde > 0)
                .ToList();

            if (creditrices.Count > 0)
            {
                decimal partParCreditrice = montantAbsolu / creditrices.Count;

                foreach (var creditrice in creditrices)
                {
                    _participantsSoldes.Remove(creditrice);
                    decimal nouveauSoldeCreditrice = creditrice.Solde - partParCreditrice;

                    if (nouveauSoldeCreditrice > 0)
                    {
                        string statut = nouveauSoldeCreditrice > 0 ? "non récupéré" : "non remboursé";
                        _participantsSoldes.Add(new ParticipantSolde(creditrice.Participant, nouveauSoldeCreditrice, statut));
                    }
                }
            }

            // Mise à jour de l'UI
            OnPropertyChanged(nameof(ParticipantsSoldes));
            return true;
        }

        public void RecupererSolde(Participant participant, decimal montant)
        {
            // Vérification de la validité du participant
            if (LesParticipantsActivite?.Contains(participant) != true)
                return;

            // Ajout du remboursement à l'historique
            LesRemboursements?.Add(new Remboursement(montant, participant));

            // Récupération et suppression de l'ancien solde
            var ancienSolde = _participantsSoldes.FirstOrDefault(p => p.Participant == participant);
            if (ancienSolde == null) return;

            _participantsSoldes.Remove(ancienSolde);

            // Calcul du nouveau solde après récupération
            decimal nouveauSolde = ancienSolde.Solde - montant;

            // Mise à jour du solde si non nul
            if (nouveauSolde != 0)
            {
                string statut = nouveauSolde > 0 ? "non récupéré" : "non remboursé";
                _participantsSoldes.Add(new ParticipantSolde(participant, nouveauSolde, statut));
            }

            // Mise à jour de l'UI
            OnPropertyChanged(nameof(ParticipantsSoldes));
        }

        public bool SupprimerDepense(Depense depense)
        {
            if (LesDepenses is not null && LesDepenses.Contains(depense))
            {
                LesDepenses.Remove(depense);
                _donneesModifiees = true;
                CalculerRemboursement();
                return true;
            }
            return false;
        }

        public void CalculerRemboursement()
        {
            // Ne recalculer que si les données ont changé
            if (!_donneesModifiees || LesParticipantsActivite?.Count == 0 || LesDepenses?.Count == 0)
                return;

            decimal totalDepenses = LesDepenses?.Sum(d => d.Montant) ?? new decimal();
            decimal montantContribution = totalDepenses / LesParticipantsActivite?.Count ?? 1;

            _participantsSoldes.Clear();

            if (LesParticipantsActivite != null)
                foreach (Participant participant in LesParticipantsActivite)
                {
                    decimal totalPaye = LesDepenses?
                        .Where(d => d.Participant?.Nom == participant.Nom).Where(d => d.Participant?.Prenom == participant.Prenom)
                        .Sum(d => d.Montant) ?? new decimal();

                    decimal solde = totalPaye - montantContribution;

                    if (solde != 0)
                    {
                        string statut = solde > 0
                            ? "non récupéré"  // Le participant a payé trop et doit récupérer de l'argent
                            : "non remboursé"; // Le participant n'a pas payé assez et doit rembourser de l'argent

                        _participantsSoldes.Add(new ParticipantSolde(participant, solde, statut));
                    }
                }


            _donneesModifiees = false;
            OnPropertyChanged(nameof(ParticipantsSoldes));
        }
        /// <summary>
        /// Génère une liste de données formatées pour le graphique des soldes par participant.
        /// </summary>
        /// <returns>Liste de GraphData représentant les soldes des participants.</returns>
        public List<GraphData> ObtenirGraphiqueSoldes()
        {
            if (_participantsSoldes == null || !_participantsSoldes.Any())
                return new List<GraphData>();

            return _participantsSoldes
                .Select(ps => new GraphData(
                    $"{ps.Participant.Prenom} {ps.Participant.Nom}",
                    (double)ps.Solde)) // Conversion de decimal → double
                .ToList();
        }

        /// <summary>
        /// Génère une liste de données formatées pour le graphique de répartition des dépenses par participant.
        /// Filtre les dépenses sans participant associé.
        /// </summary>
        /// <returns>Liste de GraphData représentant les dépenses par participant.</returns>
        public List<GraphData> ObtenirGraphiqueDepenses()
        {
            if (LesDepenses == null || !LesDepenses.Any())
                return new List<GraphData>();

            return LesDepenses
                .Where(d => d.Participant != null) // Filtre les dépenses qui n'ont pas de participant valide
                .GroupBy(d => d.Participant)
                .Select(g => new GraphData(
                    $"{g.Key?.Prenom} {g.Key?.Nom}", // Utilise l'opérateur ?. pour gérer un participant potentiellement null (bien que déjà filtré)
                    (double)g.Sum(d => d.Montant))) // g.Key est le Participant
                .ToList();
        }
    }
}
