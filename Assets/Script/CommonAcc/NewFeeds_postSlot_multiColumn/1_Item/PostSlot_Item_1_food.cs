using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_Item_1_food : PostSlot_Item_0_Base {
    [Header("Output")]
    [SerializeField] private TextMeshProUGUI rate;
    [SerializeField] private TextMeshProUGUI feedbackCount;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private Image _image;

    [Header("Fillter")]
    [SerializeField] private TextMeshProUGUI searchInput;
    [SerializeField] private TMP_Dropdown dropdown;

    private E_PostSlot_food _info;
    public override void SetInfo(E_PostSlot_0_Base newInfo) {
        _info = newInfo as E_PostSlot_food;

        // Validate Information
        if (_info is null) { Debug.Log($"Can't read {newInfo} info as food"); return; }

        // Set Information
        Title.text = _info.name;
        Content.text = _info.quantity_per_set;
        rate.text = _info.rate.ToString("0.0");
        feedbackCount.text = $"(000 feedbacks)";
        price.text = _info.price.ToString("##,#") + "đ";

        // Load Image
        WorkWithServer.Instance.DownLoadImage(_info.image_path, (Sprite output) => {
            Debug.Log($"Food Image downloaded from path: {_info.image_path}");
            _info.Image = output;
            _image.color = Color.white;
            _image.sprite = output;
        });
    }

    public override void Call2Server(int curentPage, int itemPerPage, Action<E_PostSlot_0_Base> callbackCreateItem, Action callbackEnd)
    {
        Debug.Log($"{gameObject.name} Call Food Info from Server");
        WorkWithServer.Instance.GetFoodInfo(
            curentPage, itemPerPage,
            callbackCreateItem, callbackEnd
            ,searchInput.text.Trim('\u200b'), (WorkWithServer.SearchFillter)dropdown.value
            );
    }
}
