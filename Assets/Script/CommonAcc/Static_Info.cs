using UnityEngine;

public static class Static_Info
{
    private static string UserID;
    public static E_UserInfo UserInfo { get; private set; }

    public static string GetUserID() {
        return string.IsNullOrEmpty(UserID) ? "3" : UserID;
    }
    public static void SetUserID(string userID) {
        UserID = userID;
    }

    public static void SetUserInfo(E_UserInfo userInfo)
    {
        UserInfo = userInfo;
    }
}
