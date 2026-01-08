using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.GPUSort;

public class Scoll_CopyScoll_n_ControlBack : MonoBehaviour {
    private ScrollRect this_Scroll;
    [SerializeField] private ScrollRect? _horizontal;
    [SerializeField] private ScrollRect? _vertical;

    private ScrollRect? _CONTROL_;

    void Start()
    {
        this_Scroll = GetComponent<ScrollRect>();
        this_Scroll.onValueChanged.AddListener(OnThisScroll);
        _vertical?.onValueChanged.AddListener(OnVerticalScroll);
        _horizontal?.onValueChanged.AddListener(OnHorizontalScroll);

        this_Scroll.AddComponent<OnDrag_ScrollRect>().OnBeginDragAction = () => { _CONTROL_ = this_Scroll; };
        if (_vertical != null && _vertical.GetComponent<OnDrag_ScrollRect>()) 
            _vertical.AddComponent<OnDrag_ScrollRect>().OnBeginDragAction = () => { _CONTROL_ = _vertical; };
        if (_horizontal != null && _horizontal.GetComponent<OnDrag_ScrollRect>())
            _horizontal.AddComponent<OnDrag_ScrollRect>().OnBeginDragAction = () => { _CONTROL_ = _horizontal; };
    }

    private void OnThisScroll(Vector2 arg0) {
        if (!CheckControl(this_Scroll)) return;

        if (_horizontal != null)
            _horizontal.horizontalNormalizedPosition = arg0.x;

        if (_vertical != null)
            _vertical.verticalNormalizedPosition = arg0.y;
    }

    private void OnVerticalScroll(Vector2 agr0) {
        if(!CheckControl(_vertical)) return;
        this_Scroll.verticalNormalizedPosition = agr0.y;
        
    }

    private void OnHorizontalScroll(Vector2 agr0) {
        if(!CheckControl(_horizontal)) return;
        this_Scroll.horizontalNormalizedPosition = agr0.x;
    }

    private bool CheckControl(ScrollRect control) {
        float speed = control.velocity.magnitude;
        Debug.Log($"{control.name} speed: {speed}");
        if (speed < 2) { control.StopMovement(); }
        return _CONTROL_ == control;
    }
}

public class OnDrag_ScrollRect : MonoBehaviour, IBeginDragHandler {
    public Action OnBeginDragAction;

    public void OnBeginDrag(PointerEventData eventData) {
        OnBeginDragAction?.Invoke();
    }
}