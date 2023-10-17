using MyProject.Functions;
using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyProject.Controllers
{
    public class UserController : ApiController
    {
        static List<User> userList = new List<User> {
        new User{id=0, firstName="user0", lastName="user0", email="user.0@gmail.com", phoneNumber=11112222}
        };
        // GET: api/User
        public IHttpActionResult Get([FromUri] string firstName = "")
        {
            List<User> filteredUsers = userList.FindAll(u => u.firstName.Contains(firstName));
            if (filteredUsers.Count == 0)
            {
                return Content(HttpStatusCode.NotFound, "No user first name contains  his input");
            }
            return Content(HttpStatusCode.OK, filteredUsers);
        }
        // GET: api/User/5
        public IHttpActionResult Get(int id)
        {
            User user = userList.Find(u => u.id == id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "The user was not found");
            }
            return Content(HttpStatusCode.OK, user);
        }
        // POST: api/User
        public IHttpActionResult Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "All fields need to be inputted");
            }
            Tokens token = new Tokens();
            user.token = token.generateJwtToken((user.id.ToString()), user.email);
            userList.Add(user);
            user.increment();
            return Content(HttpStatusCode.Created, "User sucessfully created and added. Here is their token:" + user.token);
        }

        // PUT: api/User/5
        public IHttpActionResult Put(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "All fields need to be inputted");
            }
            bool found = false;
            userList.ForEach(
                u =>
                {
                    if (u.id == id)
                    {
                        u.firstName = user.firstName;
                        u.lastName = user.lastName;
                        u.email = user.email;
                        u.phoneNumber = user.phoneNumber;
                        found = true;
                    }
                }
            );
            if (!found)
            {
                return Content(HttpStatusCode.NotFound, "No user matches the ID");
            }
            return Content(HttpStatusCode.OK, "User data updated successfully");
        }

        // DELETE: api/User/5
        public IHttpActionResult Delete(int id)
        {
            User user = userList.Find(u => u.id == id);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "No user matches the input");
            }
            userList.Remove(user);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
