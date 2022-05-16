using System;
using System.Collections.Generic;

namespace Giovanni.Common
{
    public abstract class CommandArguments
    {
        protected readonly Dictionary<string, string> Arguments;

        protected CommandArguments(string[] arguments)
        {
            Arguments = new Dictionary<string, string>();

            foreach (var argument in arguments)
            {
                var (key, value) = argument.Split('=');
                Console.WriteLine($"{key}, {value}");
                Arguments.Add(key, value ?? "true");
            }
        }

        protected bool GetBoolValue(string key, bool defaultValue = false)
        {
            var value = Arguments.GetValueOrDefault(key, null);

            return value is not null ? bool.Parse(value) : defaultValue;
        }
    }
}