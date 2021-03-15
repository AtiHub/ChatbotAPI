using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatbotAPI.Models {
    [Table("category")]
    public class Category {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("text")]
        [StringLength(30)]
        public string Text { get; set; }
    }
}
