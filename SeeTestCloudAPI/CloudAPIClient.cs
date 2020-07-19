using ConsoleTables;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Net;

namespace SeeTestCloudAPI
{
    /// <summary>
    /// Provides methods to Manage Mobile devices hosted in the SeeTest Cloud.
    /// </summary>
    public class CloudAPIClient
    {

        private readonly string CloudServer, Token, UserName, Password;
        string URL;
        RestClient client; RestRequest request;

        /// <summary>
        /// SeeTest Cloud Server Address is required to find your server and SeeTestAccessToken is required for authentication.
        /// </summary>
        /// <param name="SeeTestCloudServerAddress">Enter SeeTest Cloud Server address</param>
        /// <param name="SeeTestAccessToken">Login to SeeTestCloud->User Icon->Get Access Key->Copy</param>

        public CloudAPIClient(string SeeTestCloudServerAddress, string SeeTestAccessToken)
        {
            CloudServer = SeeTestCloudServerAddress.TrimEnd('/');
            Token = SeeTestAccessToken;
        }

        /// <summary>
        /// SeeTest Cloud Server Address is required to find your server and SeeTest Cloud UserName and Password is required for authentication.
        /// </summary>
        /// <param name="SeeTestCloudServerAddress">Enter SeeTest Cloud Server address</param>
        /// <param name="SeeTestCloudUserName">Enter valid Username registered with SeeTest Cloud->Copy</param>
        /// <param name="SeeTestCloudPassword">Enter valid Password for the Username</param>
        public CloudAPIClient(string SeeTestCloudServerAddress, string SeeTestCloudUserName, string SeeTestCloudPassword)
        {
            CloudServer = SeeTestCloudServerAddress.TrimEnd('/');
            UserName = SeeTestCloudUserName;
            Password = SeeTestCloudPassword;
        }


        //--------------------------------------Get All Devices---------------------------------------------------


        /// <summary>
        /// Returns the information of all the devices the user has access to(raw format).
        /// </summary>
        public string GetAllDevices()
        {
            URL = CloudServer + "/api/v1/devices";
            return ExecuteGet(URL);
        }

        /// <summary>
        /// Returns a List of values for the expected key from all the devices(the user has access to) registered in the cloud. e.g. List of device names.
        /// </summary>
        /// <param name="ExpectedKey">Type Keys. -> Will give list of available keys->Select from the list. e.g. Keys.deviceName</param>
        public List<string> GetAllDevices(Keys ExpectedKey)
        {
            return GetValueFromAllDevices(ExpectedKey);
        }

        /// <summary>
        /// Returns a List of values for the expected key from all the devices(the user has access to) based on the input key value query. e.g. List of device names.
        /// </summary>      
        /// <param name="SearchQuery">Dictionary that contains "Keys" as Key and "string" as value. e.g input.Add(Keys.deviceOs,"iOS"),input.Add(Keys.osVersion,"11")</param>
        /// <param name="ExpectedKey">Type Keys. -> Will give list of available keys->Select from the list. e.g. Keys.deviceName</param>
        public List<string> GetAllDevices(Dictionary<Keys, string> SearchQuery, Keys ExpectedKey)
        {
            return GetListValueFromAllDevicesByQuery(SearchQuery, ExpectedKey.ToString());
        }

        /// <summary>
        /// Returns a List of values for the expected key from all the devices(the user has access to) based on specified Key,value. e.g. List of device names for deviceOS="android"
        /// </summary>
        /// <param name="InputKey">Type Keys. -> Will give list of available keys->Select from the list. e.g. Keys.deviceOS</param>
        /// <param name="value">Enter value for the key e.g. value = "android"</param>
        /// <param name="ExpectedKey">Type Keys. -> Will give list of available keys->Select from the list. e.g. Keys.deviceName</param>
        public List<string> GetAllDevices(Keys InputKey, string value, Keys ExpectedKey)
        {
            return GetAllDevices(InputKey.ToString(), value, ExpectedKey.ToString());
        }

        /// <summary>
        /// Returns a list of Devices names(the user has access to) which are currently Available. Location can be "all" OR a specific location(agent location). 
        /// </summary>
        /// <param name="Location">Enter "all" to get devices connected in all locations OR Enter a specific location.</param>
        public List<string> GetAvailableDevicesNames(string Location)
        {
            Dictionary<Keys, string> InputKeyValue = new Dictionary<Keys, string>();
            InputKeyValue.Add(Keys.displayStatus, "available");
            if (!Location.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                InputKeyValue.Add(Keys.agentLocation, Location);
            }
            return GetAllDevices(InputKeyValue, Keys.deviceName);
        }


