using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTable_Item_SpawnFromList : MonoBehaviour, I_Item_SpawnFromList {
    public event Action<GameObject> OnValueChanged;
    [SerializeField] private TextMeshProUGUI BrandName;
    [SerializeField] private TextMeshProUGUI StoreName;
    [SerializeField] private TextMeshProUGUI AppointmentTime;
    [SerializeField] private TextMeshProUGUI Location;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    [SerializeField] private Button? Btn_Cancel;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

    private E_Show_TableAppoint appointmentData;

    private void Start() {
        if (Btn_Cancel !!= null) {
            Btn_Cancel.onClick.AddListener(() => OnValueChanged?.Invoke(this.gameObject));
        }
    }

    public Type GetDataType() {
        return typeof(E_Show_TableAppoint);
    }

    public void SetInfo<T>(T info) {
        appointmentData = info as E_Show_TableAppoint;
        BrandName.text = appointmentData.brand_name;
        StoreName.text = appointmentData.store_name;
        DateTime appoint_time = DateTime.Parse(appointmentData.datetime_appoint);
        DateTime finnish_time = DateTime.Parse(appointmentData.datetime_finnish);
        AppointmentTime.text = appoint_time.ToString("dd/MM/yyyy") + 
            " (" + appoint_time.ToString("HH:mm") + " - " + finnish_time.ToString("HH:mm") + ")";
        Location.text = appointmentData.location;
    }

    public int GetAppointmentID() {
        return appointmentData.id_appointment;
    }


}
[Serializable] public class E_Show_TableAppoint {
    public int id_appointment;
    public string brand_name;
    public string store_name;
    public string datetime_appoint;
    public string datetime_finnish;
    public string location;
    public int state;
}
