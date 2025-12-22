using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PostSlot_Item_1_event : PostSlot_Item_0_Base {
    [SerializeField] private TextMeshProUGUI rate;
    [SerializeField] private TextMeshProUGUI feedbackCount;
    [SerializeField] private TextMeshProUGUI DatetimepPost;
    [SerializeField] private Image _image;
    [SerializeField] private Image _brandAvata;

    private E_PostSlot_event _info;

    public override void SetInfo(E_PostSlot_0_Base newInfo) {
        _info = newInfo as E_PostSlot_event;
        // Validate Information
        if (_info is null) { Debug.Log($"Can't read {newInfo} info as event"); return; }

        // Set Information
        Title.text = _info.brand_name;
        Content.text = $"{_info.title}\n<color=#FFFFFF88>{_info.description}</color>";
        rate.text = "0.0";
        feedbackCount.text = $"(000 feedbacks)";
        DatetimepPost.text = _info.post_time;

        // Load Image
        WorkWithServer.Instance.DownLoadImage(_info.image_path, (Sprite output) => {
            Debug.Log($"Event Image downloaded from path: {_info.image_path}");
            _info._image = output;
            _image.color = Color.white;
            _image.sprite = output;
        });
        WorkWithServer.Instance.DownLoadImage(_info.brand_avata, (Sprite output) => {
            Debug.Log($"Event Brand Avata downloaded from path: {_info.brand_avata}");
            _brandAvata.color = Color.white;
            _brandAvata.sprite = output;
        });
    }
    public override void Call2Server(int curentPage, int itemPerPage, Action<E_PostSlot_0_Base> callbackCreateItem, Action callbackEnd) {
        Debug.Log($"{gameObject.name} Call Event Info from Server");
        WorkWithServer.Instance.GetEventInfo(
            curentPage, itemPerPage,
            callbackCreateItem, callbackEnd);
    }
}
