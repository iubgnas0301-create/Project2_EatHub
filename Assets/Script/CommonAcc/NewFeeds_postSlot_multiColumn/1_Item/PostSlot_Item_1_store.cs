using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_Item_1_store : PostSlot_Item_0_Base {

    [SerializeField] protected TextMeshProUGUI rate;
    [SerializeField] protected TextMeshProUGUI feedbackCount;
    [SerializeField] protected TextMeshProUGUI loction;
    [SerializeField] Image _image;
    
    private E_PostSlot_store _info;

    public override void SetInfo(E_PostSlot_0_Base newInfo) {
        _info = newInfo as E_PostSlot_store;
        // Validate Information
        if (_info is null) { Debug.Log($"Can't read {newInfo} info as store"); return; }

        // Set Information
        Title.text = _info.name;
        Content.text = _info.description;
        rate.text = _info.rate;
        feedbackCount.text = $"(000 feedbacks)";
        loction.text = _info.address;

        // Load Image
        WorkWithServer.Instance.DownLoadImage(_info.image_path, (Sprite output) => {
            Debug.Log($"Store Image downloaded from path: {_info.image_path}");
            _info.Image = output;
            _image.color = Color.white;
            _image.sprite = output;
        });
    }

    public override void Call2Server(int curentPage, int itemPerPage, Action<E_PostSlot_0_Base> callbackCreateItem, Action callbackEnd) {
        Debug.Log($"{gameObject.name} Call Store Info from Server");
        WorkWithServer.Instance.GetStoreInfo(
            curentPage, itemPerPage,
            callbackCreateItem, callbackEnd);
    }
}
