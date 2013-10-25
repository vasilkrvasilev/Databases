using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Models
{
    //[Table("Homework")]
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }

        //[Required]
        [MinLength(1000)]
        public string Content { get; set; }

        [Required]
        public DateTime SendDate { get; set; }

        //[ForeignKey("Student")]
        [Required]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        //[ForeignKey("Course")]
        [Required]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
