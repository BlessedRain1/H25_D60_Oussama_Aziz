using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartageDepense.Model
{
    public class ParticipantSolde
    {
        public Participant Participant { get; set; }
        public decimal Solde { get; set; }
        public string Etat { get; set; }


        public ParticipantSolde(Participant participant, decimal solde, string etat)
        {
            Participant = participant;
            Solde = solde;
            Etat = etat;
        }
    }
}
