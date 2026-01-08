using System;
using UnityEditor.Analytics;
using UnityEngine;

public interface I_Item_SpawnFromList
{
    public void SetInfo<T>(T info);
    public Type GetDataType();
}
