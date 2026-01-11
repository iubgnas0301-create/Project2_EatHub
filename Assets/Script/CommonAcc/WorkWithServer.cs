using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UI;

public class WorkWithServer : MonoBehaviour
{
    public static WorkWithServer Instance;

    public enum SearchFillter {
        None,
        Rate_DESC,
        Rate_ASC,
        Price_ASC,
        Price_DESC,
    }

    private string Fillter2Query(string tableName,SearchFillter filler) {
        switch (filler) {
            case SearchFillter.Rate_ASC:
                return $"ORDER BY `{tableName}`.`rate` ASC ";
            case SearchFillter.Rate_DESC:
                return $"ORDER BY `{tableName}`.`rate` DESC ";
            default:
                return "";
        }
    }
    private string Fillter2Query_food(SearchFillter filler) {
        switch (filler) {
            case SearchFillter.Rate_ASC:
                return "ORDER BY `food`.`rate` ASC ";
            case SearchFillter.Rate_DESC:
                return "ORDER BY `food`.`rate` DESC ";
            case SearchFillter.Price_ASC:
                return "ORDER BY `food`.`price` ASC ";
            case SearchFillter.Price_DESC:
                return "ORDER BY `food`.`price` DESC ";
            default:
                return "";
        }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        } else {
            if (Instance != this) Destroy(gameObject);
        }
    }
    #region ///////////////////// GetInfo /////////////////////
    public IEnumerator GetUserInfo(Action callback) {

        yield return null;

        // check UserID
        string staticUserID = Static_Info.GetUserID();
        if (string.IsNullOrEmpty(staticUserID)) {
            Debug.Log("UserID is null or empty.");
            yield break;
        }

        yield return null;

        WWWForm form = new WWWForm();
        form.AddField("userID", staticUserID);
        
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/GetUserInfo.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error: " + www.error);
            } else {
                string responseText = www.downloadHandler.text;
                if (responseText[0] != '0') { // server indicates error
                    Debug.LogError("Error# " + responseText);
                    yield break;
                }
                E_UserInfo userInfo = JsonUtility.FromJson<E_UserInfo>(responseText.Substring(1));
                Static_Info.SetUserInfo(userInfo);
                Debug.Log("User Info Retrieved: " + Static_Info.UserInfo.ToString());
                callback?.Invoke();
            }
        }
    }

    public IEnumerator GetUserAchievement(Action<E_PostSlot_achievement> callback) {

        yield return null;

        // check UserID
        Debug.Log("GetUserAchievement");
        string staticUserID = Static_Info.GetUserID();
        if (string.IsNullOrEmpty(staticUserID)) {
            Debug.Log("UserID is null or empty. Achievement");
            yield break;
        }

        yield return null;

        WWWForm form = new WWWForm();
        form.AddField("userID", staticUserID);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/GetUserAchievement.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error: " + www.error);
            }
            else {
                string responseText = www.downloadHandler.text;
                if (responseText[0] != '0') { // server indicates error
                    Debug.LogError("Error# " + responseText);
                    yield break;
                }
                Debug.Log("Achievement : " + responseText);

                //// Method 1
                //E_PostSlot_achievement E_achiev;
                //string[] JsonRespone = responseText.Substring(1).Trim('[',']').Trim('{', '}').Split("},{");
                //foreach (string Jres in JsonRespone) {
                //    string newJres = '{' + Jres + '}';
                //    E_achiev = JsonUtility.FromJson<E_PostSlot_achievement>(newJres);
                //    callback(E_achiev);
                //    yield return null;
                //}

                // Method 2 : Parse JSON array using a wrapper because JsonUtility cannot parse a raw root array
                E_PostSlot_achievement[] E_achiev_list = JsonArrayUtility.FromJsonArray<E_PostSlot_achievement>(responseText.Substring(1));
                foreach (E_PostSlot_achievement E_achiev in E_achiev_list) {
                    callback(E_achiev);
                    yield return null;

                }

            }
        }
    }
    /// <summary>
    /// 5 event per page
    /// </summary>
    /// <param name="page"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    //public IEnumerator GetEvent(int pageIndex, Action<E_PostSlot_event> callback) { 
    //    yield return null;

    //    WWWForm form = new WWWForm();
    //    form.AddField("page", pageIndex);

    //    using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/GetEvents.php", form)) {
    //        yield return www.SendWebRequest();
    //        if (www.result != UnityWebRequest.Result.Success) {
    //            Debug.LogError("Error: " + www.error);
    //        }
    //        else {
    //            string responseText = www.downloadHandler.text;
    //            if (responseText[0] != '0') { // server indicates error
    //                Debug.LogError("Error# " + responseText);
    //                yield break;
    //            }
    //            Debug.Log("Event : " + responseText);
    //            responseText = responseText.Substring(1);

    //            E_PostSlot_event[] E_event_list = JsonArrayUtility.FromJsonArray<E_PostSlot_event>(responseText);
    //            foreach (E_PostSlot_event E_event in E_event_list) {
    //                if (!string.IsNullOrEmpty(E_event.image_path)) {
    //                    // Start a coroutine to get the image
    //                    yield return StartCoroutine(GetImage("event", E_event.image_path, E_event.SetImage));
    //                } else {
    //                    E_event.SetImage(null);
    //                }
    //                callback(E_event);
    //                yield return null;
    //            }

    //        }
    //    }
    //}
    public void GetEventInfo(int pageIndex, int itemPerPage, Action<E_PostSlot_event> callback, Action EndCallback) {
        //StartCoroutine(GetTable("event", pageIndex, itemPerPage, callback, EndCallback));

        string query =
            "SELECT `brand`.`id_brand`, `brand`.`name` AS `brand_name`, `brand`.`rate` AS `rate`, " +
                "`brand`.`brand_image_path` AS `brand_avata`, " +
                "`id_post`, `post_time`, `title`, `content`, `image_path` " +
            "FROM `event` " +
            "JOIN `brand` ON `brand`.`id_brand` = `event`.`id_brand` " +
            "ORDER BY `post_time` DESC " +
            $"LIMIT {itemPerPage} " +
            $"OFFSET {pageIndex * itemPerPage} " +
            ";";
        StartCoroutine(GetFromQuery(query, callback, EndCallback));
    }
    public void GetStoreInfo(int pageIndex, int itemPerPage, Action<E_PostSlot_store> callback, Action EndCallback,
        string search = "", SearchFillter fillter = SearchFillter.None) {
        //StartCoroutine(GetTable("store", pageIndex, itemPerPage, callback, EndCallback));
        string query =
            "SELECT `brand`.`id_brand`, `brand`.`name` AS `brand_name`, `brand`.`brand_image_path` AS `brand_avata`, " + 
                "`brand`.`product` AS `brand_product`, `brand`.`rate` as `brand_rate`, " +
                "`store`.`id_store`, `store`.`name`, `store`.`address`, `store`.`rate`, `store`.`image_path` " +
            "FROM `store` " +
            "JOIN `brand` ON `brand`.`id_brand` = `store`.`id_brand` " +
            $"WHERE `brand`.`name` LIKE \"%{search}%\"  " +
                $"OR `brand`.`product` LIKE \"%{search}%\" " +
                $"OR `store`.`name` LIKE \"%{search}%\" " +
                $"OR `store`.`address` LIKE \"%{search}%\" " +
            Fillter2Query("store",fillter) +
            $"LIMIT {itemPerPage} " +
            $"OFFSET {pageIndex * itemPerPage} " +
            ";";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, EndCallback));
    }
    public void GetFoodInfo(int pageIndex, int itemPerPage, Action<E_PostSlot_food> callback, Action EndCallback,
        string search = "", SearchFillter fillter = SearchFillter.None) {
        //StartCoroutine(GetTable("food",pageIndex, itemPerPage, callback, EndCallback));
        string query =
            "SELECT `brand`.`id_brand`, " +
                "`food`.`id_food`, `food`.`name`, `food`.`price`, `food`.`quantity_per_set`, " +
                "`food`.`describle`, `food`.`rate`, `food`.`limitted_quantity`, `food`.`image_path` " +
            "FROM `food` " +
            "JOIN `brand` ON `brand`.`id_brand` = `food`.`id_brand` " +
            $"WHERE `brand`.`name` LIKE \"%{search}%\"  " +
                $"OR `food`.`name` LIKE \"%{search}%\" " +
                $"OR `food`.`describle` LIKE \"%{search}%\" " +
            Fillter2Query_food(fillter) +
            $"LIMIT {itemPerPage} " +
            $"OFFSET {pageIndex * itemPerPage} " +
            ";";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, EndCallback));
    }
    public void GetBrandInfo(int id_brand, Action<E_BrandInfo> callback) {
        string query = $"SELECT * FROM `brand` WHERE `id_brand` = {id_brand};";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, null));
    }
    public void GetStoreListOfBrand(int id_brand, Action<E_PostSlot_store> callback, Action EndCallback) {
        string query = $"SELECT * FROM `store` WHERE `id_brand` = {id_brand};";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, EndCallback));
    }
    public void Get_SlotList_OfStore(int id_brand, int id_store, Action<E_Table_Slot> callback, Action EndCallback) {
        string query = "SELECT * FROM `table_slot` "+
            $"WHERE `id_brand` = {id_brand} AND `id_store` = {id_store} ";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, EndCallback));
    }
    public void Get_SlotZONElist_OfStore(int id_brand, int id_store, Action<E_StoreZoneList> callback, Action EndCallback) {
        string query = "SELECT `zone`, COUNT(`id_slot`) AS `slot_count` FROM `table_slot` " +
            $"WHERE `id_brand` = {id_brand} AND `id_store` = {id_store} " +
            "GROUP BY `zone`";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, EndCallback));
    }
    public void Get_SlotAppointFromDatetime(int id_brand, int id_store,int id_slot, string datetime_appoint,
        Action<E_Table_Slot_Appointment> callback, Action EndCallback) {
        string query = "SELECT `datetime_appoint`, `datetime_finnish`, `state` " +
            "FROM `table_slot_appointment` " +
            "WHERE "+
                $"`id_brand` = {id_brand} AND `id_store` = {id_store} AND `id_slot` = {id_slot} AND "+
                $"`datetime_appoint` LIKE \"{datetime_appoint}%\" " +
            ";";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, EndCallback));
    }
    public void GetFoodOfBrand(int id_brand, Action<E_PostSlot_food> callback, Action EndCallBack) {
        string query = 
            "SELECT `id_brand`, `id_food`, `name`, `price`, `quantity_per_set`, " +
                "`describle`, `rate`, `limitted_quantity`, `image_path` " +
            "FROM `food` " +
            $"WHERE `id_brand` = {id_brand} " +
            "ORDER BY `rate` DESC " +
            ";";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, EndCallBack));
    }
    public void GetTableAppoint(int id_customer, Action<E_Show_TableAppoint> callback, Action EndCallback) {
        string query =
            "SELECT `table_slot_appointment`.`id_appointment`, " +
                "`brand`.`name` AS `brand_name`, " +
                "`store`.`name` AS `store_name`, " +
                "`table_slot_appointment`.`datetime_appoint` AS `datetime_appoint`, " +
                "`table_slot_appointment`.`datetime_finnish` AS `datetime_finnish`, " +
                "`store`.`address` AS `location`, " +
                "`table_slot_appointment`.`state` AS `state` " +
            "FROM `table_slot_appointment` " +
            "JOIN `store` ON " +
                "`store`.`id_brand` = `table_slot_appointment`.`id_brand` AND " +
                "`store`.`id_store` = `table_slot_appointment`.`id_store` " +
            "JOIN `brand` ON `brand`.`id_brand` = `table_slot_appointment`.`id_brand` " +
            $"WHERE `table_slot_appointment`.`id_customer` = {id_customer} " +
            "ORDER BY `table_slot_appointment`.`datetime_appoint` DESC " +
            ";";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, EndCallback));
    }
    public void CancelAppoint(int id_appoint, Action successCallback, Action ErrorCallback) {
        string query =
            "UPDATE `table_slot_appointment` " +
            $"SET `state` = {(int)E_Table_Slot_Appointment.State_Appointment.Cancelled} " +
            $"WHERE `id_appointment` = {id_appoint} " +
            ";" +
            "UPDATE `order_onside` " +
            $"SET `state` = {(int)E_Order_Onside.State_Appointment.Cancelled} " +
            $"WHERE `id_appointment` = {id_appoint} " +
            ";";
        Debug.Log(query);
        StartCoroutine(GetFromMultiQuery(query,(string nothing) => { }, successCallback, ErrorCallback));
    }
    public void GetFoodOrderTakeAway(int id_customer, Action<E_Show_FoodOrder> callback, Action EndCallback) {
        string query =
            "SELECT `order_takeaway`.`id_order_takeaway` AS `id_order_takeaway`, " +
                "`brand`.`name` AS `brand_name`, " +
                "`food`.`name` AS `food_name`, " +
                "`order_takeaway`.`datetime_appoint` AS `datetime_appoint`, " +
                "`order_takeaway`.`quantity` AS `quantity`, " +
                "`order_takeaway`.`is_shipping` AS `is_shipping`, " +
                "`order_takeaway`.`ship_address` AS `ship_address`, " +
                "`order_takeaway`.`username_appoint` AS `username_appoint`, " +
                "`order_takeaway`.`phone_number` AS `phone_number`, " +
                "`order_takeaway`.`fee` AS `fee`, " +
                "`order_takeaway`.`state` AS `state` " +
            "FROM `order_takeaway` " +
            "JOIN `food` ON " +
                "`food`.`id_brand` = `order_takeaway`.`id_brand` AND " +
                "`food`.`id_food` = `order_takeaway`.`id_food` " +
            "JOIN `brand` ON " +
                "`brand`.`id_brand` = `order_takeaway`.`id_brand` " +
            $"WHERE `order_takeaway`.`id_customer` = {id_customer} " +
            "ORDER BY `order_takeaway`.`datetime_appoint` DESC " +
            ";";
        Debug.Log(query);
        StartCoroutine(GetFromQuery(query, callback, EndCallback));
    }
    public void CancelFoodOrderTakeAway(int id_order_takeaway, Action successCallback, Action ErrorCallback) {
        string query =
            "UPDATE `order_takeaway` " +
            $"SET `state` = {(int)E_Order_TakeAway.OrderTakeAway_State.Huy} " +
            $"WHERE `id_order_takeaway` = {id_order_takeaway} " +
            ";";
        Debug.Log(query);
        StartCoroutine(PushFromQuery(query, successCallback, ErrorCallback));
    }
    #endregion

    #region ///////////////////// Insert /////////////////////
    public void InsertFoodOrderTakeAway(E_Order_TakeAway _info, Action SuccessCallback, Action ErrorCallback) {
        string query = "INSERT INTO `order_takeaway` " + 
            "(`id_order_takeaway`, `id_brand`, `id_food`, `id_customer`, " + 
            "`datetime_order`, `quantity`, `is_shipping`, `ship_address`, " + 
            "`datetime_appoint`, `username_appoint`, `phone_number`, `fee`, " +
            "`pay_after`, `state`) " + 
            "VALUES " +
            $"(NULL, {_info.id_brand}, {_info.id_food}, {_info.id_customer}, " +
            $"NULL, {_info.quantity}, {_info.is_shipping}, \"{_info.ship_address}\", " + 
            $"\"{_info.datetime_appoint}\", \"{_info.username_appoint}\", \"{_info.phone_number}\", {_info.fee}, " +
            $"{_info.pay_after}, {_info.state})";

        Debug.Log(query);
        StartCoroutine(PushFromQuery(query, SuccessCallback, ErrorCallback));
    }
    public void InsertTableSlotAppointment(E_Table_Slot_Appointment _info, Action<E_Table_Slot_Appointment> SuccessCallback, Action ErrorCallback) {
        int isAdditionFood = _info.is_addition_food?1:0;
        string query =
            "INSERT INTO `table_slot_appointment` " +
                "(`id_brand`, `id_store`, `id_slot`, `id_customer`, " +
                "`datetime_appoint`, `datetime_finnish`, " +
                "`username_appoint`, `phone_number`, `fee`, `state`, `is_addition_food`)" +
            "VALUES " +
                $"({_info.id_brand}, {_info.id_store}, {_info.id_slot}, {_info.id_customer}, " +
                $"\"{_info.datetime_appoint}\", \"{_info.datetime_finnish}\", " + 
                $"\"{_info.username_appoint}\", \"{_info.phone_number}\", " +
                $"{_info.fee}, {_info.state}, {isAdditionFood});\n" +
            "SELECT LAST_INSERT_ID() as `id_appointment`;"; // trả về giá trị auto increment vừa chèn
        Debug.Log(query);
        StartCoroutine(GetFromMultiQuery(query, SuccessCallback, null, ErrorCallback));
    }
    public void InsertOrderOnside(int id_appointment, List<E_Order_Onside> _info, Action SuccessCallback, Action ErrorCallback) {
        string query =
            "INSERT INTO `order_onside` " +
                "(`id_appointment`, `id_brand`, `id_food`, `quantity`, `state`) " +
            "VALUES ";
        foreach (E_Order_Onside order in _info) {
            query += 
                $"({id_appointment}, {order.id_brand}, {order.id_food}, " +
                $"{order.quantity}, {order.state}),";
        }
        query = query.TrimEnd(',') + ";";
        Debug.Log(query);
        StartCoroutine(PushFromQuery(query, SuccessCallback, ErrorCallback));
    }
    #endregion

    #region ///////////////////// Base /////////////////////
    private IEnumerator GetFromQuery<T>(string Query, Action<T> callback, Action EndCallback, Action ErrorCallback = null) {
        yield return null;
        WWWForm form = new WWWForm();
        form.AddField("Query", Query);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/PHP_GetSpecial/GetFromQuery.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log("Error: " + www.error);
                ErrorCallback?.Invoke();
                yield break;
            }
            else {
                string responseText = www.downloadHandler.text;
                if (responseText[0] != '0') { // server indicates error
                    Debug.Log("Error# " + responseText);
                    ErrorCallback?.Invoke();
                    yield break;
                }
                Debug.Log("Special : " + responseText);
                responseText = responseText.Substring(1);
                T[] E_item_list = JsonArrayUtility.FromJsonArray<T>(responseText);
                foreach (T E_item in E_item_list) {
                    callback?.Invoke(E_item);
                    yield return null;
                }
            }
        }
        EndCallback?.Invoke();
    }
    private IEnumerator PushFromQuery(string Query, Action EndCallback, Action ErrorCallback = null) {
        yield return null;
        WWWForm form = new WWWForm();
        form.AddField("Query", Query);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/PHP_GetSpecial/PushFromQuery.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log("Error: " + www.error);
                ErrorCallback?.Invoke();
                yield break;
            }
            else {
                string responseText = www.downloadHandler.text;
                if (responseText[0] != '0') { // server indicates error
                    Debug.Log("Error# " + responseText);
                    ErrorCallback?.Invoke();
                    yield break;
                }
                Debug.Log("Special : " + responseText);
            }
        }
        EndCallback?.Invoke();
    }
    private IEnumerator GetFromMultiQuery<T>(string MultiQuery, Action<T> callback, Action EndCallback, Action ErrorCallback = null) {
        yield return null;
        WWWForm form = new WWWForm();
        form.AddField("MultiQuery", MultiQuery);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/PHP_GetSpecial/GetFromMultiQuery.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log("Error: " + www.error);
                ErrorCallback?.Invoke();
                yield break;
            }
            else {
                string responseText = www.downloadHandler.text;
                if (responseText[0] != '0') { // server indicates error
                    Debug.Log("Error# " + responseText);
                    ErrorCallback?.Invoke();
                    yield break;
                }
                Debug.Log("Special : " + responseText);

                responseText = responseText.Substring(1);
                if (string.IsNullOrEmpty(responseText)) {
                    EndCallback?.Invoke();
                    yield break;
                }
                T[] E_item_list = JsonArrayUtility.FromJsonArray<T>(responseText);
                foreach (T E_item in E_item_list) {
                    callback?.Invoke(E_item);
                    yield return null;
                }
            }
        }
        EndCallback?.Invoke();
    }
    private IEnumerator GetTable<T>(string tableName, int pageIndex, int itemPerPage, Action<T> callback, Action EndCallback) {
        yield return null;

        WWWForm form = new WWWForm();
        form.AddField("tableName", tableName);
        form.AddField("page", pageIndex);
        form.AddField("itemPerPage", itemPerPage);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/PHP_GetTable/GetTable.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error: " + www.error);
            }
            else {
                string responseText = www.downloadHandler.text;
                if (responseText[0] != '0') { // server indicates error
                    Debug.LogError("Error# " + responseText);
                    yield break;
                }
                Debug.Log("Food : " + responseText);
                responseText = responseText.Substring(1);
                T[] E_item_list = JsonArrayUtility.FromJsonArray<T>(responseText);
                foreach (T E_item in E_item_list) {
                    callback?.Invoke(E_item);
                    yield return null;
                }
            }
        }
        EndCallback?.Invoke();
    }

    public void DownLoadImage(string imagePath, Action<Sprite> callbackSetImage) {
        if (string.IsNullOrEmpty(imagePath)) return;
        StartCoroutine(GetImage(imagePath, callbackSetImage));
    }
    public IEnumerator GetImage(string Name, Action<Sprite> callback) {
        WWWForm form = new WWWForm();
        form.AddField("ImageName", Name);
        using(UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/PHP_GetSpecial/GetImage.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error: " + www.error);
            }
            else {
                byte[] imageBytes = www.downloadHandler.data;
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);
                // Use the texture as needed
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                callback(sprite);
            }
        }
    }
    #endregion
}
