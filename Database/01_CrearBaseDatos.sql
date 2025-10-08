-- =====================================================
-- Script: Crear Base de Datos - Sistema Biblioteca Escolar
-- Descripción: Crea la base de datos SeguridadBiblioteca desde cero
-- =====================================================

USE master;
GO

-- Eliminar base de datos si existe
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SeguridadBiblioteca')
BEGIN
    ALTER DATABASE SeguridadBiblioteca SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SeguridadBiblioteca;
    PRINT '✓ Base de datos SeguridadBiblioteca eliminada'
END
GO

-- Crear base de datos
CREATE DATABASE SeguridadBiblioteca;
GO

PRINT '✓ Base de datos SeguridadBiblioteca creada exitosamente'
PRINT ''
GO

USE SeguridadBiblioteca;
GO
