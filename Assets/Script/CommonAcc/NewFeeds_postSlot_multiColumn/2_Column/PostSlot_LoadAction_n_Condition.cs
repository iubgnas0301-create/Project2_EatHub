using UnityEngine;
using System;

public class PostSlot_LoadAction_n_Condition : MonoBehaviour
{
    #region Actions
    // =============================== declare ===================================
    [SerializeField] protected GameObject tempplate;

    // =============================== Methods ===================================
    private void Start() {
        tempplate = transform.GetChild(0).gameObject;
        tempplate.gameObject.SetActive(false);
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

    public virtual void PrepareForLoad(int pageIndex, int itemPerPage) {
        tempplate.GetComponent<PostSlot_Item_0_Base>().Call2Server(
            pageIndex, itemPerPage,
            CreateItem,
            UpadateMaxScollValue);
    }
    #endregion Actions

    #region Conditions
    //=================================== declare ===================================
    public Action<PostSlot_LoadAction_n_Condition> OnReachBottom;
    private enum State { middle, bottom }

    private State currentState = State.middle;

    private float maxScrollValue = 0;

    //=================================== Methods ===================================
    public void UpadateMaxScollValue() {
        transform.parent.GetComponent<ContentResize>().ResetHeight();

        float parentHeight = transform.parent.parent.GetComponent<RectTransform>().rect.height;
        maxScrollValue = GetComponent<RectTransform>().rect.height - parentHeight;
    }

    public void IsMaxScollValueReach(float scollvalue) {
        if (scollvalue > maxScrollValue) {
            ChangeState(State.bottom);
        }
        else {
            ChangeState(State.middle);
        }
    }

    private void ChangeState(State newstate) {
        if (newstate == currentState) return;
        currentState = newstate;

        if (newstate == State.bottom) {
            OnReachBottom?.Invoke(this);
            Debug.Log($"{gameObject.name} reach Bottom");
        }
    }

    #endregion Conditions
}
