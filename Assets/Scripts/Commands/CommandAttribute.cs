using System;
using UnityEngine;

namespace Commands
{

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public readonly string CommandName;
        public readonly string CommandDescription;

        public CommandAttribute(string commandName, string commandDesciption)
        {
            CommandName = commandName;
            CommandDescription = commandDesciption;
        }

    }
}
