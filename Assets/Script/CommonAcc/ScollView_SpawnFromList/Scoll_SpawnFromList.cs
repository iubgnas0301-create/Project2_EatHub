using NUnit.Framework;
using System;
using UnityEngine;
using static UnityEditor.Progress;

public class Scoll_SpawnFromList : MonoBehaviour
{

    [SerializeField] private Transform _ItemTemplate;
    [SerializeField] private Transform _Holder;

    private void Start() {
        _ItemTemplate.gameObject.SetActive(false);
    }

    public void SpawnFromList<T>(T[] _listInfo) {
        //check
        I_Item_SpawnFromList template;
        if (!_ItemTemplate.TryGetComponent(out template)) return;
        if (template.GetDataType() is not T) {
            Debug.Log("Input and Template is not same data type");
            return;
        }
        //spawn
        foreach (var item in _listInfo) {
            Transform newItem = Instantiate(_ItemTemplate, _Holder);
            newItem.gameObject.SetActive(true);
            I_Item_SpawnFromList _newItem = newItem.GetComponent<I_Item_SpawnFromList>();
            _newItem.SetInfo(item);
        }
    }
    public GameObject SpawnFromInfo<T>(T _info) {
        // check
        I_Item_SpawnFromList template;
        if (!_ItemTemplate.TryGetComponent(out template)) return null;
        if (template.GetDataType() != typeof(T)) {
            Debug.Log($"{template.GetDataType()} != {typeof(T)}");
            Debug.Log("Input and Template is not same data type");
            return null;
        }
        // spawn
        Transform newItem = Instantiate(_ItemTemplate, _Holder);
        newItem.gameObject.SetActive(true);
        I_Item_SpawnFromList _newItem = newItem.GetComponent<I_Item_SpawnFromList>();
        _newItem.SetInfo(_info);
        return newItem.gameObject;
    }
    public void ClearAll() {
        foreach (Transform item in _Holder) {
            if (item == _ItemTemplate) continue;
            Destroy(item.gameObject);
        }
    }

}
