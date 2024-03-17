using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj2.Controllers;
using proj2.DTO;
using proj2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj2.tests
{
    public class EmployeesControllerTests
    {
        [Fact]
        public async Task PostEmployee_ValidEmployee_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<HRContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new HRContext(options))
            {
                var controller = new EmployeesController(context);
                var employee = new Employee
                {
                    FullName = "John Doe",
                    SSN = "123-45-6789",
                    Address = "123 Main St",
                    PhoneNumber = "555-1234",
                    Nationality = "American",
                    Sex = "Male",
                    Email = "john.doe@example.com",
                    BirthDate = new DateOnly(1990, 1, 1),
                    ContractDate = new DateOnly(2022, 1, 1),
                    Salary = 50000,
                    IsDeleted = false
                    // You can populate other properties as needed
                };

                // Act
                var result = await controller.PostEmployee(employee);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                var createdEmployee = Assert.IsType<Employee>(createdAtActionResult.Value);
                Assert.Equal("John Doe", createdEmployee.FullName);
                // Add more assertions as needed

                // Clean up
                context.Database.EnsureDeleted();
            }
        }
    }
}
