# SeeTestCloudAPI <img src="https://user-images.githubusercontent.com/50325649/87389535-855dcb80-c5c4-11ea-9abc-c24e9253d229.png" width="25" height="25" title="SeeTestCloud" alt="SeeTestCloud">

Library to manage Mobile Devices hosted in the SeeTest Cloud.

### Prerequisites

```
A .NetStandard v2.0 / .NetFramework v4.5.2 project

SeeTestCloud Access
```

### Installing

```
Right Click on your project in the visual studio solution explorer->Manage nuget packages-> Search for SeeTestCloudAPI -> Select SeeTestCloudAPI By Meganathan from the list -> Select project -> Install.

Gathering dependency information may take a minute or more.
```

## How to Use? It's simple! 

Import "SeeTestCloudAPI". 
```
using SeeTestCloudAPI;
```
Create a new instance of CloudAPIClient with the SeeTest Cloud server URL and credentials(Token / Username & Password)
```
CloudAPIClient client = new CloudAPIClient("CloudServerURL", "AccessToken");

CloudServerURL = The URL where the SeeTestCloud server is configured.
AccessToken = Login to SeeTestCloud->User Icon->Get Access Key->Copy.

e.g. CloudAPIClient client = new CloudAPIClient("https://xxxxx.com", "xxxxxxxxxxx");

```
Use the created instance to call the available methods.
```
e.g.
client.GetAvailableDevicesNames();
client.PrintAllDevicesImportantInformation();
```
To use Device specific methods, Device ID(Assigned by SeeTestCloud) is required. Use GetDeviceID() method to get the Device ID.
```
// Use UDID of a Device to get Device ID.
int DeviceID = client.GetDeviceID(string UDID); // iOS - UDID; Android - Serial number.

OR

// Use Queries to filter the device and Get Device ID.
Dictionary<Keys, string> SearchQuery = new Dictionary<Keys, string>(); 
SearchQuery.Add(Keys.agentLocation, "Bangalore");  // Keys is a enum which will have all the keys. So, just need to type "Keys." -> will list out all the keys.
SearchQuery.Add(Keys.deviceOs, "android");
SearchQuery.Add(Keys.displayStatus, "available");
SearchQuery.Add(Keys.model, "Nexus 5X");
int DeviceID = client.GetDeviceID(SearchQuery);

OR 

// Use below method to print all the devices Location,DeviceName,DeviceOS,OSVersion,CurrentStatus and UDID details in the console. 
client.PrintAllDevicesImportantInformation(); 
```
Use the Device ID in the Device specific methods.
```
e.g. int DeviceID = 12345;
client.ReserveDevice(DeviceID, "2020-07-13-16-30-00", "2020-07-13-16-30-00", "2020-07-13-16-50-00"); // Will Reserve the device and will return the response as string.
```

## Available Methods
```
AddDeviceTag()
EditDevice()
GetAllDevices() (+ 3 overloads)
GetAvailableDevicesNames() (+ 1 overload)
GetAvailableDevicesNamesWithDetails() (+ 1 overload)
GetDevice() (+ 2 overloads)
GetDeviceID() (+ 1 overload)
GetDeviceiOSConfigurationProfiles()
GetDeviceReservations()
GetDeviceTags()
GetOnlineDevicesNames() (+ 1 overload)
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
