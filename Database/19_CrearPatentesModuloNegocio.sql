/*******************************************************************
 * Script: 19_CrearPatentesModuloNegocio.sql
 * Descripción: Crea patentes para el módulo de negocio y las asigna al rol Recepcionista
 * Fecha: 2025-01-12
 * Base de Datos: SecurityVet
 *******************************************************************/

USE SecurityVet;
GO

PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║   CREANDO PATENTES MÓDULO NEGOCIO - VETCARE      ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''

-- =====================================================
-- PATENTES: GESTIÓN DE MASCOTAS
-- =====================================================

PRINT 'Creando patentes para Gestión de Mascotas...'

DECLARE @IdPatenteVerMascotas UNIQUEIDENTIFIER = NEWID();
DECLARE @IdPatenteAltaMascota UNIQUEIDENTIFIER = NEWID();
DECLARE @IdPatenteModificarMascota UNIQUEIDENTIFIER = NEWID();
DECLARE @IdPatenteBajaMascota UNIQUEIDENTIFIER = NEWID();

-- Patente: Ver Mascotas
IF NOT EXISTS (SELECT * FROM Patente WHERE FormName = 'gestionMascotas' AND MenuItemName = 'Ver Mascotas')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteVerMascotas, 'gestionMascotas', 'Ver Mascotas', 10, 'Ver listado de mascotas');
    PRINT '✓ Patente "Ver Mascotas" creada'
END
ELSE
BEGIN
    SELECT @IdPatenteVerMascotas = IdPatente FROM Patente WHERE FormName = 'gestionMascotas' AND MenuItemName = 'Ver Mascotas';
    PRINT '⚠ Patente "Ver Mascotas" ya existe'
END

-- Patente: Alta de Mascota
IF NOT EXISTS (SELECT * FROM Patente WHERE FormName = 'gestionMascotas' AND MenuItemName = 'Alta de Mascota')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteAltaMascota, 'gestionMascotas', 'Alta de Mascota', 20, 'Crear nuevas mascotas');
    PRINT '✓ Patente "Alta de Mascota" creada'
END
ELSE
BEGIN
    SELECT @IdPatenteAltaMascota = IdPatente FROM Patente WHERE FormName = 'gestionMascotas' AND MenuItemName = 'Alta de Mascota';
    PRINT '⚠ Patente "Alta de Mascota" ya existe'
END

-- Patente: Modificar Mascota
IF NOT EXISTS (SELECT * FROM Patente WHERE FormName = 'gestionMascotas' AND MenuItemName = 'Modificar Mascota')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteModificarMascota, 'gestionMascotas', 'Modificar Mascota', 30, 'Editar mascotas existentes');
    PRINT '✓ Patente "Modificar Mascota" creada'
END
ELSE
BEGIN
    SELECT @IdPatenteModificarMascota = IdPatente FROM Patente WHERE FormName = 'gestionMascotas' AND MenuItemName = 'Modificar Mascota';
    PRINT '⚠ Patente "Modificar Mascota" ya existe'
END

-- Patente: Baja de Mascota
IF NOT EXISTS (SELECT * FROM Patente WHERE FormName = 'gestionMascotas' AND MenuItemName = 'Baja de Mascota')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteBajaMascota, 'gestionMascotas', 'Baja de Mascota', 40, 'Eliminar mascotas');
    PRINT '✓ Patente "Baja de Mascota" creada'
END
ELSE
BEGIN
    SELECT @IdPatenteBajaMascota = IdPatente FROM Patente WHERE FormName = 'gestionMascotas' AND MenuItemName = 'Baja de Mascota';
    PRINT '⚠ Patente "Baja de Mascota" ya existe'
END

PRINT ''

-- =====================================================
-- PATENTES: GESTIÓN DE CITAS
-- =====================================================

PRINT 'Creando patentes para Gestión de Citas...'

DECLARE @IdPatenteVerCitas UNIQUEIDENTIFIER = NEWID();
DECLARE @IdPatenteAltaCita UNIQUEIDENTIFIER = NEWID();
DECLARE @IdPatenteModificarCita UNIQUEIDENTIFIER = NEWID();
DECLARE @IdPatenteCancelarCita UNIQUEIDENTIFIER = NEWID();
DECLARE @IdPatenteActualizarEstadoCita UNIQUEIDENTIFIER = NEWID();

