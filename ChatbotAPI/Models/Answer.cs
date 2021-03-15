using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatbotAPI.Models {
    [Table("answer")]
    public class Answer {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        [StringLength(300)]
        public string Text { get; set; }
    }
}