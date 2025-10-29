using System.Collections;
using UnityEngine;

public class Tab_Group_01 : MonoBehaviour, I_Tab_Group_ctrl {

    [SerializeField] private Transform GroupTab;

    private I_Tab_Element_ctrl _SelectedTab;

    private void Start() {

        StartCoroutine(FirstRun());
    }

    private IEnumerator FirstRun() {
        yield return null;
        DeselectAllChild();
        if (GroupTab.childCount > 0) {
            _SelectedTab = GroupTab.GetComponentInChildren<I_Tab_Element_ctrl>();
            _SelectedTab.Selected();
            Debug.Log($"have {_SelectedTab} child");
        }
        else {
            Debug.Log($"{this.name} has no child");
        }
    }

    public void clickNotifiFromTab(I_Tab_Element_ctrl tab) {
        if (_SelectedTab == tab)  return;
        _SelectedTab?.Deselect();
        _SelectedTab = tab;
        _SelectedTab.Selected();
    }

    private void DeselectAllChild() {
        foreach (Transform tab in GroupTab) {
            I_Tab_Element_ctrl tabElement = tab.GetComponent<I_Tab_Element_ctrl>();
            tabElement.Deselect();
            Debug.Log($"{this.name} Deselect child {tab.name}");
        }
    }
}
