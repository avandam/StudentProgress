﻿using System;
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

#nullable disable
        private StudentStatus() { }
#nullable enable

        public StudentStatus(Student student, StudentGroup group, StatusInGroup statusInGroup)
        {
            Student = student ?? throw new NullReferenceException(nameof(student));
            Group = group ?? throw new NullReferenceException(nameof(group));
            StatusInGroup = statusInGroup;
            GroupId = group.Id;
            StudentId = student.Id;
        }

        public Result Update(StatusInGroup statusInGroup)
        {
            StatusInGroup = statusInGroup;

            return Result.Success();
        }
    }
}