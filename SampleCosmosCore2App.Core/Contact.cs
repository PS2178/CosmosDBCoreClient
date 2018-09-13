using Newtonsoft.Json;
using System;

namespace SampleCosmosCore2App.Core
{
    public class Contact
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}
