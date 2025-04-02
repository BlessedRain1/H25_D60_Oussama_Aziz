using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartageDepense.Model
{
    public class Gestionnaire : ObservableObject
    {
        private ObservableCollection<Participant>? _lesParticipants;
        private ObservableCollection<Activite>? _lesActivites;
        private ObservableCollection<Sauvegarde>? _lesSauvegardes;

        public ObservableCollection<Participant>? LesParticipants
        {
            get => _lesParticipants;
            private set
            {
                if (_lesParticipants != value)
                {
                    _lesParticipants = value;
                    OnPropertyChanged(nameof(LesParticipants));
                }
            }
        }

        public ObservableCollection<Activite>? LesActivites
        {
            get => _lesActivites;
            private set
            {
                if (_lesActivites != value)
                {
                    _lesActivites = value;
                    OnPropertyChanged(nameof(LesActivites));
                }
            }
        }

        public ObservableCollection<Sauvegarde>? LesSauvegardes
        {
            get => _lesSauvegardes;
            private set
            {
                if (_lesSauvegardes != value)
                {
                    _lesSauvegardes = value;
                    OnPropertyChanged(nameof(LesSauvegardes));
                }
            }
        }

        public Gestionnaire()
        {
            _lesParticipants = new ObservableCollection<Participant>();
            _lesActivites = new ObservableCollection<Activite>();
            _lesSauvegardes = new ObservableCollection<Sauvegarde>();
        }

        public bool AjouterParticipant(string nom, string prenom)
        {
            nom = nom.Trim();
            prenom = prenom.Trim();

            if(LesParticipants?.Any(p => p.Nom == nom && p.Prenom == prenom) ?? true) return false;
            LesParticipants.Add(new Participant(nom, prenom));
            OnPropertyChanged(nameof(LesParticipants)); 
            return true;
        }

        public bool AjouterActivite(string nom)
        {
            if (string.IsNullOrEmpty(nom)) return false;

            LesActivites?.Add(new Activite(nom));
            OnPropertyChanged(nameof(LesActivites)); 
            return true;
        }

        public bool SupprimerParticipant(Participant participant)
        {
            if (participant == null || LesParticipants is not null && !LesParticipants.Contains(participant)) return false;

            // Vérifier si le participant a un solde non nul dans au moins une activité
            if(LesActivites is not null) foreach (var activite in LesActivites)
            {
                var participantSolde = activite.ParticipantsSoldes.FirstOrDefault(ps => ps.Participant.Nom == participant.Nom);
                if (participantSolde != null && participantSolde.Solde != 0)
                {
                    return false; // Impossible de supprimer si un solde est différent de zéro
                }
            }

            // Supprimer le participant de toutes les activités où il figure
            if(LesActivites is not null) foreach (var activite in LesActivites)
            {
                List<Depense>? lesDepenses = activite.LesDepenses?.ToList();

                //Supprimer toutes les dépenses liées au participant dans l'activité
                if(lesDepenses is not null) foreach(Depense d in lesDepenses)
                {
                    if(d.Participant == participant)
                    {
                        activite.SupprimerDepense(d);
                    }
                }
                activite.SupprimerParticipant(participant);
            }

            // Supprimer le participant de la liste principale
            LesParticipants?.Remove(participant);
            OnPropertyChanged(nameof(LesParticipants));

            return true;
        }

        public bool SupprimerActivite(Activite activite)
        {
            if (activite == null || LesActivites is not null && !LesActivites.Contains(activite)) return false;

            LesActivites?.Remove(activite);
            OnPropertyChanged(nameof(LesActivites));
            return true;
        }

        public void SetLists(List<Activite> a, List<Participant> p)
        {
            LesActivites = new ObservableCollection<Activite>(a);
            LesParticipants = new ObservableCollection<Participant>(p);
            OnPropertyChanged(nameof(LesParticipants));
            OnPropertyChanged(nameof(LesActivites));
        }
    }

}
