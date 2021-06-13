namespace Isitar.DependencyUpdater.Api.Services
{
    using System;

    public interface IJsonSerializer
    {
        public dynamic Deserialize(string data);
        public T Deserialize<T>(string data);
        public object Deserialize(string data, Type t);
        public string Serialize(dynamic data);
    }
}