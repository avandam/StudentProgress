﻿using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StudentProgress.Core.Entities;
using StudentProgress.Core.UseCases;
using Xunit;

namespace StudentProgress.CoreTests.UseCases
{
    [Collection("db")]
    public class StudentRemoveFromGroupTests : DatabaseTests
    {
        public StudentRemoveFromGroupTests(DatabaseFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Removes_a_student_from_the_group()
        {
            var group = Fixture.DataMother.CreateGroup(
                name: "S2 DB04",
                studentNames: new[] {"Timo", "Patrick"},
                milestones: new []{ ("1. learning outcome 1", "first milestone")});
            var milestone = group.Milestones.FirstOrDefault();
            var patrick = group.Students.FirstOrDefault(s => s.Name == "Patrick");
            Fixture.DataMother.CreateProgressUpdate(
                group, 
                patrick,
                milestoneProgresses: new []
                {
                    new MilestoneProgress(Rating.Advanced, milestone, null)
                });
            await using var ucContext = Fixture.CreateDbContext();
            var uc = new StudentRemoveFromGroup(ucContext);

            var result = await uc.HandleAsync(new StudentRemoveFromGroup.Command
            {
                StudentId = patrick!.Id, 
                GroupId = group.Id
            });

            result.IsSuccess.Should().BeTrue();
            await using var assertContext = Fixture.CreateDbContext();
            (await assertContext.Students
                    .Include(s => s.StudentGroups)
                    .FirstOrDefaultAsync(s => s.Name == patrick.Name))
                .StudentGroups
                .Should().HaveCount(0);
        }
    }
}