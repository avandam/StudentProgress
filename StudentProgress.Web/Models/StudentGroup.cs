﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProgress.Web.Models
{
    public class StudentGroup : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
