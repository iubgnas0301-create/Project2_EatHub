using System;
using UnityEngine;
[Serializable]
public class E_Order_TakeAway
{
    public int id_order_takeaway;

    public int id_brand;
    public int id_food;

    public int id_customer;
    public string datetime_order;
    public int quantity;
    public bool is_shipping;
    public string ship_address;
    public string datetime_appoint;
    public int state;

    public OrderTakeAway_State TakeAway_State;
    public enum OrderTakeAway_State {
        Huy = -1,
        ChuaTiepNhan = 0,
        DangCheBien = 1,
        SanSang = 2,
        DangVanchuyen = 3,
        KhuChoDenLay = 4,
        HoanThanh = 5,
    }
}
