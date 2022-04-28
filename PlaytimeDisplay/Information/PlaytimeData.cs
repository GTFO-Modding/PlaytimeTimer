using System.Text.Json.Serialization;

namespace PlaytimeDisplay.Information
{
    public class PlaytimeData
    {
        public PlaytimeData(string rundownName, ulong wastedSeconds)
        {
            RundownName = rundownName;
            WastedSeconds = wastedSeconds;
            Started = wastedSeconds;
        }

        public string RundownName { get; set; }
        public ulong WastedSeconds { get; set; }

        [JsonIgnore]
        public ulong Started { get; set; }
    }
}