        /// <summary>
        /// Returns a list of Devices names(the user has access to) which are currently Available with the OS Type as a filter. Location can be "all" OR a specific location(agent location). 
        /// </summary>
        /// <param name="Location">Enter "all" to get devices connected in all locations OR Enter a specific location(agent location).</param>
        /// <param name="OS">Type OSType. to display all the available options. e.g. OSType.iOS</param>
        public List<string> GetAvailableDevicesNames(string Location, OSType OS)
        {
            Dictionary<Keys, string> InputKeyValue = new Dictionary<Keys, string>();
            InputKeyValue.Add(Keys.displayStatus, "available");
            InputKeyValue.Add(Keys.deviceOs, OS.ToString());
            if (!Location.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                InputKeyValue.Add(Keys.agentLocation, Location);
            }
            return GetAllDevices(InputKeyValue, Keys.deviceName);
        }



        /// <summary>
        /// Returns a list of Devices names(the user has access to) with it's agentLocation,deviceOs,model and id which are currently Available. Location can be "all" OR a specific location(agent location). 
        /// </summary>
        /// <param name="Location">Enter "all" to get devices connected in all locations OR Enter a specific location.</param>
        public List<Tuple<string, string, string, string, string>> GetAvailableDevicesNamesWithDetails(string Location)
        {
            Dictionary<Keys, string> InputKeyValue = new Dictionary<Keys, string>();
            InputKeyValue.Add(Keys.displayStatus, "available");
            if (!Location.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                InputKeyValue.Add(Keys.agentLocation, Location);
            }
            return GetAvailableDevicesListValueFromAllDevicesByQuery(InputKeyValue);
        }


        /// <summary>
        /// Returns a list of Devices names(the user has access to) with it's agentLocation,deviceOs,model and id which are currently Available with the OS Type as a filter. Location can be "all" OR a specific location(agent location). 
        /// </summary>
        /// <param name="Location">Enter "all" to get devices connected in all locations OR Enter a specific location(agent location).</param>
        /// <param name="OS">Type OSType. to display all the available options. e.g. OSType.iOS</param>
        public List<Tuple<string, string, string, string, string>> GetAvailableDevicesNamesWithDetails(string Location, OSType OS)
        {
            Dictionary<Keys, string> InputKeyValue = new Dictionary<Keys, string>();
            InputKeyValue.Add(Keys.displayStatus, "available");
            InputKeyValue.Add(Keys.deviceOs, OS.ToString());
            if (!Location.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                InputKeyValue.Add(Keys.agentLocation, Location);
            }
            return GetAvailableDevicesListValueFromAllDevicesByQuery(InputKeyValue);
        }

        /// <summary>
        /// Returns a list of Devices names(the user has access to) which are currently Online(Currently connected to SeeTestCloud,But device status can be Available/In-Use). Location can be "all" OR a specific location(agent location). 
        /// </summary>
        /// <param name="Location">Enter "all" to get devices connected in all locations OR Enter a specific location(agent location).</param>
        public List<string> GetOnlineDevicesNames(string Location)
        {
            Dictionary<Keys, string> InputKeyValue = new Dictionary<Keys, string>();
            InputKeyValue.Add(Keys.currentStatus, "online");
            if (!Location.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                InputKeyValue.Add(Keys.agentLocation, Location);
            }
            return GetAllDevices(InputKeyValue, Keys.deviceName);
        }

        /// <summary>
        /// Returns a list of Devices names(the user has access to) which are currently Online(Currently connected to SeeTestCloud,But device status can be Available/In-Use) with the OS Type as a filter. Location can be "all" OR a specific location(agent location). 
        /// </summary>
        /// <param name="Location">Enter "all" to get devices connected in all locations OR Enter a specific location(agent location).</param>
        /// <param name="OS">Type OSType. to display all the available options. e.g. OSType.iOS</param>
        public List<string> GetOnlineDevicesNames(string Location, OSType OS)
        {
            Dictionary<Keys, string> InputKeyValue = new Dictionary<Keys, string>();
            InputKeyValue.Add(Keys.currentStatus, "online");
            InputKeyValue.Add(Keys.deviceOs, OS.ToString());
            if (!Location.Equals("all", StringComparison.InvariantCultureIgnoreCase))
            {
                InputKeyValue.Add(Keys.agentLocation, Location);
            }
            return GetAllDevices(InputKeyValue, Keys.deviceName);
        }

