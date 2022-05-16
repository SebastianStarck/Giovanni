using Discord.Commands;

namespace Giovanni.Modules
{
    [NamedArgumentType]
    public class PurgeAsyncArguments
    {
        public bool NoMessage { get; set; } = false;
        public string Prefix { get; set; } = "";
        public bool BotOnly { get; set; } = false;

        public void Deconstruct(out bool disposeBotMessage, out string prefix, out int prefixLength, out bool botOnly)
        {
            disposeBotMessage = NoMessage;
            prefix = Prefix;
            botOnly = BotOnly;
            prefixLength = prefix?.Length ?? 0;
        }
    }
}