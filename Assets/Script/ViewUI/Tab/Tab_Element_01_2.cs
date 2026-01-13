using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Element_01_2 : Tab_Element_01
{

    [Header("Visual")]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    [SerializeField] private GameObject? _Visual;
    [SerializeField] private TextMeshProUGUI? _tabText;
    [SerializeField] private Image? _tabIcon;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

    [SerializeField] private Color SelectColor = new Color(1, 0.6588f , 0, 1);
    [SerializeField] private Color DeselectColor = new Color(1, 1, 1, 0.2f);

    protected override void ChangeVisual(bool isSelect) {
        _Visual?.SetActive(isSelect);
        if (isSelect) {
            if (_tabText) _tabText.color = SelectColor;
            if (_tabIcon) _tabIcon.color = SelectColor;
        }
        else {
            if (_tabText) _tabText.color = DeselectColor;
            if (_tabIcon) _tabIcon.color = DeselectColor;
        }
    }
}
