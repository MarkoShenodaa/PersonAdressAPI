﻿namespace PersonAdressAPI.Models;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(Person user, string token)
    {
        Id = user.Id;
        Name = user.Name;
        Token = token;
    }
}