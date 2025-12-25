using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
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
        Instance = this;
    }

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
            "SELECT `brand`.`id_brand`, `brand`.`name` AS `brand_name`, `brand`.`brand_image_path` AS `brand_avata`, `brand`.`product` AS `brand_product`, " +
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
            "SELECT `brand`.`id_brand`, `brand`.`name` AS `brand_name`, " +
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
    private IEnumerator GetFromQuery<T>(string Query, Action<T> callback, Action EndCallback) {
        yield return null;
        WWWForm form = new WWWForm();
        form.AddField("Query", Query);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/PHP_GetSpecial/GetFromQuery.php", form)) {
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
}
