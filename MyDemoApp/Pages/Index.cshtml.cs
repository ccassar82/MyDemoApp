using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MyDemoApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<UserDTO> Users;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
                request.RequestUri = new Uri("http://localhost:62610/usersapi/User"); 

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var model = JsonConvert.DeserializeObject<List<UserDTO>>(await response.Content.ReadAsStringAsync());

                    this.Users = model;
                }
            }
        }
    }
}
