namespace CardMatching.Core.Interfaces
{
    public interface ISaveRepository
    {
        void Save(string key, object data);
        bool LoadInto(string key, object target);   // To overwrite existing objects like ScriptableObjects
        void Delete(string key);
        bool Exists(string key);
    }
}