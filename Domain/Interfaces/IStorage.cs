namespace DAL.Intefaces
{
    public interface IStorage
	{
        string AddData(byte[] data);
        void UpdateData(string dataID, byte[] newData);
        void DeleteData(string dataID);
        byte[] GetData(string dataID);
    }
}

