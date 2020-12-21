#if UNITY_EDITOR
namespace LinearEffects
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public partial class FlowChart : MonoBehaviour
    {
        public const string PROPERTYNAME_BLOCKARRAY = "_blocks";

        public Block Editor_GetBlock(string label)
        {
            int index = _blocks.FindIndex(x => x.BlockName == label);
            if (index == -1) return null;
            return _blocks[index];
        }
    }

}
#endif