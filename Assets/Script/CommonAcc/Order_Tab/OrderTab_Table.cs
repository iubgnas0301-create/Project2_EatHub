using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class OrderTab_Table : MonoBehaviour
{
    [SerializeField] private Scoll_SpawnFromList _Table_InProcess;
    [SerializeField] private Scoll_SpawnFromList _Table_Cancelled;
    [SerializeField] private Scoll_SpawnFromList _Table_Completed;
    private bool isFirstRun = true;

    private void OnEnable() {
        if (isFirstRun) {
            StartCoroutine(FirstRun());
            return;
        }
        LoadFromServer();
    }
    private void Start() {
        _Table_InProcess.OnItemValueChanged += OnItemValueChanged;
    }

    private IEnumerator FirstRun() {
        yield return new WaitForEndOfFrame();
        LoadFromServer();
        isFirstRun = false;
    }

    private void OnItemValueChanged(GameObject appointGO) {
        FloadUI_Control_v2.instance.Open_YesNoFload(
            "Bạn chắc chắn muốn hủy đơn này không?",
            "Hủy đơn",
            "Không"
        ).OnAnswer += (bool answer) => {
            if (answer) {
                CancelAppointment();
            }
        };
        void CancelAppointment() {
            ShowTable_Item_SpawnFromList appointItem = appointGO.GetComponent<ShowTable_Item_SpawnFromList>();
            WorkWithServer.Instance.CancelAppoint(appointItem.GetAppointmentID(), success, fail);
            void success() {
                Notifi_Action.instance.Notifi_Act("Appointment cancelled successfully");
                LoadFromServer();
            }
            void fail() {
                Notifi_Action.instance.Notifi_Act("Cancel appointment failed");
            }
        }
    }

    private void LoadFromServer() {
        RefreshShown();
        WorkWithServer.Instance.GetTableAppoint(Static_Info.UserInfo.id, SetAppointData, null);
    }
    private void SetAppointData (E_Show_TableAppoint data) {
        switch (data.state) {
            case -1:
                _Table_Cancelled.SpawnFromInfo(data);
                break;
            case 2:
                _Table_Completed.SpawnFromInfo(data);
                break;
            default:
                _Table_InProcess.SpawnFromInfo(data);
                break;
        }
    }
    private void RefreshShown() {
        _Table_Cancelled.ClearAll();
        _Table_Completed.ClearAll();
        _Table_InProcess.ClearAll();
    }
}
