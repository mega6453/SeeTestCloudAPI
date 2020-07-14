# SeeTest Cloud API <img src="https://user-images.githubusercontent.com/50325649/87389535-855dcb80-c5c4-11ea-9abc-c24e9253d229.png" width="25" height="25" title="SeeTestCloud" alt="SeeTestCloud">

Library to manage Mobile Devices hosted in the SeeTest Cloud.

### Prerequisites

```
A .NetStandard v2.0/ .NETCore v2.0 / .NetFramework v4.6.1 project
```

### Installing

```
Right Click on your project in the visual studio solution explorer->Manage nuget packages-> Search for SeeTestCloudAPI -> Select project -> Install.
Gathering dependency information may take a minute or more.
```

## How to Use? It's simple! 

Import "SeeTestCloudAPI". 
```
using SeeTestCloudAPI;
```
Create a new instance of CloudAPIClient with the SeeTest Cloud server URL and credentials(Token / Username & Password)
```
CloudAPIClient client = new CloudAPIClient("https://cloud.seetest.io", "APIToken");
```
Use the created instance to call the available methods.e.g.
```
client.GetAvailableDeviceNames();
client.PrintAllDevicesImportantInformation();
client.ReserveDevice(12345, "2020-07-13-16-30-00", "2020-07-13-16-30-00", "2020-07-13-16-50-00");
```
## Available Methods
```
AddDeviceTag()
EditDevice()
GetAllDevices() (+ 3 overloads)
GetAvailableDeviceNames() (+ 1 overload)
GetDevice() (+ 2 overloads)
GetDeviceID() (+ 1 overload)
GetDeviceiOSConfigurationProfiles()
GetDeviceReservations()
GetDeviceTags()
GetOnlineDeviceNames() (+ 1 overload)
PrintAllDevicesImportantInformation()
RebootDevice()
ReleaseDevice()
RemoveAllTagsOfDevice()
RemoveDeviceTag()
ReserveDevice()
ReserveMultipleDevices()
ResetUSBConnection()
StartWebControl() (+ 1 overload)
```
More methods will be added in the future releases.

## Built With

* [SeeTestCloud APIs](https://docs.experitest.com/display/PM/Devices+Rest+API) - The Official Raw Rest APIs
* [Restsharp](https://www.nuget.org/packages/RestSharp) - API Management
* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) - Json Management

## Authors

* [**Meganathan C**](https://mega6453.carrd.co)

## License

This project is licensed under the [MIT License](https://licenses.nuget.org/MIT)
