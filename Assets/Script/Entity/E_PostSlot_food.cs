using System;
using UnityEngine;

[Serializable]
public class E_PostSlot_food : E_PostSlot_0_Base
{
    public string id_brand;
    public string id_food;
    public string name;
    public int price;
    public string quantity_per_set;
    public string describe;
    public float rate;
    public string img_url;
    public int limitted_quantity;
    public string image_path;

    public new string title => name;
    public new string description => describe;
    public Sprite Image;
    public void SetImage(Sprite sprite)
    {
        Image = sprite;
    }
}
