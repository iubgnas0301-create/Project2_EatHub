using System;
using UnityEngine;

[Serializable]
public class E_UserInfo
{
    public int id;
    public string username;
    public int score;

    public int expPerRank { get { return 100; } }
    public int rank {
        get { return score / expPerRank; }
    }
    public int exp {
        get { return score % expPerRank; }
    }

    public override string ToString()
    {
        return $"UserID: {id}, UserName: {username}, Score: {score}";
    }
}
