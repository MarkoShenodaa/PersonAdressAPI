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

    public async Task<Person> GetPersonByLoginData(string userName, string password)
    {
        return _person.SingleOrDefault(x => x.UserName == userName && x.Password == password);
    }

    public PepoleService(IOptions<Tokens> tokens)
    {
        _person.Add(new Person(tokens)
        {
            Id = 1,
            FirstName = "Marko",
            LastName = "Shenodaa",
            Address = "Cairo",
            Password = "1",
            UserName = "1"
        });

        _person.Add(new Person(tokens)
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
    
}