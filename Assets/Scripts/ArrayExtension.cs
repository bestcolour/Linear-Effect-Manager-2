#if UNITY_EDITOR
namespace LinearEffects
{
    //This script is just a lazy man's way of resizing an array
    //NOTE: USE THIS METHODS ONLY IN INFREQUENT EDITOR CODE. USE A LIST DURING RUNTIME CODE
    using UnityEngine;
    using System.Collections.Generic;
    public static class ArrayExtension
    {
        ///<Summary>
        ///This method is meant to be used in Editor Code, do not use this during runtime code, use a list instead!
        ///</Summary>
        public static void Add<T>(ref T[] array, T elementToAdd)
        {
            List<T> tempList = new List<T>(array);
            tempList.Add(elementToAdd);
            array = tempList.ToArray();
        }

        public static void Remove<T>(ref T[] array, T elementToRemove)
        {
            List<T> tempList = new List<T>(array);
            tempList.Remove(elementToRemove);
            array = tempList.ToArray();
        }

        public static void RemoveAt<T>(ref T[] array, int index)
        {
            List<T> tempList = new List<T>(array);
            tempList.RemoveAt(index);
            array = tempList.ToArray();
        }


    }

}
#endif