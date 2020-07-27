using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Central_De_Erros.ViewModel
{
    public class AuthMessageSenderOptions
    {

        public string SendGridUser { get; set; }

        public string SendGridKey { get; set; }

        public string FromFullName { get; set; }

        public string FromEmail { get; set; }
    }
}
