using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartageDepense.Model
{
    public class Depense
    {
        public decimal Montant {  get; private set; }

        public string? Description { get; private set; }

        public DateTime? Date { get; private set; }

        public Participant? Participant { get; private set; }

        public Depense(Participant p, string description, decimal montant, DateTime? date) 
        {
            Participant = p;
            Description = description;
            Montant = montant;
            Date = date;
        }
    }
}
