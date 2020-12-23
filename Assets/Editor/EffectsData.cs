namespace LinearEffectsEditor
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using LinearEffects;
    using LinearEffects.DefaultEffects;

    //This file stores all the data of all the types of executor as well as their label names
    public static class EffectsData
    {
        public static bool TryGetExecutor(string fullExecutorName, out Type typeToAdd)
        {
            if (!ExecutorLabel_To_EffectExecutor.TryGetValue(fullExecutorName, out Type value))
            {
                typeToAdd = null;
                Debug.LogError($"Executor Label Name of {fullExecutorName} is not found! Please check if you are sending the correct label name");
                return false;
            }

            if (!value.IsSubclassOf(typeof(BaseEffectExecutor)))
            {
                typeToAdd = null;
                Debug.LogError($"{value.Name} with the Key value of {fullExecutorName} inside of the ExecutorLabel_To_EffectExecutor dictionary does not inherits from {nameof(BaseEffectExecutor)}!");
                return false;
            }

            typeToAdd = value;
            return true;
        }

        public static string[] GetEffectStrings()
        {
            return ExecutorLabel_To_EffectExecutor.Keys.ToArray();
        }



        //For Users:
        //Dictionary key: The path of the effect executor to be shown in the SearchBox. There are two parts to the key: FullExecutorName and ExecutorName.
        //The FullExecutorName is the entire string path with all the slashes. The anything inbetween the start of the string path to the start of the ExecutorName can be changed freely
        //The ExecutorName is whatever you call the Executor at the end of the last slash. The ExecutorName can be named differently from the Executor Type's name (which is the Dictionary's Value) but should not be renamed after the executor has been used.

        //Dictionary Value: The System type of your own custom effect executor.

        // For an example on how to add your own custom effect executor, look in the Dictionary constructor down below

        #region Editor Note
        //For Editor:
        //Renaming Keys' FullEffectName pathing and folders: As of now, there doesnt seem to be any consequences but it is still better to be safe than sorry. 
        //Renaming Keys' EffectName: (This will cause removing effects from blocks to no longer update other Block's DataElmt if the removed effect's index is smaller than the currently compared DataElmtIndex) 
        //Renaming Values: (This will cause current Blocks which are using the Executor to lose their references and hence you will need to create them) 
        #endregion

        ///<Summary>A dictionary which stores all the effects. You can add custom effect executors here.  Do note that it is unwise to rename any of the Keys or the Values of the Dictionary after using the Executor. </Summary>
        static readonly Dictionary<string, Type> ExecutorLabel_To_EffectExecutor = new Dictionary<string, Type>()
        {
            {"General/Timer", typeof(TimerExecutor)},
            {"General/PlayBlock", typeof(PlayBlockExecutor)},
            {"Visual/Graphic/LerpColour", typeof(LerpGraphicColourExecutor)},
            {"Visual/Graphic/LerpAlpha", typeof(LerpGraphicAlphaExecutor)},
            {"Transform/LerpTransform", typeof(LerpTransformExecutor)},
            {"Transform/LerpRectTransform", typeof(LerpRectTransformExecutor)},

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