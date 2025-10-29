using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class Typing_anim : MonoBehaviour
{
    public static Typing_anim Instance;


    private void Start() {
        Instance = this;
    }

    private Label refLabel;

    public void Typing(ref Label UILable, string taget, float speed = 0.1f) {
        refLabel = UILable;
        StartCoroutine(Typingg( taget, speed));
    }
    private IEnumerator Typingg(string taget, float speed) {
        int i = 0;
        int tagetLength = taget.Length;
        while (i < tagetLength) {
            i++;
            refLabel.text = taget.Substring(0,i);
            yield return new WaitForSeconds(speed);
        }
    }

    
}
