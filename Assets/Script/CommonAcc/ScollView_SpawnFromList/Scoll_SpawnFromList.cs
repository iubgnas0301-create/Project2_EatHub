using NUnit.Framework;
using System;
using UnityEngine;
using static UnityEditor.Progress;

public class Scoll_SpawnFromList : MonoBehaviour
{
    public event Action<GameObject> OnItemValueChanged;

    [SerializeField] private Transform _ItemTemplate;
    [SerializeField] private Transform _Holder;

    private void Start() {
        _ItemTemplate.gameObject.SetActive(false);
    }

    private void OnDetectedItemValuechange(GameObject item) {
        OnItemValueChanged?.Invoke(item);
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
            _newItem.OnValueChanged += OnDetectedItemValuechange;
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
        _newItem.OnValueChanged += OnDetectedItemValuechange;
        return newItem.gameObject;
    }
    public void ClearAll() {
        foreach (Transform item in _Holder) {
            if (item == _ItemTemplate) continue;
            item.GetComponent<I_Item_SpawnFromList>().OnValueChanged -= OnDetectedItemValuechange;
            Destroy(item.gameObject);
        }
    }

    public Transform GetHolder() {
        return _Holder;
    }
}