-- Patente: Ver Citas
IF NOT EXISTS (SELECT * FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Ver Citas')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteVerCitas, 'gestionCitas', 'Ver Citas', 10, 'Ver listado de citas');
    PRINT '✓ Patente "Ver Citas" creada'
END
ELSE
BEGIN
    SELECT @IdPatenteVerCitas = IdPatente FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Ver Citas';
    PRINT '⚠ Patente "Ver Citas" ya existe'
END

-- Patente: Alta de Cita
IF NOT EXISTS (SELECT * FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Alta de Cita')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteAltaCita, 'gestionCitas', 'Alta de Cita', 20, 'Agendar nuevas citas');
    PRINT '✓ Patente "Alta de Cita" creada'
END
ELSE
BEGIN
    SELECT @IdPatenteAltaCita = IdPatente FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Alta de Cita';
    PRINT '⚠ Patente "Alta de Cita" ya existe'
END

-- Patente: Modificar Cita
IF NOT EXISTS (SELECT * FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Modificar Cita')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteModificarCita, 'gestionCitas', 'Modificar Cita', 30, 'Editar citas existentes');
    PRINT '✓ Patente "Modificar Cita" creada'
END
ELSE
BEGIN
    SELECT @IdPatenteModificarCita = IdPatente FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Modificar Cita';
    PRINT '⚠ Patente "Modificar Cita" ya existe'
END

-- Patente: Cancelar Cita
IF NOT EXISTS (SELECT * FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Cancelar Cita')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteCancelarCita, 'gestionCitas', 'Cancelar Cita', 40, 'Cancelar citas');
    PRINT '✓ Patente "Cancelar Cita" creada'
END
ELSE
BEGIN
    SELECT @IdPatenteCancelarCita = IdPatente FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Cancelar Cita';
    PRINT '⚠ Patente "Cancelar Cita" ya existe'
END

-- Patente: Actualizar Estado Cita
IF NOT EXISTS (SELECT * FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Actualizar Estado')
BEGIN
    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (@IdPatenteActualizarEstadoCita, 'gestionCitas', 'Actualizar Estado', 50, 'Actualizar estado de citas');
    PRINT '✓ Patente "Actualizar Estado" creada'
END
ELSE
BEGIN
    SELECT @IdPatenteActualizarEstadoCita = IdPatente FROM Patente WHERE FormName = 'gestionCitas' AND MenuItemName = 'Actualizar Estado';
    PRINT '⚠ Patente "Actualizar Estado" ya existe'
END

PRINT ''

-- =====================================================
-- ASIGNAR PATENTES AL ROL RECEPCIONISTA
-- =====================================================

PRINT 'Asignando patentes al rol ROL_Recepcionista...'

DECLARE @IdFamiliaRecepcionista UNIQUEIDENTIFIER;
SELECT @IdFamiliaRecepcionista = IdFamilia FROM Familia WHERE Nombre = 'ROL_Recepcionista';

IF @IdFamiliaRecepcionista IS NULL
BEGIN
    PRINT '✗ ERROR: No se encontró el rol ROL_Recepcionista'
    RETURN;
END

-- Obtener IDs de patentes de Clientes (frmGestionClientes -> gestionClientes)
DECLARE @IdPatenteVerClientes UNIQUEIDENTIFIER;
DECLARE @IdPatenteAltaCliente UNIQUEIDENTIFIER;
DECLARE @IdPatenteModificarCliente UNIQUEIDENTIFIER;
DECLARE @IdPatenteBajaCliente UNIQUEIDENTIFIER;

SELECT @IdPatenteVerClientes = IdPatente FROM Patente WHERE FormName = 'frmGestionClientes' AND MenuItemName = 'Ver Clientes';
SELECT @IdPatenteAltaCliente = IdPatente FROM Patente WHERE FormName = 'frmGestionClientes' AND MenuItemName = 'Alta de Cliente';
SELECT @IdPatenteModificarCliente = IdPatente FROM Patente WHERE FormName = 'frmGestionClientes' AND MenuItemName = 'Modificar Cliente';
SELECT @IdPatenteBajaCliente = IdPatente FROM Patente WHERE FormName = 'frmGestionClientes' AND MenuItemName = 'Baja de Cliente';

-- Asignar patentes de Clientes
IF @IdPatenteVerClientes IS NOT NULL AND NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteVerClientes)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteVerClientes);
    PRINT '✓ Patente "Ver Clientes" asignada'
END

IF @IdPatenteAltaCliente IS NOT NULL AND NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteAltaCliente)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteAltaCliente);
    PRINT '✓ Patente "Alta de Cliente" asignada'
END

IF @IdPatenteModificarCliente IS NOT NULL AND NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteModificarCliente)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteModificarCliente);
    PRINT '✓ Patente "Modificar Cliente" asignada'
END

IF @IdPatenteBajaCliente IS NOT NULL AND NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteBajaCliente)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteBajaCliente);
    PRINT '✓ Patente "Baja de Cliente" asignada'
END

-- Asignar patentes de Mascotas
IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteVerMascotas)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteVerMascotas);
    PRINT '✓ Patente "Ver Mascotas" asignada'
END

IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteAltaMascota)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteAltaMascota);
    PRINT '✓ Patente "Alta de Mascota" asignada'
END

IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteModificarMascota)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteModificarMascota);
    PRINT '✓ Patente "Modificar Mascota" asignada'
END

IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteBajaMascota)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteBajaMascota);
    PRINT '✓ Patente "Baja de Mascota" asignada'
END

-- Asignar patentes de Citas
IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteVerCitas)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteVerCitas);
    PRINT '✓ Patente "Ver Citas" asignada'
END

IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteAltaCita)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteAltaCita);
    PRINT '✓ Patente "Alta de Cita" asignada'
END

IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteModificarCita)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteModificarCita);
    PRINT '✓ Patente "Modificar Cita" asignada'
END

IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteCancelarCita)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteCancelarCita);
    PRINT '✓ Patente "Cancelar Cita" asignada'
END

IF NOT EXISTS (SELECT * FROM FamiliaPatente WHERE idFamilia = @IdFamiliaRecepcionista AND idPatente = @IdPatenteActualizarEstadoCita)
BEGIN
    INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES (@IdFamiliaRecepcionista, @IdPatenteActualizarEstadoCita);
    PRINT '✓ Patente "Actualizar Estado" asignada'
END

PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║   PATENTES CREADAS Y ASIGNADAS EXITOSAMENTE      ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Patentes creadas:'
PRINT '  • Mascotas: 4 patentes (Ver, Alta, Modificar, Baja)'
PRINT '  • Citas: 5 patentes (Ver, Alta, Modificar, Cancelar, Actualizar Estado)'
PRINT ''
PRINT 'Patentes asignadas a ROL_Recepcionista:'
PRINT '  • Clientes: 4 patentes'
PRINT '  • Mascotas: 4 patentes'
PRINT '  • Citas: 5 patentes'
PRINT '  • TOTAL: 13 patentes'
PRINT ''
GO
