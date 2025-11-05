-- =====================================================
-- SCRIPT MAESTRO: Instalación Completa BD VetCare (Negocio)
-- =====================================================
-- Este script ejecuta todos los scripts de creación en orden
--
-- ADVERTENCIA: Esto eliminará la base de datos VetCareDB
-- y la recreará desde cero
-- =====================================================

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║  INSTALACIÓN BD - VETCARE (MÓDULO NEGOCIO)       ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Este script ejecutará:'
PRINT '  1. Crear base de datos VetCareDB'
PRINT '  2. Crear tablas Cliente, Mascota, Cita y Veterinario'
PRINT '  3. Crear Stored Procedures'
PRINT ''
PRINT 'ADVERTENCIA: La base VetCareDB será eliminada si existe'
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
PRINT '▶ PASO 1: Creando Base de Datos VetCareDB...'
PRINT '════════════════════════════════════════════════════'
:r "14_CrearBaseDatosNegocio.sql"

-- =====================================================
-- PASO 2: Crear Tablas
-- =====================================================
PRINT ''
PRINT '▶ PASO 2: Creando Tablas...'
PRINT '════════════════════════════════════════════════════'
:r "15_CrearTablasNegocio.sql"

-- =====================================================
-- PASO 3: Crear Stored Procedures
-- =====================================================
PRINT ''
PRINT '▶ PASO 3: Creando Stored Procedures...'
PRINT '════════════════════════════════════════════════════'
:r "16_CrearSP_Negocio.sql"

-- =====================================================
-- PASO 4: Crear Tabla Cita
-- =====================================================
PRINT ''
PRINT '▶ PASO 4: Creando Tabla Cita...'
PRINT '════════════════════════════════════════════════════'
:r "17_CrearTablaCita.sql"

-- =====================================================
-- PASO 5: Crear Stored Procedures de Cita
-- =====================================================
PRINT ''
PRINT '▶ PASO 5: Creando Stored Procedures de Cita...'
PRINT '════════════════════════════════════════════════════'
:r "18_CrearSP_Cita.sql"

-- =====================================================
-- PASO 6: Crear Tabla Veterinario
-- =====================================================
PRINT ''
PRINT '▶ PASO 6: Creando Tabla Veterinario...'
PRINT '════════════════════════════════════════════════════'
:r "21_CrearTablaVeterinario.sql"

-- =====================================================
-- PASO 7: Modificar Tabla Cita (agregar IdVeterinario)
-- =====================================================
PRINT ''
PRINT '▶ PASO 7: Modificando Tabla Cita (agregar IdVeterinario)...'
PRINT '════════════════════════════════════════════════════'
:r "22_ModificarTablaCitaAgregarVeterinario.sql"

-- =====================================================
-- PASO 8: Crear Stored Procedures de Veterinario
-- =====================================================
PRINT ''
PRINT '▶ PASO 8: Creando Stored Procedures de Veterinario...'
PRINT '════════════════════════════════════════════════════'
:r "23_CrearSP_Veterinario.sql"

-- =====================================================
-- PASO 9: Crear Tabla Medicamento
-- =====================================================
PRINT ''
PRINT '▶ PASO 9: Creando Tabla Medicamento...'
PRINT '════════════════════════════════════════════════════'
:r "24_CrearTablaMedicamento.sql"

-- =====================================================
-- PASO 10: Crear Tablas ConsultaMedica y ConsultaMedicamento
-- =====================================================
PRINT ''
PRINT '▶ PASO 10: Creando Tablas ConsultaMedica y ConsultaMedicamento...'
PRINT '════════════════════════════════════════════════════'
:r "25_CrearTablaConsultaMedica.sql"

-- =====================================================
-- PASO 11: Crear Stored Procedures de Medicamento
-- =====================================================
PRINT ''
PRINT '▶ PASO 11: Creando Stored Procedures de Medicamento...'
PRINT '════════════════════════════════════════════════════'
:r "26_CrearSP_Medicamento.sql"

-- =====================================================
-- PASO 12: Crear Stored Procedures de ConsultaMedica
-- =====================================================
PRINT ''
PRINT '▶ PASO 12: Creando Stored Procedures de ConsultaMedica...'
PRINT '════════════════════════════════════════════════════'
:r "27_CrearSP_ConsultaMedica.sql"

-- =====================================================
-- FINALIZACIÓN
-- =====================================================
PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║          INSTALACIÓN COMPLETADA CON ÉXITO         ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Base de datos: VetCareDB'
PRINT 'Tablas: Cliente, Mascota, Cita, Veterinario, Medicamento, ConsultaMedica, ConsultaMedicamento'
PRINT 'Stored Procedures: 54 (7 Cliente, 7 Mascota, 11 Cita, 8 Veterinario, 8 Medicamento, 13 ConsultaMedica)'
PRINT ''
PRINT 'Verificar instalación:'
PRINT '  USE VetCareDB;'
PRINT '  SELECT * FROM Cliente;'
PRINT '  SELECT * FROM Mascota;'
PRINT '  SELECT * FROM Cita;'
PRINT '  SELECT * FROM Veterinario;'
PRINT '  SELECT * FROM Medicamento;'
PRINT '  SELECT * FROM ConsultaMedica;'
PRINT '  SELECT * FROM ConsultaMedicamento;'
PRINT ''
PRINT 'Connection String sugerido:'
PRINT '  Data Source=localhost;Initial Catalog=VetCareDB;'
PRINT '  Integrated Security=True;TrustServerCertificate=True'
PRINT ''
GO
