using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatbotAPI.Models {
    [Table("question")]
    public class Question {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("categoryId")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Column("answerId")]
        [ForeignKey("Answer")]
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        [Column("text")]
        [StringLength(500)]
        public string Text { get; set; }
    }
}