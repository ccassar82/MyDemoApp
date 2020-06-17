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
    public class DeleteModel : PageModel
    {
        public async Task<IActionResult> OnGet(Guid id)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var response = await client.DeleteAsync("http://localhost:62610/usersapi/User/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return Redirect("/");
                }
            }

            return Page();
        }
    }
}