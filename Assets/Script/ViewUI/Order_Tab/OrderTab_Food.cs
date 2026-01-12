using UnityEngine;

public class OrderTab_Food : MonoBehaviour
{
    [SerializeField] private Scoll_SpawnFromList _Food_InProcess;
    [SerializeField] private Scoll_SpawnFromList _Food_Cancelled;
    [SerializeField] private Scoll_SpawnFromList _Food_Completed;
    private bool isFirstRun = true;
    
    private void OnEnable() {
        if (isFirstRun) {
            StartCoroutine(FirstRun());
            return;
        }
        LoadFromServer();
    }
    private void Start() {
        _Food_InProcess.OnItemValueChanged += OnItemValueChanged;
    }
    private System.Collections.IEnumerator FirstRun() {
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
                CancelFoodOrder();
            }
        };
        void CancelFoodOrder() {
            ShowFood_item_SpawnFromList appointItem = appointGO.GetComponent<ShowFood_item_SpawnFromList>();
            WorkWithServer.Instance.CancelFoodOrderTakeAway(appointItem.GetFoodOrdertID(), success, fail);
            void success() {
                Notifi_Action.instance.Notifi_Act("Food order cancelled successfully");
                LoadFromServer();
            }
            void fail() {
                Notifi_Action.instance.Notifi_Act("Cancel food order failed");
            }
        }
    }
    private void LoadFromServer() {
        RefreshShown();
        WorkWithServer.Instance.GetFoodOrderTakeAway(Static_Info.UserInfo.id, SetAppointData, null);
    }
    private void SetAppointData (E_Show_FoodOrder data) {
        switch (data.state) {
            case -1:
                _Food_Cancelled.SpawnFromInfo(data);
                break;
            case 5:
                _Food_Completed.SpawnFromInfo(data);
                break;
            default:
                _Food_InProcess.SpawnFromInfo(data);
                break;
        }
    }
    private void RefreshShown() {
        _Food_Cancelled.ClearAll();
        _Food_Completed.ClearAll();
        _Food_InProcess.ClearAll();
    }
}
