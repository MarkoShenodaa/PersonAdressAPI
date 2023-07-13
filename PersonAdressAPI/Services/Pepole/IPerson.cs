namespace PersonAdressAPI.Services.Pepole
{
    public interface IPerson : IAPIService<Person>
    {
        Task<List<Person>> GetPersonByName(string name);

        Task<Person> GetPersonByLoginData(string userName, string password);
    }
}