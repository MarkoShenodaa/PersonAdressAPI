using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonAdressAPI.Services.Pepole;

public class PepoleService : IPerson
{
    public async Task<List<Person>> AddModel(Person model)
    {
        _person.Add(model);

        return await GetAllModel();
    }

    public async Task<List<Person>> DeleteModel(int id)
    {
        var person = await GetModelByID(id);
        if (person != null) { _person.Remove(person); }
        return await GetAllModel();
    }

    public async Task<List<Person>> GetAllModel()
    {
        return _person;
    }

    public async Task<Person> GetModelByID(int id)
    {
        return _person.Find(x => x.Id == id);
    }

    public async Task<List<Person>> GetPersonByName(string name)
    {
        return _person.FindAll(x => x.Name.ToLower().Contains(name.ToLower()));
    }

    public async Task<List<Person>> UpdateModel(Person person)
    {
        var oldPerson = await GetModelByID(person.Id);
        _person.Add(person);
        _person.Remove(oldPerson);

        return await GetAllModel();
    }

    public async Task<AuthenticateResponse> GetPersonByLoginData(AuthenticateRequest request)
    {
        var person = _person.SingleOrDefault(x => x.UserName == request.UserName && x.Password == request.Password);
        
        return person != null ? new AuthenticateResponse(person) { Token = GenerateJwtToken(person.Id.ToString()) } : null;
    }

    private string GenerateJwtToken(string id)
    {
        // generate token that is valid for 1 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenKey.TokenKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", id) }),

            Expires = DateTime.UtcNow.AddDays(1),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<List<Person>> AddLoginToPerson(int personID, AuthenticateRequest request)
    {
        var person = await GetModelByID(personID);

        if(person != null)
        {
            person.UserName = request.UserName;
            person.Password = request.Password;
            await UpdateModel(person);
        }

        return await GetAllModel();
    }

    public PepoleService(IOptions<Tokens> tokens)
    {
        _tokenKey = tokens.Value;
        _person.Add(new Person()
        {
            Id = 1,
            FirstName = "Marko",
            LastName = "Shenodaa",
            Address = "Cairo",
            Password = "1",
            UserName = "1"
        });

        _person.Add(new Person()
        {
            Id = 2,
            FirstName = "Hala",
            LastName = "Kmeid",
            Address = "Cairo",
            Password = "456",
            UserName = "user"
        });
    }

    private List<Person> _person = new List<Person>();
    private readonly Tokens _tokenKey;
}