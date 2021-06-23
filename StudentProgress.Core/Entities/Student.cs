using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;

namespace StudentProgress.Core.Entities
{
    public class Student : Entity<int>
    {
        [Required]
        public string Name { get; private set; }
        public IEnumerable<ProgressUpdate> ProgressUpdates { get; private set; }
        public IEnumerable<StudentGroup> StudentGroups { get; private set; }
        private readonly List<StudentStatus> studentStatuses;
        public IEnumerable<StudentStatus> StudentStatuses => studentStatuses;

        public string? Note { get; private set; }

#nullable disable
        private Student() { }
        #nullable enable

        public Student(string name)
        {
            Name = name ?? throw new NullReferenceException(nameof(name));
            ProgressUpdates = new List<ProgressUpdate>();
            StudentGroups = new List<StudentGroup>();
            studentStatuses = new List<StudentStatus>();
        }

        public Result Update(string? name, string? note)
        {
            Name = name ?? Name;
            Note = note;
            return Result.Success();
        }

        public Result AddStudentStatus(StudentGroup group)
        {
            studentStatuses.Add(new StudentStatus(this, group, StatusInGroup.Active));
            return Result.Success();
        }
    }
}
