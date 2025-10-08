-- =====================================================
-- Script: Crear Módulo de Gestión de Clientes
-- Descripción: Crea Familias, Patentes y asigna permisos a ROL_Recepcionista
-- =====================================================

USE SecurityVet;
GO

PRINT 'Creando módulo de Gestión de Clientes...'
PRINT ''

-- Variables para IDs
DECLARE @IdFamiliaClientes UNIQUEIDENTIFIER = NEWID()
DECLARE @IdRolRecepcionista UNIQUEIDENTIFIER

-- Patentes
DECLARE @IdPatAltaCliente UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatBajaCliente UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatModificarCliente UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatVerClientes UNIQUEIDENTIFIER = NEWID()

-- =====================================================
-- PASO 1: Verificar si existe ROL_Recepcionista
-- =====================================================
PRINT 'Verificando ROL_Recepcionista...'

SELECT @IdRolRecepcionista = IdFamilia
FROM Familia
WHERE Nombre = 'ROL_Recepcionista';

IF @IdRolRecepcionista IS NULL
BEGIN
    PRINT '  → ROL_Recepcionista no existe, creándolo...'
    SET @IdRolRecepcionista = NEWID()
    INSERT INTO Familia (IdFamilia, Nombre, Descripcion)
    VALUES (@IdRolRecepcionista, 'ROL_Recepcionista', 'Rol de Recepcionista - Gestión de clientes y turnos')
    PRINT '  ✓ ROL_Recepcionista creado'
END
ELSE
BEGIN
    PRINT '  ✓ ROL_Recepcionista ya existe'
END

-- =====================================================
-- PASO 2: Crear Familia de Gestión de Clientes
-- =====================================================
PRINT 'Creando Familia de Gestión de Clientes...'

-- Verificar si ya existe
IF NOT EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'Gestión de Clientes')
BEGIN
    INSERT INTO Familia (IdFamilia, Nombre, Descripcion)
    VALUES (@IdFamiliaClientes, 'Gestión de Clientes', 'Administración de clientes y mascotas del sistema')
    PRINT '  ✓ Familia "Gestión de Clientes" creada'
END
ELSE
BEGIN
    PRINT '  → Familia "Gestión de Clientes" ya existe'
    SELECT @IdFamiliaClientes = IdFamilia FROM Familia WHERE Nombre = 'Gestión de Clientes'
END

-- =====================================================
-- PASO 3: Crear Patentes de Gestión de Clientes
-- =====================================================
PRINT 'Creando Patentes de Gestión de Clientes...'

-- Verificar y crear patentes
IF NOT EXISTS (SELECT 1 FROM Patente WHERE MenuItemName = 'Alta de Cliente')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatAltaCliente, 'frmGestionClientes', 'Alta de Cliente', 1, 'Crear nuevos clientes')
END
ELSE
BEGIN
    SELECT @IdPatAltaCliente = IdPatente FROM Patente WHERE MenuItemName = 'Alta de Cliente'
END

IF NOT EXISTS (SELECT 1 FROM Patente WHERE MenuItemName = 'Baja de Cliente')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatBajaCliente, 'frmGestionClientes', 'Baja de Cliente', 2, 'Eliminar clientes')
END
ELSE
BEGIN
    SELECT @IdPatBajaCliente = IdPatente FROM Patente WHERE MenuItemName = 'Baja de Cliente'
END

IF NOT EXISTS (SELECT 1 FROM Patente WHERE MenuItemName = 'Modificar Cliente')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatModificarCliente, 'frmGestionClientes', 'Modificar Cliente', 3, 'Editar clientes existentes')
END
ELSE
BEGIN
    SELECT @IdPatModificarCliente = IdPatente FROM Patente WHERE MenuItemName = 'Modificar Cliente'
END

IF NOT EXISTS (SELECT 1 FROM Patente WHERE MenuItemName = 'Ver Clientes')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatVerClientes, 'frmGestionClientes', 'Ver Clientes', 4, 'Ver listado de clientes')
END
ELSE
BEGIN
    SELECT @IdPatVerClientes = IdPatente FROM Patente WHERE MenuItemName = 'Ver Clientes'
END

PRINT '  ✓ Patentes creadas/verificadas'

-- =====================================================
-- PASO 4: Asignar Patentes a Familia "Gestión de Clientes"
-- =====================================================
PRINT 'Asignando Patentes a Familia...'

-- Familia: Gestión de Clientes
IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaClientes AND idPatente = @IdPatAltaCliente)
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaClientes, @IdPatAltaCliente)

IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaClientes AND idPatente = @IdPatBajaCliente)
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaClientes, @IdPatBajaCliente)

IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaClientes AND idPatente = @IdPatModificarCliente)
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaClientes, @IdPatModificarCliente)

IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaClientes AND idPatente = @IdPatVerClientes)
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaClientes, @IdPatVerClientes)

PRINT '  ✓ Patentes asignadas a Familia'

-- =====================================================
-- PASO 5: Asignar Familia a ROL_Recepcionista
-- =====================================================
PRINT 'Asignando Familia a ROL_Recepcionista...'

IF NOT EXISTS (SELECT 1 FROM FamiliaFamilia WHERE IdFamiliaPadre = @IdRolRecepcionista AND IdFamiliaHijo = @IdFamiliaClientes)
BEGIN
    INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHijo)
    VALUES (@IdRolRecepcionista, @IdFamiliaClientes)
    PRINT '  ✓ Familia asignada a ROL_Recepcionista'
END
ELSE
BEGIN
    PRINT '  → Familia ya estaba asignada a ROL_Recepcionista'
END

-- =====================================================
-- PASO 6: Mostrar resumen
-- =====================================================
PRINT ''
PRINT '============================================='
PRINT 'Resumen de la configuración:'
PRINT '============================================='
PRINT 'Familia creada: Gestión de Clientes'
PRINT 'Patentes creadas: 4'
PRINT '  - Alta de Cliente'
PRINT '  - Baja de Cliente'
PRINT '  - Modificar Cliente'
PRINT '  - Ver Clientes'
PRINT 'ROL asignado: ROL_Recepcionista'
PRINT ''
PRINT '✓ Módulo de Gestión de Clientes configurado correctamente'
PRINT '============================================='

GO
