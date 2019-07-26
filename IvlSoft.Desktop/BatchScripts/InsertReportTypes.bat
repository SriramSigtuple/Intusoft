@echo off
mode 1000
title Report Types  
color 17

setlocal enabledelayedexpansion
set dbusername=0
set dbpassword=0
set dbname=0
set count=0
set Mysqlpath=C:\Program Files\MySQL\MySQL Server 5.7\bin

for /f "tokens=1-3 delims==" %%i IN (Intusoft-runtime.properties) DO (
if "%%i"=="connection.username" (
set dbusername=%%j
)
if "%%i"=="connection.password" (
set dbpassword=%%j
)
if "%%i"=="connection.DBname" (
set dbname=%%j
)
)

echo !dbusername!

echo !dbpassword!

echo !dbname!

echo %Mysqlpath%
cd /D %Mysqlpath%
echo %cd%

mysql -u !dbusername! -p!dbpassword! -D !dbname! -e "INSERT INTO `report_type` (`report_type_id`, `name`, `description`, `uuid`) VALUES (1, 'Fundus Image Report', 'Report for fundus image.', 'd38849b7-6ee9-43a4-8a1d-9d27fdae8656');"
mysql -u !dbusername! -p!dbpassword! -D !dbname! -e "INSERT INTO `report_type` (`report_type_id`, `name`, `description`, `uuid`) VALUES (2, 'Annotation Report', 'Report for annotation.', '27dac695-d6f3-41c1-87e0-d525b07bbcc7');"
mysql -u !dbusername! -p!dbpassword! -D !dbname! -e "INSERT INTO `report_type` (`report_type_id`, `name`, `description`, `uuid`) VALUES (3, 'Glaucoma Analysis Report', 'Report for CDR.', 'b09445ba-2838-49cd-970a-f9cc0c965a82');"

IF %ERRORLEVEL% EQU 1 (
  echo Data already exists
)

IF %ERRORLEVEL% EQU 0 (
  echo Data has been added successfully
)


pause 
color 07
color 07
echo on
