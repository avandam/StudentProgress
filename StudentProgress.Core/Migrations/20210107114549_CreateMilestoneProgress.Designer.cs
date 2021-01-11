﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StudentProgress.Core.Entities;

namespace StudentProgress.Core.Migrations
{
    [DbContext(typeof(ProgressContext))]
    [Migration("20210107114549_CreateMilestoneProgress")]
    partial class CreateMilestoneProgress
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("StudentProgress.Core.Entities.Milestone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Artefact")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LearningOutcome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StudentGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("StudentGroupId");

                    b.ToTable("Milestone");
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.MilestoneProgress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MilestoneId")
                        .HasColumnType("integer");

                    b.Property<int?>("ProgressUpdateId")
                        .HasColumnType("integer");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("MilestoneId");

                    b.HasIndex("ProgressUpdateId");

                    b.ToTable("MilestoneProgress");
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.ProgressUpdate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Feedback")
                        .HasColumnType("text");

                    b.Property<string>("Feedforward")
                        .HasColumnType("text");

                    b.Property<string>("Feedup")
                        .HasColumnType("text");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<int>("ProgressFeeling")
                        .HasColumnType("integer");

                    b.Property<int>("StudentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("StudentId");

                    b.ToTable("ProgressUpdate");
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.StudentGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Mnemonic")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("StudentGroup");
                });

            modelBuilder.Entity("StudentStudentGroup", b =>
                {
                    b.Property<int>("StudentGroupsId")
                        .HasColumnType("integer");

                    b.Property<int>("StudentsId")
                        .HasColumnType("integer");

                    b.HasKey("StudentGroupsId", "StudentsId");

                    b.HasIndex("StudentsId");

                    b.ToTable("StudentStudentGroup");
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.Milestone", b =>
                {
                    b.HasOne("StudentProgress.Core.Entities.StudentGroup", "StudentGroup")
                        .WithMany("Milestones")
                        .HasForeignKey("StudentGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StudentGroup");
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.MilestoneProgress", b =>
                {
                    b.HasOne("StudentProgress.Core.Entities.Milestone", "Milestone")
                        .WithMany()
                        .HasForeignKey("MilestoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentProgress.Core.Entities.ProgressUpdate", null)
                        .WithMany("MilestonesProgress")
                        .HasForeignKey("ProgressUpdateId");

                    b.Navigation("Milestone");
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.ProgressUpdate", b =>
                {
                    b.HasOne("StudentProgress.Core.Entities.StudentGroup", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentProgress.Core.Entities.Student", "Student")
                        .WithMany("ProgressUpdates")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentStudentGroup", b =>
                {
                    b.HasOne("StudentProgress.Core.Entities.StudentGroup", null)
                        .WithMany()
                        .HasForeignKey("StudentGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentProgress.Core.Entities.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.ProgressUpdate", b =>
                {
                    b.Navigation("MilestonesProgress");
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.Student", b =>
                {
                    b.Navigation("ProgressUpdates");
                });

            modelBuilder.Entity("StudentProgress.Core.Entities.StudentGroup", b =>
                {
                    b.Navigation("Milestones");
                });
#pragma warning restore 612, 618
        }
    }
}