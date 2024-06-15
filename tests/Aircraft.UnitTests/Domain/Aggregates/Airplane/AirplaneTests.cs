namespace Aircraft.UnitTests.Domain.Aggregates.Airplane
{
    public class AirplaneTests
    {
        [Fact]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            var id = Guid.NewGuid();
            var model = "B737-800";
            var weight = "41140 kg";
            var manufacturer = "Boeing";

            var airplane = new Aircraft.Domain.Aggregates.Airplane.Airplane(id, model, weight, manufacturer);

            Assert.Equal(id, airplane.Id);
            Assert.Equal(model, airplane.Model);
            Assert.Equal(weight, airplane.Weight);
            Assert.Equal(manufacturer, airplane.Manufacturer);
        }
    }
}
