using System.Collections.Generic;
using UnityEngine;

public class FloadUI_Control_v2 : MonoBehaviour
{
    private static List<GameObject> Fload_Stack = new List<GameObject>();

    public static FloadUI_Control_v2 instance;
    private void Start() {
        instance = this;
    }

    public GameObject Open_Fload(GameObject _fload) {
        Transform NewFload = Instantiate(_fload.transform, transform);
        NewFload.gameObject.SetActive(true);
        Fload_Stack.Add(NewFload.gameObject);
        return NewFload.gameObject;
    }

    public void Close_Fload() {
        int lastindex = Fload_Stack.Count - 1;
        if (lastindex >= 0) {
            GameObject last = Fload_Stack[lastindex];
            Destroy(last);
            Fload_Stack.RemoveAt(lastindex);
        }
    }
}
