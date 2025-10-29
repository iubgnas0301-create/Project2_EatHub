using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Login_control : MonoBehaviour
{
    // refference
    private VisualElement root;
    private Button Btn_CreateAccount;
    private Button Btn_SignUp_Back;
    private VisualElement VE_SignUp;
    private VisualElement VE_SignUpBG;
    private Label LoginLabel;

    // const String
    private string SIGN_UP = "SignUpSkrim";
    private string SIGNUP_BG = "SignUp";
    private string BTN_LOGIN_SIGNUP = "Btn_Login_SignUp";
    private string BTN_SIGNUP_BACK = "Btn_SignUp_Back";
    private string CLASS_HIDE_SIGNUP_BG = "SignUp-hide";
    private string CLASS_HIDE_SIGNUP_SKRIM = "SignUpSkrim-hide";
    private string LOGIN_LABEL = "LoginTitle";

    void Awake()
    {
        // tim Visual Element dau tien
        root = GetComponent<UIDocument>().rootVisualElement;

        // tim kiem cac bien con lai
        VE_SignUp = root.Q<VisualElement>(SIGN_UP);
        VE_SignUpBG = root.Q<VisualElement>(SIGNUP_BG);
        Btn_CreateAccount = root.Q<Button>(BTN_LOGIN_SIGNUP);
        Btn_SignUp_Back = root.Q<Button>(BTN_SIGNUP_BACK);
        LoginLabel = root.Q<Label>(LOGIN_LABEL);

        // Defaut State (Not Display Sign Up form)
        VE_SignUp.style.display = DisplayStyle.None;

        // lable typing animation
        LoginLabel.text = string.Empty;
        StartCoroutine(FirstUpdate());
        //Typing_anim.Instance.Typing(ref LoginLabel, "Member login");

        //// action button
        //Btn_CreateAccount.RegisterCallback<ClickEvent>(OnCreateAccount);
        //Btn_SignUp_Back.RegisterCallback<ClickEvent>(OnCancelSigningUp);

        //// Undisplay Sign Up form (only do when transision END)
        //VE_SignUpBG.RegisterCallback<TransitionEndEvent>(HideSignUpUI);

    }

    private void OnEnable() {
        // action button
        Btn_CreateAccount.RegisterCallback<ClickEvent>(OnCreateAccount);
        Btn_SignUp_Back.RegisterCallback<ClickEvent>(OnCancelSigningUp);

        //Undisplay Sign Up form(only do when transision END)
        VE_SignUpBG.RegisterCallback<TransitionEndEvent>(HideSignUpUI);
    }

    private void OnDisable() {
        Btn_CreateAccount.UnregisterCallback<ClickEvent>(OnCreateAccount);
        Btn_SignUp_Back.UnregisterCallback<ClickEvent>(OnCancelSigningUp);
        VE_SignUpBG.UnregisterCallback<TransitionEndEvent>(HideSignUpUI);
    }

    private void OnCreateAccount(ClickEvent e) {
        // Display Sign Up form
        VE_SignUp.style.display = DisplayStyle.Flex;

        // transition
        VE_SignUp.RemoveFromClassList(CLASS_HIDE_SIGNUP_SKRIM);
        VE_SignUpBG.RemoveFromClassList(CLASS_HIDE_SIGNUP_BG);
    }

    private void OnCancelSigningUp(ClickEvent e) {

        // transition
        VE_SignUp.AddToClassList(CLASS_HIDE_SIGNUP_SKRIM);
        VE_SignUpBG.AddToClassList(CLASS_HIDE_SIGNUP_BG);

    }

    private void HideSignUpUI(TransitionEndEvent e) {
        Debug.Log("end transision");
        if (VE_SignUpBG.ClassListContains(CLASS_HIDE_SIGNUP_BG)) {
            // only do it only on Hide action
            VE_SignUp.style.display = DisplayStyle.None;
        }
    }

    #region corotine
    private IEnumerator FirstUpdate() {
        yield return null;
        Typing_anim.Instance.Typing(ref LoginLabel, "Member login");
    }
    #endregion

}
