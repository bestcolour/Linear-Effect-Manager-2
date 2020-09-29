using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinearCommands
{
    //This is the command class to inherit from if you wish to create 
    //a command that requires more than 1 frame to execute completely.
    public abstract class UpdateCommandInfo : CommandInfo
    {
        [Header("Update Settings")]
        [Tooltip("When set to false, the command will immediately go to the next command after starting this one.")]
        public bool UntilCompletion = true;

        public override bool IsInstantCommand => false;


    }
}

