﻿// <auto-generated />
using System;
using HippAdministrata.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HippAdministrata.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241219211131_AddingWarehouse")]
    partial class AddingWarehouse
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HippAdministrata.Models.Domains.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CarModel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicensePlate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("OrderId1")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("OrderStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId1");

                    b.HasIndex("UserId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SupervisorId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SupervisorId");

                    b.HasIndex("UserId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("DeliveryDestination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeeId1")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderHistoryId")
                        .HasColumnType("int");

                    b.Property<int?>("OrderHistoryId1")
                        .HasColumnType("int");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("SalesPersonId")
                        .HasColumnType("int");

                    b.Property<int?>("SalesPersonId1")
                        .HasColumnType("int");

                    b.Property<int?>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("DriverId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EmployeeId1");

                    b.HasIndex("OrderHistoryId1");

                    b.HasIndex("ProductId");

                    b.HasIndex("SalesPersonId");

                    b.HasIndex("SalesPersonId1");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("LabeledQuantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UnlabeledQuantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.SalesPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Location")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SalesPersons");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.CarDrivers", b =>
                {
                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<int?>("DriverId1")
                        .HasColumnType("int");

                    b.HasKey("DriverId", "CarId");

                    b.HasIndex("DriverId1");

                    b.ToTable("CarDrivers");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.EmployeeProductLabel", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("LabeledQuantity")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("EmployeeProductLabel");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.OrderHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("NewStatus")
                        .HasColumnType("int");

                    b.Property<int>("OldStatus")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("UpdatedByEmployeeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("UpdatedByEmployeeId");

                    b.ToTable("orderHistories");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.OrderProduct", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.SalesPersonClientProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("SalesPersonClientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SalesPersonClientId");

                    b.ToTable("SalesPersonClientProducts");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.SalesPersonClients", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryDestination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SalesPersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("SalesPersonId");

                    b.ToTable("SalesPersonClients");
                });

            modelBuilder.Entity("HippAdministrata.Models.Requests.ClientOrderRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryDestination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("SalesPersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("SalesPersonId");

                    b.ToTable("ClientOrderRequests");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Client", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Driver", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId1");

                    b.HasOne("HippAdministrata.Models.Domains.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Employee", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Manager", "Supervisor")
                        .WithMany()
                        .HasForeignKey("SupervisorId");

                    b.HasOne("HippAdministrata.Models.Domains.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supervisor");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Manager", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Order", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.Employee", null)
                        .WithMany("AssignedOrders")
                        .HasForeignKey("EmployeeId1");

                    b.HasOne("HippAdministrata.Models.JunctionTables.OrderHistory", "OrderHistory")
                        .WithMany()
                        .HasForeignKey("OrderHistoryId1");

                    b.HasOne("HippAdministrata.Models.Domains.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.SalesPerson", "SalesPerson")
                        .WithMany("Orders")
                        .HasForeignKey("SalesPersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.SalesPerson", null)
                        .WithMany("ManagedOrders")
                        .HasForeignKey("SalesPersonId1");

                    b.HasOne("HippAdministrata.Models.Domains.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId");

                    b.Navigation("Client");

                    b.Navigation("Driver");

                    b.Navigation("Employee");

                    b.Navigation("OrderHistory");

                    b.Navigation("Product");

                    b.Navigation("SalesPerson");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.SalesPerson", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.User", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.CarDrivers", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.Driver", null)
                        .WithMany("CarDrivers")
                        .HasForeignKey("DriverId1");

                    b.Navigation("Driver");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.EmployeeProductLabel", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Employee", "Employee")
                        .WithMany("EmployeeProductLabels")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.Product", "Product")
                        .WithMany("EmployeeProductLabels")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.OrderHistory", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.Employee", "UpdatedByEmployee")
                        .WithMany()
                        .HasForeignKey("UpdatedByEmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("UpdatedByEmployee");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.OrderProduct", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.SalesPersonClientProduct", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.JunctionTables.SalesPersonClients", "SalesPersonClient")
                        .WithMany("Products")
                        .HasForeignKey("SalesPersonClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("SalesPersonClient");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.SalesPersonClients", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Client", "Client")
                        .WithMany("SalesPersonsClients")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.SalesPerson", "SalesPerson")
                        .WithMany("SalesPersonsClients")
                        .HasForeignKey("SalesPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("SalesPerson");
                });

            modelBuilder.Entity("HippAdministrata.Models.Requests.ClientOrderRequest", b =>
                {
                    b.HasOne("HippAdministrata.Models.Domains.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HippAdministrata.Models.Domains.SalesPerson", "SalesPerson")
                        .WithMany()
                        .HasForeignKey("SalesPersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("SalesPerson");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Client", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("SalesPersonsClients");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Driver", b =>
                {
                    b.Navigation("CarDrivers");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Employee", b =>
                {
                    b.Navigation("AssignedOrders");

                    b.Navigation("EmployeeProductLabels");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Order", b =>
                {
                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Product", b =>
                {
                    b.Navigation("EmployeeProductLabels");

                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("HippAdministrata.Models.Domains.SalesPerson", b =>
                {
                    b.Navigation("ManagedOrders");

                    b.Navigation("Orders");

                    b.Navigation("SalesPersonsClients");
                });

            modelBuilder.Entity("HippAdministrata.Models.JunctionTables.SalesPersonClients", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
