using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PartageDepense.Model.JSON
{
    public class DepenseModel
    {
        [JsonPropertyName("Montant")]
        public decimal montant {  get; set; }

        [JsonPropertyName("Description")]
        public string? description { get; set; }

        [JsonPropertyName("Participant")]
        public ParticipantModel? participant { get; set; }

        [JsonPropertyName("Date")]
        public DateTime date { get; set; }
    }
}
