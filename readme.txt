# CheeseCaveDotnet

## Description

CheeseCaveDotnet is an IoT application designed to monitor and control environmental conditions in a cheese cave. It uses a BME280 sensor to read temperature and humidity data and communicates with Azure IoT Hub to report these conditions and receive commands. The project is implemented in C# and targets .NET 6.0.

## Setup Instructions

### Prerequisites

- Raspberry Pi or similar device with GPIO and I2C capabilities.
- BME280 sensor for temperature and humidity measurements.
- Fan connected to a GPIO pin.
- .NET 6.0 SDK
- Azure IoT Hub account
- Visual Studio or Visual Studio Code for development

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/CheeseCaveDotnet.git
    cd CheeseCaveDotnet
    ```

2. Open the project in Visual Studio or Visual Studio Code.

3. Restore the NuGet packages:
    ```sh
    dotnet restore
    ```

4. Build the project:
    ```sh
    dotnet build
    ```

## Usage

1. Set your Azure IoT Hub device connection string in the `s_deviceConnectionString` variable in `Program.cs`.

2. Deploy the application to your Raspberry Pi or similar device.

3. Run the application:
    ```sh
    dotnet run
    ```

4. The application will start monitoring temperature and humidity conditions and report them to Azure IoT Hub. You can also send direct method calls to control the fan state.

## Contributor Guidelines

We welcome contributions to the CheeseCaveDotnet project! Here are some guidelines to get started:

1. Fork the repository.

2. Create a new branch for your feature or bugfix:
    ```sh
    git checkout -b feature-name
    ```

3. Make your changes and commit them with a clear message:
    ```sh
    git commit -m "Description of the feature or fix"
    ```

4. Push your changes to your fork:
    ```sh
    git push origin feature-name
    ```

5. Create a pull request with a description of your changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.