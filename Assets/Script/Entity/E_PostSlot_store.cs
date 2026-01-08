using System;
using UnityEngine;

[Serializable]
public class E_PostSlot_store : E_PostSlot_0_Base
{
    public int id_brand;
    public string brand_name;
    public string brand_avata;
    public string brand_product;
    public float brand_rate;

    public int id_store;
    public string name;
    public string address;
    public float rate;

    public Sprite Image;
    public Sprite Avata;

    public new string title => name;
    public new string description => brand_product;
}
