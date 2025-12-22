using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class E_PostSlot_event : E_PostSlot_0_Base
{
    public int id_brand;
    public string brand_name;
    public string brand_avata;

    public int id_post;
    public string post_time;
    public string content;
    public Sprite _image;

    public new string description => content;
}
