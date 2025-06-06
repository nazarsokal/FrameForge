﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(FrameForgeDbContext))]
    [Migration("20250515161410_GroupModelAdded")]
    partial class GroupModelAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Entities.EnrolledLevels", b =>
                {
                    b.Property<Guid>("LevelsEnrolledKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("LevelTopicName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<double>("MoneyReward")
                        .HasColumnType("double");

                    b.Property<int>("StarsReward")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("char(36)");

                    b.HasKey("LevelsEnrolledKey");

                    b.ToTable("EnrolledLevels", (string)null);
                });

            modelBuilder.Entity("Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("TeacherId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("Groups", (string)null);
                });

            modelBuilder.Entity("Entities.User", b =>
                {
                    b.Property<Guid>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("GoogleId")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Picture")
                        .HasColumnType("longtext");

                    b.Property<string>("TypeOfUser")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("varchar(8)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("StudentId");

                    b.ToTable("Users", (string)null);

                    b.HasDiscriminator<string>("TypeOfUser").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Entities.Student", b =>
                {
                    b.HasBaseType("Entities.User");

                    b.Property<Guid?>("GroupId")
                        .HasColumnType("char(36)");

                    b.Property<double>("MoneyAmount")
                        .HasColumnType("double");

                    b.Property<int>("StarsAmount")
                        .HasColumnType("int");

                    b.HasIndex("GroupId");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("Entities.Teacher", b =>
                {
                    b.HasBaseType("Entities.User");

                    b.HasDiscriminator().HasValue("Teacher");
                });

            modelBuilder.Entity("Entities.Group", b =>
                {
                    b.HasOne("Entities.Teacher", "Teacher")
                        .WithMany("Groups")
                        .HasForeignKey("TeacherId");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Entities.Student", b =>
                {
                    b.HasOne("Entities.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Entities.Group", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("Entities.Teacher", b =>
                {
                    b.Navigation("Groups");
                });
#pragma warning restore 612, 618
        }
    }
}
