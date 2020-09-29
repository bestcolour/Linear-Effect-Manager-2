using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinearCommands
{
    //The base class for which code get called on and execute a certain effect.
    [System.Serializable]
    public abstract class CommandInfo
    {
        //Returns true when execute command is completed
        public abstract bool UpdateCommand();

        public abstract void ResetCommand();

        //Return true when the command is done. This is needed for commands which take more than 
        //frame call to execute completely
        public abstract bool IsInstantCommand { get; }

    }

}