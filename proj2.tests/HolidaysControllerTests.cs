using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj2.Controllers;
using proj2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace proj2.tests
{
    public class HolidaysControllerTests
    {
        private DbContextOptions<HRContext> _options;
        private HRContext _context;

        public HolidaysControllerTests()
        {
            _options = new DbContextOptionsBuilder<HRContext>()
                .UseInMemoryDatabase(databaseName: "Hrproj3")
                .Options;
            _context = new HRContext(_options);
            SeedTestData();
        }

        private void SeedTestData()
        {
            var holidays = new List<Holiday>
            {
                new() { Id = 1, Name = "New Year", Date = new DateOnly(2024, 1, 1) },
                new Holiday { Id = 2, Name = "Christmas", Date = new DateOnly(2024, 12, 25) },
            };

            _context.AddRange(holidays);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetHolidays_ReturnsAllHolidays()
        {
            // Arrange
            var controller = new HolidaysController(_context);

            // Act
            var result = await controller.GetHolidays();

            // Assert
            var holidays = Assert.IsType<ActionResult<IEnumerable<Holiday>>>(result);
            Assert.Equal(2, holidays.Value.Count());
        }

    }
}
