using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PostSlot_LoadCondition : MonoBehaviour
{
    public Action<GameObject> OnReachBottom;
    private enum State {middle, bottom }

    private State currentState = State.middle;

    private float maxScrollValue = 0;

    public void UpadateMaxScollValue() {
        transform.parent.GetComponent<ContentResize>().SetAndGetHeight();

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
            OnReachBottom?.Invoke(this.gameObject);
            Debug.Log($"{gameObject.name} reach Bottom");
        }
    }
}
