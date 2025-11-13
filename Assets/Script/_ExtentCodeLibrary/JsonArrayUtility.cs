using UnityEngine;


/// <summary>
/// Provides utility methods for working with JSON arrays. Code by Copilot
/// </summary>
/// <remarks>This class includes methods to facilitate the deserialization of JSON arrays into strongly-typed
/// arrays.</remarks>
public static class JsonArrayUtility
{
    [System.Serializable]
    private class Wrapper<T> {
        public T[] Items;
    }

    public static T[] FromJsonArray<T>(string json) {
        // Wrap raw array in an object with property name "Items"
        string newJson = "{\"Items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.Items ?? new T[0];
    }
}
