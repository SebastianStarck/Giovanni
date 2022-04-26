using Newtonsoft.Json;

namespace Giovanni.Models
{
    public class Paginated<T>
    {
        [JsonProperty("limit")] public int Limit;
        [JsonProperty("total")] public int Total;
        [JsonProperty("offset")] public int Offset;

        [JsonProperty("next")] public string Next;
        [JsonProperty("previous")] public string Previous;

        [JsonProperty("items")] public T[] Items;

        public override string ToString()
        {
            return $"{Items.Length} {typeof(T)}";
        }
    }
}