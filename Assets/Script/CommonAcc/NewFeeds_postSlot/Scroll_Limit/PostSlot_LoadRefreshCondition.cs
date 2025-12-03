using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PostSlot_LoadRefreshCondition : MonoBehaviour
{
    private enum StateScoll { Top, Mid, Bot }
    private StateScoll state;

    public Action OnEndReach;
    public Action OnTopReach;

    private ScrollRect m_scrollRect;
    [SerializeField] private float ScollValue;
    [SerializeField] private float MaxScollValue;

    private void Start() {
        m_scrollRect = GetComponent<ScrollRect>();
        m_scrollRect.onValueChanged.AddListener(Act_WhenScrolling);
    }

    private void OnEnable() {
        StartCoroutine(Update_MaxScollVallue());
    }

    public void Update_MAX_ScollVallue() {
        if (!this.isActiveAndEnabled) { return; }
        StartCoroutine(Update_MaxScollVallue());
    }

    public IEnumerator Update_MaxScollVallue() {
        yield return null;
        MaxScollValue = - m_scrollRect.GetComponent<RectTransform>().rect.height + m_scrollRect.content.rect.height;
    }

    private void Act_WhenScrolling(Vector2 VtChange) {
        ScollValue = m_scrollRect.content.anchoredPosition.y;

        if (ScollValue < -200)                  SwitchState(StateScoll.Top);
        else if (ScollValue > MaxScollValue)    SwitchState(StateScoll.Bot);
        else                                    SwitchState(StateScoll.Mid);

        void SwitchState(StateScoll s){
            if (s == state) return;
            state = s;
            if (state == StateScoll.Top) { OnTopReach?.Invoke(); Debug.Log("reach Top"); }
            if (state == StateScoll.Bot) { OnEndReach?.Invoke(); Debug.Log("reach End"); }
        }
    }
}
