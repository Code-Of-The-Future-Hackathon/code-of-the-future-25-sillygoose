@echo off
for /l %%x in (1, 1, 100) do (
   start %cd%/bin/Debug/net8.0/TcpClientApp.exe
)