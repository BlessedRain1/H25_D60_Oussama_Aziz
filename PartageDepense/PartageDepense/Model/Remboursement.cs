using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartageDepense.Model
{
    public class Remboursement
    {
        public decimal? Montant { get; private set; }

        public Participant? Participant { get; private set; }

        public Remboursement(decimal? montant, Participant? participant)
        {
            Montant = montant;
            Participant = participant;
        }
    }
}
