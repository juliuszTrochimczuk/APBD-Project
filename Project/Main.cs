using Project;

const string filePath = "..\\..\\..\\input.txt";

DeviceManager deviceManager = DeviceManager.Factory.CreateDeviceManager(filePath);
deviceManager.ShowAllDevices();


/*deviceManager.AddDevice("SW-2,Xiamomi MI 8,False, 74%");
deviceManager.ShowAllDevices();
deviceManager.SaveDevicesToFile();*/
