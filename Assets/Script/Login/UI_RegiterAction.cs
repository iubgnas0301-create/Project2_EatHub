using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class UI_RegiterAction : MonoBehaviour
{
    //reference
    private VisualElement root;
    private TextField TF_username;
    private Label LB_User_errorNotifi;
    private TextField TF_password;
    private TextField TF_confirmpassword;
    private Label LB_Pass_errorNotifi;
    private Button Btn_Register;
    private UI_Login_control loginControl;

    //const String
    private const string TEXTFIELD_BASE           = "RoundTextField";
    private const string TF_USERNAME              = "SU_Username";
    private const string LB_USER_ERRORNOTIFI      = "SU_UserError";
    private const string TF_PASSWORD              = "SU_Password";
    private const string TF_CONFIRM_PASSWORD      = "SU_ConfirmPassword";
    private const string LB_PASS_ERRORNOTIFI      = "SU_PassError";
    private const string BTN_REGISTER             = "SU_BtnSignUp";
    private const string CLASS_ERRORNOTIFI_FIX    = "Error_Lable_fixOK";

    // variables
    private UsernameStatus _usernameStartus;
    private PasswordStatus _passwordStatus;

    [SerializeField] private float inputDelay = 1f;

    private enum UsernameStatus {
        Available,
        TooShort,
        Taken,
        Checking
    }

    private enum PasswordStatus {
        Checking,
        TooWeak,
        Mismatch,
        Match
    }

    private void Awake() {
        // find Visual Element root
        root = GetComponent<UIDocument>().rootVisualElement;
        // find other elements
        TF_username = root.Q<VisualElement>(TF_USERNAME).Q<TextField>(TEXTFIELD_BASE);
        LB_User_errorNotifi = root.Q<Label>(LB_USER_ERRORNOTIFI);
        TF_password = root.Q<VisualElement>(TF_PASSWORD).Q<TextField>(TEXTFIELD_BASE);
        TF_confirmpassword = root.Q<VisualElement>(TF_CONFIRM_PASSWORD).Q<TextField>(TEXTFIELD_BASE);
        LB_Pass_errorNotifi = root.Q<Label>(LB_PASS_ERRORNOTIFI);
        Btn_Register = root.Q<Button>(BTN_REGISTER);
        // get login control
        loginControl = GetComponent<UI_Login_control>();
    }

    private void OnEnable() {
        // action button
        Btn_Register.RegisterCallback<ClickEvent>(OnRegister);
        TF_username.RegisterValueChangedCallback(OnUsernameChange);
        TF_password.RegisterValueChangedCallback(OnPasswordChange);
        TF_confirmpassword.RegisterValueChangedCallback(OnPasswordChange);
    }
    private void OnDisable() {
        // remove action button
        Btn_Register.UnregisterCallback<ClickEvent>(OnRegister);
        TF_username.UnregisterValueChangedCallback(OnUsernameChange);
        TF_password.UnregisterValueChangedCallback(OnPasswordChange);
        TF_confirmpassword.UnregisterValueChangedCallback(OnPasswordChange);
    }

    private void Start() {
        UpdateAndNotifi_Username(UsernameStatus.Checking);
        UpdateAndNotifi_Pass(PasswordStatus.Checking);
        Btn_Register.SetEnabled(false);
    }

    private void OnRegister(ClickEvent evt) {
        StartCoroutine(RegisterRequest());
        Btn_Register.SetEnabled(false);
    }

    private IEnumerator RegisterRequest() {
        yield return null;
        Debug.Log("Registering User: " + TF_username.value);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/PlayerRegister.php", new WWWForm())) {
            WWWForm form = new WWWForm();
            form.AddField("username", TF_username.value);
            form.AddField("password", TF_password.value);
            www.uploadHandler = new UploadHandlerRaw(form.data);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error Registering: " + www.error);
            }
            else {
                Debug.Log("User Registered Successfully: " + www.downloadHandler.text);
                //CloseRegisterForm();
            }
        }
    }


    #region Username Checking
    Coroutine UsernameCheckCorotine;

    private void OnUsernameChange(ChangeEvent<string> evt) {
        UpdateAndNotifi_Username(UsernameStatus.Checking);
        if (UsernameCheckCorotine != null) StopCoroutine(UsernameCheckCorotine);
        UsernameCheckCorotine = StartCoroutine(UsernameCheckDelay(inputDelay));
    }
    private IEnumerator UsernameCheckDelay(float timeDelay = 2f) {
        yield return new WaitForSeconds(timeDelay);
        string username = TF_username.value;

        // Check username status
        if (username.Length < 8) {
            UpdateAndNotifi_Username(UsernameStatus.TooShort);
            yield break;
        }


        // request to server
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/UserCheckExist.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("LoginError " + www.error);

                LB_User_errorNotifi.text = "Error" + www.error;
                LB_User_errorNotifi.RemoveFromClassList(CLASS_ERRORNOTIFI_FIX);
            }
            else {
                string[] responseText = www.downloadHandler.text.Split("\t");
                if (responseText[0] == "0") {
                    // Unique Username
                    UpdateAndNotifi_Username(UsernameStatus.Available);
                }
                else if (responseText[0] == "1") {
                    // Username is exist
                    UpdateAndNotifi_Username(UsernameStatus.Taken);
                }
                else {
                    // another error
                    LB_User_errorNotifi.text = "Error" + www.downloadHandler.text;
                    Debug.Log("Error: " + www.downloadHandler.text);
                    LB_User_errorNotifi.RemoveFromClassList(CLASS_ERRORNOTIFI_FIX);
                }
            }
        }


        //WWW CheckUser = new WWW("http://localhost/EatHubConnect/UserCheckExist.php", form);
        //yield return CheckUser;

        //string[] responseText = CheckUser.text.Split("\t");
        //if (responseText[0] == "0") {
        //    // Unique Username
        //    UpdateAndNotifi_Username(UsernameStatus.Available);
        //}
        //else if (responseText[0] == "1") {
        //    // Username is exist
        //    UpdateAndNotifi_Username(UsernameStatus.Taken);
        //}
        //else {
        //    // another error
        //    LB_User_errorNotifi.text = "Error" + CheckUser.text;
        //    Debug.Log("Error: " + CheckUser.text);
        //    LB_User_errorNotifi.RemoveFromClassList(CLASS_ERRORNOTIFI_FIX);
        //}

        RegisterBtn_InputCondition();

    }
    private void UpdateAndNotifi_Username(UsernameStatus status) {
        _usernameStartus = status;
        switch (status) {
            case UsernameStatus.Available:
                LB_User_errorNotifi.text = "Avaiable";
                LB_User_errorNotifi.AddToClassList(CLASS_ERRORNOTIFI_FIX);
                break;
            case UsernameStatus.Taken:
                LB_User_errorNotifi.text = "Username be taken. Please choose another name";
                LB_User_errorNotifi.RemoveFromClassList(CLASS_ERRORNOTIFI_FIX); 
                break;
            case UsernameStatus.TooShort:
                LB_User_errorNotifi.text = "Username at least 8 charecter";
                LB_User_errorNotifi.RemoveFromClassList(CLASS_ERRORNOTIFI_FIX);
                break;
            default:
                LB_User_errorNotifi.text = "";
                break;
        }
    }
    #endregion

    #region Password Checking
    private Coroutine PasswordCheckCorotine;
    private void OnPasswordChange(ChangeEvent<string> evt) {
        UpdateAndNotifi_Pass(PasswordStatus.Checking);
        if (PasswordCheckCorotine != null) { StopCoroutine(PasswordCheckCorotine); }
        PasswordCheckCorotine = StartCoroutine(PasswordCheckDelay(inputDelay));
    }
    private IEnumerator PasswordCheckDelay(float timeDelay = 2f) {
        yield return new WaitForSeconds(timeDelay);
        string password = TF_password.value;
        string confirmPass = TF_confirmpassword.value;

        if (password.Length < 8) {
            // too short pass
            UpdateAndNotifi_Pass(PasswordStatus.TooWeak);
        } else if (password == confirmPass) {
            // pass ok and matah
            UpdateAndNotifi_Pass(PasswordStatus.Match);
        } else {
            // not mactch
            UpdateAndNotifi_Pass(PasswordStatus.Mismatch);
        }

        RegisterBtn_InputCondition();
    }
    private void UpdateAndNotifi_Pass(PasswordStatus status) {
        _passwordStatus = status;
        switch (status) {
            case PasswordStatus.TooWeak:
                LB_Pass_errorNotifi.text = "Username at least 8 charecter";
                LB_Pass_errorNotifi.RemoveFromClassList(CLASS_ERRORNOTIFI_FIX);
                break;
            case PasswordStatus.Mismatch:
                LB_Pass_errorNotifi.text = "Password and Confirm password are not match";
                LB_Pass_errorNotifi.RemoveFromClassList(CLASS_ERRORNOTIFI_FIX);
                break;
            case PasswordStatus.Match:
                LB_Pass_errorNotifi.text = "password ready to use";
                LB_Pass_errorNotifi.AddToClassList(CLASS_ERRORNOTIFI_FIX);
                break;
            default:
                LB_Pass_errorNotifi.text = "";
                break;
        }
    }
    #endregion

    private void RegisterBtn_InputCondition() {
        Btn_Register.SetEnabled(
            _usernameStartus == UsernameStatus.Available &&
            _passwordStatus == PasswordStatus.Match
        );
    }

    private void CloseRegisterForm() {
        loginControl.BackToLoginForm();
    }

}
