using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Central_De_Erros.ViewModel
{
    public class UserViewCreated
    {
        public string Id { get; set; }
        
        public string UserName { get; set; }

        public string Email { get; set; }

    }
}
