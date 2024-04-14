using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    [Table("UserCourse")]
    public class UserCourseEntity
    {
        public string UserId { get; set; }
        public UserEntity User { get; set; }

        public int CourseId { get; set; }
        public CourseEntity Course { get; set; }
    }
}
