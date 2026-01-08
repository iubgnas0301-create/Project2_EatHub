using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteAlways]
[DisallowMultipleComponent]
public class TMP_Text_copy_Text : MonoBehaviour {
    [SerializeField] TextMeshProUGUI original;
    [SerializeField] TextMeshProUGUI this_text;

    private void Awake() {
        this_text = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable() {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChange);
    }
    private void OnDisable() {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChange);
    }

    void OnTextChange(Object WhoChange) {
        if (!original || !this_text) return;
        if (WhoChange == original) {
            if (original.text != this_text.text) {
                this_text.text = original.text;
#if UNITY_EDITOR
                // lưu vào scene hoặc prefab nếu có thay đổi
                UnityEditor.EditorUtility.SetDirty(this_text);
#endif
            }
        }
    }
}
