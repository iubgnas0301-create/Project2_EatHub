#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Foldout_Attribute))]
public class Foldout_Drawer : PropertyDrawer {
    private bool foldout = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Foldout_Attribute foldoutAttribute = (Foldout_Attribute)attribute;

        // Vẽ foldout
        foldout = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), 
            foldout, foldoutAttribute.title, true);

        if (foldout) {
            EditorGUI.indentLevel++; // thụt vào để hiển thị đúng cấp độ
            EditorGUI.PropertyField(
                new Rect(
                    position.x, 
                    position.y + EditorGUIUtility.singleLineHeight, 
                    position.width,
                    EditorGUI.GetPropertyHeight(property)), 
                property, true);
            EditorGUI.indentLevel--; // bỏ thụt sau khi hoàn thành
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (foldout) {
            return EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.singleLineHeight;
        } else {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif