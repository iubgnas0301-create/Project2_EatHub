using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class E_PostSlot_event : E_PostSlot_0_Base
{
    public int id_brand;
    public int id_post;
    public string post_time;
    public string content;
    public string image_path;
    public Sprite _image;

    public new string description => content;
    public void SetImage(Sprite img) {
        _image = img;
    }
}
