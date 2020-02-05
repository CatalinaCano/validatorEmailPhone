using Newtonsoft.Json;


namespace Fx_validator_email_phone.Serialization
{
    /*
     * Clase de serialización para los parametros de entrada
     */
    public class InputData
    {
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("phone_number")]
        public string cellphonenumber { get; set; }
    }
}
