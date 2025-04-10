﻿namespace API;

public class JwtSettings
{
    public string SecretKey { get; set; }
    public int Expires { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
