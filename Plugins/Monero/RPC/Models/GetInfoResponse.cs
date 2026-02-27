using Newtonsoft.Json;

namespace BTCPayServer.Plugins.Monero.RPC.Models
{
    public class GetInfoResponse
    {
        [JsonProperty("height")] public long Height { get; set; }
        [JsonProperty("busy_syncing")] public bool BusySyncing { get; set; }
        [JsonProperty("status")] public string Status { get; set; }
        [JsonProperty("version")] public string Version { get; set; }
        [JsonProperty("restricted")] public bool Restricted { get; set; }
        [JsonProperty("target_height")] public long? TargetHeight { get; set; }
    }
}