namespace Aircraft.Domain.Aggregates.Airplane
{
    public class Airplane
    {
        public Guid Id { get; private set; }
        public string Model { get; private set; }
        public string Weight { get; private set; }
        public string Manufacturer { get; private set; }

        public Airplane(Guid id, string model, string weight, string manufacturer)
        {
            Id = id;
            Model = model;
            Weight = weight;
            Manufacturer = manufacturer;
        }
    }
}
