using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PartageDepense.Model.JSON
{
    public class ParticipantModel
    {
        [JsonPropertyName("Nom")]
        public string? nom {  get; set; }

        [JsonPropertyName("Prenom")]
        public string? prenom {  get; set; }
    }
}
