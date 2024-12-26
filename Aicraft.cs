using System.Collections.Generic;
using System;

public abstract class Aircraft
{
    public string Name { get; set; }
    public string Model { get; set; }
    public int Range { get; set; }
    public decimal FuelConsumption { get; set; }
    public DateTime ManufactureDate { get; set; }
    public string Foto { get; set; }

    // Абстрактные методы
    public abstract string GetInfo();
    public abstract void WriteToFile(List<Aircraft> aircrafts, string filePath);
}
