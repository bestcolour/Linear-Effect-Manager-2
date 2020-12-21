namespace LinearEffectsEditor
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using LinearEffects;

    public static class BlockCallerData
    {
        //A block caller is basically a proxy monobehaviour script which calls a block during the gameplay.
        //For example, a monobehaviour script which calls a block to execute its line of effects on Awake is a simple BlockCaller

        public static bool TryGetCaller(string blockCallerName, out Type typeToAdd)
        {
            if (!CallerName_To_CallerType.TryGetValue(blockCallerName, out Type value))
            {
                typeToAdd = null;
                Debug.LogError($"Executor Label Name of {blockCallerName} is not found! Please check if you are sending the correct label name");
                return false;
            }

            typeToAdd = value;
            return true;
        }

        public static string[] GetCallerStrings()
        {
            return CallerName_To_CallerType.Keys.ToArray();
        }


        ///<Summary>A dictionary which stores all the block callers. You can add custom callers here.  Do note that it is unwise to rename any of the Keys or the Values of the Dictionary after using the Caller. </Summary>
        static readonly Dictionary<string, Type> CallerName_To_CallerType = new Dictionary<string, Type>()
        {

            //=================== EXAMPLE ===================
            //{"Example/DebuggerExecutor", typeof(<INSERT EXECUTOR NAME HERE>)},
            //    ^            ^                                ^
            //    A            B                                C
            //
            //A: How you want to catergorise your executors, use backslash to create a folder
            //B: What you want your Executor to be named when it shows up in the search box
            //C: Your Executor type, reminder to not rename this once you started using it in Blocks



            //============ ADD YOUR CUSTOM EXECUTORS HERE =============

        };


    }

}