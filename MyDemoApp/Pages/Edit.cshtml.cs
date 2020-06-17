using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FrontEnd.Pages
{

    public class EditModel : PageModel
    {
        [BindProperty]
        public UserDTO User { get; set; }

        public string Title { get; set; }

        public async Task OnGet()
        {
            this.Title = "Add User";

            if (Request.Query["Id"].Any())
            {
                string id = Request.Query["Id"][0];

                this.Title = "Edit User";

                // get user
                using (var client = new System.Net.Http.HttpClient())
                {
                    var request = new System.Net.Http.HttpRequestMessage();
                    request.RequestUri = new Uri("http://localhost:62610/usersapi/User/" + id);

                    var response = client.SendAsync(request).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var model = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());

                        this.User = new UserDTO();
                        this.User.Id = model.Id;
                        this.User.Name = model.Name;
                        this.User.Surname = model.Surname;
                        this.User.Email = model.Email;
                        this.User.BirthDate = model.BirthDate;
                    }   
                }
            }

        }

        public async Task<IActionResult> OnPost(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), 
                        Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;

                    if ( (!user.Id.HasValue) || user.Id == Guid.Empty) // new record
                        response =  await client.PostAsync("http://localhost:62610/usersapi/User", content);
                    else
                         response = await client.PutAsync("http://localhost:62610/usersapi/User/" + user.Id, content);

                    if (response.IsSuccessStatusCode)
                        return Redirect("/");
                    else
                    {
                        ModelState.AddModelError("Error", response.Content.ReadAsStringAsync().Result);
                    }
                }
            }

            return Page();
        }
    }
}