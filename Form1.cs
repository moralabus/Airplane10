using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Airplane10
{
    public partial class Form1 : Form
    {
        // Список для хранения информации о самолетах
        private List<Airplane> airplanes = new List<Airplane>();

        // Диалоговые окна для выбора шрифта и цвета
        private FontDialog fontDialog;
        private ColorDialog colorDialog;

        public Form1()
        {
            InitializeComponent();
            comboBoxModel.Items.AddRange(new string[] { "CargoAirplane", "PassengerAirplane" });
            comboBoxModel.SelectedIndex = 0; // Устанавливаем начальное значение комбобокса

            // Инициализация диалогов
            fontDialog = new FontDialog();
            colorDialog = new ColorDialog();

            // Подписка на событие AirplaneAdded (статическая подписка)
            Airplane.AirplaneAdded += Airplane_AirplaneAdded;

            // Динамическая подписка на событие
            Airplane.AirplaneAdded += DynamicHandler;
            Airplane.AirplaneAdded += AnotherHandler;
        }

        // Обработчик события AirplaneAdded (статическая подписка)
        private void Airplane_AirplaneAdded(object sender, EventArgs e)
        {
            if (sender is Airplane airplane)
            {
                // Отображение информации о добавленном самолете
                listBoxAirplanes.Items.Add($"{airplane.Name} (ID: {airplane.ID})");
                MessageBox.Show($"Самолет {airplane.Name} был добавлен.");
            }
        }

        // Динамический обработчик
        private void DynamicHandler(object sender, EventArgs e)
        {
            if (sender is Airplane airplane)
            {
                MessageBox.Show($"Динамическая обработка: Самолет {airplane.Name} добавлен!");
            }
        }

        // Дополнительный обработчик для демонстрации цепочки обработчиков
        private void AnotherHandler(object sender, EventArgs e)
        {
            if (sender is Airplane airplane)
            {
                MessageBox.Show($"Дополнительная обработка: Самолет {airplane.Name} добавлен!");
            }
        }

        // Обработчик для кнопки добавления самолета с фото
        private void buttonAddAirplaneWithFoto_Click(object sender, EventArgs e)
        {
            try
            {
                string name = textBoxAirplaneName.Text;
                string model = comboBoxModel.SelectedItem.ToString();
                int range = (int)numericUpDownRange.Value;
                decimal fuelConsumption = (decimal)numericUpDownFuelConsumption.Value;
                DateTime manufactureDate = dateTimePickerManufactureDate.Value;

                // Создание нового самолета
                Airplane newAirplane = CreateAirplane(model, name, range, fuelConsumption, manufactureDate);

                if (newAirplane != null)
                {
                    airplanes.Add(newAirplane);
                    listBoxAirplanes.Items.Add($"{newAirplane.Name} (ID: {newAirplane.ID})"); // Добавляем имя и ID в ListBox

                    // Очистка элементов управления после добавления
                    ClearInputs();
                    MessageBox.Show("Самолет добавлен.");
                }
                else
                {
                    MessageBox.Show("Неверный тип самолета.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении самолета: {ex.Message}");
            }
        }

        private Airplane CreateAirplane(string model, string name, int range, decimal fuelConsumption, DateTime manufactureDate)
        {
            switch (model)
            {
                case "CargoAirplane":
                    int cargoCapacity = (int)numericUpDownCargoCapacity.Value;
                    string cargoType = textBoxCargoType.Text;
                    return new CargoAirplane(name, model, range, fuelConsumption, manufactureDate, null, cargoCapacity, cargoType);

                case "PassengerAirplane":
                    int passengerCapacity = (int)numericUpDownPassengerCapacity.Value;
                    bool hasBusinessClass = checkBoxHasBusinessClass.Checked;
                    return new PassengerAirplane(name, model, range, fuelConsumption, manufactureDate, null, passengerCapacity, hasBusinessClass);

                default:
                    return null; // Неверный тип самолета
            }
        }

        private void ClearInputs()
        {
            textBoxAirplaneName.Clear();
            numericUpDownCargoCapacity.Value = 0;
            textBoxCargoType.Clear();
            numericUpDownPassengerCapacity.Value = 0;
            checkBoxHasBusinessClass.Checked = false;
        }

        // Метод для загрузки данных из файла
        private void buttonLoadFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                airplanes = Airplane.ReadFromFile(openFileDialog.FileName); // Чтение данных из файла
                listBoxAirplanes.Items.Clear();
                foreach (Airplane airplane in airplanes)
                {
                    listBoxAirplanes.Items.Add($"{airplane.Name} (ID: {airplane.ID})"); // Добавляем только имя и ID в ListBox
                }
                MessageBox.Show("Данные загружены из файла.");
            }
        }

        private void buttonShowAirplaneInfo_Click(object sender, EventArgs e)
        {
            if (listBoxAirplanes.SelectedItem != null)
            {
                // Извлекаем только имя самолета (если в ListBox хранится информация в формате "Имя ID" или что-то подобное)
                string selectedAirplaneName = listBoxAirplanes.SelectedItem.ToString().Split(' ')[0];

                // Ищем самолет в списке по имени
                Airplane selectedAirplane = airplanes.FirstOrDefault(a => a.Name.Equals(selectedAirplaneName, StringComparison.OrdinalIgnoreCase));

                if (selectedAirplane != null)
                {
                    // Очистить ListBox перед добавлением новой информации
                    listBoxAirplanes.Items.Clear();

                    // Добавляем информацию о выбранном самолете в ListBox
                    listBoxAirplanes.Items.Add(selectedAirplane.GetInfo());
                }
                else
                {
                    MessageBox.Show("Самолет не найден.");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите самолет.");
            }
        }



        // Смена видимости элементов управления при изменении модели самолета
        private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedModel = comboBoxModel.SelectedItem.ToString();
            ToggleVisibility(selectedModel);
        }

        private void ToggleVisibility(string selectedModel)
        {
            bool isCargo = selectedModel == "CargoAirplane";
            numericUpDownCargoCapacity.Visible = isCargo;
            textBoxCargoType.Visible = isCargo;
            textBoxCargoCapacityLabel.Visible = isCargo;
            textBoxCargoTypeLabel.Visible = isCargo;

            bool isPassengerOrBusiness = selectedModel == "PassengerAirplane";
            numericUpDownPassengerCapacity.Visible = isPassengerOrBusiness;
            checkBoxHasBusinessClass.Visible = isPassengerOrBusiness;
            textBoxPassengerCapacityLabel.Visible = isPassengerOrBusiness;
        }

        // Кнопка для выбора шрифта
        private void buttonChooseFont_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                listBoxAirplanes.Font = fontDialog.Font;
            }
        }

        // Кнопка для выбора цвета текста
        private void buttonChooseColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                listBoxAirplanes.ForeColor = colorDialog.Color;
            }
        }
    }
}
