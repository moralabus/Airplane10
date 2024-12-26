using Airplane10;
using System;

public class CargoAirplane : Airplane
{
    public int CargoCapacity { get; set; }
    public string CargoType { get; set; }

    // Конструктор для инициализации
    public CargoAirplane(string name, string model, int range, decimal fuelConsumption, DateTime manufactureDate, string foto, int cargoCapacity, string cargoType)
        : base(name, model, range, fuelConsumption, manufactureDate, foto)
    {
        CargoCapacity = cargoCapacity;
        CargoType = cargoType;
    }
}
