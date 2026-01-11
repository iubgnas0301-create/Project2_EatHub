using System;
using UnityEditor.Analytics;
using UnityEngine;

public interface I_Item_SpawnFromList
{
#pragma warning disable 0414 // Disable "assigned but not used" warning
    public event Action<GameObject> OnValueChanged;
#pragma warning restore 0414 // Restore "assigned but not used" warning
    public void SetInfo<T>(T info);
    public Type GetDataType();
}
