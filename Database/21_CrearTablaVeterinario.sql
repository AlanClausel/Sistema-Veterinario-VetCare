/*******************************************************************
 * Script: 21_CrearTablaVeterinario.sql
 * Descripción: Crea la tabla Veterinario para gestión de veterinarios
 * Fecha: 2025-01-19
 * Base de Datos: VetCareDB
 *
 * IMPORTANTE:
 * - IdVeterinario coincide con IdUsuario de SecurityVet
 * - El campo Nombre se sincroniza desde la tabla Usuario de SecurityVet
 * - Solo usuarios con rol "ROL_Veterinario" deberían estar en esta tabla
 * - Los veterinarios se gestionan desde "Gestión de Usuarios"
 * - Este registro se crea automáticamente al asignar el rol ROL_Veterinario
 *******************************************************************/

USE VetCareDB;
GO

-- Eliminar tabla si existe (solo para desarrollo)
IF OBJECT_ID('Veterinario', 'U') IS NOT NULL
    DROP TABLE Veterinario;
GO

-- Crear tabla Veterinario
CREATE TABLE Veterinario
(
    IdVeterinario UNIQUEIDENTIFIER PRIMARY KEY,  -- Mismo GUID que IdUsuario en SecurityVet
    Nombre NVARCHAR(150) NOT NULL,               -- Sincronizado desde SecurityVet (solo lectura)
    Matricula NVARCHAR(50),                      -- Número de matrícula profesional (opcional)
    Telefono NVARCHAR(20),                       -- Teléfono de contacto del consultorio (opcional)
    Email NVARCHAR(100),                         -- Email profesional (opcional)
    Observaciones NVARCHAR(500),                 -- Notas adicionales
    FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT UQ_Veterinario_Matricula UNIQUE (Matricula)
);
GO

-- Índices para optimizar búsquedas
CREATE INDEX IX_Veterinario_Nombre ON Veterinario(Nombre);
CREATE INDEX IX_Veterinario_Activo ON Veterinario(Activo);
GO

PRINT 'Tabla Veterinario creada exitosamente';
GO
