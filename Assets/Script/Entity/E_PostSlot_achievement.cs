using System;
using UnityEngine;

[Serializable]
public class E_PostSlot_achievement : E_PostSlot_0_Base
{
    public int id;
    public string a_name;
    public string a_describe;
    public string a_condition;
    public string a_date;

    public new string title => a_name;
    public new string description => $"{a_describe} \n <color=#FFFFFF88>{a_condition}</color>";

    public override void SetImage(Sprite i) {}
}
