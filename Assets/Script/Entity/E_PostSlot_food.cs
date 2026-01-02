using System;
using UnityEngine;

[Serializable]
public class E_PostSlot_food : E_PostSlot_0_Base
{
    public int id_brand;
    public string brand_name;
    public float brand_rate;
    public string brand_avata;

    public int id_food;
    public string name;
    public int price;
    public string quantity_per_set;
    public string describle;
    public float rate;
    public int limitted_quantity;

    public new string title => name;
    public new string description => describle;

    public Sprite Image;
    public Sprite Brand_Image;
}
