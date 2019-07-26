@echo off

setlocal enabledelayedexpansion

set Mysqlpath=C:\Program Files\MySQL\MySQL Server 5.7\bin
net stop MySQL57



echo %Mysqlpath%
cd /D %Mysqlpath%
echo %cd%




echo on
