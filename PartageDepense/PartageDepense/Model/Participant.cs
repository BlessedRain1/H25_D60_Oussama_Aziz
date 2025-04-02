using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartageDepense.Model
{
    public class Participant
    {
        public string? Nom { get; private set; }

        public string? Prenom { get; private set; }

        //Constructeur

        public Participant(string nom, string prenom) 
        { 
            Nom = nom;
            Prenom = prenom;
        }
    }
}
