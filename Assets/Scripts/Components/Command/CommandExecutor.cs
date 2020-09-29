using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinearCommands
{
    //Stores the list of command info which the user will add to when editting the block
    [System.Serializable]
    [RequireComponent(typeof(Block))]
    public abstract class CommandExecutor : MonoBehaviour
    {
        [SerializeField]
        List<CommandInfo> _infoList = default;

        #region Runtime Methods

        //Executes the command using the info at the inputted index
        public bool ExecuteCommand(int infoIndex)
        {
            CommandInfo commandInfo = _infoList[infoIndex];
            return commandInfo.UpdateCommand();
        }


        #endregion






    }


}