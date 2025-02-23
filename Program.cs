using System;
using LibreHardwareMonitor.Hardware;
using MySql.Data.MySqlClient;

public class DataCollection
{
    static void Main()
    {
        Computer computer = new Computer
        {
            IsCpuEnabled = true,
            IsStorageEnabled = true,
            IsMemoryEnabled = true,
            IsNetworkEnabled = true
        };

        computer.Open();

        float? core1_temp=0, core1_dist_max=0;
        float? cpu_max_temp = 0;
        float? cpu_avg_temp = 0;
        float? cpu_usage = 0;

        float? used_memory_percent = 0;

        float? used_storage_percent = 0;

        float? wifi_up = 0, wifi_down = 0;
        float? ethernet_up = 0, ethernet_down = 0;

        string connectionString = "server=localhost;user=root;password=mamati1;database=Sensors";

        using MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();

        while(true)
        {
            foreach (IHardware hardware in computer.Hardware)
            {
                hardware.Update();
                //Console.WriteLine($"Hardware: {hardware.Name} ({hardware.HardwareType})");

                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.Name == "Memory")
                    {
                        used_memory_percent = sensor.Value;
                    }
                    if (sensor.Name == "Used Space")
                    {
                        used_storage_percent = sensor.Value;
                    }
                    if (sensor.SensorType == SensorType.Temperature && sensor.Name == "CPU Core #1")
                    {
                        core1_temp = sensor.Value;
                    }
                    if (sensor.SensorType == SensorType.Temperature && sensor.Name == "CPU Core #1 Distance to TjMax")
                    {
                        core1_dist_max = sensor.Value;
                    }
                    if (sensor.SensorType == SensorType.Temperature && sensor.Name == "Core Average")
                    {
                        cpu_avg_temp = sensor.Value;
                    }
                    if (sensor.Name == "CPU Total")
                    {
                        cpu_usage = sensor.Value;
                    }
                    if (hardware.Name.Contains("WiFi") && sensor.Name == "Data Uploaded")
                    {
                        wifi_up = sensor.Value;
                    }
                    if (hardware.Name.Contains("WiFi") && sensor.Name == "Data Downloaded")
                    {
                        wifi_down = sensor.Value;
                    }
                    if (hardware.Name.Contains("Ethernet") && sensor.Name == "Data Uploaded")
                    {
                        ethernet_up = sensor.Value;
                    }
                    if (hardware.Name.Contains("Ethernet") && sensor.Name == "Data Downloaded")
                    {
                        ethernet_down = sensor.Value;
                    }

                    cpu_max_temp = core1_temp + core1_dist_max;



                }

            }

            string query = "INSERT INTO SensorReadings (Time_Of_Reading, RAM_Used_Percent, CPU_Max_Temp, CPU_Temp, CPU_Usage, " +
                        "Storage_Used_Percent, Wifi_Up, Wifi_Down, Ethernet_Up, Ethernet_Down) " +
                        "VALUES (@Time_Of_Reading, @RAM_Used_Percent, @CPU_Max_Temp, @CPU_Temp, @CPU_Usage, @Storage_Used_Percent, " +
                        "@Wifi_Up, @Wifi_Down, @Ethernet_Up, @Ethernet_Down)";

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Time_Of_Reading", DateTime.Now);
            cmd.Parameters.AddWithValue("@RAM_Used_Percent", used_memory_percent);
            cmd.Parameters.AddWithValue("@CPU_Max_Temp", cpu_max_temp);
            cmd.Parameters.AddWithValue("@CPU_Temp", cpu_avg_temp);
            cmd.Parameters.AddWithValue("@CPU_Usage", cpu_usage);
            cmd.Parameters.AddWithValue("@Storage_Used_Percent", used_storage_percent);
            cmd.Parameters.AddWithValue("@Wifi_Up", wifi_up);
            cmd.Parameters.AddWithValue("@Wifi_Down", wifi_down);
            cmd.Parameters.AddWithValue("@Ethernet_Up", ethernet_up);
            cmd.Parameters.AddWithValue("@Ethernet_Down", ethernet_down);

            int rowsAffected = cmd.ExecuteNonQuery();
            Console.WriteLine(rowsAffected);

            Thread.Sleep(2000);
        }
        

        Console.WriteLine();

        computer.Close();
        conn.Close();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}