using System;
using TMPro;
using UnityEditor.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class Fload_Orderfood_handler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private Image _FoodImage;
    [SerializeField] private TextMeshProUGUI _FoodName;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private NumberPicker _amount;

    [SerializeField] private TextMeshProUGUI _BrandName;
    [SerializeField] private TextMeshProUGUI _BrandRate;
    [SerializeField] private Image _BrandImage;
    [SerializeField] private TextMeshProUGUI _BrandFeedbackcount;

    [SerializeField] private TextMeshProUGUI _FoodRate;

    [Header("Output")]
    [SerializeField] private TextMeshProUGUI _TienMon;
    [SerializeField] private TextMeshProUGUI _PhiVanChuyen;
    [SerializeField] private TextMeshProUGUI _TotalFee;

    [Header("data")]
    private E_PostSlot_food food_info;
    private E_Order_TakeAway the_order;

    private void OnDisable() {
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
        the_order.is_shipping = true;
    }
}
