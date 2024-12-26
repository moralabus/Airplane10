using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

public class Airplane
{
    public string Name { get; set; }
    public string Model { get; set; }
    public int Range { get; set; }
    public decimal FuelConsumption { get; set; }
    public DateTime ManufactureDate { get; set; }
    public string Foto { get; set; }
    public Guid ID { get; set; } // Уникальный идентификатор для каждого самолета

    // Конструктор для создания нового самолета
    public Airplane(string name, string model, int range, decimal fuelConsumption, DateTime manufactureDate, string foto)
    {
        ID = Guid.NewGuid(); // Генерация уникального идентификатора
        Name = name;
        Model = model;
        Range = range;
        FuelConsumption = fuelConsumption;
        ManufactureDate = manufactureDate;
        Foto = foto;
    }

    // Метод для получения информации о самолете
    public string GetInfo()
    {
        return $"ID: {ID}\nИмя: {Name}\nМодель: {Model}\nДиапазон: {Range} км\nРасход топлива: {FuelConsumption} л/ч\nДата производства: {ManufactureDate.ToShortDateString()}";
    }

    // Пример метода для чтения самолетов из файла
    public static List<Airplane> ReadFromFile(string fileName)
    {
        List<Airplane> airplanes = new List<Airplane>();
        try
        {
            foreach (var line in File.ReadLines(fileName))
            {
                var parts = line.Split(',');

                if (parts.Length >= 6)
                {
                    Guid id = Guid.Parse(parts[0]);
                    string name = parts[1];
                    string model = parts[2];
                    int range = int.Parse(parts[3]);
                    decimal fuelConsumption = decimal.Parse(parts[4]);
                    DateTime manufactureDate = DateTime.Parse(parts[5]);
                    string foto = parts.Length > 6 ? parts[6] : null;

                    var airplane = new Airplane(name, model, range, fuelConsumption, manufactureDate, foto)
                    {
                        ID = id // Устанавливаем ID из файла
                    };
                    airplanes.Add(airplane);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при чтении файла: {ex.Message}");
        }

        return airplanes;
    }

    // Событие для добавления нового самолета
    public static event EventHandler AirplaneAdded;

    protected virtual void OnAirplaneAdded()
    {
        AirplaneAdded?.Invoke(this, EventArgs.Empty);
    }
}
