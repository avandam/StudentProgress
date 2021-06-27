using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace StudentProgress.Core.Entities
{
    public class StudentStatus : AuditableEntity<int>
    {
        public Student Student { get; private set; }
        public StudentGroup Group { get; private set; }
        public int GroupId { get; private set; }
        public int StudentId { get; private set; }
        public StatusInGroup StatusInGroup { get; private set; }
        public bool WantsToSpeakToTeacher { get; private set; }

#nullable disable
        private StudentStatus() { }
#nullable enable

        public StudentStatus(Student student, StudentGroup group, StatusInGroup statusInGroup, bool wantsToSpeakToTeacher)
        {
            Student = student ?? throw new NullReferenceException(nameof(student));
            Group = group ?? throw new NullReferenceException(nameof(group));
            StatusInGroup = statusInGroup;
            GroupId = group.Id;
            StudentId = student.Id;
            WantsToSpeakToTeacher = wantsToSpeakToTeacher;
        }

        public Result Update(StatusInGroup statusInGroup, bool wantsToSpeakToTeacher)
        {
            StatusInGroup = statusInGroup;
            WantsToSpeakToTeacher = wantsToSpeakToTeacher;

            return Result.Success();
        }

        public Result Update(bool wantsToSpeakToTeacher)
        {
            WantsToSpeakToTeacher = wantsToSpeakToTeacher;

            return Result.Success();
        }
    }
}
