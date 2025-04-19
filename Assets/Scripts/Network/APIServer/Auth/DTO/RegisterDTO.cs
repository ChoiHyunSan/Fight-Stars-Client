using System;

[System.Serializable]
public class RegisterRequest
{
    public string email;
    public string password;
    public string username;
}

[System.Serializable]
public class RegisterResponse
{
    public string username { get; set; }
    public string email { get; set; }
    public DateTime createAt { get; set; }
}
