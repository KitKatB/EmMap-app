using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTabs.DataModels
{
    public class emmaptable
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "Emotion")]
        public string Emotion { get; set; }

        [JsonProperty(PropertyName = "Longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "Latitude")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string createdAT { get; set; }

    }
}
