using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class DeviceManager
    {
        private List<Device> allDevices = new();

        public DeviceManager(string filePath) 
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            foreach (string line in File.ReadLines(filePath))
            {
                string[] values = line.Split(',');
                if (values[0].Contains("SW"))
                {
                    Console.WriteLine("It's smartwatch");
                }
                else if (values[0].Contains('P'))
                {
                    Console.WriteLine("It's personal Computer");
                }
                else if (values[0].Contains("ED"))
                {
                    Console.WriteLine("It's embedded device");
                }
                else
                    continue;

                Console.WriteLine("[{0}]", string.Join(", ", values));
            }
        }
    }
}
