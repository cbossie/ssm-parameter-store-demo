using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace param_web_app
{
    public class SsmModel : IEnumerable<KeyValuePair<string, string>>
    {
        [JsonPropertyName("languagename")]
        public string LanguageName { get; set; }

        [JsonPropertyName("cityname")]
        public string Cityname { get; set; }

        [JsonPropertyName("planetname")]
        public string PlanetName { get; set; }

        [JsonPropertyName("statename")]
        public string StateName { get; set; }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            yield return new KeyValuePair<string, string>(nameof(LanguageName), LanguageName);
            yield return new KeyValuePair<string, string>(nameof(Cityname), Cityname);
            yield return new KeyValuePair<string, string>(nameof(PlanetName), PlanetName);
            yield return new KeyValuePair<string, string>(nameof(StateName), StateName);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
