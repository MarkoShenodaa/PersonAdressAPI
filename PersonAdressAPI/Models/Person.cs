global using System.Text.Json.Serialization;

namespace PersonAdressAPI.Models;

public class Person
{
    public int Id { get; set; }

    public string Name { get => string.Format("{0} {1}", FirstName, LastName); }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? Address { get; set; }

    [JsonIgnore]
    public string? UserName { get; set; }

    [JsonIgnore]
    public string? Password { get; set; }
}