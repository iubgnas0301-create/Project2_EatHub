using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TableState_Item_SpawnFromList : MonoBehaviour, I_Item_SpawnFromList {

    [SerializeField] private Transform _busyTemplate;
    private List<(DateTime start, DateTime end)> list_busy = new List<(DateTime start, DateTime end)>();

    private void Start() {
        _busyTemplate.gameObject.SetActive(false);
    }
    public Type GetDataType() {
        return typeof(E_TableSlot_n_DateAppoint);
    }

    public void SetInfo<T>(T info) {
        list_busy.Clear();
        E_TableSlot_n_DateAppoint table = info as E_TableSlot_n_DateAppoint;
        WorkWithServer.Instance.Get_SlotAppointFromDatetime(table.id_brand, table.id_store, table.id_slot, 
            table.date_appoint.Fomat_SlashDate_2_Datetime().ToString("yyyy-MM-dd"), 
            callback, null);
    }

    void callback(E_Table_Slot_Appointment slotAppoint) {
        ToShowBusyState(
            slotAppoint.datetime_appoint.Fomat_string2datetime(),
            slotAppoint.datetime_finnish.Fomat_string2datetime()
        );
    }

    void ToShowBusyState(DateTime start, DateTime end) {
        list_busy.Add((start, end));
        Transform busy = Instantiate(_busyTemplate, this.transform);
        busy.gameObject.SetActive(true);
        RectTransform rect = busy.GetComponent<RectTransform>();
        //Debug.Log($"Start: {start.Hour}, End: {end.Minute}");
        float startPoint = 50 * (start.Hour + (start.Minute/60.0f)); // 50 pixels per hour
        float durationHours = (float)(end - start).TotalHours;
        rect.anchoredPosition = new Vector2(startPoint, 0);
        rect.sizeDelta = new Vector2(50 * durationHours, rect.sizeDelta.y);
    }

    public List<(DateTime start, DateTime end)> GetBusyList() {
        return list_busy;
    }
}
public class E_TableSlot_n_DateAppoint {
    public int id_brand;
    public int id_store;
    public int id_slot;
    public string date_appoint;
}
