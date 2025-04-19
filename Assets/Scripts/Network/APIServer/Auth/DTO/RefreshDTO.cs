using System;

[Serializable]
public class RefreshRequest
{
    public string refreshToken;
}

[Serializable]
public class RefreshResponse
{
    public string accessToken;
    public string refreshToken;
}