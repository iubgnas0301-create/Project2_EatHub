using UnityEngine;
using System;

public class PostSlot_LoadAction_0_Base : MonoBehaviour
{
    [SerializeField] protected GameObject tempplate;
    private PostSlot_LoadCondition loadCondition;

    private void Start() {
        tempplate = transform.GetChild(0).gameObject;
        tempplate.SetActive(false);

        loadCondition = GetComponent<PostSlot_LoadCondition>();
    }
    public virtual void CreateItem(E_PostSlot_0_Base info) {
        Transform newItem = Instantiate(tempplate.transform, transform);
        newItem.gameObject.SetActive(true);
        newItem.name = info.title;
        newItem.GetComponent<PostSlot_Item_0_Base>()?.SetInfo(info);

    }

    public virtual void ClearAllItem() {
        foreach (Transform item in transform) {
            if (item.gameObject == tempplate) continue;
            Destroy(item.gameObject);
        }
    }
}