        /// <summary>
        /// Prints all the device's(the user has access to) Location,DeviceName,DeviceOS,OSVersion,CurrentStatus,DeviceID and UDID details in the console. Use this method to see UDID of a device, helpful for GetDeviceID(string UDID).
        /// Copy and paste the output into a text editor like Notepad.
        /// </summary>
        public void PrintAllDevicesImportantInformation()
        {
            List<Tuple<string, string, string, string, string, string, string>> output = new List<Tuple<string, string, string, string, string, string, string>>();
            client = new RestClient(CloudServer + "/api/v1/devices");
            client.Timeout = -1;
            request = new RestRequest(Method.GET);
            Authenticator(client, request);
            string response = Execute(request);
            var token = JToken.Parse(response);
            var roles = token.Value<JArray>("data");
            var count = roles.Count;
            dynamic jObj = JObject.Parse(response);
            var table = new ConsoleTable("Location", "DeviceName", "DeviceOS", "OSVersion", "CurrentStatus", "DeviceID", "UDID");
            for (int i = 0; i < count; i++)
            {
                output.Add(Tuple.Create((string)jObj.data[i].agentLocation, (string)jObj.data[i].deviceName, (string)jObj.data[i].deviceOs, (string)jObj.data[i].osVersion, (string)jObj.data[i].displayStatus, (string)jObj.data[i].id, (string)jObj.data[i].udid));
                output.Sort();
            }
            foreach (var item in output)
            {
                table.AddRow(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6, item.Item7);
            }
            table.Write();
        }


        //-----------------------------------------------------------------------------------------



        /// <summary>
        /// Returns a Specific device details in raw format. This API is available only for cloud admin.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        public string GetDevice(int DeviceID)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID;
            return ExecuteGet(URL);
        }


        /// <summary>
        /// Returns a Specific value from a device data as a string for the Expected Key. This API is available only for cloud admin.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="ExpectedKey">Type Keys. -> Will give list of available keys->Select from the list. e.g. Keys.displayStatus, it returns "Available/In-Use/Offline"</param>
        public string GetDevice(int DeviceID, Keys ExpectedKey)
        {
            return GetStringValueFromSpecificDeviceByID(DeviceID, ExpectedKey);
        }

        /// <summary>
        /// Returns a Specific value from a device data as a Dictionary. This API is available only for cloud admin.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="ExpectedKeys">Create a List of required Keys and pass it. e.g. Keys.deviceName,Keys.deviceOs will return both values in dictionary</param>

        public Dictionary<Keys, string> GetDevice(int DeviceID, List<Keys> ExpectedKeys)
        {
            return GetCollectionValueFromSpecificDeviceByID(DeviceID, ExpectedKeys);
        }


        //-----------------------------------------------------------------------------------------



        /// <summary>
        /// Returns Device ID assigned by SeeTestCloud for the entered UDID from all the devices(the user has access to).
        /// </summary>
        /// <param name="UDID">Case-InSensitive; Android->About->Serial number; iOS->iTunes->UDID OR Use PrintAllDevicesImportantInformation() method to get UDID details from the Cloud.</param>
        public int GetDeviceID(string UDID)
        {
            int deviceID = int.Parse(GetStringValueFromAllDevicesByUDID(UDID, "id"));
            return deviceID;
        }

        /// <summary>
        /// Returns Device ID assigned by SeeTestCloud for the entered Query from all the devices(the user has access to).
        /// </summary>
        /// <param name="SearchQuery">Dictionary that contains "Keys" as Key and "string" as value. e.g input.Add(Keys.deviceName,"myphone"),input.Add(Keys.agentLocation,"Bangalore")</param>
        public int GetDeviceID(Dictionary<Keys, string> SearchQuery)
        {
            int deviceID = int.Parse(GetStringValueFromAllDevicesByQuery(SearchQuery, "id"));
            return deviceID;
        }


        /// <summary>
        /// Returns list of tags that were added to device. This API is available for all user roles.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        public List<string> GetDeviceTags(int DeviceID)
        {
            return GetTagsListValueFromSpecificDeviceByID(DeviceID);
        }


        /// <summary>
        /// (Only for iOS) - Returns list of iOS Configuration Profiles for the Entered Device ID. This API is available only for cloud admin.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        public List<string> GetDeviceiOSConfigurationProfiles(int DeviceID)
        {
            return GetListValueFromSpecificDeviceByID(DeviceID, "iosConfigurationProfiles");
        }


