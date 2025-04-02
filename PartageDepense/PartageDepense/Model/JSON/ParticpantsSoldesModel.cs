using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace PartageDepense.Model.JSON
{
    public class ParticpantsSoldesModel
    {
        [JsonPropertyName("Participant")]
        public ParticipantModel? participant { get; set; }

        [JsonPropertyName("Solde")]
        public decimal solde { get; set; }

        [JsonPropertyName("Etat")]
        public string? etat { get; set; }
    }
}
