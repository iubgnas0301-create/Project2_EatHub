using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class E_PostSlot_event : E_PostSlot_Base
{
    public int id_brand;
    public int id_post;
    public string post_time;
    public string content;
    public string inage_path;

    public new string description => content;
}