        /// <summary>
        /// Returns Device Reservation details for the entered Device ID in the specified duration. This API is available only for the cloud admin.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="ClientCurrentTimestamp">YYYY-MM-DD-hh-mm-ss, Time should be in 24 hours format</param>
        /// <param name="StartTime">YYYY-MM-DD-hh-mm-ss, Time should be in 24 hours format</param>
        /// <param name="EndTime">YYYY-MM-DD-hh-mm-ss, Time should be in 24 hours format</param>
        public string GetDeviceReservations(int DeviceID, string ClientCurrentTimestamp, string StartTime, string EndTime)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/reservations";
            var request = ReturnRequest(URL, Method.GET);
            request.AddParameter("current_timestamp", ClientCurrentTimestamp);
            request.AddParameter("start", StartTime);
            request.AddParameter("end", EndTime);
            return Execute(request);
        }



        //--------------------------------------POST / PUT--------------------------------------------------



        /// <summary>
        /// Updates the Device Name,Notes and Category for the Entered Device ID. This API is available only for the cloud admin. Returns the response as string.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="Name">Enter as "NoChange" to keep old value OR Enter a Name you wish to update for the Device. e.g. iPhone8</param>
        /// <param name="Notes">Enter as "NoChange" to keep old value OR Enter Notes you wish to update for the Device. e.g. Testing phone</param>
        /// <param name="DeviceCategory">Type Category. -> Will display available category. e.g. Category.PHONE</param>
        public string EditDevice(int DeviceID, string Name, string Notes, Category DeviceCategory)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID;
            var request = ReturnRequest(URL, Method.POST);
            if (!Name.Equals("NoChange", StringComparison.InvariantCultureIgnoreCase))
            {
                request.AddParameter("name", Name);
            }
            if (!Notes.Equals("NoChange", StringComparison.InvariantCultureIgnoreCase))
            {
                request.AddParameter("notes", Notes);
            }
            if (!DeviceCategory.ToString().Equals("NoChange", StringComparison.InvariantCultureIgnoreCase))
            {
                request.AddParameter("category", DeviceCategory.ToString());
            }
            return Execute(request);
        }


        /// <summary>
        /// Reserves a device for the current user(Access Token/Credentials used). Returns the response as string.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="ClientCurrentTimestamp">YYYY-MM-DD-hh-mm-ss, Time should be in 24 hours format</param>
        /// <param name="StartTime">YYYY-MM-DD-hh-mm-ss, Time should be in 24 hours format</param>
        /// <param name="EndTime">YYYY-MM-DD-hh-mm-ss, Time should be in 24 hours format</param>
        public string ReserveDevice(int DeviceID, string ClientCurrentTimestamp, string StartTime, string EndTime)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/reservations/new";
            var request = ReturnRequest(URL, Method.POST);
            request.AddParameter("clientCurrentTimestamp", ClientCurrentTimestamp);
            request.AddParameter("start", StartTime);
            request.AddParameter("end", EndTime);
            return Execute(request);
        }

        /// <summary>
        /// Reserves multiple devices for the current user(Access Token/Credentials used). This API is available only for the cloud admin. Returns the response as string.
        /// </summary>
        /// <param name="DevicesList">Enter list of ID of the device (as "8,235,54") assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="ClientCurrentTimestamp">YYYY-MM-DD-hh-mm-ss, Time should be in 24 hours format</param>
        /// <param name="StartTime">YYYY-MM-DD-hh-mm-ss, Time should be in 24 hours format</param>
        /// <param name="EndTime">YYYY-MM-DD-hh-mm-ss, Time should be in 24 hours format</param>
        public string ReserveMultipleDevices(string DevicesList, string ClientCurrentTimestamp, string StartTime, string EndTime)
        {
            URL = CloudServer + "/api/v1/devices/reservations/new";
            var request = ReturnRequest(URL, Method.POST);
            request.AddParameter("devicesList", DevicesList);
            request.AddParameter("clientCurrentTimestamp", ClientCurrentTimestamp);
            request.AddParameter("start", StartTime);
            request.AddParameter("end", EndTime);
            return Execute(request);
        }

        /// <summary>
        /// Releases the device from its current user. Returns the response as string.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        public string ReleaseDevice(int DeviceID)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/release";
            return ExecutePost(URL);
        }

        /// <summary>
        /// Reboot the device. This API is available only for cloud admin. Returns the response as string.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        public string RebootDevice(int DeviceID)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/reboot";
            return ExecutePost(URL);
        }

        /// <summary>
        /// Resets the USB Connection. This API is available only for cloud admin. Returns the response as string.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        public string ResetUSBConnection(int DeviceID)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/resetusb";
            return ExecutePost(URL);
        }

        /// <summary>
        /// Start web access control of the device. When choosing 'Debug' option - Grid must run (from the same user who ran the API).
        /// <para>This API is available only for cloud admin. Returns the response as string.</para>
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="Type">Type ControlType. -> will get list of available options. Select from the list. e.g. ControlType.DEBUG</param>
        public string StartWebControl(int DeviceID, ControlType Type)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/web-control";
            var request = ReturnRequest(URL, Method.PUT);
            request.AddParameter("type", (int)Type);
            return Execute(request);
        }


        /// <summary>
        /// Start web access control of the device. When choosing 'Debug' option - Grid must run (from the same user who ran the API).
        /// <para>This API is available only for cloud admin. Returns the response as string.</para>
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="Type">Type ControlType. -> will get list of available options. Select from the list. e.g. ControlType.DEBUG</param>
        /// <param name="EmulatorInstanceName">Enter Name for the emulator.</param>
        public string StartWebControl(int DeviceID, ControlType Type, string EmulatorInstanceName)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/web-control";
            var request = ReturnRequest(URL, Method.PUT);
            request.AddParameter("type", Type);
            request.AddParameter("emulatorInstanceName", EmulatorInstanceName);
            return Execute(request);
        }

        /// <summary>
        /// Add tag to device. This API is available for Cloud Administrators and Project Administrators. Returns the response as string.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="tag">Enter tag that you want to add. Can contain only letters, digits, and underscore character. Tags are not case sensitive.</param>
        public string AddDeviceTag(int DeviceID, string tag)
        {
            if (tag.Length == 0)
            {
                throw new Exception("\n\n Tag should not be empty.");
            }
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/tags/" + tag;
            return ExecutePut(URL);
        }

        /// <summary>
        /// Deletes a specified tag from a Device. This API is available for Cloud Administrators and Project Administrators. Returns the response as string.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        /// <param name="tag">Enter tag that you want to delete.</param>
        public string RemoveDeviceTag(int DeviceID, string tag)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/tags/" + tag;
            return ExecuteDelete(URL);
        }

        /// <summary>
        /// Deletes all tags from a Device. This API is available for Cloud Administrators and Project Administrators. Returns the response as string.
        /// </summary>
        /// <param name="DeviceID">Enter Unique Device ID assigned by SeeTestCloud. Use GetDeviceID()/PrintAllDevicesImportantInformation() method first to get ID of a device.</param>
        public string RemoveAllTagsOfDevice(int DeviceID)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/tags/";
            return ExecuteDelete(URL);
        }

        /// <summary>
        /// This is NOT SeeTestCloudAPI method. Use this method to parse your "String" response and get the value. Note: The response should be in JSON format.e.g. {"status":"success"}
        /// </summary>
        /// <param name="Response">Pass the response of a method. e.g. Return string of AddDeviceTag() method.</param>
        /// <param name="Key">Enter a key from the response. e.g. Response is {"status":"success"},Here Key is "status" and return value be "success".</param>

        public string ParseResponse(string Response, string Key)
        {
            var token = JToken.Parse(Response);
            return token[Key].ToString();
        }




        //---------------------------------------------PRIVATE METHODS-------------------------------------------------------//

        private RestRequest ReturnRequest(string URL, Method method)
        {
            client = new RestClient(URL);
            client.Timeout = -1;
            request = new RestRequest(method);
            Authenticator(client, request);
            return request;
        }

        private string Execute(IRestRequest request)
        {
            IRestResponse response = client.Execute(request);
            return ThrowIfResponseStatusHasError(response);
        }

        private void Authenticator(RestClient client, RestRequest request)
        {
            if (Token == null)
            {
                client.Authenticator = new HttpBasicAuthenticator(UserName, Password);
            }
            else
            {
                request.AddHeader("Authorization", "Bearer " + Token);
            }
        }

        private string ExecuteGet(string URL)
        {
            var client = new RestClient(URL);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            Authenticator(client, request);
            IRestResponse response = client.Execute(request);
            return ThrowIfResponseStatusHasError(response);
        }

        private string ExecutePost(string URL)
        {
            var client = new RestClient(URL);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            Authenticator(client, request);
            IRestResponse response = client.Execute(request);
            return ThrowIfResponseStatusHasError(response);
        }

        private string ExecutePut(string URL)
        {
            var client = new RestClient(URL);
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            Authenticator(client, request);
            IRestResponse response = client.Execute(request);
            return ThrowIfResponseStatusHasError(response);
        }

        private string ExecuteDelete(string URL)
        {
            var client = new RestClient(URL);
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            Authenticator(client, request);
            IRestResponse response = client.Execute(request);
            return ThrowIfResponseStatusHasError(response);
        }

        private Dictionary<Keys, string> GetCollectionValueFromSpecificDeviceByID(int DeviceID, List<Keys> ExpectedKeys)
        {
            Dictionary<Keys, string> output = new Dictionary<Keys, string>();
            URL = CloudServer + "/api/v1/devices/" + DeviceID;
            string response = ExecuteGet(URL);
            dynamic jObj = JObject.Parse(response);
            if (jObj.data.id == DeviceID)
            {
                foreach (var item in ExpectedKeys)
                {
                    output.Add(item, (string)jObj.data[item.ToString()]);
                }
                return output;
            }
            throw new Exception("\n\nNo Device found in the SeeTest Cloud for the entered Device ID.\n");
        }

        private string GetStringValueFromSpecificDeviceByID(int DeviceID, Keys ExpectedKey)
        {
            string Expected = ExpectedKey.ToString();
            string output = null;
            URL = CloudServer + "/api/v1/devices/" + DeviceID;
            string response = ExecuteGet(URL);
            dynamic jObj = JObject.Parse(response);
            if (jObj.data.id == DeviceID)
            {
                output = (string)jObj.data[Expected];
                return output;
            }
            throw new Exception("\n\nNo Device found in the SeeTest Cloud for the entered Device ID.\n");
        }

        private string GetStringValueFromAllDevicesByUDID(string UDID, string key)
        {
            string output = null;
            URL = CloudServer + "/api/v1/devices";
            string response = ExecuteGet(URL);
            var token = JToken.Parse(response);
            var roles = token.Value<JArray>("data");
            var count = roles.Count;
            dynamic jObj = JObject.Parse(response);
            JArray jArray = new JArray();
            for (int i = 0; i < count; i++)
            {
                if (string.Equals((string)jObj.data[i].udid, UDID, StringComparison.InvariantCultureIgnoreCase))
                {
                    output = (string)jObj.data[i][key];
                    return output;
                }
            }
            throw new Exception("\n\nNo Device found in the SeeTest Cloud for the entered UDID.\n");
        }

        private List<string> GetListValueFromSpecificDeviceByID(int DeviceID, string key)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID;
            string response = ExecuteGet(URL);
            dynamic jObj = JObject.Parse(response);
            JArray array = new JArray();
            List<string> list = new List<string>();
            if (jObj.data.id == DeviceID)
            {
                array = jObj.data[key];
                if (array == null & jObj.data.deviceOs == "Android")
                {
                    throw new Exception("\n\nGetDeviceiOSConfigurationProfiles : This method supports only iOS Device. You've entered Android Device's ID. Please Enter ID of iOS Device.\n");
                }
                foreach (var item in array)
                {
                    list.Add(item.ToString());
                }
                return list;
            }
            throw new Exception("\n\nNo Device found in the SeeTest Cloud for the entered Device ID.\n");
        }

        private List<string> GetTagsListValueFromSpecificDeviceByID(int DeviceID)
        {
            URL = CloudServer + "/api/v1/devices/" + DeviceID + "/tags";
            string response = ExecuteGet(URL);
            dynamic jObj = JObject.Parse(response);
            JArray array = new JArray();
            List<string> list = new List<string>();
            array = jObj.data;
            foreach (var item in array)
            {
                list.Add(item.ToString());
            }
            return list;
        }

        private string GetStringValueFromAllDevicesByQuery(Dictionary<Keys, string> SearchQuery, string ExpectedKey)
        {
            string output = null;
            URL = CloudServer + "/api/v1/devices";
            string response = ExecuteGet(URL);
            var token = JToken.Parse(response);
            var roles = token.Value<JArray>("data");
            var count = roles.Count;
            dynamic jObj = JObject.Parse(response);
            int listCount = SearchQuery.Count;
            int k = 0;
            for (int i = 0; i < count; i++)
            {
                int j = 0;
                dynamic oneObj = jObj.data[i];
                foreach (var item in SearchQuery.Keys)
                {
                    string ValueFromUser = SearchQuery[item];
                    string ValueFromCloud = (string)oneObj[item.ToString()];
                    if (ValueFromUser.Equals(ValueFromCloud, StringComparison.InvariantCultureIgnoreCase))
                    {

                        j++;
                        if (listCount == j)
                        {
                            output = (string)oneObj[ExpectedKey];
                            k++;
                        }
                        else
                        {
                            continue;
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (k == 1)
            {
                return output;
            }
            else if (k > 1)
            {
                throw new Exception("\n\n" + k + " Devices found in the SeeTest Cloud for your Search Query.Try to add few other Key Values to get specific Device ID.\n");
            }
            else
            {
                throw new Exception("\n\nNo Device found in the SeeTest Cloud for your Search Query. Please make sure your query is valid.\n");
            }
        }

        private List<string> GetListValueFromAllDevicesByQuery(Dictionary<Keys, string> SearchQuery, string ExpectedKey)
        {
            URL = CloudServer + "/api/v1/devices";
            string response = ExecuteGet(URL);
            var token = JToken.Parse(response);
            var roles = token.Value<JArray>("data");
            var count = roles.Count;
            dynamic jObj = JObject.Parse(response);
            List<string> list = new List<string>();
            int listCount = SearchQuery.Count;
            int k = 0;
            for (int i = 0; i < count; i++)
            {
                int j = 0;
                dynamic oneObj = jObj.data[i];
                foreach (var item in SearchQuery.Keys)
                {
                    string ValueFromUser = SearchQuery[item];
                    string ValueFromCloud = (string)oneObj[item.ToString()];
                    if (ValueFromUser.Equals(ValueFromCloud, StringComparison.InvariantCultureIgnoreCase))
                    {
                        j++;
                        if (listCount == j)
                        {
                            var value = oneObj[ExpectedKey];
                            list.Add(value.ToString());
                            k++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (k == 0)
            {
                throw new Exception("\n\nNo Device found in the SeeTest Cloud for your Search Query.\n");
            }
            else
            {
                return list;
            }
        }

        private List<Tuple<string, string, string, string, string>> GetAvailableDevicesListValueFromAllDevicesByQuery(Dictionary<Keys, string> SearchQuery)
        {
            URL = CloudServer + "/api/v1/devices";
            string response = ExecuteGet(URL);
            var token = JToken.Parse(response);
            var roles = token.Value<JArray>("data");
            var count = roles.Count;
            dynamic jObj = JObject.Parse(response);
            List<Tuple<string, string, string, string, string>> list = new List<Tuple<string, string, string, string, string>>();
            int listCount = SearchQuery.Count;
            int k = 0;
            for (int i = 0; i < count; i++)
            {
                int j = 0;
                dynamic oneObj = jObj.data[i];
                foreach (var item in SearchQuery.Keys)
                {
                    string ValueFromUser = SearchQuery[item];
                    string ValueFromCloud = (string)oneObj[item.ToString()];
                    if (ValueFromUser.Equals(ValueFromCloud, StringComparison.InvariantCultureIgnoreCase))
                    {
                        j++;
                        if (listCount == j)
                        {
                            string agentLocation = oneObj.agentLocation;
                            string deviceOs = oneObj.deviceOs;
                            string deviceName = oneObj.deviceName;
                            string model = oneObj.model;
                            string id = oneObj.id;
                            list.Add(Tuple.Create(agentLocation, deviceOs, deviceName, model, id));
                            k++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (k == 0)
            {
                throw new Exception("\n\nNo Device found in the SeeTest Cloud for your Search Query.\n");
            }
            else
            {
                return list;
            }
        }

        private string ThrowIfResponseStatusHasError(IRestResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            int numericStatusCode = (int)statusCode;
            if (numericStatusCode.ToString() == "0")
            {
                throw new Exception("\n\nVerify whether you are connected with your Organization's network(If server runs on organization's network) and the SeeTestCloud Server URL is proper.\n\nOriginal Exception: \n" + response.ErrorMessage + "\n");
            }
            else if (numericStatusCode.ToString() == "401")
            {
                throw new Exception("\n\nVerify your SeeTestCloud Access Key / Login Credentials.\n\nOriginal Exception: \n" + response.Content + "\n");
            }
            else if (numericStatusCode.ToString() == "403")
            {
                throw new Exception("\n\nThis API is NOT available for the current user role. Check \"SeeTest Devices Rest API\" documentation for more details.\n\nOriginal Exception: \n" + response.Content + "\n");
            }
            else if (numericStatusCode.ToString()[0] == '4' | numericStatusCode.ToString()[0] == '5')
            {
                if (response.Content.Length == 0)
                {
                    throw new Exception("\n\n" + response.StatusCode);
                }
                else
                {
                    var charsToRemove = new string[] { "\"", "{", "}" };
                    var token = JToken.Parse(response.Content);
                    string error = null;
                    try
                    {
                        error = token["data"].ToString();
                    }
                    catch (Exception)
                    {
                        error = token["message"].ToString();
                    }
                    finally
                    {
                        if (error == null)
                        {
                            throw new Exception("\n\n" + response.Content + "\n");
                        }
                    }
                    foreach (var c in charsToRemove)
                    {
                        error = error.Replace(c, string.Empty);
                    }
                    throw new Exception("\n\n" + error.Trim() + "\n\nOriginal Exception: \n" + response.Content + "\n");
                }
            }
            else
            {
                return response.Content;
            }
        }

        private List<string> GetValueFromAllDevices(Keys ExpectedKey)
        {
            List<string> output = new List<string>();
            var client = new RestClient(CloudServer + "/api/v1/devices");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + Token);
            IRestResponse response = client.Execute(request);
            var token = JToken.Parse(response.Content);
            var roles = token.Value<JArray>("data");
            var count = roles.Count;
            dynamic jObj = JObject.Parse(response.Content);
            for (int i = 0; i < count; i++)
            {
                output.Add((string)jObj.data[i][ExpectedKey.ToString()]);
            }
            return output;
        }

        private List<string> GetAllDevices(string Key, string Value, string ExpectedKey)
        {
            List<string> output = new List<string>();
            var client = new RestClient(CloudServer + "/api/v1/devices");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + Token);
            IRestResponse response = client.Execute(request);
            var token = JToken.Parse(response.Content);
            var roles = token.Value<JArray>("data");
            var count = roles.Count;
            dynamic jObj = JObject.Parse(response.Content);
            for (int i = 0; i < count; i++)
            {
                string ValueFromCloud = (string)jObj.data[i][Key];
                if (ValueFromCloud.Equals(Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    output.Add((string)jObj.data[i][ExpectedKey]);
                }
            }
            return output;
        }
    }
    //-----------------------------------Public Enum--------------------------------

    /// <summary>
    /// Type of Device. Watch/Tablet/Phone/Unknown.
    /// </summary>
    public enum Category
    {
        /// <summary>
        /// Pass the Device Category as WATCH
        /// </summary>
        WATCH = 0,
        /// <summary>
        /// Pass the Device Category as TABLET
        /// </summary>
        TABLET = 1,
        /// <summary>
        /// Pass the Device Category as PHONE
        /// </summary>
        PHONE = 2,
        /// <summary>
        /// Pass the Device Category as UNKNOWN
        /// </summary>
        UNKNOWN = 3
    }

    /// <summary>
    /// Type of Web Control. Manual/View/Automation/Debug.
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// Pass the Control Type as MANUAL
        /// </summary>
        MANUAL = 0,
        /// <summary>
        /// Pass the Control Type as VIEW
        /// </summary>
        VIEW = 1,
        /// <summary>
        /// Pass the Control Type as AUTOMATION
        /// </summary>
        AUTOMATION = 2,
        /// <summary>
        /// Pass the Control Type as DEBUG
        /// </summary>
        DEBUG = 3
    }


    /// <summary>
    /// Device information as Keys.
    /// </summary>
    public enum Keys
    {
        /// <summary>
        /// Unique Device ID provided by SeeTest Cloud.
        /// </summary>
        id = 1,
        /// <summary>
        /// UDID of the Device.Incase of Android, it's Serial number.
        /// </summary>
        udid = 2,
        /// <summary>
        /// (Only for iOS) - iOS UDID of the Device provided by SeeTest Cloud.
        /// </summary>
        iosUdid = 3,
        /// <summary>
        /// Name of the Device.
        /// </summary>
        deviceName = 4,
        /// <summary>
        /// Notes of the Device.
        /// </summary>
        notes = 5,
        /// <summary>
        /// OS Type of the Device. Android/iOS.
        /// </summary>
        deviceOs = 6,
        /// <summary>
        /// OS Version of the Device.
        /// </summary>
        osVersion = 7,
        /// <summary>
        /// Model of the Device.
        /// </summary>
        model = 8,
        /// <summary>
        /// Manufacturer of the Device.
        /// </summary>
        manufacturer = 9,
        /// <summary>
        /// Current User of the Device.
        /// </summary>
        currentUser = 10,
        /// <summary>
        /// Category of the Device. Watch/Tablet/Phone/Unknown.
        /// </summary>
        deviceCategory = 11,
        /// <summary>
        /// Up time of the Device.
        /// </summary>
        uptime = 12,
        /// <summary>
        /// Is the Device Emulator?
        /// </summary>
        isEmulator = 13,
        /// <summary>
        /// Profiles of the Device.
        /// </summary>
        profiles = 14,
        /// <summary>
        /// Agent Name of the Device where it's connected.
        /// </summary>
        agentName = 15,
        /// <summary>
        /// Agent IP of the Device where it's connected.
        /// </summary>
        agentIp = 16,
        /// <summary>
        /// Agent Location of the Device where it's connected.
        /// </summary>
        agentLocation = 17,
        /// <summary>
        /// currentStatus of the Device. Online/Offline.
        /// </summary>
        currentStatus = 18,
        /// <summary>
        /// statusTooltip of the Device.
        /// </summary>
        statusTooltip = 19,
        /// <summary>
        /// lastUsedDateTime of the Device.
        /// </summary>
        lastUsedDateTime = 20,
        /// <summary>
        /// previousStatus of the Device.
        /// </summary>
        previousStatus = 21,
        /// <summary>
        /// statusAgeInMinutes of the Device.
        /// </summary>
        statusAgeInMinutes = 22,
        /// <summary>
        /// statusModifiedAt of the Device.
        /// </summary>
        statusModifiedAt = 23,
        /// <summary>
        /// statusModifiedAtDateTime of the Device.
        /// </summary>
        statusModifiedAtDateTime = 24,
        /// <summary>
        /// displayStatus of the Device. Available/In-Use/Offline.
        /// </summary>
        displayStatus = 25,
        /// <summary>
        /// Is Cleanup Enabled for the device?
        /// </summary>
        isCleanupEnabled = 26,
        /// <summary>
        /// Default Language of the Device.
        /// </summary>
        defaultDeviceLanguage = 27,
        /// <summary>
        /// Default Region of the Device.
        /// </summary>
        defaultDeviceRegion = 28,
        /// <summary>
        /// Screen Width of the Device.
        /// </summary>
        screenWidth = 29,
        /// <summary>
        /// Screen Height of the Device.
        /// </summary>
        screenHeight = 30
    }

    /// <summary>
    /// Type of Operating System. Android/iOS.
    /// </summary>
    public enum OSType
    {
        /// <summary>
        /// Pass the OS Type as Android
        /// </summary>
        Android = 0,
        /// <summary>
        /// Pass the OS Type as iOS
        /// </summary>
        iOS = 1
    }
}
