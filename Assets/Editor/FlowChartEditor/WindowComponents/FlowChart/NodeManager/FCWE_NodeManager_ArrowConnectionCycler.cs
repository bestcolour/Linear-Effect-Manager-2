namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using LinearEffects;
    using System;
    using System.Collections.Generic;
    //This handles the adding & destroying of arrow connections
    public partial class FlowChartWindowEditor : EditorWindow
    {
        #region Static Methods
        ///<Summary>Checks if a blocknode is connected to the currently selected blocknode</Summary>
        public static bool NodeManager_ArrowConnectionCycler_IsConnectedFromSelectedBlockNode(string blockName)
        {
            return instance.selectedBlock.CheckConnectionTowards(blockName);
        }

        ///<Summary>Is used by the BlockNode's constructor to create a new arrow connection line if there is a connection</Summary>
        public static void NodeManager_ArrowConnectionCycler_CreateNewArrowConnectionLine(BlockNode blockToConnectFrom)
        {
            //This will only occur when there is only one selected block
            BlockNode endNode = instance._allBlockNodesDictionary[blockToConnectFrom.ConnectedTowardsBlockName];
            ArrowConnectionLine arrowConnectionLine = new ArrowConnectionLine(blockToConnectFrom, endNode, instance.NodeManager_ArrowConnectionCycler_DeleteArrowConnectionLine);
            //Add a new arrow connection line to the list
            instance._arrowConnectionLines.Add(arrowConnectionLine);
        }

        #endregion


        ///<Summary>Is used by the BlockNode's OnConnect button to create a new connectionline using the endNode</Summary>
        void NodeManager_ArrowConnectionCycler_ConnectToBlockNode(BlockNode endNode)
        {
            //This will only occur when there is only one selected block
            selectedBlock.ConnectedTowardsBlockName = endNode.Label;
            ArrowConnectionLine arrowConnectionLine = new ArrowConnectionLine(selectedBlock, endNode, NodeManager_ArrowConnectionCycler_DeleteArrowConnectionLine);
            //Add a new arrow connection line to the list
            _arrowConnectionLines.Add(arrowConnectionLine);
        }


        //Two ways where arrows need to be deleted:
        //1) A node is deleted
        //2) The user chose to click on a button on the arrrow to delete it
        ///<Summary>Deletes an arrow connection line with the following start and end node labels</Summary>
        void NodeManager_ArrowConnectionCycler_DeleteArrowConnectionLine(string startNodeLabe, string endNodeLabel)
        {
            int index = _arrowConnectionLines.FindIndex(x => (x.StartNode.Label == startNodeLabe && x.EndNode.Label == endNodeLabel));

            if (index == -1)
            {
                Debug.LogWarning($"Failed to find an arrow connection line with starting node block: {startNodeLabe} and ending node block: {endNodeLabel}");
                return;
            }

            _arrowConnectionLines[index].StartNode.ConnectedTowardsBlockName = string.Empty;
            _arrowConnectionLines.RemoveAt(index);
        }

        ///<Summary>Deletes all of the arrow connections which are connected from a start node</Summary>
        void NodeManager_ArrowConnectionCycler_DeleteAllArrowConnectionLinesFrom(string startNodeLabel)
        {
            //Find all connection lines that relate to this the from or to node
            List<int> results = _arrowConnectionLines.FindAllIndexOf(x => (x.StartNode.Label == startNodeLabel));

            if (results.Count <= 0)
            {
                // Debug.LogWarning($"Failed to find an arrow connection line with starting node block: {from} and ending node block: {to}");
                return;
            }

            foreach (var index in results)
            {
                _arrowConnectionLines[index].StartNode.ConnectedTowardsBlockName = string.Empty;
                _arrowConnectionLines.RemoveAt(index);
            }
        }

        ///<Summary>Deletes all of the arrow connections which are connected to an end node</Summary>
        void NodeManager_ArrowConnectionCycler_DeleteAllArrowConnectionLinesTo(string endNodeLabel)
        {
            //Find all connection lines that relate to this the from or to node
            List<int> results = _arrowConnectionLines.FindAllIndexOf(x => (x.EndNode.Label == endNodeLabel));

            if (results.Count <= 0)
            {
                // Debug.LogWarning($"Failed to find an arrow connection line with starting node block: {from} and ending node block: {to}");
                return;
            }

            foreach (var index in results)
            {
                _arrowConnectionLines[index].StartNode.ConnectedTowardsBlockName = string.Empty;
                _arrowConnectionLines.RemoveAt(index);
            }
        }




    }

}