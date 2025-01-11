﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VacationsManager.Data;

#nullable disable

namespace VacationsManager.Data.Migrations
{
    [DbContext(typeof(VacationsManagerDbContext))]
    partial class VacationsManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VacationsManager.Data.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleType")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3619),
                            Name = "CEO",
                            RoleType = 1,
                            UpdatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3620)
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3669),
                            Name = "Developer",
                            RoleType = 2,
                            UpdatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3669)
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3686),
                            Name = "TeamLead",
                            RoleType = 3,
                            UpdatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3686)
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3701),
                            Name = "Unassigned",
                            RoleType = 4,
                            UpdatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3701)
                        });
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("TeamLeaderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TeamLeaderId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("TeamId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3779),
                            FirstName = "Admin",
                            LastName = "User",
                            Password = "immQRxaPfG/2bE5N/IWPlLTDkO98GxjyOv1zuHdtPPOgmec6o30kiAqlp+XaeXOR",
                            RoleId = 1,
                            UpdatedAt = new DateTime(2025, 1, 11, 13, 22, 25, 489, DateTimeKind.Utc).AddTicks(3780),
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.VacationRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Attachment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHalfDay")
                        .HasColumnType("bit");

                    b.Property<int>("RequesterId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("VacationType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequesterId");

                    b.ToTable("VacationRequests");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.Notification", b =>
                {
                    b.HasOne("VacationsManager.Data.Entities.User", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.Team", b =>
                {
                    b.HasOne("VacationsManager.Data.Entities.Project", "Project")
                        .WithMany("Teams")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VacationsManager.Data.Entities.User", "TeamLeader")
                        .WithMany()
                        .HasForeignKey("TeamLeaderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("TeamLeader");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.User", b =>
                {
                    b.HasOne("VacationsManager.Data.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VacationsManager.Data.Entities.Team", "Team")
                        .WithMany("Developers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Role");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.VacationRequest", b =>
                {
                    b.HasOne("VacationsManager.Data.Entities.User", "Requester")
                        .WithMany("VacationRequests")
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Requester");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.Project", b =>
                {
                    b.Navigation("Teams");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.Team", b =>
                {
                    b.Navigation("Developers");
                });

            modelBuilder.Entity("VacationsManager.Data.Entities.User", b =>
                {
                    b.Navigation("VacationRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
