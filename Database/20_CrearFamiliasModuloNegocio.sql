/*******************************************************************
 * Script: 20_CrearFamiliasModuloNegocio.sql
 * Descripción: Crea Familias (grupos) de permisos para el módulo de negocio
 * Fecha: 2025-01-12
 * Base de Datos: SecurityVet
 *******************************************************************/

USE SecurityVet;
GO

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║   CREANDO FAMILIAS MÓDULO NEGOCIO - VETCARE     ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''

-- =====================================================
-- CREAR FAMILIAS DE PERMISOS
-- =====================================================

DECLARE @IdFamiliaClientes UNIQUEIDENTIFIER;
DECLARE @IdFamiliaMascotas UNIQUEIDENTIFIER;
DECLARE @IdFamiliaCitas UNIQUEIDENTIFIER;

-- Familia: Gestión de Clientes (usando NCHAR(243) para ó, evita problemas de codificación)
IF NOT EXISTS (SELECT * FROM Familia WHERE Nombre = 'Gesti' + NCHAR(243) + 'n de Clientes')
BEGIN
    SET @IdFamiliaClientes = NEWID();
    INSERT INTO Familia (IdFamilia, Nombre, Descripcion)
    VALUES (@IdFamiliaClientes, 'Gesti' + NCHAR(243) + 'n de Clientes', 'Permisos para gestionar clientes');
    PRINT '✓ Familia "Gestión de Clientes" creada'
END
ELSE
BEGIN
    SELECT @IdFamiliaClientes = IdFamilia FROM Familia WHERE Nombre = 'Gesti' + NCHAR(243) + 'n de Clientes';
    PRINT '⚠ Familia "Gestión de Clientes" ya existe'
END

-- Familia: Gestión de Mascotas (usando NCHAR(243) para ó, evita problemas de codificación)
IF NOT EXISTS (SELECT * FROM Familia WHERE Nombre = 'Gesti' + NCHAR(243) + 'n de Mascotas')
BEGIN
    SET @IdFamiliaMascotas = NEWID();
    INSERT INTO Familia (IdFamilia, Nombre, Descripcion)
    VALUES (@IdFamiliaMascotas, 'Gesti' + NCHAR(243) + 'n de Mascotas', 'Permisos para gestionar mascotas');
    PRINT '✓ Familia "Gestión de Mascotas" creada'
END
ELSE
BEGIN
    SELECT @IdFamiliaMascotas = IdFamilia FROM Familia WHERE Nombre = 'Gesti' + NCHAR(243) + 'n de Mascotas';
    PRINT '⚠ Familia "Gestión de Mascotas" ya existe'
END

-- Familia: Gestión de Citas (usando NCHAR(243) para ó, evita problemas de codificación)
IF NOT EXISTS (SELECT * FROM Familia WHERE Nombre = 'Gesti' + NCHAR(243) + 'n de Citas')
BEGIN
    SET @IdFamiliaCitas = NEWID();
    INSERT INTO Familia (IdFamilia, Nombre, Descripcion)
    VALUES (@IdFamiliaCitas, 'Gesti' + NCHAR(243) + 'n de Citas', 'Permisos para gestionar citas veterinarias');
    PRINT '✓ Familia "Gestión de Citas" creada'
END
ELSE
BEGIN
    SELECT @IdFamiliaCitas = IdFamilia FROM Familia WHERE Nombre = 'Gesti' + NCHAR(243) + 'n de Citas';
    PRINT '⚠ Familia "Gestión de Citas" ya existe'
END

PRINT ''

-- =====================================================
-- ASIGNAR PATENTES A LAS FAMILIAS
-- =====================================================

PRINT 'Asignando patentes a las familias...'

-- Obtener IDs de las patentes
DECLARE @PatentesClientes TABLE (IdPatente UNIQUEIDENTIFIER);
DECLARE @PatentesMascotas TABLE (IdPatente UNIQUEIDENTIFIER);
DECLARE @PatentesCitas TABLE (IdPatente UNIQUEIDENTIFIER);

INSERT INTO @PatentesClientes
SELECT IdPatente FROM Patente WHERE FormName = 'gestionClientes';

INSERT INTO @PatentesMascotas
SELECT IdPatente FROM Patente WHERE FormName = 'gestionMascotas';

