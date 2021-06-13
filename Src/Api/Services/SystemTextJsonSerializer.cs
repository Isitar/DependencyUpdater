namespace Isitar.DependencyUpdater.Api.Services
{
    using System;
    using System.Text.Json;

    public class SystemTextJsonSerializer : IJsonSerializer
    {
        public dynamic Deserialize(string data) => JsonSerializer.Deserialize(data, typeof(object));

        public T Deserialize<T>(string data) => JsonSerializer.Deserialize<T>(data);

        public object Deserialize(string data, Type t) => JsonSerializer.Deserialize(data, t);

        public string Serialize(dynamic data) => JsonSerializer.Serialize(data);
    }
}