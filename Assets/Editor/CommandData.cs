namespace LinearEffectsEditor
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using LinearEffects;

    //This file stores all the data of all the types of executor as well as their label names
    public class CommandData
    {
        public static bool TryGetExecutor(string executorLabelName, out Type typeToAdd)
        {
            if (!CommandLabel_To_CommandExecutor.TryGetValue(executorLabelName, out Type value))
            {
                typeToAdd = null;
                Debug.LogError($"Executor Label Name of {executorLabelName} is not found! Please check if you are sending the correct label name");
                return false;
            }

            typeToAdd = value;
            return true;
        }

        static readonly Dictionary<string, Type> CommandLabel_To_CommandExecutor = new Dictionary<string, Type>()
        {
            {"DebuggerExecutor", typeof(DebuggerExecutor)},
            {"TestUpdateExecutor", typeof(TestUpdateExecutor)},
        };







    }

}