using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using proj2.Controllers;
using proj2.DTO;
using proj2.Models;
using proj2.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj2.tests
{
  public class EmployeeAttndensControllerrTests
    {
         [Fact]
        public async Task GetEmployeeAttndens_ValidId_ReturnsOkObjectResultWithEmployeeAttndensDTO()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<HRContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new HRContext(options))
            {
                context.EmployeesAttndens.Add(new EmployeeAttndens { id = 1, empID = 1, Date = DateOnly.FromDateTime(DateTime.Today) });

                context.SaveChanges();

                var controller = new EmployeeAttndensController(context);

                // Act
                var result = await controller.GetEmployeeAttndens(1);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<OkObjectResult>(result.Result);
            }
        }
    }
}
