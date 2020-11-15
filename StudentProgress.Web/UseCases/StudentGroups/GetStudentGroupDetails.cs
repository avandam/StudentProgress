﻿using Microsoft.EntityFrameworkCore;
using StudentProgress.Web.Data;
using StudentProgress.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProgress.Web.UseCases.StudentGroups
{
    public class GetStudentGroupDetails
    {
        private ProgressContext context;

        public GetStudentGroupDetails(ProgressContext context)
        {
            this.context = context;
        }

        public record Request(int Id);

        public record Response
        {
            public int Id { get; init; }
            public string Name { get; init; }
            public DateTime CreatedAt { get; init; }
            public DateTime UpdatedAt { get; init; }
            public IEnumerable<StudentsResponse> Students { get; init; }

            public record StudentsResponse
            {
                public int Id { get; }
                public string Name { get; }
                [Display(Name = "Amount of feedback")]
                public int AmountOfProgressItems { get; }
                [Display(Name = "Latest Feeling")]
                public Feeling? FeelingOfLatestProgress { get; }

                public StudentsResponse(int id, string name, int amountOfProgressItems, Feeling? feelingOfLatestProgress)
                {
                    Id = id;
                    Name = name;
                    AmountOfProgressItems = amountOfProgressItems;
                    FeelingOfLatestProgress = feelingOfLatestProgress;
                }
            }
        }

        public async Task<Response> HandleAsync(Request request)
        {
            var studentGroup = await context.StudentGroup.FirstOrDefaultAsync(g => g.Id == request.Id);

            if (studentGroup == null)
            {
                return null;
            }

            var students = context.Student.Include(_ => _.ProgressUpdates)
                .Where(s => s.StudentGroups.Any(g => g.Id == request.Id))
                .Select(student => new Response.StudentsResponse(
                    student.Id,
                    student.Name,
                    student.ProgressUpdates.Count(p => p.Group.Id == request.Id),
                    student.ProgressUpdates.OrderByDescending(p => p.UpdatedDate).FirstOrDefault().ProgressFeeling
                    ));

            return new Response
            {
                Id = studentGroup.Id,
                Name = studentGroup.Name,
                CreatedAt = studentGroup.CreatedDate,
                UpdatedAt = studentGroup.UpdatedDate,
                Students = students
            };
        }
    }
}
