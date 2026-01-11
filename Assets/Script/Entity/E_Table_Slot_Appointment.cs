using System;
using UnityEngine;

[Serializable]
public class E_Table_Slot_Appointment
{
    public int id_appointment;
    public int id_brand;
    public int id_store;
    public int id_slot;
    public int id_customer;
    public string datetime_appoint;
    public string datetime_finnish;
    public string username_appoint;
    public string phone_number;
    public int fee;
    public int state;
    public bool is_addition_food;

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