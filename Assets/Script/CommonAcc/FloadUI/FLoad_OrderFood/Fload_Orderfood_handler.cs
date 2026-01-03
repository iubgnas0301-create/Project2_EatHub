using System;
using TMPro;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class Fload_Orderfood_handler : MonoBehaviour
{
    [Header("Step")]
    [SerializeField] private GameObject _step1;
    [SerializeField] private GameObject _step2;
    [SerializeField] private GameObject _step3;

    [Header("Show")]
    [SerializeField] private Image _FoodImage;
    [SerializeField] private TextMeshProUGUI _FoodName;
    [SerializeField] private TextMeshProUGUI _price;

    [SerializeField] private TextMeshProUGUI _BrandName;
    [SerializeField] private TextMeshProUGUI _BrandRate;
    [SerializeField] private Image _BrandImage;
    [SerializeField] private TextMeshProUGUI _BrandFeedbackcount;

    [SerializeField] private TextMeshProUGUI _FoodRate;

    [Header("Input")]
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
    private E_PostSlot_food food_info;
    private E_Order_TakeAway the_order;

    private void OnDisable() {
        Step0();
        the_order = null;
        GC.Collect();
    }

    public void ShowInfo(E_PostSlot_food foodInfo) {
        food_info = foodInfo;

        _FoodName.text = food_info.name;
        _price.text = food_info.price.ToString() + " VND";
        _FoodImage.sprite = food_info.Image;
        _amount.ResetValue();


        if (food_info.brand_name != null) {
            //show brand info directly
            ShowBrandInfo();
        } else {
            //get brand information
            WorkWithServer.Instance.GetBrandInfo(food_info.id_brand, (brandinfo) => {
                if (brandinfo == null) { 
                    Debug.Log($"have no brand with ID = {food_info.id_brand}");
                    return;
                }
                food_info.brand_name = brandinfo.name;
                food_info.brand_rate = brandinfo.rate;
                if (!string.IsNullOrEmpty(brandinfo.brand_image_path)) {
                    WorkWithServer.Instance.DownLoadImage(brandinfo.brand_image_path, (sprite) => {
                        food_info.Brand_Image = sprite;
                        _BrandImage.color = Color.white;
                        _BrandImage.sprite = sprite;
                    });
                }
                ShowBrandInfo();
            });
        }
    }

    private void ShowBrandInfo() {
        _BrandName.text = food_info.brand_name;
        _BrandRate.text = food_info.brand_rate.ToString("0.0");
        _BrandImage.sprite = food_info.Brand_Image;
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
    public void Step1() {
        // update E_Order_takeaway
        the_order = new E_Order_TakeAway();

        the_order.id_customer = Static_Info.UserInfo.id;
        the_order.id_brand = food_info.id_brand;
        the_order.id_food = food_info.id_food;
        the_order.quantity = _amount.GetValue();

        Debug.Log($"Customer name {Static_Info.UserInfo.username} start ordering {food_info.name}");
        Debug.Log($"cutomerID:{the_order.id_customer}|brandID:{the_order.id_brand}|foodID:{the_order.id_food}|quantity:{the_order.quantity}");
    }

    public void Step2() {
        // validate input
        if (_shippingChoise.isOn && string.IsNullOrEmpty(_DiaChiGiaoHang.text)) {
            Notifi_Action.instance.Notifi_Act("Địa chỉ giao hàng không thể bỏ trống");
            return;
        }

        // update info
        the_order.is_shipping = _shippingChoise.isOn;
        if (the_order.is_shipping) {
            the_order.ship_address = _DiaChiGiaoHang.text;
        }
        else {
            the_order.datetime_appoint = string.IsNullOrEmpty(_ThoiGianHenLayHang.text)? "Sớm nhất có thể" : _ThoiGianHenLayHang.text;
        }

        Debug.Log($"is_shipping = {the_order.is_shipping}\n" +
            $"dia chi giao hang : {the_order.ship_address}\n" +
            $"thoi gian lay hang : {the_order.datetime_appoint}");

        // go to next step
        _step2.SetActive(false); _step3.SetActive(true);

        // prepair to next step
        _TenNguoiDatMon.text = Static_Info.UserInfo.username;

        int TienMon = food_info.price * the_order.quantity;
        float PhiVanChuyen = the_order.is_shipping ? (TienMon * 0.1f) : 0.0f;
        float TotalFee = TienMon + PhiVanChuyen;
        
        _TienMon.text = TienMon.ToString("N0") + " VND";
        _PhiVanChuyen.text = PhiVanChuyen.ToString("N0") + " VND";
        _TotalFee.text = TotalFee.ToString("N0") + " VND";

        the_order.fee = TotalFee;
    }

    public void Step3() {
        // validate input
        if (string.IsNullOrEmpty(_TenNguoiDatMon.text)) {
            Notifi_Action.instance.Notifi_Act("Tên người đặt món không được bỏ trống");
            return;
        }
        if (string.IsNullOrEmpty(_SoDienThoai.text)) {
            Notifi_Action.instance.Notifi_Act("Số điện thoại liên hệ không được bỏ trống");
            return;
        }

        // update information
        the_order.username_appoint = _TenNguoiDatMon.text;
        the_order.phone_number = _SoDienThoai.text;
        the_order.pay_after = _ThanhToanKhiNhanHang.isOn;
        the_order.TakeAway_State = E_Order_TakeAway.OrderTakeAway_State.ChuaTiepNhan;

        // Upload to server
        WorkWithServer.Instance.InsertFoodOrderTakeAway(the_order, Success, Fail);
        void Success() { Notifi_Action.instance.Notifi_Act("Đặt hàng thành công!"); }
        void Fail() { Notifi_Action.instance.Notifi_Act("Đặt hàng thất bại!"); }

        // finish show
        gameObject.SetActive(false);
    }
}
