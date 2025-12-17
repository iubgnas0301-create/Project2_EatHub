using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_Item_1_food : PostSlot_Item_0_Base {
    [SerializeField] private TextMeshProUGUI rate;
    [SerializeField] private TextMeshProUGUI feedbackCount;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Image _image;
    public override void SetInfo(E_PostSlot_0_Base newInfo) {
        E_PostSlot_food info = newInfo as E_PostSlot_food;

        // Validate Information
        if (info is null) { Debug.Log($"Can't read {newInfo} info as food"); return; }

        // Set Information
        Title.text = info.name;
        Content.text = info.quantity_per_set;
        rate.text = info.rate.ToString("0.0");
        feedbackCount.text = $"(000 feedbacks)";
        price.text = info.price.ToString("##,#") + "đ";

        // Load Image
        _image.sprite = info.Image;
    }

    public override void Call2Server(int curentPage, int itemPerPage,
        Action<E_PostSlot_0_Base> callbackCreateItem,
        Action callbackEnd) {
        Debug.Log($"{gameObject.name} Call Food Info from Server");
        WorkWithServer.Instance.GetFoodInfo(
            curentPage, itemPerPage,
            callbackCreateItem, callbackEnd);
    }
}
