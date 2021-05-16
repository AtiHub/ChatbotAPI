using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatbotAPI.Models {
    [Table("faq")]
    public class FAQ {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("questionId")]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
