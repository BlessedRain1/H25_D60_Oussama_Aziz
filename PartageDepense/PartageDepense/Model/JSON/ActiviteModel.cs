using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PartageDepense.Model.JSON
{
    public class ActiviteModel
    {
        [JsonPropertyName("NomActivite")]
        public string? nomActivite { get; set; }

        [JsonPropertyName("LesParticipantsActivite")]
        public List<ParticipantModel>? lesParticipants { get; set; }

        [JsonPropertyName("LesDepenses")]
        public List<DepenseModel>? lesDepenses { get; set; }

        [JsonPropertyName("LesRemboursements")]
        public List<RemboursementModel>? lesRemboursements { get; set; }

        [JsonPropertyName("ParticipantsSoldes")]
        public List<ParticpantsSoldesModel>? participantsSoldes { get; set; }
    }
}
