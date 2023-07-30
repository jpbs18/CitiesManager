﻿// <auto-generated />
using System;
using CitiesManager.Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CitiesManager.WebAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230723084136_CityNameRequired")]
    partial class CityNameRequired
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CitiesManager.WebAPI.Models.City", b =>
                {
                    b.Property<Guid>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CityId");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            CityId = new Guid("b94b2c7f-b6e3-4ca1-a804-e961f89b22a2"),
                            CityName = "Porto"
                        },
                        new
                        {
                            CityId = new Guid("06eb4eac-e753-4d41-adb3-e33de57abc96"),
                            CityName = "Lisbon"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}