namespace Jarloo.Sojurn.Data
{
    public interface IPersistenceManager
    {
        void Save<T>(string key, T o);
        T Retrieve<T>(string key) where T : new();
    }
}