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
    private string UserID;

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
        UpdateAndNotifi_Password(PasswordStatus.Checking);
        
        yield return StartCoroutine(LoginCheck_());
        
        Login_WhenAllCheckDone();
    }
    private IEnumerator LoginCheck_() {
        WWWForm form = new WWWForm();
        form.AddField("username", TF_Username.value);
        form.AddField("password", TF_Password.value);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/EatHubConnect/PlayerLogin.php", form)) {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.LogError("LoginError " + www.error);
                UpdateAndNotifi_Password(PasswordStatus.OtherError, " : can't connect to server");
            }
            else {
                string responseText = www.downloadHandler.text;
                if (responseText[0] == '0') {
                    UpdateAndNotifi_Username(UsernameStatus.Exist);
                    UpdateAndNotifi_Password(PasswordStatus.Correct);
                    UserID = responseText.Split('\t')[1];
                }
                else {
                    if (responseText[0] == '5') {
                        UpdateAndNotifi_Username(UsernameStatus.NotExist);
                    }
                    else if (responseText[0] == '5') {
                        UpdateAndNotifi_Password(PasswordStatus.Incorrect);
                    }
                    else {
                        UpdateAndNotifi_Password(PasswordStatus.OtherError, responseText);
                    }
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

    private void Login_WhenAllCheckDone() {
        Btn_Login.SetEnabled( true );

        if (_usernameStartus == UsernameStatus.Exist &&
            _passwordStatus == PasswordStatus.Correct) {
            // Successful login
            Debug.Log("Login successful!");
            Static_Info.SetUserID(UserID);
            Loader.LoadScene(SCENES.CustomerScene);
        }
    }
}
