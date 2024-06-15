using Aircraft.Domain.Aggregates.Airplane;

namespace Aircraft.Api.Application.Commands
{
    public class CreateAirplane
    {
        public string Model { get; set; }
        public string Weight { get; set; }
        public string Manufacturer { get; set; }

        public Airplane ToDomain()
        {
            return new Airplane(id: Guid.NewGuid(), model: Model, weight: Weight, manufacturer: Manufacturer);
        }

    }
}
