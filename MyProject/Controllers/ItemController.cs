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
    public class ItemController : ApiController
    {
        static List<Item> itemList = new List<Item> {
            new Item{ id= 0, name= "testDummy", price= 0, quantity= 0 }
        };
        // GET: searching by inputted name in URI
        public IHttpActionResult Get([FromUri] string name = "")
        {
            List<Item> filteredList = itemList.FindAll(i => i.name.Contains(name));
            if (filteredList.Count == 0)
            {
                return Content(HttpStatusCode.NotFound, "No item name contains the input");
            }
            return Content(HttpStatusCode.OK, filteredList);
        }
        // GET: api/Item/5
        public IHttpActionResult Get(int id)
        {
            Item item = itemList.Find(i => i.id == id);
            if (item == null)
            {
                return Content(HttpStatusCode.NotFound, "The item was not found");
            }
            return Content(HttpStatusCode.OK, item);
        }
        // POST: api/Item
        [ApiAuthentication]
        public IHttpActionResult Post([FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "All fields need to be inputted");
            }
            Item i = itemList.Find(i2 => i2.id == item.id);
            if (!(i == null))
            {
                return Content(HttpStatusCode.Conflict, "The ID already exists for another item");
            }
            itemList.Add(item);
            Item.increment();
            return Content(HttpStatusCode.Created, "Item successfully created and added");
        }

        // PUT: api/Item/5
        [ApiAuthentication]
        public IHttpActionResult Put(int id, [FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "All fields need to be inputted");
            }
            bool found = false;
            itemList.ForEach(
                i =>
                {
                    if (i.id == id)
                    {
                        i.name = item.name;
                        i.price = item.price;
                        i.quantity = item.quantity;
                        found = true;
                    }
                }
            );
            if (!found)
            {
                return Content(HttpStatusCode.NotFound, "No item matches the ID");
            }
            return Content(HttpStatusCode.OK, "Item data updated successfully");
        }

        // DELETE: api/Item/5
        [ApiAuthentication]
        public IHttpActionResult Delete(int id)
        {
            Item item = itemList.Find(i => i.id == id);
            if (item == null)
            {
                return Content(HttpStatusCode.NotFound, "No item matches the input");
            }
            itemList.Remove(item);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
