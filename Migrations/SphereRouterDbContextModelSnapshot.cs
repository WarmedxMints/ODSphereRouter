﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ODSphereRouter.DbContexts;

#nullable disable

namespace ODSphereRouter.Migrations
{
    [DbContext(typeof(SphereRouterDbContext))]
    partial class SphereRouterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("ODSphereRouter.DTOs.RouteListDTO", b =>
                {
                    b.Property<long>("Address")
                        .HasColumnType("INTEGER");

                    b.Property<string>("System")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Address");

                    b.ToTable("RouteList", (string)null);
                });

            modelBuilder.Entity("ODSphereRouter.DTOs.SphereDTO", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Systems")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.ToTable("Spheres", (string)null);
                });

            modelBuilder.Entity("ODSphereRouter.DTOs.StarSystemDTO", b =>
                {
                    b.Property<long>("Address")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<long>("Population")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PowerState")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Powers")
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.Property<double>("Z")
                        .HasColumnType("REAL");

                    b.HasKey("Address");

                    b.ToTable("StarSystems", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}