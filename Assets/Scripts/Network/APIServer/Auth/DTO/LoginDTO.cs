using UnityEngine;

[System.Serializable]
public class LoginRequest
{
    public string email;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public string accessToken;
    public string refreshToken;
    public int userId;
    public string userName;
    public string role;
}
