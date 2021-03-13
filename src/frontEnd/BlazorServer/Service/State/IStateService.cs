namespace BlazorServer.Service.State
{
    public interface IStateService
    {
        T GetState<T>(string key);
        void SaveState<T>(string key, T value);
    }
}