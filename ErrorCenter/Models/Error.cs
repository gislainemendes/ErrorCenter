using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Central_De_Erros.Models
{
    [Table("error")]
    public class Error
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }//tem um id o erro reportado

        [Column("user_id")]
        public string UserId { get; set; } // Erro tem um Usuario

        [ForeignKey("log_description")]
        public string LogDescription {get;set; }//tem um titulo vamos usar ele para identificar o tipo e a quantidade, não dá para usar enum pq são muitas opções;

        [Column("frequency")]
        [Required]
        public Frequency frequency { get; set; }


        [Column("level")]
        [StringLength(15)]
        [Required]
        public string Level {get; set; }//ele tem um level que é um Enum

        [Column("ip_origin")]
        [StringLength(100)]
        [Required]
        public string IpOrigin {get;set; }// tem uma origem  do log

        [Column("environment")]
        [StringLength(15)]
        [Required]
        public string Environment {get;set;}


        [Column("details")]
        [StringLength(255)]
        [Required]
        public string Details {get;set; }//erro tem detalhes

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("archived")]
        [Required]
        public bool Archived { get; set; }
    }
}
