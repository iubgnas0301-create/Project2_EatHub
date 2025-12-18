using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class PostSlot_Item_0_Base : MonoBehaviour {
    [SerializeField] protected TextMeshProUGUI Title;
    [SerializeField] protected TextMeshProUGUI Content;

    public abstract void SetInfo(E_PostSlot_0_Base newInfo);
    public abstract void Call2Server(
        int curentPage, int itemPerPage,
        Action<E_PostSlot_0_Base> callbackCreateItem,
        Action callbackEnd);
}
