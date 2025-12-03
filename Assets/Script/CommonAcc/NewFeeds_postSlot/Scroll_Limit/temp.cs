using UnityEngine;
using UnityEngine.UI;

public class temp : MonoBehaviour
{
    [SerializeField] private PostSlot_manager PostLoad_script;
    private ScrollRect m_scrollRect;
    [SerializeField] private float ScollValue;
    [SerializeField] private float MaxScollValue;

    private void Start() {
        m_scrollRect = GetComponent<ScrollRect>();
        PostLoad_script.OnNumberOfPostChange += Update_MaxScollVallue;
    }

    private void Update_MaxScollVallue() {
        //MaxScollValue = - GetComponent<RectTransform>().rect.height + m_scrollRect.content.rect.height;
    }

    private void Update() {
        MaxScollValue = -GetComponent<RectTransform>().rect.height + m_scrollRect.content.rect.height;
        ScollValue = m_scrollRect.content.anchoredPosition.y;
    }
}
