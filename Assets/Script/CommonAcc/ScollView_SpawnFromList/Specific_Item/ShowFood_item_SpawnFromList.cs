using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowFood_item_SpawnFromList : MonoBehaviour, I_Item_SpawnFromList
{
    public event Action<GameObject> OnValueChanged;
    [SerializeField] private TextMeshProUGUI FoodName;
    [SerializeField] private TextMeshProUGUI BrandName;
    [SerializeField] private TextMeshProUGUI TakeAwayType;
    [SerializeField] private TextMeshProUGUI FoodState;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    [SerializeField] private Button? Btn_Cancel;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

    private E_Show_FoodOrder FoodOrderData;

    private void Start() {
        if (Btn_Cancel! != null) {
            Btn_Cancel.onClick.AddListener(() => OnValueChanged?.Invoke(this.gameObject));
        }
    }

    public Type GetDataType() {
        return typeof(E_Show_FoodOrder);
    }

    public void SetInfo<T>(T info) {
        FoodOrderData = info as E_Show_FoodOrder;
        FoodName.text = FoodOrderData.food_name;
        BrandName.text = FoodOrderData.brand_name;
        TakeAwayType.text = 
            FoodOrderData.is_shipping ? 
            $"Giao hàng ({FoodOrderData.ship_address})" : 
            $"Tự đến lấy ({FoodOrderData.datetime_appoint})";
        FoodState.text = E_Order_TakeAway.GetStateString(FoodOrderData.state);
    }

    public int GetFoodOrdertID() {
        return FoodOrderData.id_order_takeaway;
    }


}
[Serializable]
public class E_Show_FoodOrder {
    public int id_order_takeaway;
    public string brand_name;
    public string food_name;
    public bool is_shipping;
    public string ship_address;
    public string datetime_appoint;
    public int state;
}