﻿// <auto-generated />
using System;
using Gatam.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Gatam.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Gatam.Domain.ApplicationModule", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Modules");

                    b.HasData(
                        new
                        {
                            Id = "cf91760c-0153-4222-9e97-94f562e9f619",
                            Category = "SollicitatieTraining",
                            CreatedAt = new DateTime(2024, 11, 23, 16, 45, 56, 591, DateTimeKind.Utc).AddTicks(4515),
                            Title = "Solliciteren voor beginners"
                        });
                });

            modelBuilder.Entity("Gatam.Domain.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BegeleiderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RolesIds")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "96b82fa0-f8b4-4bbb-908d-345580008078",
                            Email = "admin@app.com",
                            IsActive = false,
                            Name = "admin",
                            PasswordHash = "AQAAAAIAAYagAAAAELzFIAUEjrRBCRR0uX3SDFLt7VFxT7vVw9Wziu5iog3Gnrf3LDZMqYcAoyUULR2log==",
                            PhoneNumber = "+32 9966554411",
                            Picture = "png",
                            RolesIds = "[\"rol_3BsJHRFokYkbjr5O\"]",
                            Surname = "Suradmin",
                            Username = "adminSuradmin"
                        },
                        new
                        {
                            Id = "502cb1a5-f321-4dba-8ee7-224f955d6e66",
                            Email = "john.doe@example.com",
                            IsActive = false,
                            Name = "JohnDoe",
                            PasswordHash = "AQAAAAIAAYagAAAAEONNkSH8zIbcPZPkbeTqZjTQovNY2yvvF+sl6oYr9enaHeADGQ8w3OpiUAL0VxVQjg==",
                            PhoneNumber = "+32 456789166",
                            Picture = "png",
                            RolesIds = "[\"rol_2SgoYL1AoK9tXYXW\"]",
                            Surname = "JOHNDOE",
                            Username = "JohnDoeJOHNDOE"
                        },
                        new
                        {
                            Id = "2e907a03-1af2-4b23-9fab-6a184bbd776c",
                            Email = "jane.doe@example.com",
                            IsActive = false,
                            Name = "JaneDoe",
                            PasswordHash = "AQAAAAIAAYagAAAAENRvzPJyHB5eD9NKp6W4s6bLlIm/Q0Mkm59+j/RfUz84Y7V6WX/S3IVD9PvFjUuqPA==",
                            PhoneNumber = "+32 568779633",
                            Picture = "png",
                            RolesIds = "[\"rol_tj8keS38380ZU4NR\"]",
                            Surname = "JANEDOE",
                            Username = "JaneDoeJANEDOE"
                        },
                        new
                        {
                            Id = "db869a62-e9a8-454c-ae7d-9dd4a313f492",
                            Email = "lautje.doe@example.com",
                            IsActive = false,
                            Name = "Lautje",
                            PasswordHash = "AQAAAAIAAYagAAAAEJMklvEYOOXWKvyNwK8Zq6r/AUDIgNeOr/Rgw0Ljv2NjrwOrBa3EcgsBGIx4DLXWmQ==",
                            PhoneNumber = "+23 7896544336",
                            Picture = "png",
                            RolesIds = "[\"rol_tj8keS38380ZU4NR\"]",
                            Surname = "LAUTJE",
                            Username = "LautjeLAUTJE"
                        });
                });

            modelBuilder.Entity("Gatam.Domain.Question", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ApplicationModuleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastUpdatedUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionTitle")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<short>("QuestionType")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationModuleId");

                    b.ToTable("Questions");

                    b.HasData(
                        new
                        {
                            Id = "31a796ad-84fa-4159-b683-e6cc4c53142a",
                            CreatedAt = new DateTime(2024, 11, 23, 16, 45, 56, 592, DateTimeKind.Utc).AddTicks(2040),
                            CreatedUserId = "123",
                            LastUpdatedAt = new DateTime(2024, 11, 23, 16, 45, 56, 592, DateTimeKind.Utc).AddTicks(2042),
                            LastUpdatedUserId = "123",
                            QuestionTitle = "Wat wil je later bereiken? ",
                            QuestionType = (short)1
                        });
                });

            modelBuilder.Entity("Gatam.Domain.QuestionAnswer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AnswerValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");

                    b.HasData(
                        new
                        {
                            Id = "1e042bcc-0e5a-463a-af54-2ed62067429c",
                            Answer = "OPEN",
                            QuestionId = "31a796ad-84fa-4159-b683-e6cc4c53142a"
                        });
                });

            modelBuilder.Entity("Gatam.Domain.UserModule", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ModuleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserModule");
                });

            modelBuilder.Entity("Gatam.Domain.Question", b =>
                {
                    b.HasOne("Gatam.Domain.ApplicationModule", "ApplicationModule")
                        .WithMany("Questions")
                        .HasForeignKey("ApplicationModuleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ApplicationModule");
                });

            modelBuilder.Entity("Gatam.Domain.QuestionAnswer", b =>
                {
                    b.HasOne("Gatam.Domain.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Gatam.Domain.UserModule", b =>
                {
                    b.HasOne("Gatam.Domain.ApplicationModule", "Module")
                        .WithMany("UserModules")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gatam.Domain.ApplicationUser", "User")
                        .WithMany("UserModules")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Gatam.Domain.ApplicationModule", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("UserModules");
                });

            modelBuilder.Entity("Gatam.Domain.ApplicationUser", b =>
                {
                    b.Navigation("UserModules");
                });

            modelBuilder.Entity("Gatam.Domain.Question", b =>
                {
                    b.Navigation("Answers");
                });
#pragma warning restore 612, 618
        }
    }
}
