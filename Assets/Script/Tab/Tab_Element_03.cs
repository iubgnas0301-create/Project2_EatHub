using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Element_03 : Tab_Element_01
{

    [Header("Visual")]
    [SerializeField] private GameObject _Visual;
    [SerializeField] private TextMeshProUGUI _tabText;
    [SerializeField] private Image _tabIcon;

    private Color Gold = new Color(1, 0.6588f , 0, 1);
    private Color halfWhite = new Color(1, 1, 1, 0.2f);

    protected override void ChangeVisual(bool isSelect) {
        _Visual.SetActive(isSelect);
        if (isSelect) {
            _tabText.color = Gold;
            _tabIcon.color = Gold;
        }
        else {
            _tabText.color = halfWhite;
            _tabIcon.color = halfWhite;
        }
    }
}
