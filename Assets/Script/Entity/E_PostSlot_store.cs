using System;
using UnityEngine;

[Serializable]
public class E_PostSlot_store : E_PostSlot_Base
{
    public string id_brand;
    public string id_store;
    public string name;
    public string address;
    public string rate;
    public string image_path;

    public Sprite _image;
    public void SetImage(Sprite img) {
        _image = img;
    }

    public new string title => name;
    public new string description => address;
}
