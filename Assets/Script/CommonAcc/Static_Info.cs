using UnityEngine;

public static class Static_Info
{
    public static string UserID { get; private set; }
    public static E_UserInfo UserInfo { get; private set; }

    public static void SetUserID(string userID)
    {
        UserID = userID;
    }

    public static void SetUserInfo(E_UserInfo userInfo)
    {
        UserInfo = userInfo;
    }


}
