using UnityEngine;

public class ContentResize : MonoBehaviour
{
    public void ResetHeight()
    {
        float maxChildrenHeight = 0f;
        foreach (Transform item in transform) {
            float itemHeight = item.GetComponent<RectTransform>().rect.height;
            maxChildrenHeight = Mathf.Max(maxChildrenHeight, itemHeight);
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = maxChildrenHeight;
        rectTransform.sizeDelta = sizeDelta;
    }
}
