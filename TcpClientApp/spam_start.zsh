#!/bin/zsh
for ((i=0; i<40000; i++;)) 
do
   start $PWD/bin/Debug/net8.0/TcpClientApp
done
