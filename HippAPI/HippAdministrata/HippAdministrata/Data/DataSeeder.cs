using HippAdministrata.Models.Domains;
using HippAdministrata.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HippAdministrata.Data
{
    public static class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Ensure the database is created
                context.Database.EnsureCreated();

                if (context.Users.Any())
                    return; // Database already seeded

                // Seed Roles
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new Role { RoleName = "Admin" },
                        new Role { RoleName = "Employee" },
                        new Role { RoleName = "Manager" },
                        new Role { RoleName = "SalesPerson" },
                        new Role { RoleName = "Driver" },
                        new Role { RoleName = "Client" }
                    );
                    context.SaveChanges();
                }

                var roles = context.Roles.ToDictionary(r => r.RoleName, r => r.RoleId);

                // Seed Users and related entities
                var adminRoleId = roles["Admin"];
                context.Users.AddRange(
                    new User
                    {
                        Name = "Admin",
                        Email = "admin@gmail.com",
                        PasswordHash = HashPassword("Admin123!"),
                        RoleId = adminRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "Admin"
                    }
                    );

                // Add Users for Clients
                var clientRoleId = roles["Client"];
                context.Users.AddRange(
                    new User
                    {
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        PasswordHash = HashPassword("John123!"),
                        RoleId = clientRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "Client"
                    },
                    new User
                    {
                        Name = "Jane Smith",
                        Email = "jane.smith@example.com",
                        PasswordHash = HashPassword("Jane123!"),
                        RoleId = clientRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "Client"
                    }
                );
                context.SaveChanges();

                var clientUsers = context.Users.Where(u => u.RoleId == clientRoleId).ToList();
                context.Clients.AddRange(
                    new Client
                    {
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        Phone = "123-456-7890",
                        Address = "123 Main St",
                        UserId = clientUsers[0].UserId
                    },
                    new Client
                    {
                        Name = "Jane Smith",
                        Email = "jane.smith@example.com",
                        Phone = "987-654-3210",
                        Address = "456 Elm St",
                        UserId = clientUsers[1].UserId
                    }
                );

                // Add Users for SalesPersons
                var salesPersonRoleId = roles["SalesPerson"];
                context.Users.AddRange(
                    new User
                    {
                        Name = "Kevin",
                        Email = "kevin@example.com",
                        PasswordHash = HashPassword("Kevin123!"),
                        RoleId = salesPersonRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "SalesPerson"
                    },
                    new User
                    {
                        Name = "James",
                        Email = "james@example.com",
                        PasswordHash = HashPassword("James123!"),
                        RoleId = salesPersonRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "SalesPerson"
                    }
                );
                context.SaveChanges();

                var salesPersonUsers = context.Users.Where(u => u.RoleId == salesPersonRoleId).ToList();
                context.SalesPersons.AddRange(
                    new SalesPerson
                    {
                        Username = "Kevin",
                        Password = HashPassword("Kevin123!"),
                        Location = Location.Prishtina,
                        UserId = salesPersonUsers[0].UserId
                    },
                    new SalesPerson
                    {
                        Username = "James",
                        Password = HashPassword("James123!"),
                        Location = Location.Peja,
                        UserId = salesPersonUsers[1].UserId
                    }
                );

                // Add Users for Employees
                var employeeRoleId = roles["Employee"];
                context.Users.AddRange(
                    new User
                    {
                        Name = "Joe",
                        Email = "joe@example.com",
                        PasswordHash = HashPassword("Joe123!"),
                        RoleId = employeeRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "Employee"
                    },
                    new User
                    {
                        Name = "Michael",
                        Email = "michael@example.com",
                        PasswordHash = HashPassword("Michael123!"),
                        RoleId = employeeRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "Employee"
                    }
                );
                context.SaveChanges();

                var employeeUsers = context.Users.Where(u => u.RoleId == employeeRoleId).ToList();
                context.Employees.AddRange(
                    new Employee
                    {
                        Name = "Joe",
                        Password = HashPassword("Joe123!"),
                        UserId = employeeUsers[0].UserId
                    },
                    new Employee
                    {
                        Name = "Michael",
                        Password = HashPassword("Michael123!"),
                        UserId = employeeUsers[1].UserId
                    }
                );

                // Add Users for Drivers
                var driverRoleId = roles["Driver"];
                context.Users.AddRange(
                    new User
                    {
                        Name = "Brahim",
                        Email = "brahim@example.com",
                        PasswordHash = HashPassword("Brahim123!"),
                        RoleId = driverRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "Driver"
                    },
                    new User
                    {
                        Name = "Cristiano",
                        Email = "cristiano@example.com",
                        PasswordHash = HashPassword("Cristiano123!"),
                        RoleId = driverRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "Driver"
                    }
                );
                context.SaveChanges();

                var driverUsers = context.Users.Where(u => u.RoleId == driverRoleId).ToList();
                context.Drivers.AddRange(
                    new Driver
                    {
                        Name = "Brahim",
                        Password = HashPassword("Brahim123!"),
                        LicensePlate = "ABC123",
                        CarModel = "Toyota",
                        UserId = driverUsers[0].UserId
                    },
                    new Driver
                    {
                        Name = "Cristiano",
                        Password = HashPassword("Cristiano123!"),
                        LicensePlate = "DEF456",
                        CarModel = "Honda",
                        UserId = driverUsers[1].UserId
                    }
                );

                // Add Users for Managers
                var managerRoleId = roles["Manager"];
                context.Users.AddRange(
                    new User
                    {
                        Name = "Scoffield",
                        Email = "scoffield@example.com",
                        PasswordHash = HashPassword("Scoffield123!"),
                        RoleId = managerRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "Manager"
                    },
                    new User
                    {
                        Name = "Rodriguez",
                        Email = "rodriguez@example.com",
                        PasswordHash = HashPassword("Rodriguez123!"),
                        RoleId = managerRoleId,
                        CreatedAt = DateTime.UtcNow,
                        RoleName = "Manager"
                    }
                );
                context.SaveChanges();

                var managerUsers = context.Users.Where(u => u.RoleId == managerRoleId).ToList();
                context.Managers.AddRange(
                    new Manager
                    {
                        Name = "Scoffield",
                        Password = HashPassword("Scoffield123!"),
                        UserId = managerUsers[0].UserId
                    },
                    new Manager
                    {
                        Name = "Rodriguez",
                        Password = HashPassword("Rodriguez123!"),
                        UserId = managerUsers[1].UserId
                    }
                );

                // Save changes to the database
                context.SaveChanges();
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }


}
