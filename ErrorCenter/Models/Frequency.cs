using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Central_De_Erros.Models
{
    [Table("frequency")]
    public class Frequency
    {

        [Key]
        [Column("log_description")]
        public string LogDescription { get; set; }

        [Column("number_of_events")]
        [Required]
        public int NumberOfEvents { get; set; }

    }
}
