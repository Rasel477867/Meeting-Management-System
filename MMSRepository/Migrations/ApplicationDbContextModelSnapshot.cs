﻿// <auto-generated />
using System;
using MMSRepository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MMSRepository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MMSCore.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DaysToRepeatedOn")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndRepeatedDate")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("Host")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RepetitionOption")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RequestedOn")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
