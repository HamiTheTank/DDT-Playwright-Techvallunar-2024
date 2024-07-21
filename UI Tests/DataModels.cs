using Newtonsoft.Json;

namespace UI_Test__Playwright_
{
    public class DataModels
    {
        public class FilterCriteria
        {
            [JsonProperty("Produktart")]
            public string Produktart { get; set; }

            [JsonProperty("Marke")]
            public string Marke { get; set; }

            [JsonProperty("FurWen")]
            public string FurWen { get; set; }

            [JsonProperty("Highlights")]
            public string Highlights { get; set; }

            [JsonProperty("GeschenkFur")]
            public string GeschenkFur { get; set; }

            [JsonProperty("ExpectedCount")]
            public int ExpectedCount { get; set; }
        }
    }
}