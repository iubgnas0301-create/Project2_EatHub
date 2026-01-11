using UnityEngine;

public class Foldout_Attribute : PropertyAttribute
{
    public string title;
    public Foldout_Attribute(string title) {
        this.title = title;
    }
}
