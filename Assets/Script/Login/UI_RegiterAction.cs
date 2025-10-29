using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_RegiterAction : MonoBehaviour
{
    //reference
    private VisualElement root;
    private TextField TF_username;
    private TextField TF_password;
    private TextField TF_confirmpassword;
    private Button Btn_Register;

    //const String
    private string TF_USERNAME = "SU_Username";
    private string TF_PASSWORD = "SU_Password";
    private string TF_CONFIRM_PASSWORD = "SU_ConfirmPassword";
    private string BTN_REGISTER = "SU_BtnSignUp";

    private void Awake() {
        // find Visual Element root
        root = GetComponent<UIDocument>().rootVisualElement;
        // find other elements
        TF_username = root.Q<TextField>(TF_USERNAME);
        TF_password = root.Q<TextField>(TF_PASSWORD);
        TF_confirmpassword = root.Q<TextField>(TF_CONFIRM_PASSWORD);
        Btn_Register = root.Q<Button>(BTN_REGISTER);
    }

    private void OnEnable() {
        // action button
        Btn_Register.RegisterCallback<ClickEvent>(OnRegister);
        TF_username.RegisterValueChangedCallback(OnUsernameChange);
    }
    private void OnDisable() {
        // remove action button
        Btn_Register.UnregisterCallback<ClickEvent>(OnRegister);
    }

    private void OnRegister(ClickEvent evt) {
        string username = TF_username.value;
        string password = TF_password.value;
        string confirmPassword = TF_confirmpassword.value;
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword)) {
            Debug.Log("Please fill in all fields.");
            return;
        }
        if (password != confirmPassword) {
            Debug.Log("Passwords do not match.");
            return;
        }
        // Proceed with registration logic
        Debug.Log($"Registering user: {username}");
        // Add your registration logic here (e.g., send data to server)



        // thank you copilot AI for helping me write this code
    }

    Coroutine UsernameCheckCorotine;
    private void OnUsernameChange(ChangeEvent<string> evt) {
        if (UsernameCheckCorotine != null) StopCoroutine(UsernameCheckCorotine);
        UsernameCheckCorotine = StartCoroutine(UsernameCheckDelay());
    }
    private IEnumerator UsernameCheckDelay() {
        yield return new WaitForSeconds(0.5f);
        string username = TF_username.value;
        // Simulate a username availability check
        bool isAvailable = (username.Length % 2 == 0); // Dummy check: even length usernames are available
        if (isAvailable) {
            Debug.Log($"Username '{username}' is available.");
        } else {
            Debug.Log($"Username '{username}' is already taken.");
        }
    }
}
