# SeeTestCloudAPI <img src="https://user-images.githubusercontent.com/50325649/89872924-49c01c80-dbd7-11ea-92e5-b9296c11e8d6.png" width="25" height="25" title="SeeTestCloud" alt="SeeTestCloud">

A simple and easy library to manage Mobile Devices hosted in the SeeTest Cloud.

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

// Use below method to print all the device's Location,DeviceName,DeviceOS,OSVersion,CurrentStatus,DeviceID and UDID details in the console.
client.PrintAllDevicesImportantInformation(); 
```
Use the Device ID in the Device specific methods.
```
e.g. 
client.ReserveDevice(DeviceID, "2020-07-13-16-30-00", "2020-07-13-16-30-00", "2020-07-13-16-50-00"); // Will Reserve the device and will return the response as string.
```

## Available Methods
No much information added here about the methods since all the methods are having description and parameter info which will be displayed while using the methods.

<img src="https://user-images.githubusercontent.com/50325649/87870238-a9098300-c9c3-11ea-8202-4b8d34159425.png" width="820" height="80" title="MethodDesc" alt="MethodDesc">

```
AddDeviceTag()
EditDevice()
GetAllDevices() (+ 3 overloads)
GetAvailableDevicesIDs() (+ 1 overload) -  Added in v1.2.0
GetAvailableDevicesNames() (+ 1 overload)
GetAvailableDevicesNamesWithDetails() (+ 1 overload)
GetDevice() (+ 2 overloads)
GetDeviceID() (+ 1 overload)
GetDeviceIDList() -  Added in v1.2.0
GetDeviceiOSConfigurationProfiles()
GetDeviceReservations()
GetDeviceTags()
GetOnlineDevicesIDs() (+ 1 overload) -  Added in v1.2.0
GetOnlineDevicesNames() (+ 1 overload)
PrintAllDevicesImportantInformation()
RebootDevice()
ReleaseDevice()
RemoveAllTagsOfDevice()
RemoveDeviceTag()
ReserveDevice()
ReserveMultipleDevices()
ResetUSBConnection()
StartWebControl() (+ 3 overloads) -  Added two new methods in v1.1.0
```
More methods will be added in the future releases.

## Built With

* [SeeTestCloud APIs](https://docs.experitest.com/display/PM/Devices+Rest+API) - The Official Raw Rest APIs
* [Restsharp](https://www.nuget.org/packages/RestSharp) - API Management
* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) - Json Management

## Authors

* [**Meganathan C**](https://mega6453.carrd.co)

## Want to add features or fix things?
* Clone the Repo
* Make changes
* Create a pull request

## License

This project is licensed under the [MIT License](https://licenses.nuget.org/MIT)
