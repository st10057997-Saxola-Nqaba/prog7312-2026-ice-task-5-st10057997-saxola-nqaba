using System.Globalization;

namespace ICE5_SensorLookup;

// STEP 1: Sensor class with DeviceId, Location and Temperature properties.
public class Sensor
{
    public string DeviceId { get; set; } = "";
    public string Location { get; set; } = "";
    public double Temperature { get; set; }

    public Sensor(string deviceId, string location, double temperature)
    {
        DeviceId = deviceId;
        Location = location;
        Temperature = temperature;
    }

    public override string ToString() =>
        $"DeviceId: {DeviceId} | Location: {Location} | Temperature: {Temperature:0.0} °C";
}

public class Program
{
    public static void Main()
    {
        // STEP 2: Dictionary<string, Sensor> using DeviceId as the key, with three sample sensors.
        // OrdinalIgnoreCase means "sns-001" and "SNS-001" are treated as the same key.
        var sensors = new Dictionary<string, Sensor>(StringComparer.OrdinalIgnoreCase);

        AddSensor(sensors, new Sensor("SNS-001", "Greenhouse A", 24.5));
        AddSensor(sensors, new Sensor("SNS-002", "Warehouse B", 18.2));
        AddSensor(sensors, new Sensor("SNS-003", "Server Room", 21.0));

        bool running = true;
        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("===== Smart-X Sensor Lookup =====");
            Console.WriteLine("1. Look up a sensor by DeviceId");
            Console.WriteLine("2. Update a sensor's temperature");
            Console.WriteLine("3. Display all sensors");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": LookUpSensor(sensors); break;
                case "2": UpdateTemperature(sensors); break;
                case "3": DisplayAll(sensors); break;
                case "4": running = false; break;
                default: Console.WriteLine("Invalid option. Please enter 1-4."); break;
            }
        }

        Console.WriteLine("Goodbye.");
    }

    private static void AddSensor(Dictionary<string, Sensor> sensors, Sensor sensor)
    {
        // TryAdd avoids an exception if the key already exists.
        if (!sensors.TryAdd(sensor.DeviceId, sensor))
            Console.WriteLine($"Sensor {sensor.DeviceId} already exists and was not added.");
    }

    // STEP 3: Ask for a DeviceId and use TryGetValue to show details or a "not found" message.
    private static void LookUpSensor(Dictionary<string, Sensor> sensors)
    {
        Console.Write("Enter DeviceId: ");
        string id = Console.ReadLine()?.Trim() ?? "";

        if (sensors.TryGetValue(id, out Sensor? sensor))
            Console.WriteLine($"Found -> {sensor}");
        else
            Console.WriteLine($"Sensor '{id}' was not found.");
    }

    // STEP 4: Update the temperature by DeviceId; unknown IDs and bad numbers are handled safely.
    private static void UpdateTemperature(Dictionary<string, Sensor> sensors)
    {
        Console.Write("Enter DeviceId to update: ");
        string id = Console.ReadLine()?.Trim() ?? "";

        if (!sensors.TryGetValue(id, out Sensor? sensor))
        {
            Console.WriteLine($"Sensor '{id}' was not found. No changes made.");
            return;
        }

        Console.Write($"Current temperature is {sensor.Temperature:0.0} °C. Enter new temperature: ");
        // Accept both "22.5" and "22,5" (South African regional settings use a comma).
        string input = (Console.ReadLine() ?? "").Trim().Replace(',', '.');

        if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double newTemp))
        {
            sensor.Temperature = newTemp;
            Console.WriteLine($"Updated -> {sensor}");
        }
        else
        {
            Console.WriteLine("That is not a valid number. No changes made.");
        }
    }

    // STEP 5: Display all registered sensors.
    private static void DisplayAll(Dictionary<string, Sensor> sensors)
    {
        Console.WriteLine($"--- Registered sensors ({sensors.Count}) ---");
        foreach (Sensor sensor in sensors.Values)
            Console.WriteLine(sensor);
    }
}