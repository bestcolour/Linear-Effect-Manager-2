using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinearCommands
{
    // public enum CommandType { Instant, OverTime };

    //The base class for which code get called on and execute a certain effect.
    [System.Serializable]
    public abstract class CommandInfo
    {
        // [Header("Command Settings")]
        // [Tooltip("When set to true, the command will immediately go to the next command after starting the current one.")]
        // public bool NextImmediately = false;


        //Return true when the command is done. This is needed for commands which take more than 
        //frame call to execute completely
        public abstract bool ExecuteCommand();

    }

}