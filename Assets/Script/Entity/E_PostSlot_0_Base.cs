using System;
using UnityEngine;

[Serializable]
public abstract class E_PostSlot_0_Base {
    public string title;
    public string description;
    public string image_path;

    public abstract void SetImage(Sprite img);
}
