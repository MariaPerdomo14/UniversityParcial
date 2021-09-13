using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.BL.Models
{
    [Table("OfficeAssignment", Schema = "dbo")]
    public class officeAssignment
    {
        [Key]
        [ForeignKey("Instructor")]
        public int InstructorID { get; set; }
        public string Location { get; set; }

        //navs
        public Instructor Instructor { get; set; }
        public object Credits { get; set; }
        public object Title { get; set; }
        public object CourseID { get; set; }
    }
}