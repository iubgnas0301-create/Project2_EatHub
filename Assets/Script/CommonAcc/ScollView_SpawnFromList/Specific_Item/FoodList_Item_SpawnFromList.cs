using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodList_Item_SpawnFromList : MonoBehaviour, I_Item_SpawnFromList {

    [SerializeField] private Toggle selectionToggle;
    [SerializeField] private TextMeshProUGUI foodNameText;
    [SerializeField] private TextMeshProUGUI foodPriceText;
    [SerializeField] private NumberPicker foodQuantityNumPick;
    
    private E_PostSlot_food foodData;

    public event Action<GameObject> OnValueChanged;

    private void Start() {
        selectionToggle.onValueChanged.AddListener(OnSelectionToggleChanged);
        foodQuantityNumPick.OnValueChanged += (arg0) => OnValueChanged?.Invoke(gameObject);
    }

    private void OnSelectionToggleChanged(bool arg0) {
        foodQuantityNumPick.gameObject.SetActive(arg0);
        OnValueChanged?.Invoke(this.gameObject);
    }

    public bool IsSelected() {
        return selectionToggle.isOn;
    }
    public Type GetDataType() {
        return typeof(E_PostSlot_food);
    }

    public void SetInfo<T>(T info) {
        foodData = info as E_PostSlot_food;

        foodNameText.text = foodData.name;
        foodPriceText.text = $"{foodData.price.ToString("N0")} VND";
        selectionToggle.isOn = false;
        foodQuantityNumPick.gameObject.SetActive(false);
    }

    public (E_PostSlot_food data, int quantity) GetValue() {
        int quantity = IsSelected()?  foodQuantityNumPick.GetValue() : 0;
        return (foodData, quantity);
    }
}
