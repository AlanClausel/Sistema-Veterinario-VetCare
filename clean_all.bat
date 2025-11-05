@echo off
echo ========================================
echo Limpiando todas las carpetas bin y obj
echo ========================================

cd /d "%~dp0"

echo.
echo Limpiando UI...
if exist "UI\bin" rmdir /s /q "UI\bin" && echo   - UI\bin eliminado
if exist "UI\obj" rmdir /s /q "UI\obj" && echo   - UI\obj eliminado

echo.
echo Limpiando VetCareNegocio\BLL...
if exist "VetCareNegocio\BLL\bin" rmdir /s /q "VetCareNegocio\BLL\bin" && echo   - BLL\bin eliminado
if exist "VetCareNegocio\BLL\obj" rmdir /s /q "VetCareNegocio\BLL\obj" && echo   - BLL\obj eliminado

echo.
echo Limpiando VetCareNegocio\DAL...
if exist "VetCareNegocio\DAL\bin" rmdir /s /q "VetCareNegocio\DAL\bin" && echo   - DAL\bin eliminado
if exist "VetCareNegocio\DAL\obj" rmdir /s /q "VetCareNegocio\DAL\obj" && echo   - DAL\obj eliminado

echo.
echo Limpiando VetCareNegocio\DomainModel...
if exist "VetCareNegocio\DomainModel\bin" rmdir /s /q "VetCareNegocio\DomainModel\bin" && echo   - DomainModel\bin eliminado
if exist "VetCareNegocio\DomainModel\obj" rmdir /s /q "VetCareNegocio\DomainModel\obj" && echo   - DomainModel\obj eliminado

echo.
echo Limpiando VetCareNegocio\Services...
if exist "VetCareNegocio\Services\bin" rmdir /s /q "VetCareNegocio\Services\bin" && echo   - Services\bin eliminado
if exist "VetCareNegocio\Services\obj" rmdir /s /q "VetCareNegocio\Services\obj" && echo   - Services\obj eliminado

echo.
echo Limpiando ServicesSeguridad...
if exist "ServicesSeguridad\bin" rmdir /s /q "ServicesSeguridad\bin" && echo   - ServicesSeguridad\bin eliminado
if exist "ServicesSeguridad\obj" rmdir /s /q "ServicesSeguridad\obj" && echo   - ServicesSeguridad\obj eliminado

echo.
echo ========================================
echo Limpieza completa!
echo ========================================
echo.
echo Ahora abre Visual Studio y haz Rebuild Solution
pause
