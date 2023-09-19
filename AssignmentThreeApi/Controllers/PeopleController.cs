using AssignmentThreeApi.Data;
using AssignmentThreeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssignmentThreeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleDbContext _dbContext;
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(PeopleDbContext dbContext, ILogger<PeopleController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Adds a new account.
        /// </summary>
        [HttpPost("AddAccount")]
        public async Task<ActionResult<PersonalDetails>> AddAccount([FromBody] PersonalDetails details)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state");
                return BadRequest(ModelState);
            }

            try
            {
                AddPersonalDetail(details);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding account: {ex}");
                return StatusCode(500, "Internal server error");
            }

            return CreatedAtAction(nameof(GetAccount), new { id = details.ID }, details);
        }

        /// <summary>
        /// Adds multiple accounts.
        /// </summary>
        [HttpPost("AddAccounts")]
        public async Task<ActionResult<IEnumerable<PersonalDetails>>> AddAccounts([FromBody] List<PersonalDetails> accountList)
        {
            foreach (var details in accountList)
            {
                AddPersonalDetail(details);
            }

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccounts), accountList);
        }

        private void AddPersonalDetail(PersonalDetails details)
        {
            _dbContext.PersonalDetailsTable.Add(details);
        }

        /// <summary>
        /// Retrieves all accounts.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonalDetails>>> GetAccounts()
        {
            return await _dbContext.PersonalDetailsTable.ToListAsync();
        }

        /// <summary>
        /// Retrieves an account by its ID.
        /// </summary>
        [HttpGet("GetAccounts")]
        public async Task<ActionResult<PersonalDetails>> GetAccount(int id)
        {
            var account = await _dbContext.PersonalDetailsTable.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        /// <summary>
        /// Updates an account.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, [FromBody] PersonalDetails updatedDetails)
        {
            if (id != updatedDetails.ID)
            {
                return BadRequest();
            }

            _dbContext.Update(updatedDetails);

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
            return _dbContext.PersonalDetailsTable.AnyAsync(e => e.ID == id).Result;
        }

        /// <summary>
        /// Deletes an account.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _dbContext.PersonalDetailsTable.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _dbContext.PersonalDetailsTable.Remove(account);
            await _dbContext.SaveChangesAsync();

            return Ok(account);
        }
    }
}