INSERT INTO @PatentesCitas
SELECT IdPatente FROM Patente WHERE FormName = 'gestionCitas';

-- Asignar patentes de Clientes a la Familia
INSERT INTO FamiliaPatente (idFamilia, idPatente)
SELECT @IdFamiliaClientes, IdPatente
FROM @PatentesClientes pc
WHERE NOT EXISTS (
    SELECT 1 FROM FamiliaPatente fp
    WHERE fp.idFamilia = @IdFamiliaClientes AND fp.idPatente = pc.IdPatente
);
PRINT '✓ Patentes de Clientes asignadas a Familia'

-- Asignar patentes de Mascotas a la Familia
INSERT INTO FamiliaPatente (idFamilia, idPatente)
SELECT @IdFamiliaMascotas, IdPatente
FROM @PatentesMascotas pm
WHERE NOT EXISTS (
    SELECT 1 FROM FamiliaPatente fp
    WHERE fp.idFamilia = @IdFamiliaMascotas AND fp.idPatente = pm.IdPatente
);
PRINT '✓ Patentes de Mascotas asignadas a Familia'

-- Asignar patentes de Citas a la Familia
INSERT INTO FamiliaPatente (idFamilia, idPatente)
SELECT @IdFamiliaCitas, IdPatente
FROM @PatentesCitas pc
WHERE NOT EXISTS (
    SELECT 1 FROM FamiliaPatente fp
    WHERE fp.idFamilia = @IdFamiliaCitas AND fp.idPatente = pc.IdPatente
);
PRINT '✓ Patentes de Citas asignadas a Familia'

PRINT ''

-- =====================================================
-- ASIGNAR FAMILIAS AL ROL RECEPCIONISTA
-- =====================================================

PRINT 'Asignando familias al ROL_Recepcionista...'

DECLARE @IdRolRecepcionista UNIQUEIDENTIFIER;
SELECT @IdRolRecepcionista = IdFamilia FROM Familia WHERE Nombre = 'ROL_Recepcionista';

IF @IdRolRecepcionista IS NULL
BEGIN
    PRINT '✗ ERROR: No se encontró el rol ROL_Recepcionista'
    RETURN;
END

-- Primero, eliminar las patentes directas del rol (ya que ahora usaremos familias)
DELETE FROM FamiliaPatente
WHERE idFamilia = @IdRolRecepcionista
  AND idPatente IN (
      SELECT IdPatente FROM Patente
      WHERE FormName IN ('gestionClientes', 'gestionMascotas', 'gestionCitas')
  );
PRINT '✓ Patentes directas removidas del rol (se reemplazan por familias)'

-- Asignar las Familias al rol usando FamiliaFamilia (relación Familia-Familia)
IF NOT EXISTS (SELECT * FROM FamiliaFamilia WHERE IdFamiliaPadre = @IdRolRecepcionista AND IdFamiliaHijo = @IdFamiliaClientes)
BEGIN
    INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHijo)
    VALUES (@IdRolRecepcionista, @IdFamiliaClientes);
    PRINT '✓ Familia "Gestión de Clientes" asignada al rol'
END

IF NOT EXISTS (SELECT * FROM FamiliaFamilia WHERE IdFamiliaPadre = @IdRolRecepcionista AND IdFamiliaHijo = @IdFamiliaMascotas)
BEGIN
    INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHijo)
    VALUES (@IdRolRecepcionista, @IdFamiliaMascotas);
    PRINT '✓ Familia "Gestión de Mascotas" asignada al rol'
END

IF NOT EXISTS (SELECT * FROM FamiliaFamilia WHERE IdFamiliaPadre = @IdRolRecepcionista AND IdFamiliaHijo = @IdFamiliaCitas)
BEGIN
    INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHijo)
    VALUES (@IdRolRecepcionista, @IdFamiliaCitas);
    PRINT '✓ Familia "Gestión de Citas" asignada al rol'
END

PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║   FAMILIAS CREADAS Y ASIGNADAS EXITOSAMENTE     ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Familias creadas:'
PRINT '  • Gestión de Clientes (4 patentes)'
PRINT '  • Gestión de Mascotas (4 patentes)'
PRINT '  • Gestión de Citas (5 patentes)'
PRINT ''
PRINT 'Asignadas al ROL_Recepcionista mediante FamiliaFamilia'
PRINT ''
GO
