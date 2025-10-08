-- =====================================================
-- SCRIPT MAESTRO: Instalación Completa BD Sistema Biblioteca Escolar
-- =====================================================
-- Este script ejecuta todos los scripts de creación en orden
--
-- ADVERTENCIA: Esto eliminará la base de datos SeguridadBiblioteca
-- y la recreará desde cero
-- =====================================================

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║  INSTALACIÓN BD - SISTEMA BIBLIOTECA ESCOLAR      ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Este script ejecutará:'
PRINT '  1. Crear base de datos SeguridadBiblioteca'
PRINT '  2. Crear tablas del sistema'
PRINT '  3. Insertar datos iniciales'
PRINT ''
PRINT 'ADVERTENCIA: La base SeguridadBiblioteca será eliminada si existe'
PRINT ''
PRINT 'Presione Ctrl+C para cancelar o continúe...'
PRINT ''
WAITFOR DELAY '00:00:03';
PRINT 'Iniciando instalación...'
PRINT ''
PRINT '════════════════════════════════════════════════════'
GO

-- =====================================================
-- PASO 1: Crear Base de Datos
-- =====================================================
PRINT ''
PRINT '▶ PASO 1: Creando Base de Datos...'
PRINT '════════════════════════════════════════════════════'
:r "01_CrearBaseDatos.sql"

-- =====================================================
-- PASO 2: Crear Tablas
-- =====================================================
PRINT ''
PRINT '▶ PASO 2: Creando Tablas...'
PRINT '════════════════════════════════════════════════════'
:r "02_CrearTablas.sql"

-- =====================================================
-- PASO 3: Datos Iniciales
-- =====================================================
PRINT ''
PRINT '▶ PASO 3: Insertando Datos Iniciales...'
PRINT '════════════════════════════════════════════════════'
:r "03_DatosIniciales.sql"

-- =====================================================
-- PASO 4: Crear Stored Procedures
-- =====================================================
PRINT ''
PRINT '▶ PASO 4: Creando Stored Procedures...'
PRINT '════════════════════════════════════════════════════'
:r "05_CrearStoredProcedures.sql"

-- =====================================================
-- FINALIZACIÓN
-- =====================================================
PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║          INSTALACIÓN COMPLETADA CON ÉXITO         ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Base de datos: SeguridadBiblioteca'
PRINT 'Usuario admin: admin'
PRINT 'Password admin: admin123'
PRINT ''
PRINT '⚠ IMPORTANTE: Cambiar la contraseña del admin en producción'
PRINT ''
PRINT 'Verificar instalación:'
PRINT '  SELECT * FROM Usuario;'
PRINT '  SELECT * FROM Familia WHERE Nombre LIKE ''ROL_%'';'
PRINT ''
GO
