using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_Management : MonoBehaviour {
    //=============================== Declaration ==============================
    // enum
    private enum State { top, middle }
    private State state = State.middle;
    // variables
    [SerializeField] private int pageIndex = 0;
    [SerializeField] private int itemPerPage = 9;
    private ScrollRect _scrollRect;
    private Transform Columns;

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
            PostSlot_LoadAction_n_Condition con = column.GetComponent<PostSlot_LoadAction_n_Condition>();
            con.OnReachBottom += BottomReach2;
            Debug.Log($"Subscribe BottomReach to {column}");
        }
        Reload();
    }

    private void OnScrollValueChanged(Vector2 pos) {
        ScrollValue = _scrollRect.content.anchoredPosition.y;
        if (ScrollValue < -200) {
            ChangeState(State.top);
        }
        else if (ScrollValue > 0) {
            ChangeState(State.middle);
        }


        if (state == State.middle) {
            foreach (Transform item in Columns) {
                item.GetComponent<PostSlot_LoadAction_n_Condition>().IsMaxScollValueReach(ScrollValue);
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


    public void Reload() {
        pageIndex = 0;
        foreach (Transform column in Columns) {
            PostSlot_LoadAction_n_Condition COL1 = column.GetComponent<PostSlot_LoadAction_n_Condition>();
            COL1.ClearAllItem();
            COL1.PrepareForLoad(pageIndex, itemPerPage);
            pageIndex++;
        }
    }

    private void BottomReach2(PostSlot_LoadAction_n_Condition whoReach) {
        whoReach.PrepareForLoad(pageIndex, itemPerPage);
        pageIndex++;
    }
}