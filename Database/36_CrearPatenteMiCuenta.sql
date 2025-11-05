-- =============================================
-- Script: 36_CrearPatenteMiCuenta.sql
-- Descripción: Crea la patente "Mi Cuenta" y la asigna
--              a TODOS los roles (Administrador, Veterinario, Recepcionista)
-- Base de datos: SecurityVet
-- =============================================

USE SecurityVet;
GO

PRINT '════════════════════════════════════════════════════'
PRINT 'Creando patente Mi Cuenta...'
PRINT '════════════════════════════════════════════════════'

-- =============================================
-- Variables para IDs
-- =============================================
DECLARE @IdPatenteMiCuenta UNIQUEIDENTIFIER = NEWID();
DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER;
DECLARE @IdFamiliaVeterinario UNIQUEIDENTIFIER;
DECLARE @IdFamiliaRecepcionista UNIQUEIDENTIFIER;

-- Obtener IDs de las familias (roles)
SELECT @IdFamiliaAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';
SELECT @IdFamiliaVeterinario = IdFamilia FROM Familia WHERE Nombre = 'ROL_Veterinario';
SELECT @IdFamiliaRecepcionista = IdFamilia FROM Familia WHERE Nombre = 'ROL_Recepcionista';

-- =============================================
-- Crear Patente: Mi Cuenta
-- =============================================
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'FormMiCuenta')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (
        @IdPatenteMiCuenta,
        'FormMiCuenta',
        'Mi Cuenta',
        300,
        'Permite al usuario ver su información personal, cambiar su contraseña y configurar su idioma'
    );

    PRINT '✓ Patente FormMiCuenta creada';
END
ELSE
BEGIN
    SELECT @IdPatenteMiCuenta = IdPatente FROM Patente WHERE FormName = 'FormMiCuenta';
    PRINT '✓ Patente FormMiCuenta ya existe';
END

-- =============================================
-- Asignar Patente a ROL_Administrador
-- =============================================
IF @IdFamiliaAdmin IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaAdmin AND idPatente = @IdPatenteMiCuenta)
    BEGIN
        INSERT INTO FamiliaPatente (idFamilia, idPatente)
        VALUES (@IdFamiliaAdmin, @IdPatenteMiCuenta);

        PRINT '✓ Patente Mi Cuenta asignada a ROL_Administrador';
    END
    ELSE
    BEGIN
        PRINT '✓ Patente Mi Cuenta ya estaba asignada a ROL_Administrador';
    END
END

-- =============================================
-- Asignar Patente a ROL_Veterinario
-- =============================================
IF @IdFamiliaVeterinario IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaVeterinario AND idPatente = @IdPatenteMiCuenta)
    BEGIN
        INSERT INTO FamiliaPatente (idFamilia, idPatente)
        VALUES (@IdFamiliaVeterinario, @IdPatenteMiCuenta);

        PRINT '✓ Patente Mi Cuenta asignada a ROL_Veterinario';
    END
    ELSE
    BEGIN
        PRINT '✓ Patente Mi Cuenta ya estaba asignada a ROL_Veterinario';
    END
END

-- =============================================
-- Asignar Patente a ROL_Recepcionista
-- =============================================
IF @IdFamiliaRecepcionista IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteMiCuenta)
    BEGIN
        INSERT INTO FamiliaPatente (idFamilia, idPatente)
        VALUES (@IdFamiliaRecepcionista, @IdPatenteMiCuenta);

        PRINT '✓ Patente Mi Cuenta asignada a ROL_Recepcionista';
    END
    ELSE
    BEGIN
        PRINT '✓ Patente Mi Cuenta ya estaba asignada a ROL_Recepcionista';
    END
END

-- =============================================
-- Resumen
-- =============================================
PRINT ''
PRINT '════════════════════════════════════════════════════'
PRINT 'Patente Mi Cuenta creada exitosamente'
PRINT '════════════════════════════════════════════════════'
PRINT ''
PRINT 'Patente: FormMiCuenta'
PRINT 'Nombre en menú: Mi Cuenta'
PRINT 'Orden: 300'
PRINT ''
PRINT 'Asignada a TODOS los roles:'
PRINT '  - ROL_Administrador'
PRINT '  - ROL_Veterinario'
PRINT '  - ROL_Recepcionista'
PRINT ''
PRINT 'Funcionalidades:'
PRINT '  - Ver información del usuario'
PRINT '  - Cambiar contraseña'
PRINT '  - Configurar idioma (Español/Inglés)'
PRINT ''

GO
