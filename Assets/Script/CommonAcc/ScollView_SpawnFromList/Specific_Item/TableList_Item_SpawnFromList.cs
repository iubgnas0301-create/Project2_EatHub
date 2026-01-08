using System;
using TMPro;
using UnityEngine;

public class TableList_Item_SpawnFromList : MonoBehaviour, I_Item_SpawnFromList {

    [SerializeField] private TextMeshProUGUI output;
    private GameObject _appointState;

    private void OnEnable() {
        _appointState?.SetActive(true);
    }
    private void OnDisable() {
        _appointState?.SetActive(false);
    }

    public Type GetDataType() {
        return typeof(E_Table_Slot);
    }

    public void SetInfo<T>(T info) {
        E_Table_Slot table = info as E_Table_Slot;
        output.text = table.name;
    }

    public void SetGameObjectFollow(GameObject follow) {
        _appointState = follow;
    }
}
