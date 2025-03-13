using System.Device.Gpio;
using System.Device.I2c;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.ReadResult;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System.Text;

namespace CheeseCaveDotnet;

class Device
{
    private static readonly int s_pin = 21; // GPIO pin number
    private static GpioController s_gpio; // GPIO controller instance
    private static I2cDevice s_i2cDevice; // I2C device instance
    private static Bme280 s_bme280; // BME280 sensor instance

    const double DesiredTempLimit = 5; // Acceptable range above or below the desired temp, in degrees F.
    const double DesiredHumidityLimit = 10; // Acceptable range above or below the desired humidity, in percentages.
    const int IntervalInMilliseconds = 5000; // Interval at which telemetry is sent to the cloud.

    private static DeviceClient s_deviceClient; // Azure IoT device client
    private static stateEnum s_fanState = stateEnum.off; // Initial fan state

    private static readonly string s_deviceConnectionString = "YOUR DEVICE CONNECTION STRING HERE"; // Device connection string

    enum stateEnum
    {
        off, // Fan is off
        on, // Fan is on
        failed // Fan has failed
    }

    private static void Main(string[] args)
    {
        s_gpio = new GpioController(); // Initialize GPIO controller
        s_gpio.OpenPin(s_pin, PinMode.Output); // Open GPIO pin for output

        var i2cSettings = new I2cConnectionSettings(1, Bme280.DefaultI2cAddress); // I2C settings
        s_i2cDevice = I2cDevice.Create(i2cSettings); // Create I2C device

        s_bme280 = new Bme280(s_i2cDevice); // Initialize BME280 sensor

        ColorMessage("Cheese Cave device app.\n", ConsoleColor.Yellow); // Display startup message

        s_deviceClient = DeviceClient.CreateFromConnectionString(s_deviceConnectionString, TransportType.Mqtt); // Create device client

        s_deviceClient.SetMethodHandlerAsync("SetFanState", SetFanState, null).Wait(); // Set direct method handler

        MonitorConditionsAndUpdateTwinAsync(); // Start monitoring conditions and updating twin

        Console.ReadLine(); // Wait for user input to exit
        s_gpio.ClosePin(s_pin); // Close GPIO pin
    }

    private static async void MonitorConditionsAndUpdateTwinAsync()
    {
        while (true)
        {
            Bme280ReadResult sensorOutput = s_bme280.Read(); // Read sensor data

            await UpdateTwin(
                    sensorOutput.Temperature.Value.DegreesFahrenheit, 
                    sensorOutput.Humidity.Value.Percent); // Update twin with sensor data

            await Task.Delay(IntervalInMilliseconds); // Wait for the specified interval
        }
    }

    private static Task<MethodResponse> SetFanState(MethodRequest methodRequest, object userContext)
    {
        if (s_fanState is stateEnum.failed)
        {
            string result = "{\"result\":\"Fan failed\"}"; // Fan failed response
            RedMessage("Direct method failed: " + result); // Display error message
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 400)); // Return error response
        }
        else
        {
            try
            {
                var data = Encoding.UTF8.GetString(methodRequest.Data); // Get method request data

                data = data.Replace("\"", ""); // Remove quotes from data

                s_fanState = (stateEnum)Enum.Parse(typeof(stateEnum), data); // Parse fan state
                GreenMessage("Fan set to: " + data); // Display success message

                s_gpio.Write(s_pin, s_fanState == stateEnum.on ? PinValue.High : PinValue.Low); // Set GPIO pin value

                string result = "{\"result\":\"Executed direct method: " + methodRequest.Name + "\"}"; // Success response
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200)); // Return success response
            }
            catch
            {
                string result = "{\"result\":\"Invalid parameter\"}"; // Invalid parameter response
                RedMessage("Direct method failed: " + result); // Display error message
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 400)); // Return error response
            }
        }
    }

    private static async Task UpdateTwin(double currentTemperature, double currentHumidity)
    {
        var reportedProperties = new TwinCollection(); // Create twin collection
        reportedProperties["fanstate"] = s_fanState.ToString(); // Add fan state to twin
        reportedProperties["humidity"] = Math.Round(currentHumidity, 2); // Add humidity to twin
        reportedProperties["temperature"] = Math.Round(currentTemperature, 2); // Add temperature to twin
        await s_deviceClient.UpdateReportedPropertiesAsync(reportedProperties); // Update reported properties

        GreenMessage("Twin state reported: " + reportedProperties.ToJson()); // Display success message
    }

    private static void ColorMessage(string text, ConsoleColor clr)
    {
        Console.ForegroundColor = clr; // Set console text color
        Console.WriteLine(text); // Display message
        Console.ResetColor(); // Reset console text color
    }
    
    private static void GreenMessage(string text) => 
        ColorMessage(text, ConsoleColor.Green); // Display green message

    private static void RedMessage(string text) => 
        ColorMessage(text, ConsoleColor.Red); // Display red message
}
