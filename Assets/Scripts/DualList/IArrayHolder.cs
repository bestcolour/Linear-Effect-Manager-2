namespace DualList
{
#if UNITY_EDITOR
    public delegate void ChangeObjectArrayCallBack(int objectIndex);
#endif

    public interface IArrayHolder
    {
#if UNITY_EDITOR
        event ChangeObjectArrayCallBack OnRemoveObject;
        event ChangeObjectArrayCallBack OnInsertObject;

        int AddNewObject(bool isInsert);
        void RemoveObjectAt(int index);
#endif
    }

}