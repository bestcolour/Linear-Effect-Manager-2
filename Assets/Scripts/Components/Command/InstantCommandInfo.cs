using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinearCommands
{
    //This is the command class to inherit from if you wish to create 
    //a command that requires only 1 frame to execute completely.
    public abstract class InstantCommandInfo : CommandInfo
    {
        public override bool IsInstantCommand => true;

    }
}
