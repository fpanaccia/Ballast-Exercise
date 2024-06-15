using Aircraft.Api.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aircraft.UnitTests.Application.Commands
{
    public class CreateAirplaneTests
    {
        [Fact]
        public void ToDomain_Should_Convert_Properties_Correctly()
        {
            var createAirplane = new CreateAirplane
            {
                Model = "B737-800",
                Weight = "41140 kg",
                Manufacturer = "Boeing"
            };

            var airplane = createAirplane.ToDomain();

            Assert.NotEqual(Guid.Empty, airplane.Id);
            Assert.Equal(createAirplane.Model, airplane.Model);
            Assert.Equal(createAirplane.Weight, airplane.Weight);
            Assert.Equal(createAirplane.Manufacturer, airplane.Manufacturer);
        }
    }
}
