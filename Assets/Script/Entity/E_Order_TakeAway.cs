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
    public string username_appoint;
    public string phone_number;
    public float fee;
    public bool pay_after;
    public int state;

    public OrderTakeAway_State TakeAway_State { 
        get {
            return (OrderTakeAway_State)state;
        }
        set {
            state = (int)value;
        }
    }
    public enum OrderTakeAway_State {
        Huy = -1,
        ChuaTiepNhan = 0,
        DangCheBien = 1,
        SanSang = 2,
        DangVanchuyen = 3,
        KhuChoDenLay = 4,
        HoanThanh = 5,
    }
    public static string GetStateString(int state_in) {
        return state_in switch {
            (int)OrderTakeAway_State.Huy => "Đã hủy",
            (int)OrderTakeAway_State.ChuaTiepNhan => "Chưa tiếp nhận",
            (int)OrderTakeAway_State.DangCheBien => "Đang chế biến",
            (int)OrderTakeAway_State.SanSang => "Sẵn sàng",
            (int)OrderTakeAway_State.DangVanchuyen => "Đang vận chuyển",
            (int)OrderTakeAway_State.KhuChoDenLay => "Khu chờ đến lấy",
            (int)OrderTakeAway_State.HoanThanh => "Hoàn thành",
            _ => "Không xác định",
        }; // thanks to Copilot for this switch expression
    }
}
