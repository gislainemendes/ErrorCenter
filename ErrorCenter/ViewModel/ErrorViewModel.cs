using System;

namespace Central_De_Erros.Models
{
    public class ErrorViewModel 
    {
        public int Id { get; set; }

        public string LogDescription { get; set; }

        public string IpOrigin { get; set; }

        public DateTime CreatedAt { get; set; }

        public Frequency Frequency { get; set; }

        public string Level { get; set; }
    }
}