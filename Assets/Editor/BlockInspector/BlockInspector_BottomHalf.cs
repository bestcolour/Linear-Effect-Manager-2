namespace LinearEffectsEditor
{
    using UnityEngine;
    using UnityEditor;
    using System;
    using LinearEffects;
    using System.Collections.Generic;

    //The bottom half class will render the current observed command as well as the command toolbar (add,minus coppy etc)
    public partial class BlockInspector : ImprovedEditor
    {
        GameObject BlockGameObject => _target.BlockGameObject;

        #region ClipBoard Fields
        List<int> _clipBoardIndices = default;
        HashSet<int> _clipBoardUnOrderedIndices = default;

        bool HadPreviouslyCopied => _clipBoardIndices.Count > 0;
        #endregion

        #region Observed Effect Fields
        SerializedProperty _currObservedProperty = default;
        #endregion

        #region SearchBar Fields
        bool _isSearchBoxOpened = false;
        CategorizedSearchBox _searchBox = default;

        #region Constants
        // static readonly Vector2 SEARCHBOX_RECTSIZE = new Vector2(500f, EditorGUIUtility.singleLineHeight);
        const float SEARCHBAR_PADDING_TOP = 10f
        , SEARCHBOX_HEIGHT = 75f
        , SEARCHBAR_PADDING_BOT = 10f
        ;

        #endregion

        #endregion

        #region Constants
        const string DEBUG_EFFECTEXECUTOR = "TestUpdateExecutor";
        const float OBSERVED_EFFECTBG_BORDER = 50f,
        OBSERVED_EFFECT_YOFFSET = 20f
        ;
        #endregion

        #region LifeTime Method
        void BottomHalf_OnEnable()
        {
            _clipBoardIndices = new List<int>();
            _clipBoardUnOrderedIndices = new HashSet<int>();
            _searchBox = new CategorizedSearchBox();
            _isSearchBoxOpened = false;
        }

        void BottomHalf_OnDisable()
        {

        }

        void BottomHalf_OnInspectorGUI()
        {
            BottomHalf_DrawToolBar();
            BottomHalf_SearchBox_OnGUI();
            BottomHalf_OnGUI_ObservedEffect(Screen.width);

        }


        #endregion


        #region ToolBar

        const float BUTTON_SIZE = 35f;

        void BottomHalf_DrawToolBar()
        {
            EditorGUILayout.BeginHorizontal();
            // //============DRAW PARENT BOX=====================
            // EditorGUILayout.LabelField(string.Empty);

            //================DRAW NEXT/PREV COMMAND BUTTONS===============
            if (GUILayout.Button("【↑】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {

            }
            else if (GUILayout.Button("【↓】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {

            }


            //================DRAW SPACE===============
            EditorGUILayout.Space();

            BottomHalf_DrawSearchBoxButtons();

            //================ DRAW COPY BUTTON ===============
            if (GUILayout.Button("【❏】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                //Copy will not actually copy selected element. It will only copy elements which are in the range of the firstclickedindex and currentclickedindex
                BottomHalf_CopySelectedToClipBoard();
            }
            //================ DRAW PASTE BUTTON =========================
            else if (GUILayout.Button("【≚】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                BottomHalf_PasteClipBoardEffects();
            }
            //=================== DRAW DELETE BUTTON ===================
            else if (GUILayout.Button("【╳】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
            {
                BottomHalf_DeleteAllSelectedEffects();
            }


            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region Observed Effect

        void BottomHalf_OnGUI_ObservedEffect(float inspectorWidth)
        {
            if (_currObservedProperty != null && _prevClickedIndex == CurrentClickedListIndex)
            {
                //Current effect is still the same
                BottomHalf_DrawObservedEffect(inspectorWidth);
                return;
            }

            if (!BottomHalf_TryGetNewObservedEffect())
            {
                return;
            }


            BottomHalf_DrawObservedEffect(inspectorWidth);
        }

        void BottomHalf_DrawObservedEffect(float inspectorWidth)
        {
            //======== DRAWING EFFECT ==========
            float height = EditorGUI.GetPropertyHeight(_currObservedProperty);

            // ========== DRAWING BG BOX =============
            // Color prevColor = GUIExtensions.Start_GUI_ColourChange(OBSERVED_EFFECT_BOXCOLOUR);
            GUILayout.Box(string.Empty, GUILayout.Height(height + OBSERVED_EFFECTBG_BORDER), GUILayout.MaxWidth(inspectorWidth));
            // GUIExtensions.End_GUI_ColourChange(prevColor);

            //========== DRAWING EFFECT =============
            Rect prevRect = GUILayoutUtility.GetLastRect();
            prevRect.y += OBSERVED_EFFECT_YOFFSET;
            EditorGUI.PropertyField(prevRect, _currObservedProperty, true);

            //========= SAVE EFFECT'S CHANGES ===========
            if (_currObservedProperty.serializedObject.ApplyModifiedProperties())
            {
                _currObservedProperty.serializedObject.Update();
            }
        }

        bool BottomHalf_TryGetNewObservedEffect()
        {
            //Get currently selected order element
            if (!TopHalf_GetOrderArrayElement(CurrentClickedListIndex, out SerializedProperty orderElement))
            {
                return false;
            }

            //===== GETTING OBSERVED EFFECT =====
            //convert holder to serializedobject
            BaseEffectExecutor holder = (BaseEffectExecutor)orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_REFHOLDER).objectReferenceValue;
            SerializedObject holderObject = new SerializedObject(holder);

            //Get the effectDatas array as serializedProperty
            SerializedProperty effectDataArray = holderObject.FindProperty(BaseEffectExecutor.PROPERTYNAME_EFFECTDATAS);

            //Get dataelementindex from orderElement in block
            int dataElementIndex = orderElement.FindPropertyRelative(Block.EffectOrder.PROPERTYNAME_DATAELEMENTINDEX).intValue;

            //Somehow the effect order instance still exists when i delete them so i can still apparently get the dataelementindex but the effectDataArray already has deleted all the array elements and hence causes an error when i try to GetArrayElementAtIndex()
            if (dataElementIndex >= effectDataArray.arraySize)
                return false;


            //Get current selected effect through the use of the EffectData array and dataelementindex
            _currObservedProperty = effectDataArray.GetArrayElementAtIndex(dataElementIndex);
            return true;
        }

        #endregion

        #region Commands

        #region SearchBar Methods
        void BottomHalf_DrawSearchBoxButtons()
        {
            //================ DRAW SEARCHBOX BUTTON ===============
            switch (_isSearchBoxOpened)
            {
                case true:
                    if (GUILayout.Button("【―】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
                    {
                        BottomHalf_SearchBox_Disable();
                    }
                    break;
                case false:
                    if (GUILayout.Button("【＋】", GUILayout.Height(BUTTON_SIZE), GUILayout.Width(BUTTON_SIZE)))
                    {
                        BottomHalf_SearchBox_Enable();
                    }

                    break;
            }
        }

        void BottomHalf_SearchBox_Enable()
        {
            // if (!CommandData.TryGetExecutor(DEBUG_EFFECTEXECUTOR, out Type type))
            // {
            //     return;
            // }
            // _target.Block.AddNewOrderElement(BlockGameObject, type, DEBUG_EFFECTEXECUTOR);
            // _target.SaveModifiedProperties();

            _isSearchBoxOpened = true;
            _searchBox.EnableSearchBox();

        }

        void BottomHalf_SearchBox_Disable()
        {
            _isSearchBoxOpened = false;
            _searchBox.DisableSearchBox();
        }

        void BottomHalf_SearchBox_OnGUI()
        {
            if (!_isSearchBoxOpened) return;

            float height = _searchBox.Handle_OnGUI(BottomHalf_SearchBox_GetSearchBarRect(), SEARCHBOX_HEIGHT);

            //This ensures that the search box will always have enough space to be rendered (and if it cant fit in the the window then it will be considered as part of the scroll height)
            EditorGUILayout.LabelField(string.Empty, GUILayout.MinHeight(height + SEARCHBAR_PADDING_BOT));
        }

        Rect BottomHalf_SearchBox_GetSearchBarRect()
        {
            Rect rect = GUILayoutUtility.GetLastRect();
            rect.y += rect.height + SEARCHBAR_PADDING_TOP;
            rect.height = EditorGUIUtility.singleLineHeight;
            return rect;
        }

        #endregion


        void BottomHalf_PasteClipBoardEffects()
        {
            if (!HadPreviouslyCopied) return;
            //Check if there is nothing selected
            int currentInsertPosition = CurrentClickedListIndex == -1 ? _list.count : CurrentClickedListIndex;

            foreach (var elementIndexWhichYouIntendToCopy in _clipBoardIndices)
            {
                if (!BottomHalf_GetCopyOfOrderObjectFromArray(elementIndexWhichYouIntendToCopy, out var effectOrder))
                {
                    Debug.Log($"Unable to copy index {elementIndexWhichYouIntendToCopy} because index is out of bounds!");
                    continue;
                }

                if (!CommandData.TryGetExecutor(effectOrder.EffectName, out Type executorType))
                {
                    Debug.Log($"The Executor {effectOrder.EffectName} doesnt exist in CommandData.cs!");
                    continue;
                }

                //Add the effectorder into the currently selected index (if there isnt any selected index on the list, add to the end)
                _target.Block.InsertOrderElement(_target.BlockGameObject, executorType, effectOrder, currentInsertPosition);
                currentInsertPosition++;
            }

            Debug.Log($"Copied the current {_clipBoardIndices[0]}th element to the {_clipBoardIndices[_clipBoardIndices.Count - 1]}th element.");

            _target.SaveModifiedProperties();
            _clipBoardIndices.Clear();
            _clipBoardUnOrderedIndices.Clear();
        }

        void BottomHalf_DeleteAllSelectedEffects()
        {
            if (!TopHalf_GetSelectedForLoopValues(out int diff, out int direction, out int firstClickedIndex))
            {
                return;
            }

            //Get the bigger starting index
            int startingIndex = direction > 0 ? CurrentClickedListIndex : _firstClickedIndex;

            //Remove elements from the biggest index to the lowest index
            for (int i = 0; i <= diff; i++)
            {
                int index = startingIndex - i;
                _target.Block.RemoveOrderElementAt(index);
            }

            _selectedElements.Clear();
            TopHalf_ResetFirstClickedIndex();
            _target.SaveModifiedProperties();
        }

        void BottomHalf_CopySelectedToClipBoard()
        {
            //as hashset does not garuntee order, i will be using index from and to ensure the copied elements are in the correct order
            if (!TopHalf_GetSelectedForLoopValues(out int diff, out int direction, out _firstClickedIndex))
            {
                return;
            }

            // _clipBoard.Clear();
            _clipBoardIndices.Clear();
            _clipBoardUnOrderedIndices.Clear();

            //Always ensure that the order of the elements copied starts from the smallest index to the largest index
            int startingIndex = direction > 0 ? _firstClickedIndex : CurrentClickedListIndex;
            for (int i = 0; i <= diff; i++)
            {
                int index = startingIndex + i;
                _clipBoardIndices.Add(index);
                _clipBoardUnOrderedIndices.Add(index);
            }
        }

        bool BottomHalf_GetCopyOfOrderObjectFromArray(int index, out Block.EffectOrder orderData)
        {
            if (!TopHalf_GetOrderArrayElement(index, out SerializedProperty p))
            {
                orderData = null;
                return false;
            }

            orderData = new Block.EffectOrder();
            orderData.LoadFromSerializedProperty(p);
            return true;
        }
        #endregion

    }

}