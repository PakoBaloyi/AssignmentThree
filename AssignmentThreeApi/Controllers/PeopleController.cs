using AssignmentThreeApi.Data;
using AssignmentThreeApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentThreeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        //adding dbContext
        private readonly PeopleDbContext _dbContext;
        public PeopleController(PeopleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetAccounts")]
        public async Task<ActionResult<IEnumerable<PersonalDetails>>> GetAccounts()
        {
            // checking if there are records in the database
            if (_dbContext == null)
            {
                return NotFound();
            }
            return await _dbContext.PersonalDetailsTable.ToListAsync();
        }
        [HttpGet("{id}")]
       // [Route("GetAccount")]

        //retrieve a specific personal detail by its ID.
        public async Task<ActionResult<PersonalDetails>> GetAccount(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }
            var account = await _dbContext.PersonalDetailsTable.FindAsync(id);
            //checking if the account ID is present
            if (account == null)
            {
                return NotFound();
            }
            return account;
        }

        [HttpPost]
       [Route("AddAccount")]
        public async Task<ActionResult<PersonalDetails>>AddAccount(PersonalDetails details)
        {
            _dbContext.PersonalDetailsTable.Add(details);
             await _dbContext.SaveChangesAsync();
            // returning single account with the ID
            return CreatedAtAction(nameof(GetAccount), new {id= details.ID}, details);
        }
    }
}