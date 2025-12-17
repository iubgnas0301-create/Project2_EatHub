using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_Management : MonoBehaviour
{
    //=============================== Declaration ==============================
    // enum
    private enum State {top, middle}
    private State state = State.middle;
    // variables
    [SerializeField] private int pageIndex = 0;
    [SerializeField] private int itemPerPage = 9;
    private ScrollRect _scrollRect;
    private Transform Columns ;

    [SerializeField] private float ScrollValue = 0f;

    //================================= Logic ===================================
    private void Start() {
        _scrollRect = GetComponent<ScrollRect>();
        _scrollRect.onValueChanged.AddListener(OnScrollValueChanged);

        Columns = _scrollRect.content;
        StartCoroutine(FistUpdate());
    }

    private IEnumerator FistUpdate() {
        yield return new WaitForEndOfFrame();
        foreach (Transform column in Columns) {
            PostSlot_LoadCondition con = column.GetComponent<PostSlot_LoadCondition>();
            con.OnReachBottom += BottomReach;
            Debug.Log($"Subscribe BottomReach to {column}");
        }
        Reload();
    }

    private void OnScrollValueChanged(Vector2 pos) {
        ScrollValue = _scrollRect.content.anchoredPosition.y;
        if (ScrollValue < -200) {
            ChangeState(State.top);
        } else if (ScrollValue > 0) {
            ChangeState(State.middle);
        }


        if (state == State.middle) {
            foreach (Transform item in Columns) {
                item.GetComponent<PostSlot_LoadCondition>().IsMaxScollValueReach(ScrollValue);
            }
        }


        void ChangeState(State s) {
            if (s == state) return;
            state = s;
            if (state == State.top) {
                Reload();
                Debug.Log("reach Top"); 
            }
        }
    }

    private void Reload() {
        pageIndex = 0;
        foreach (Transform column in Columns) {
            PostSlot_LoadAction_0_Base COL1 = column.GetComponent<PostSlot_LoadAction_0_Base>();
            PostSlot_LoadCondition COL2 = column.GetComponent<PostSlot_LoadCondition>();
            COL1.ClearAllItem();
            WorkWithServer.Instance.GetFoodInfo(pageIndex, itemPerPage, COL1.CreateItem, COL2.UpadateMaxScollValue );
            pageIndex++;
        }
    }

    private void BottomReach(GameObject whoReach) {
        WorkWithServer.Instance.GetFoodInfo(++pageIndex, itemPerPage, 
            whoReach.GetComponent<PostSlot_LoadAction_0_Base>().CreateItem,
            whoReach.GetComponent<PostSlot_LoadCondition>().UpadateMaxScollValue);
    }
}
