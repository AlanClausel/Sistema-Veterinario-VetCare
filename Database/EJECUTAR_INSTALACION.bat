@echo off
echo ========================================
echo Instalacion Base de Datos
echo Sistema Biblioteca Escolar
echo ========================================
echo.
echo Este script recreara la base de datos SeguridadBiblioteca
echo ADVERTENCIA: Se eliminaran todos los datos existentes
echo.
pause
echo.
echo Ejecutando scripts...
sqlcmd -S localhost -E -i "00_EJECUTAR_TODO.sql"
echo.
echo ========================================
echo Instalacion completada
echo ========================================
echo.
echo Credenciales de acceso:
echo Usuario: admin
echo Password: admin123
echo.
pause
