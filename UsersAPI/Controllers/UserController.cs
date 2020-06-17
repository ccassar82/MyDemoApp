using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using UsersAPI;

namespace UsersAPI.Controllers
{
    ///<Summary>
    /// This is the API controller that controls main user functionality
    ///</Summary>
    [Route("usersapi/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly MessageService _messageService;

        ///<Summary>
        /// This is the API contructor
        ///</Summary>
        /// <param name="context">This is the db context which is used to store users</param>
        /// <param name="messageService">This is MessageService instance to use for RabbitMQ</param>
        public UserController(ApiContext context, IMessageService messageService = null)
        {
            _context = context;

            if (messageService != null)
                _messageService = (MessageService)messageService;

        }

        ///<Summary>
        /// This is the API method that retrieves all users : GET: usersapi/User
        ///</Summary>
        /// <returns>Returns all Users</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        ///<Summary>
        /// This is the API method that retrieves a user : GET: api/User/5
        ///</Summary>
        /// <param name="id">User's Unique Identifier</param>
        /// <returns>Returns a UserDTO object</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserDTO(Guid id)
        {
            var userDTO = await _context.Users.FindAsync(id);

            if (userDTO == null)
            {
                return NotFound();
            }

            return userDTO;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// This method allows a user to be updated : PUT: usersapi/User/5
        /// </summary>
        /// <param name="id">User's Unique Identifier</param>
        /// <param name="userDTO">UserDTO object to be updated</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDTO(Guid id, UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDTOExists(id))
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// This method allows a user to be created : POST: usersapi/User
        /// After the user is created, a message is sent to RabbitMQ Queue.
        /// </summary>
        /// <param name="userDTO">UserDTO object to be created</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUserDTO(UserDTO userDTO)
        {
            _context.Users.Add(userDTO);
            await _context.SaveChangesAsync();

            // sends message to RabbitMQ
            _messageService.Enqueue("UserCreated");

            return CreatedAtAction("GetUserDTO", new { id = userDTO.Id }, userDTO);

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// This method allows a user to be deleted : DELETE: usersapi/User/5
        /// </summary>
        /// <param name="id">User's Unique Identifier</param>
        /// <returns>UserDTO object that has been deleted</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDTO>> DeleteUserDTO(Guid id)
        {
            var userDTO = await _context.Users.FindAsync(id);
            if (userDTO == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userDTO);
            await _context.SaveChangesAsync();

            return userDTO;
        }

        /// <summary>
        /// This method checks if a user already exists
        /// </summary>
        /// <param name="id">User's Unique Identifier</param>
        /// <returns>True if exists, False if not exists</returns>
        private bool UserDTOExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
