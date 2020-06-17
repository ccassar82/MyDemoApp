using Microsoft.EntityFrameworkCore;

namespace UsersAPI
{
    /// <summary>
    /// This is the class for the DB Context used
    /// </summary>
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// This method is the main property to get/set users in memory
        /// </summary>
        /// <param name="userDTO">UserDTO object to be created</param>
        /// <returns></returns>
        public DbSet<UserDTO> Users { get; set; }
    }
}