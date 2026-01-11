using System;
using UnityEngine;

[Serializable]
public class E_Order_Onside
{
    public int id_food_order;
    public int id_appointment;
    public int id_brand;
    public int id_food;
    public int quantity;
    public int state;

    public State_Appointment _state_ {
        get { return (State_Appointment)state; }
        set { state = (int)value; }
    }

    public enum State_Appointment {
        Cancelled = -1,
        Appointed = 0,
        Serving = 1,
        Completed = 2
    }
}
