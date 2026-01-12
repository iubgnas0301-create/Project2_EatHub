using System.Collections;
using UnityEngine;
using System;

public class GetUserInfo : MonoBehaviour
{
    public Action GetUser_callback;
    private void Start() {
        StartCoroutine(WorkWithServer.Instance.GetUserInfo(GetUser_callback));
    }
}
