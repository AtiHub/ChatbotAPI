using ChatbotAPI.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatbotAPI.Models {
    [Table("user")]
    public class User {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Column("firstname")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Column("lastname")]
        [StringLength(100)]
        public string LastName { get; set; }

        [JsonIgnore]
        [Column("passwordHash")]
        public byte[] PasswordHash { get; set; }

        [JsonIgnore]
        [Column("passwordSalt")]
        public byte[] PasswordSalt { get; set; }
    }
}
