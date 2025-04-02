using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PartageDepense.Model.JSON
{
    public class ResponseModel
    {
        [JsonPropertyName("LesParticipants")]
        public List<ParticipantModel>? lesParticipants { get; set; }

        [JsonPropertyName("LesActivites")]
        public List<ActiviteModel>? lesActivites { get; set; }
    }
}
