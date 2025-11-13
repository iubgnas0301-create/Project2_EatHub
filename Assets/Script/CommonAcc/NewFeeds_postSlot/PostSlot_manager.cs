using UnityEngine;

public class PostSlot_manager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform postSlot_Template;
    [SerializeField] private Transform postSlot_Holder;

    private void Start() {
        postSlot_Template.gameObject.SetActive(false);
    }

    public void SpawnPost(E_PostSlot_Base postSlot) {
        Transform instance = Instantiate(postSlot_Template, postSlot_Holder);
        instance.gameObject.SetActive(true);
        instance.gameObject.name = postSlot.title;
        PostSlot_S0_Base postSlot_single = instance.GetComponent<PostSlot_S0_Base>();
        postSlot_single?.SetInfo(postSlot);
    }

    public void DestroyAllPost() {
        foreach (Transform post in postSlot_Holder) {
            if (post == postSlot_Template) continue;
            Destroy(post);
        }
    }
}
