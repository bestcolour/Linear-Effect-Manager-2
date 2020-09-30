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
        ///This method is meant to be used in Editor Code, do not use this during runtime code, use a list instead! Returns the index in which the new element has been added to.
        ///</Summary>
        public static void Add<T>(ref T[] array, T elementToAdd)
        {
            List<T> tempList = new List<T>(array);
            tempList.Add(elementToAdd);
            array = tempList.ToArray();
        }

         ///<Summary>
        ///This method is meant to be used in Editor Code, do not use this during runtime code, use a list instead! Returns the index in which the new element has been added to.
        ///</Summary>
        public static int AddReturn<T>(ref T[] array, T elementToAdd)
        {
            List<T> tempList = new List<T>(array);
            tempList.Add(elementToAdd);
            array = tempList.ToArray();
            return array.Length - 1;
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

        public static T Find<T>(this T[] array, System.Predicate<T> match)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (match.Invoke(array[i]))
                {
                    return array[i];
                }
            }
            return default;
        }

        public static int FindIndex<T>(this T[] array, System.Predicate<T> match)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (match.Invoke(array[i]))
                {
                    return i;
                }
            }
            return -1;
        }

    }

}
#endif