using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WorkWithServer : MonoBehaviour
{
    public static WorkWithServer Instance;
    void Start() {
        Instance = this;
    }

    public IEnumerator GetUserInfo(Action callback) {
        yield return null;
        WWWForm form = new WWWForm();
        form.AddField("userID", Static_Info.UserID);
        
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
}
