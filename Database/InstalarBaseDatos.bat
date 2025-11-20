@echo off
REM ═══════════════════════════════════════════════════════════════════
REM   INSTALADOR DE BASES DE DATOS - VETCARE
REM ═══════════════════════════════════════════════════════════════════

echo.
echo ═══════════════════════════════════════════════════════════════════
echo   INSTALACION DE BASES DE DATOS - VETCARE
echo ═══════════════════════════════════════════════════════════════════
echo.

REM Verificar que se ejecuta como administrador
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo ERROR: Este script debe ejecutarse como Administrador
    echo.
    echo Haga click derecho en el archivo y seleccione:
    echo "Ejecutar como administrador"
    echo.
    pause
    exit /b 1
)

REM Solicitar servidor SQL
set /p SQLSERVER="Ingrese el servidor SQL (por defecto: localhost): "
if "%SQLSERVER%"=="" set SQLSERVER=localhost

echo.
echo Servidor SQL configurado: %SQLSERVER%
echo.
echo Instalando bases de datos...
echo.

REM Verificar que sqlcmd existe
where sqlcmd >nul 2>&1
if %errorLevel% neq 0 (
    echo ERROR: sqlcmd no esta instalado
    echo.
    echo Instale SQL Server Command Line Utilities desde:
    echo https://aka.ms/sqlcmd
    echo.
    pause
    exit /b 1
)

REM Ejecutar scripts en orden
echo [1/3] Instalando SecurityVet...
sqlcmd -S %SQLSERVER% -i "00_EJECUTAR_TODO.sql" -b
if %errorLevel% neq 0 (
    echo.
    echo ERROR: Fallo la instalacion de SecurityVet
    echo.
    pause
    exit /b 1
)
echo OK - SecurityVet instalado correctamente
echo.

echo [2/3] Instalando VetCareDB...
sqlcmd -S %SQLSERVER% -i "00_EJECUTAR_TODO_NEGOCIO.sql" -b
if %errorLevel% neq 0 (
    echo.
    echo ERROR: Fallo la instalacion de VetCareDB
    echo.
    pause
    exit /b 1
)
echo OK - VetCareDB instalado correctamente
echo.

echo [3/3] Instalando Bitacora...
sqlcmd -S %SQLSERVER% -i "40_EJECUTAR_TODO_BITACORA.sql" -b
if %errorLevel% neq 0 (
    echo.
    echo ERROR: Fallo la instalacion de Bitacora
    echo.
    pause
    exit /b 1
)
echo OK - Bitacora instalado correctamente
echo.

echo ═══════════════════════════════════════════════════════════════════
echo   INSTALACION COMPLETADA EXITOSAMENTE
echo ═══════════════════════════════════════════════════════════════════
echo.
echo Bases de datos instaladas:
echo   - SecurityVet
echo   - VetCareDB
echo.
echo Credenciales por defecto:
echo   Usuario: admin
echo   Contraseña: admin123
echo.
echo IMPORTANTE: Cambie la contraseña del administrador despues del
echo primer inicio de sesion.
echo.
echo Ya puede ejecutar VetCare.
echo.
pause
