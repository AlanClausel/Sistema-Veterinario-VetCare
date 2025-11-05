/*******************************************************************
 * Script: 22_ModificarTablaCitaAgregarVeterinario.sql
 * Descripción: Modifica la tabla Cita para agregar relación con Veterinario
 * Fecha: 2025-01-19
 * Base de Datos: VetCareDB
 *
 * IMPORTANTE:
 * - Agrega campo IdVeterinario como FK a tabla Veterinario
 * - Mantiene campo Veterinario (NVARCHAR) temporalmente para compatibilidad
 * - En nuevas citas, se debe usar IdVeterinario
 * - El campo Veterinario (string) se puede eliminar en el futuro
 *******************************************************************/

USE VetCareDB;
GO

-- Verificar que existe la tabla Veterinario
IF OBJECT_ID('Veterinario', 'U') IS NULL
BEGIN
    PRINT 'ERROR: Debe ejecutar primero 21_CrearTablaVeterinario.sql';
    RETURN;
END
GO

-- Agregar columna IdVeterinario si no existe
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Cita') AND name = 'IdVeterinario')
BEGIN
    ALTER TABLE Cita
    ADD IdVeterinario UNIQUEIDENTIFIER NULL;  -- NULL para compatibilidad con datos existentes

    PRINT 'Columna IdVeterinario agregada a tabla Cita';
END
ELSE
BEGIN
    PRINT 'La columna IdVeterinario ya existe en tabla Cita';
END
GO

-- Crear FK a Veterinario
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Cita_Veterinario')
BEGIN
    ALTER TABLE Cita
    ADD CONSTRAINT FK_Cita_Veterinario FOREIGN KEY (IdVeterinario)
        REFERENCES Veterinario(IdVeterinario);

    PRINT 'Foreign key FK_Cita_Veterinario creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La foreign key FK_Cita_Veterinario ya existe';
END
GO

-- Crear índice en IdVeterinario
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Cita_IdVeterinario')
BEGIN
    CREATE INDEX IX_Cita_IdVeterinario ON Cita(IdVeterinario);
    PRINT 'Índice IX_Cita_IdVeterinario creado exitosamente';
END
ELSE
BEGIN
    PRINT 'El índice IX_Cita_IdVeterinario ya existe';
END
GO

-- Hacer el campo Veterinario (string) nullable para nuevas citas
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Cita') AND name = 'Veterinario' AND is_nullable = 0)
BEGIN
    ALTER TABLE Cita
    ALTER COLUMN Veterinario NVARCHAR(150) NULL;

    PRINT 'Columna Veterinario modificada a nullable';
END
GO

PRINT 'Modificación de tabla Cita completada exitosamente';
PRINT 'NOTA: Las nuevas citas deben usar IdVeterinario en lugar del campo Veterinario (string)';
GO
