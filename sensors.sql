CREATE DATABASE IF NOT EXISTS Sensors;
USE Sensors;

drop table if exists SensorReadings;
CREATE TABLE IF NOT EXISTS SensorReadings(
	Time_Of_Reading DATETIME PRIMARY KEY,
    RAM_Used_Percent DECIMAL(5, 2),
    CPU_Max_Temp DECIMAL(5, 1),
    CPU_Temp DECIMAL(5, 1),
    CPU_Usage DECIMAL(5, 2),
    Storage_Used_Percent DECIMAL(5, 2),
    Wifi_Up DECIMAL(5, 2),
    Wifi_Down DECIMAL(5, 2),
    Ethernet_Up DECIMAL(5, 2),
    Ethernet_Down DECIMAL(5, 2)
    
);

DROP TABLE IF EXISTS WarningLogs;
CREATE TABLE IF NOT EXISTS WarningLogs(
	Time_Of_Logging DATETIME,
    Warning_Type VARCHAR(50),
    Warning_Message VARCHAR(100)
);

select * from SensorReadings;

select * from WarningLogs;
