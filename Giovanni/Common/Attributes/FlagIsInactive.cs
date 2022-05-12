using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace Giovanni.Common.Attributes
{
    public class FlagIsInactive : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            throw new NotImplementedException();
        }
    }
}