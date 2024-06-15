using FluentAssertions;
using Rental.Models;
using Rental.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental2._0.Tests
{
    public class PropertyServiceTests : IClassFixture<DbContextFixture>
    {
        private readonly PropertyService _propertyService;
        private readonly DbContextFixture _fixture;

        public PropertyServiceTests(DbContextFixture fixture)
        {
            _fixture = fixture;
            _propertyService = new PropertyService(_fixture.Context);
        }

        [Fact]
        public async Task Add_Property_Success()
        {
            RealEstate property = new RealEstate
            {
                Type = "Квартира", 
                Address = "Улица Пушкина", 
                Square = 25.0, 
                Price = 20000m, 
                Description = "Отличное описание", 
                OwnerID =  1, 
                Pledge = 10000m
            };
            var actual = await _propertyService.AddPropertyAsync(property);
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task Edit_Property_Success()
        {
            RealEstate property = new RealEstate
            {
                Type = "Квартира",
                Address = "Улица Пушкина",
                Square = 25.0,
                Price = 20000m,
                Description = "Отличное описание",
                OwnerID = 1,
                Pledge = 10000m
            };

            var actual = await _propertyService.EditAsync(property);
            actual.Should().BeTrue();
        }
    }
}
