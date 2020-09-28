using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinearCommands
{
    //Stores the list of command info which the user will add to when editting the block
    [System.Serializable]
    [RequireComponent(typeof(Block))]
    public abstract class CommandExecutor<T> : MonoBehaviour where T : CommandInfo
    {
        [SerializeField]
        List<T> _infoList = default;

        #region Runtime Methods

        //Executes the command using the info at the inputted index
        public bool ExecuteCommand(int infoIndex)
        {
            T commandInfo = _infoList[infoIndex];
            return commandInfo.UpdateCommand();
        }






        #endregion




#if UNITY_EDITOR
        #region Editor Methods
        public int AddInfo(T info)
        {
            _infoList.Add(info);
            return (_infoList.Count - 1);
        }

        public void RemoveInfoAt(int index)
        {
            int lastIndex = _infoList.Count - 1;
            T lastInfo = _infoList[lastIndex];
            _infoList[index] = lastInfo;
            _infoList.RemoveAt(lastIndex);
        }

        public void DeleteCommandExecutor()
        {

        }



        #endregion


#endif

    }


}