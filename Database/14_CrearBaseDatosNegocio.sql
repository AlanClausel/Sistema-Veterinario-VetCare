-- =====================================================
-- Script: Crear Base de Datos de Negocio - VetCare
-- Descripción: Crea la base de datos VetCareDB
-- =====================================================

USE master;
GO

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║   CREACIÓN BASE DE DATOS NEGOCIO - VETCARE       ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''

-- =====================================================
-- Verificar y eliminar base de datos existente
-- =====================================================
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'VetCareDB')
BEGIN
    PRINT 'Eliminando base de datos VetCareDB existente...'

    ALTER DATABASE VetCareDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE VetCareDB;

    PRINT '✓ Base de datos VetCareDB eliminada'
    PRINT ''
END

-- =====================================================
-- Crear base de datos
-- =====================================================
PRINT 'Creando base de datos VetCareDB...'

CREATE DATABASE VetCareDB;
GO

PRINT '✓ Base de datos VetCareDB creada exitosamente'
PRINT ''

-- =====================================================
-- Configurar base de datos
-- =====================================================
USE VetCareDB;
GO

-- Establecer opciones de base de datos
ALTER DATABASE VetCareDB SET RECOVERY SIMPLE;
ALTER DATABASE VetCareDB SET AUTO_SHRINK OFF;
ALTER DATABASE VetCareDB SET AUTO_CREATE_STATISTICS ON;
ALTER DATABASE VetCareDB SET AUTO_UPDATE_STATISTICS ON;

PRINT '✓ Configuración de base de datos completada'
PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║         BASE DE DATOS CREADA CON ÉXITO           ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Nombre: VetCareDB'
PRINT 'Estado: Listo para crear tablas'
PRINT ''
GO
