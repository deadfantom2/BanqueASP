using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BanqueASP.Controllers
{
    public class EditProfileData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [Produces("application/json")]
    [Route("api/users")]
    public class UsersController : Controller
    {
        readonly ApiContext context;

        public UsersController(ApiContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var user = context.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult Get()
        {
            return Ok(GetSecureUser());
        }

        [Authorize]
        [HttpPost("me")]
        public ActionResult Post([FromBody] EditProfileData profileData)
        {
            var user = GetSecureUser();

            user.FirstName = profileData.FirstName ?? user.FirstName;
            user.LastName = profileData.LastName ?? user.LastName;

            context.SaveChanges();

            return Ok(user);
        }

        Models.User GetSecureUser()
        {
            var id = HttpContext.User.Claims.First().Value;
            return context.Users.SingleOrDefault(u => u.Id == id);
        }
    }
}