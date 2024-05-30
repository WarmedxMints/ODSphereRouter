﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ODSphereRouter.DbContexts;

#nullable disable

namespace ODSphereRouter.Migrations
{
    [DbContext(typeof(SphereRouterDbContext))]
    [Migration("20240527032830_Spheres")]
    partial class Spheres
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("ODSphereRouter.DTOs.SphereDTO", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Systems")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("Spheres");
                });
#pragma warning restore 612, 618
        }
    }
}