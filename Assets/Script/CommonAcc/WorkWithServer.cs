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
    public IEnumerator GetEvent(int pageIndex, Action<E_PostSlot_event> callback) { 
        yield return null;

        WWWForm form = new WWWForm();
        form.AddField("page", pageIndex);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/GetEvents.php", form)) {
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
                Debug.Log("Event : " + responseText);
                responseText = responseText.Substring(1);

                E_PostSlot_event[] E_event_list = JsonArrayUtility.FromJsonArray<E_PostSlot_event>(responseText);
                foreach (E_PostSlot_event E_event in E_event_list) {
                    if (!string.IsNullOrEmpty(E_event.image_path)) {
                        // Start a coroutine to get the image
                        yield return StartCoroutine(GetImage("event", E_event.image_path, E_event.SetImage));
                    } else {
                        E_event.SetImage(null);
                    }
                    callback(E_event);
                    yield return null;
                }

            }
        }
    }

    public IEnumerator GetStore(int pageIndex, Action<E_PostSlot_store> callback) {
        yield return null;

        WWWForm form = new WWWForm();
        form.AddField("page", pageIndex);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/GetStores.php", form)) {
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
                Debug.Log("Store : " + responseText);
                responseText = responseText.Substring(1);

                E_PostSlot_store[] E_store_list = JsonArrayUtility.FromJsonArray<E_PostSlot_store>(responseText);
                Debug.Log("Store count: " + E_store_list.Length);
                foreach (E_PostSlot_store E_store in E_store_list) {
                    if (!string.IsNullOrEmpty(E_store.image_path)) {
                        // Start a coroutine to get the image
                        //yield return StartCoroutine(GetImage("store", E_store.image_path, E_store.SetImage));
                    }
                    else {
                        E_store.SetImage(null);
                    }
                    //Debug.Log("Store Name: " + E_store.name);
                    callback(E_store);
                    yield return null;
                }

            }
        }
    }

    public void GetFoodInfo(int pageIndex, int itemPerPage, Action<E_PostSlot_food> callback, Action EndCallback) {
        StartCoroutine(GetFood(pageIndex, itemPerPage, callback, EndCallback));
    }
    public IEnumerator GetFood(int pageIndex, int itemPerPage, Action<E_PostSlot_food> callback, Action EndCallback) {
        yield return null;

        WWWForm form = new WWWForm();
        form.AddField("page", pageIndex);
        form.AddField("itemPerPage", itemPerPage);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/GetFoods.php", form)) {
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
                E_PostSlot_food[] E_food_list = JsonArrayUtility.FromJsonArray<E_PostSlot_food>(responseText);
                foreach (E_PostSlot_food E_food in E_food_list) {
                    if (!string.IsNullOrEmpty(E_food.image_path)) {
                        // Start a coroutine to get the image
                        //yield return StartCoroutine(GetImage("food", E_food.image_path, E_food.SetImage));
                    }
                    else {
                        E_food.SetImage(null);
                    }
                    callback?.Invoke(E_food);
                    yield return null;
                }
            }
        }
        EndCallback?.Invoke();
    }

    public IEnumerator GetImage(string Folder, string Name, Action<Sprite> callback) {
        WWWForm form = new WWWForm();
        form.AddField("FolderName", Folder);
        form.AddField("ImageName", Name);
        using(UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/GetImage.php", form)) {
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
