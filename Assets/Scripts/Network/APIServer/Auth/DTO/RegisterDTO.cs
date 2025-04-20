using System;

[System.Serializable]
public class RegisterRequest
{
    public string Email;
    public string Password;
    public string Username;
}

[System.Serializable]
public class RegisterResponse
{
    public string username { get; set; }
    public string email { get; set; }
    public DateTime createAt { get; set; }
}
