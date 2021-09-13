using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.BL.DTOs
{
    public class EnrollmentDTO
    {
        [Required()]
        public int EnrollmentID { get; set; }

       //allí ya sigues validando los creas necesario...Ahí ya están los 7  DTO ya los puedes usar en el controller ahora si 
       // toca crear todo Create Edit, index y Delete sip  a cual ?
        public int CourseID { get; set; }

       
        public int StudentID { get; set; }

        ////navs
        public CourseDTO Course { get; set; }
        public StudentDTO Student { get; set; }
    }
}
