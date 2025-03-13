# CheeseCaveDotnet Project Documentation

## Overview

The CheeseCaveDotnet project is an IoT application designed to monitor and control environmental conditions in a cheese cave. It uses a BME280 sensor to read temperature and humidity data and communicates with Azure IoT Hub to report these conditions and receive commands. The project is implemented in C# and targets .NET 6.0.

## Dependencies

The project relies on several NuGet packages to interact with hardware components and Azure IoT Hub:

- **System.Device.Gpio**: Provides access to GPIO pins.
- **System.Device.I2c**: Provides access to I2C devices.
- **Iot.Device.Bmxx80**: Library for interacting with BME280 sensors.
- **Microsoft.Azure.Devices.Client**: Azure IoT Hub client library.
- **Microsoft.Azure.Devices.Shared**: Shared library for Azure IoT Hub.
- **System.Text**: Provides text encoding and decoding functionalities.

## Features

### Initialization
- **GPIO Initialization**: Initializes the GPIO controller and opens a pin for output.
- **I2C Initialization**: Configures I2C settings and creates an I2C device instance.
- **Sensor Initialization**: Initializes the BME280 sensor to read temperature and humidity data.
- **Azure IoT Hub Connection**: Establishes a connection to Azure IoT Hub using a device connection string.

### Main Functionality
- **Startup Message**: Displays a startup message in the console.
- **Direct Method Handler**: Sets up a method handler for direct method calls from Azure IoT Hub to control the fan state.
- **Monitoring and Reporting**: Continuously reads sensor data and updates the device twin properties in Azure IoT Hub at regular intervals.

### Direct Method Handling
- **Set Fan State**: Handles direct method calls from Azure IoT Hub to set the fan state (on, off, or failed).
  - **Parse Fan State**: Parses the fan state from the method request data.
  - **Update GPIO Pin**: Updates the GPIO pin state based on the fan state.
  - **Return Response**: Returns appropriate responses for the direct method calls (success or error).

### Monitoring and Updating
- **Monitor Conditions**: Continuously monitors the temperature and humidity conditions using the BME280 sensor.
- **Update Twin Properties**: Updates the device twin properties in Azure IoT Hub with the current temperature, humidity, and fan state.

### Helper Methods
- **Color Message**: Displays messages in the console with different colors for better readability.
  - **Green Message**: Displays a message in green color.
  - **Red Message**: Displays a message in red color.

### Constants
- **Desired Temperature Limit**: Acceptable range above or below the desired temperature, in degrees Fahrenheit.
- **Desired Humidity Limit**: Acceptable range above or below the desired humidity, in percentages.
- **Telemetry Interval**: Interval at which telemetry is sent to the cloud, in milliseconds.

### Enumerations
- **Fan State Enumeration**: Defines the possible states of the fan (off, on, failed).

### Azure IoT Hub Integration
- **Device Client**: Manages the connection and communication with Azure IoT Hub.
- **Device Connection String**: Stores the connection string for the Azure IoT device.

### Sensor Data Handling
- **Read Sensor Data**: Reads temperature and humidity data from the BME280 sensor.
- **Update Reported Properties**: Updates the reported properties of the device twin in Azure IoT Hub with the current sensor data.

### Console Interaction
- **Wait for User Input**: Waits for user input to exit the application.
- **Close GPIO Pin**: Closes the GPIO pin when the application exits.

## Requirements

- **Hardware**:
  - Raspberry Pi or similar device with GPIO and I2C capabilities.
  - BME280 sensor for temperature and humidity measurements.
  - Fan connected to a GPIO pin.

- **Software**:
  - .NET 6.0 SDK
  - Azure IoT Hub account
  - Visual Studio or Visual Studio Code for development

## Constraints

- **Network Connectivity**: The device must have network connectivity to communicate with Azure IoT Hub.
- **Power Supply**: Ensure the device has a stable power supply to avoid interruptions.
- **Sensor Placement**: Place the BME280 sensor in an appropriate location within the cheese cave for accurate readings.

## Summary

The CheeseCaveDotnet project provides a comprehensive solution for monitoring and controlling environmental conditions in a cheese cave. By leveraging the capabilities of the BME280 sensor and Azure IoT Hub, the project enables real-time data collection, reporting, and remote control of the fan state. The project is designed to be easily extensible and adaptable to various IoT scenarios, making it a valuable tool for managing cheese cave environments.