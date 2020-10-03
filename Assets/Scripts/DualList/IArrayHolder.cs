namespace DualList
{
    public delegate void ChangeObjectArrayCallBack(int objectIndex);

    public interface IArrayHolder
    {
        event ChangeObjectArrayCallBack OnRemoveObject;
        event ChangeObjectArrayCallBack OnInsertObject;

        int AddNewObject(bool isInsert);
        void RemoveObjectAt(int index);
    }

}