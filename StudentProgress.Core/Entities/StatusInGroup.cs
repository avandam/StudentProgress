using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProgress.Core.Entities
{
    public enum StatusInGroup
    {
        Active = 1, // The student is still active in the course
        Training = 2, // The student is stopped, but still tries to learn things and hence active in the course without the goal to get a S/G/O this period, the grade will be U 
        Stopped = 3, // The student has stopped the semester, the grade will be U
        Inactive = 4, // The student is out of the picture, the grade will be U
        Graded = 5 // The student has been graded and finished the semester
    }
}
