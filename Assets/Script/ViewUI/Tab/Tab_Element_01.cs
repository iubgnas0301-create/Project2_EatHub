using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Element_01 : MonoBehaviour, I_Tab_Element_ctrl {

    [Header("REF")]
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject TabGroup;


    private Button _btn;
    private I_Tab_Group_ctrl _tabGroup;

    private void Start() {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(InformToTabGroup);
        _tabGroup = TabGroup.GetComponent<I_Tab_Group_ctrl>();
    }

    private void InformToTabGroup() {
        _tabGroup.clickNotifiFromTab(this);
    }

    protected virtual void ChangeVisual(bool isSelect) {

    }

    public void Deselect() {
        content.SetActive(false);
        ChangeVisual(false);
        //Debug.Log($"Deselect {this.name}");
    }

    public void Selected() {
        content.SetActive(true);
        ChangeVisual(true);
        //Debug.Log($"Select {this.name}");
    }
}
