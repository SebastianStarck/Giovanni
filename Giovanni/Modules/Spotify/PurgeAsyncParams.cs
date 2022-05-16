using System.Collections.Generic;
using Giovanni.Common;

namespace Giovanni.Modules.Spotify
{
    public class PurgeAsyncParams : CommandArguments
    {
        public PurgeAsyncParams(string[] arguments) : base(arguments)
        {
        }

        public void Deconstruct(out bool disposeBotMessage, out string prefix, out bool botOnly)
        {
            disposeBotMessage = GetBoolValue("disposeBotMessage", false);
            prefix = Arguments.GetValueOrDefault("prefix", "");
            botOnly = GetBoolValue("botOnly");
        }
    }
}