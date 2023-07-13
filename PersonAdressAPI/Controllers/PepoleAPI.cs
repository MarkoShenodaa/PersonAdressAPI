global using PersonAdressAPI.Models;

namespace PersonAdressAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PepoleAPIController : ControllerBase
{
    private readonly IPerson _aPIService;

    public PepoleAPIController(IPerson aPIService)
    {
        _aPIService = aPIService;
    }

    [Authorize]
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<Person>>> GetPersonesAsync()
    {
        return Ok(await _aPIService.GetAllModel());
    }

    [Authorize]
    [HttpGet("GetByID/{id}")]
    public async Task<ActionResult<Person>> GetPersonAsync(int id)
    {
        var person = await _aPIService.GetModelByID(id);
        return person != null ? Ok(person) : BadRequest("Can't Find any person with this ID");
    }

    [Authorize]
    [HttpGet("GetPersonByName/{name}")]
    public async Task<ActionResult<List<Person>>> GetPeopleAsync(string name)
    {
        var person = await _aPIService.GetPersonByName(name);
        return person != null && person.Count > 0 ? Ok(person) : BadRequest("Can't Find any person with this ID");
    }

    [Authorize]
    [HttpPost("AddNewPerson")]
    public async Task<ActionResult<List<Person>>> AddPeoplesAsync(Person person)
    {
        if (person == null) return BadRequest("Can't Add Empty person data.");

        if (string.IsNullOrEmpty(person.FirstName)) return BadRequest("FirstName Can't be Empty.");

        if (string.IsNullOrEmpty(person.LastName)) return BadRequest("LastName Can't be Empty.");

        return Ok(await _aPIService.AddModel(person));
    }

    [Authorize]
    [HttpPut("AddPersonLoginData")]
    public async Task<ActionResult<List<Person>>> AddLoginData(string uname, string pass, int id)
    {
        var person = await _aPIService.GetModelByID(id);

        if (string.IsNullOrEmpty(uname)) return BadRequest("Username Can't be Empty.");

        if (string.IsNullOrEmpty(pass)) return BadRequest("Password Can't be Empty.");

        person.UserName = uname;
        person.Password = pass;

        return Ok(await _aPIService.UpdateModel(person));
    }

    [Authorize]
    [HttpDelete("DeletePerson/{id}")]
    public async Task<ActionResult<List<Person>>> DeletePersonesAsync(int id)
    {
        return Ok(await _aPIService.DeleteModel(id));
    }

    [Authorize]
    [HttpPut("UpdatePersonData")]
    public async Task<ActionResult<List<Person>>> UpdateAsync(Person person)
    {
        if (person == null) return BadRequest("Can't find person data.");

        if (string.IsNullOrEmpty(person.FirstName)) return BadRequest("FirstName Can't be Empty.");

        if (string.IsNullOrEmpty(person.LastName)) return BadRequest("LastName Can't be Empty.");

        return Ok(await _aPIService.UpdateModel(person));
    }

    [Authorize]
    [HttpPut("AddPersonLogin")]
    public async Task<ActionResult<List<Person>>> AddPersonLoginAsync(int person,AuthenticateRequest request)
    {
        if (person == null) return BadRequest("Can't find person data.");

        if (string.IsNullOrEmpty(request.UserName)) return BadRequest("Username Can't be Empty.");

        if (string.IsNullOrEmpty(request.Password)) return BadRequest("Password Can't be Empty.");

        return Ok(await _aPIService.AddLoginToPerson(person,request));
    }

    [HttpGet("Login")]
    public async Task<ActionResult<AuthenticateResponse>> GetPersonByLoginData(AuthenticateRequest logIn)
    {
        if (string.IsNullOrEmpty(logIn.UserName)) return BadRequest("Username can't be empty");

        if (string.IsNullOrEmpty(logIn.Password)) return BadRequest("Password can't be empty");

        var person = await _aPIService.GetPersonByLoginData(logIn);

        if (person != null)
        {
            return Ok(person);
        }

        return BadRequest("Username or Password is wrong try agin!");
    }
}