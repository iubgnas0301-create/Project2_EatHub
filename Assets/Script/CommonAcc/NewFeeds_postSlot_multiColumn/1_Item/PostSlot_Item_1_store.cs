using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_Item_1_store : PostSlot_Item_0_Base {

    [Header("Output")]
    [SerializeField] protected TextMeshProUGUI rate;
    [SerializeField] protected TextMeshProUGUI feedbackCount;
    [SerializeField] protected TextMeshProUGUI loction;
    [SerializeField] Image _image;
    [SerializeField] private Image _avata;
    [SerializeField] private TextMeshProUGUI store_name;

    [Header("Fillter")]
    [SerializeField] private TextMeshProUGUI searchInput;
    [SerializeField] private TMP_Dropdown dropdown;

    private E_PostSlot_store _info;

    public override void SetInfo(E_PostSlot_0_Base newInfo) {
        _info = newInfo as E_PostSlot_store;
        // Validate Information
        if (_info is null) { Debug.Log($"Can't read {newInfo} info as store"); return; }

        // Set Information
        Title.text = _info.name;
        Content.text = _info.description;
        rate.text = _info.rate.ToString("0.0");
        feedbackCount.text = $"(000 feedbacks)";
        loction.text = _info.address;
        store_name.text = _info.name;

        // Load Image
        WorkWithServer.Instance.DownLoadImage(_info.image_path, (Sprite output) => {
            Debug.Log($"Store Image downloaded from path: {_info.image_path}");
            _info.Image = output;
            _image.color = Color.white;
            _image.sprite = output;
        });
        WorkWithServer.Instance.DownLoadImage(_info.brand_avata, (Sprite output) => {
            Debug.Log($"Store Brand Avata downloaded from path: {_info.brand_avata}");
            _info.Avata = output;
            _avata.color = Color.white;
            _avata.sprite = output;
        });
    }

    public override void Call2Server(int curentPage, int itemPerPage, Action<E_PostSlot_0_Base> callbackCreateItem, Action callbackEnd) {
        Debug.Log($"{gameObject.name} Call Store Info from Server"); 
        callbackEnd += this.callbackEnd;
        Debug.Log("callbackEnd have " + callbackEnd.GetInvocationList().Length);
        dropdown.interactable = false;

        WorkWithServer.Instance.GetStoreInfo(
            curentPage, itemPerPage,
            callbackCreateItem, callbackEnd,
            searchInput.text.Trim('\u200b'), (WorkWithServer.SearchFillter)dropdown.value
            );
    }

    public void callbackEnd() {
        dropdown.interactable = true;
    }
}
