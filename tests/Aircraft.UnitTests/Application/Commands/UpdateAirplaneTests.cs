using Aircraft.Api.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aircraft.UnitTests.Application.Commands
{
    public class UpdateAirplaneTests
    {
        [Fact]
        public void ToDomain_Should_Convert_Properties_Correctly()
        {
            var updateAirplane = new UpdateAirplane
            {
                Id = Guid.NewGuid(),
                Model = "B737-800",
                Weight = "41140 kg",
                Manufacturer = "Boeing"
            };

            var airplane = updateAirplane.ToDomain();

            Assert.Equal(updateAirplane.Id, airplane.Id);
            Assert.Equal(updateAirplane.Model, airplane.Model);
            Assert.Equal(updateAirplane.Weight, airplane.Weight);
            Assert.Equal(updateAirplane.Manufacturer, airplane.Manufacturer);
        }
    }
}
