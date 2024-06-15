using Aircraft.Domain.Aggregates.Airplane;
using System.ComponentModel.DataAnnotations;

namespace Aircraft.Api.Application.Commands
{
    public class UpdateAirplane
    {
        [Required]
        public Guid Id { get; set; }
        public string Model { get; set; }
        public string Weight { get; set; }
        public string Manufacturer { get; set; }

        public Airplane ToDomain()
        {
            return new Airplane(id: Id, model: Model, weight: Weight, manufacturer: Manufacturer);
        }

    }
}
