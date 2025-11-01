using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class UI_LoginAction : MonoBehaviour {
    //reference
    private VisualElement root;
    private TextField TF_Username;
    private TextField TF_Password;
    private Label LB_ErrorNotifi;
    private Button Btn_Login;

    //const String
    private const string TEXTFIELD_BASE = "RoundTextField";
    private const string TF_USERNAME = "Username";
    private const string TF_PASSWORD = "Password";
    private const string LB_ERRORNOTIFI = "LoginErrorNotifi";
    private const string BTN_LOGIN = "Btn_Login";

    // variables
    private UsernameStatus _usernameStartus;
    private PasswordStatus _passwordStatus;

    private enum UsernameStatus {
        Checking,
        Exist,
        NotExist,
        OtherError
    }

    private enum PasswordStatus {
        Checking,
        Correct,
        Incorrect,
        OtherError
    }

    private void Awake() {
        // find Visual Element root
        root = GetComponent<UIDocument>().rootVisualElement;
        // find other elements
        TF_Username = root.Q<VisualElement>(TF_USERNAME).Q<TextField>(TEXTFIELD_BASE);
        TF_Password = root.Q<VisualElement>(TF_PASSWORD).Q<TextField>(TEXTFIELD_BASE);
        LB_ErrorNotifi = root.Q<Label>(LB_ERRORNOTIFI);
        Btn_Login = root.Q<Button>(BTN_LOGIN);
    }

    private void OnEnable() {
        // action button
        Btn_Login.RegisterCallback<ClickEvent>(OnLogin);
    }

    private void OnDisable() {
        // remove action button
        Btn_Login.UnregisterCallback<ClickEvent>(OnLogin);
    }
    private void Start() {
        // initial status
        UpdateAndNotifi_Username(UsernameStatus.Checking);
        UpdateAndNotifi_Password(PasswordStatus.Checking);
    }



    private void OnLogin(ClickEvent evt) {
        Debug.Log("Login button clicked");
        Btn_Login.SetEnabled(false); // Avoid multiple clicks
        StartCoroutine(LoginCheck());
    }

    
    private IEnumerator LoginCheck() {
        UpdateAndNotifi_Username(UsernameStatus.Checking);

        yield return StartCoroutine(LoginCheck_Username());

        if (_usernameStartus == UsernameStatus.Exist) {
            yield return StartCoroutine(LoginCheck_Password());
        }

        Login_WhenAllCheckDone();
    }
    #region Username Check
    private IEnumerator LoginCheck_Username() {
        WWWForm form = new WWWForm();
        form.AddField("username", TF_Username.value);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/UserCheckExist.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error checking username: " + www.error);
                UpdateAndNotifi_Username(UsernameStatus.OtherError, www.error);
            }
            else {
                string[] responseText = www.downloadHandler.text.Split("\t");
                if (responseText[0] == "1") {
                    UpdateAndNotifi_Username(UsernameStatus.Exist);
                }
                else if (responseText[0] == "0") {
                    UpdateAndNotifi_Username(UsernameStatus.NotExist);
                }
                else {
                    _usernameStartus = UsernameStatus.OtherError;
                    UpdateAndNotifi_Username(UsernameStatus.OtherError, responseText[1]);
                }
            }
        }
    }
    private void UpdateAndNotifi_Username(UsernameStatus status, string otherError = null) {
        _usernameStartus = status;
        switch (status) {
            case UsernameStatus.NotExist:
                LB_ErrorNotifi.text = "Username does not exist.";
                break;
            case UsernameStatus.OtherError:
                LB_ErrorNotifi.text = "Error#" + otherError;
                break;
            default:
                LB_ErrorNotifi.text = "";
                break;

        }
    }
    #endregion

    #region Password Check
    private IEnumerator LoginCheck_Password() {
        WWWForm form = new WWWForm();
        form.AddField("username", TF_Username.value);
        form.AddField("password", TF_Password.value);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/PlayerLogin.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error checking password: " + www.error);
                UpdateAndNotifi_Password(PasswordStatus.OtherError, www.error);
            }
            else {
                string responseText = www.downloadHandler.text.Trim();
                if (responseText == "0") {
                    UpdateAndNotifi_Password(PasswordStatus.Correct);
                }
                else {
                    UpdateAndNotifi_Password(PasswordStatus.Incorrect);
                }
            }
        }
    }
    private void UpdateAndNotifi_Password(PasswordStatus status, string otherError = null) {
        _passwordStatus = status;
        switch (status) {
            case PasswordStatus.Incorrect:
                LB_ErrorNotifi.text = "Incorrect password.";
                break;
            case PasswordStatus.OtherError:
                LB_ErrorNotifi.text = "Error#" + otherError;
                break;
            default:
                LB_ErrorNotifi.text = "";
                break;
        }
    }
    #endregion

    private void Login_WhenAllCheckDone() {
        Btn_Login.SetEnabled( true );

        if (_usernameStartus == UsernameStatus.Exist &&
            _passwordStatus == PasswordStatus.Correct) {
            // Successful login
            Debug.Log("Login successful!");
        }
    }
}
