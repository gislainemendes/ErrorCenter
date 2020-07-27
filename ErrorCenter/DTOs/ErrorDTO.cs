using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Central_De_Erros.DTOs
{   
   
    public class ErrorDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string LogDescription { get; set; }
        [Required]
        public string Level { get; set; }
        [Required]
        public string Environment { get; set; }
        [Required]
        public string IpOrigin { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
