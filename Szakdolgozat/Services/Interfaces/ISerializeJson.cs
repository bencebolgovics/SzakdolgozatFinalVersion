namespace Szakdolgozat.Services.Interfaces
{
    public interface ISerializeJson
    {
        static string Target { get; set; }

        void Serialize<T>(T obj);
        T[] Deserialize<T>();
    }
}
