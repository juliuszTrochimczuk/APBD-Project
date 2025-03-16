using Project;

const string filePath = "..\\..\\..\\input.txt";

DeviceManager deviceManager = new DeviceManager(filePath);
deviceManager.ShowAllDevices();