using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fload_BookASlot_handler : MonoBehaviour {
    [Header("Step")]
    [SerializeField] private GameObject _step1;
    [SerializeField] private GameObject _step2;
    [SerializeField] private GameObject _step3;

    [Header("Show")]
    [SerializeField] private TextMeshProUGUI _BrandName;
    [SerializeField] private TextMeshProUGUI _BrandRate;
    [SerializeField] private TextMeshProUGUI _BrandPoroduct;
    [SerializeField] private Image _BrandImage;
    [SerializeField] private TextMeshProUGUI _BrandFeedbackcount;

    [Header("Input")]
    // step1
    [SerializeField] private TMP_Dropdown _ChiNhanh;
    List<TMP_Dropdown.OptionData> optionlist = new List<TMP_Dropdown.OptionData>();
    // step2
    [SerializeField] private TMP_Dropdown _SelectionZone;
    [SerializeField] private TMP_Dropdown _SelectionSlot;
    [SerializeField] private Scoll_SpawnFromList View_tableName;
    [SerializeField] private Scoll_SpawnFromList View_tableStatus;
    [SerializeField] private TMP_Dropdown View_DateSelection;
    [SerializeField] private TimeInput Time_start;
    [SerializeField] private TimeInput Time_end;
    [SerializeField] private TMP_Dropdown Table_selection;
    //step3

    //
    [SerializeField] private NumberPicker _amount;

    [SerializeField] private Toggle _shippingChoise;
    [SerializeField] private TMP_InputField _DiaChiGiaoHang;
    [SerializeField] private TMP_InputField _ThoiGianHenLayHang;

    [SerializeField] private TMP_InputField _TenNguoiDatMon;
    [SerializeField] private TMP_InputField _SoDienThoai;
    [SerializeField] private Toggle _ThanhToanKhiNhanHang;

    [Header("Output")]
    [SerializeField] private TextMeshProUGUI _TienMon;
    [SerializeField] private TextMeshProUGUI _PhiVanChuyen;
    [SerializeField] private TextMeshProUGUI _TotalFee;

    [Header("data")]
    private E_PostSlot_store Store_info;
    private List<E_PostSlot_store> Store_info_list = new List<E_PostSlot_store>();
    private List<E_Table_Slot> ListTable = new List<E_Table_Slot>();
    private List<E_StoreZoneList> ListZONE = new List<E_StoreZoneList>();

    private E_Table_Slot_Appointment the_order = new E_Table_Slot_Appointment();


    private void OnEnable() {
        Step0();
    }
    private void OnDisable() {
        GC.Collect();
    }

    public void ShowInfo(E_PostSlot_store storeInfo) {
        Store_info = storeInfo;
        ShowBrandInfo();

        _ChiNhanh.interactable = false;
        WorkWithServer.Instance.GetStoreListOfBrand(Store_info.id_brand, SetAnOption, RefreshDropdown);
        void SetAnOption(E_PostSlot_store info) {
            string[] ChiNhanh = info.name.Split(" - ");
            optionlist.Add(new TMP_Dropdown.OptionData(ChiNhanh[1]));
            Store_info_list.Add(info);
        }
        void RefreshDropdown() {
            _ChiNhanh.options = optionlist;
            _ChiNhanh.RefreshShownValue();

            int index = 0;
            foreach (E_PostSlot_store item in Store_info_list) {
                if (Store_info.id_store == item.id_store) { break; }
                index++;
            }
            _ChiNhanh.value = index;

            _ChiNhanh.interactable = true;
        }
    }

    private void ShowBrandInfo() {
        _BrandName.text = Store_info.brand_name;
        _BrandRate.text = Store_info.brand_rate.ToString("0.0");

        if (Store_info.Avata != null) {_BrandImage.color = Color.white;}
        _BrandImage.sprite = Store_info.Avata;

        _BrandPoroduct.text = Store_info.brand_product;
    }

    public void ChangeStoreInfo(int index) {
        // on _ChiNhanh Dropdown change value
        Store_info = Store_info_list[index];
    }

    public void on_SelectionZone_Change(int index) {
        string selectedZone = _SelectionZone.options[index].text;
        Transform tablenameList = View_tableName.GetComponent<ScrollRect>().content;
        for (int i = 0; i < ListTable.Count; i++) {
            GameObject tablename_obj = tablenameList.GetChild(i+1).gameObject;
            if (ListTable[i].zone == selectedZone || index == 0) {
                tablename_obj.SetActive(true);
            }
            else {
                tablename_obj.SetActive(false);
            }
        }
    }

    /// <summary>
    /// reset step show
    /// </summary>
    private void Step0() {
        _step1.SetActive(true);
        _step2.SetActive(false);
        _step3.SetActive(false);
    }

    /// <summary>
    /// After finish step1 do it
    /// </summary>
    /// 
    E_PostSlot_store olddata;
    public void Step1() {
        if (Store_info != olddata) {
            // load information - prepair for step 2
            _SelectionSlot.interactable = false;
            _SelectionZone.interactable = false;
            TMP_Dropdown.OptionDataList SlotListOptions = new TMP_Dropdown.OptionDataList();
            TMP_Dropdown.OptionDataList ZoneListOptions = new TMP_Dropdown.OptionDataList();
            ZoneListOptions.options.Add(new TMP_Dropdown.OptionData("-- Tất cả --"));
            View_tableName.ClearAll();
            View_tableStatus.ClearAll();
            ListTable.Clear();
            ListZONE.Clear();

            WorkWithServer.Instance.Get_SlotList_OfStore(Store_info.id_brand, Store_info.id_store, Slot_ItemData, Slot_UpdateOptionData);
            void Slot_ItemData(E_Table_Slot tableSlot) {
                SlotListOptions.options.Add(new TMP_Dropdown.OptionData(
                    $"{tableSlot.name} ({tableSlot.capacity} seats) " +
                    $"<color=#FFFFFF55><size=70%>price:{tableSlot.price / 1000}k</size></color>"
                ));
                // add to view
                GameObject view_tb_name = View_tableName.SpawnFromInfo(tableSlot);
                E_TableSlot_n_DateAppoint Table_n_Date = new E_TableSlot_n_DateAppoint() {
                    id_brand = tableSlot.id_brand,
                    id_store = tableSlot.id_store,
                    id_slot = tableSlot.id_slot,
                    date_appoint = View_DateSelection.options[View_DateSelection.value].text,
                };
                GameObject view_tb_sts = View_tableStatus.SpawnFromInfo(Table_n_Date);
                view_tb_name.GetComponent<TableList_Item_SpawnFromList>().SetGameObjectFollow(view_tb_sts);

                ListTable.Add(tableSlot);
            }
            void Slot_UpdateOptionData() {
                _SelectionSlot.options = SlotListOptions.options;
                _SelectionSlot.RefreshShownValue();
                _SelectionSlot.interactable = true;
            }

            WorkWithServer.Instance.Get_SlotZONElist_OfStore(Store_info.id_brand, Store_info.id_store, Zone_LoadOptionData, Zone_UpdateOptionData);
            void Zone_LoadOptionData(E_StoreZoneList zone) {
                ZoneListOptions.options.Add(new TMP_Dropdown.OptionData(zone.zone));
                ListZONE.Add(zone);
            }
            void Zone_UpdateOptionData() {
                _SelectionZone.options = ZoneListOptions.options;
                _SelectionZone.RefreshShownValue();
                _SelectionZone.interactable = true;
            }
            olddata = Store_info;
        }
        else { Debug.Log("Step2 no need change"); }

        // Show Step 2
        _step1.SetActive(false);
        _step2.SetActive(true);
    }

    public void Step2() {
        // validate input
        if (Time_start.value == null) {
            Notifi_Action.instance.Notifi_Act("Thời gian bắt đầu (Time/Start) không được bỏ trống");
            return;
        }
        if (Time_end.value == null) {
            Notifi_Action.instance.Notifi_Act("Thời gian kết thúc (Time/End) không được bỏ trống");
            return;
        }
        if (Time_start.value.Value >= Time_end.value.Value) {
            Notifi_Action.instance.Notifi_Act("Thời gian kết thúc phải sau thời gian bắt đầu");
            return;
        }
        if (Table_selection.options.Count == 0) {
            Notifi_Action.instance.Notifi_Act("Không có bàn để chọn, vui lòng chọn chi nhánh khác");
            return;
        }

        int tableIndex = Table_selection.value;
        TableState_Item_SpawnFromList table = View_tableStatus.transform.GetChild(tableIndex + 1).GetComponent<TableState_Item_SpawnFromList>();
        List<(DateTime start, DateTime end)> busyList = table.GetBusyList();
        foreach (var busy in busyList) {
            if (!(Time_end.value.Value <= busy.start || Time_start.value.Value >= busy.end)) {
                Notifi_Action.instance.Notifi_Act("Bàn đã được đặt trong khung thời gian này, vui lòng chọn bàn khác hoặc khung giờ khác");
                return;
            }
        } // check time overlap : thank to Copilot


        // update info
        the_order.id_brand = Store_info.id_brand;
        the_order.id_store = Store_info.id_store;
        the_order.id_slot = ListTable[tableIndex].id_slot;
        the_order.id_customer = Static_Info.UserInfo.id;
        DateTime dateSelected = View_DateSelection.options[View_DateSelection.value].text.Fomat_SlashDate_2_Datetime();
        the_order.datetime_appoint = dateSelected.ToString("yyyy-MM-dd ") +
            Time_start.value.Value.ToString("HH:mm:ss");
        the_order.datetime_finnish = dateSelected.ToString("yyyy-MM-dd ") +
            Time_end.value.Value.ToString("HH:mm:ss");
        the_order._state_ = E_Table_Slot_Appointment.State_Appointment.Appointed;



        //// prepair to next step
        //_TenNguoiDatMon.text = Static_Info.UserInfo.username;

        //int TienMon = food_info.price * the_order.quantity;
        //float PhiVanChuyen = the_order.is_shipping ? (TienMon * 0.1f) : 0.0f;
        //float TotalFee = TienMon + PhiVanChuyen;

        //_TienMon.text = TienMon.ToString("N0") + " VND";
        //_PhiVanChuyen.text = PhiVanChuyen.ToString("N0") + " VND";
        //_TotalFee.text = TotalFee.ToString("N0") + " VND";

        //the_order.fee = TotalFee;

        // go to next step
        _step2.SetActive(false); _step3.SetActive(true);
    }

    public void Step3() {
        //// validate input
        //if (string.IsNullOrEmpty(_TenNguoiDatMon.text)) {
        //    Notifi_Action.instance.Notifi_Act("Tên người đặt món không được bỏ trống");
        //    return;
        //}
        //if (string.IsNullOrEmpty(_SoDienThoai.text)) {
        //    Notifi_Action.instance.Notifi_Act("Số điện thoại liên hệ không được bỏ trống");
        //    return;
        //}

        //// update information
        //the_order.username_appoint = _TenNguoiDatMon.text;
        //the_order.phone_number = _SoDienThoai.text;
        //the_order.pay_after = _ThanhToanKhiNhanHang.isOn;
        //the_order.TakeAway_State = E_Order_TakeAway.OrderTakeAway_State.ChuaTiepNhan;

        //// Upload to server
        //WorkWithServer.Instance.InsertFoodOrderTakeAway(the_order, Success, Fail);
        //void Success() { Notifi_Action.instance.Notifi_Act("Đặt hàng thành công!"); }
        //void Fail() { Notifi_Action.instance.Notifi_Act("Đặt hàng thất bại!"); }

        //// finish show
        //gameObject.SetActive(false);
    }
}
