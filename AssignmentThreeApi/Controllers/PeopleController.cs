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

       [HttpPost]
       [Route("AddAccount")]
        public async Task<ActionResult<PersonalDetails>>AddAccount(PersonalDetails details)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _dbContext.PersonalDetailsTable.Add(details);
             await _dbContext.SaveChangesAsync();
            // returning single account with the ID
            return CreatedAtAction(nameof(GetAccount), new {id= details.ID}, details);
        }
        // adding list accounts

        [HttpPost]
        [Route("AddAccounts")]
        public async Task<ActionResult<IEnumerable<PersonalDetails>>> AddAccounts(List<PersonalDetails> accountList)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            foreach (var details in accountList)
            {
                _dbContext.PersonalDetailsTable.Add(details);
            }

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccounts), accountList);
        }

        //getting the list of accounts
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
        //updating details

        [HttpPut("{id}")]
        //[Route("UpdateAccount")]

        public async Task<IActionResult> PutAccount(int id, PersonalDetails updatedDetails)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            if (id != updatedDetails.ID)
            {
                return BadRequest();
            }

            _dbContext.Entry(updatedDetails).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _dbContext.PersonalDetailsTable.Any(e => e.ID == id);
        }

        //deleting the user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if (_dbContext == null)
            {
                return NotFound();
            }

            var account = await _dbContext.PersonalDetailsTable.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _dbContext.PersonalDetailsTable.Remove(account);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}