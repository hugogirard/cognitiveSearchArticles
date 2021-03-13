using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorServer.Service.State
{
    public class StateService : IStateService
    {
        private Dictionary<string, string> _state;

        public StateService()
        {
            _state = new Dictionary<string, string>();
        }

        public void SaveState<T>(string key, T value)
        {
            var serializedObject = JsonSerializer.Serialize(value);
            _state.Add(key, serializedObject);
        }

        public T GetState<T>(string key)
        {
            if (_state.ContainsKey(key))
            {
                var serializedObject = _state[key];

                return JsonSerializer.Deserialize<T>(serializedObject);
            }

            return default(T);
        }
    }
}
