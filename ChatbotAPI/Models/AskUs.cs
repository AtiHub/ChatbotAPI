using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatbotAPI.Models {
    [Table("askus")]
    public class AskUs {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Column("firstname")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Column("lastname")]
        [StringLength(100)]
        public string Lastname { get; set; }

        [Column("text")]
        [StringLength(1000)]
        public string Text { get; set; }

        [Column("answered")]
        public bool Answered { get; set; }
    }
}
