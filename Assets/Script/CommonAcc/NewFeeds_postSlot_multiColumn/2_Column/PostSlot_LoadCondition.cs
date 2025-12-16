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
        float parentHeight = transform.parent.parent.GetComponent<RectTransform>().rect.height;
        maxScrollValue = GetComponent<RectTransform>().rect.height - parentHeight;

        // a little ajustment ro get true size of parent height
        StartCoroutine(WaitAndCheckBottom());
    }

    public void IsMaxScollValueReach(float scollvalue) {
        if (scollvalue > maxScrollValue) {
            ChangeState(State.bottom);
        }
        else {
            ChangeState(State.middle);
        }
    }

    private IEnumerator WaitAndCheckBottom() {
        yield return new WaitForSeconds(2);
        transform.position = transform.position;
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
