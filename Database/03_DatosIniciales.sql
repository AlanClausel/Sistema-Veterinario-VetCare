-- =====================================================
-- Script: Datos Iniciales - Sistema Biblioteca Escolar
-- Descripción: Inserta roles, patentes y usuario admin
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Insertando datos iniciales...'
PRINT ''

-- =====================================================
-- PASO 1: Crear Familias de ROL
-- =====================================================
PRINT 'Creando Familias de ROL...'

DECLARE @IdRolAdmin UNIQUEIDENTIFIER = NEWID()
DECLARE @IdRolDocente UNIQUEIDENTIFIER = NEWID()

INSERT INTO Familia (IdFamilia, Nombre, Descripcion) VALUES
(@IdRolAdmin, 'ROL_Administrador', 'Rol de Administrador del Sistema - Gestión completa de la biblioteca'),
(@IdRolDocente, 'ROL_Docente', 'Rol de Docente - Gestión de préstamos y consultas de libros');

PRINT '  ✓ Roles creados: Administrador, Docente'

-- =====================================================
-- PASO 2: Crear Familias de Permisos (Grupos)
-- =====================================================
PRINT 'Creando Familias de Permisos...'

DECLARE @IdFamGestionUsuarios UNIQUEIDENTIFIER = NEWID()
DECLARE @IdFamGestionPermisos UNIQUEIDENTIFIER = NEWID()
DECLARE @IdFamConfiguracion UNIQUEIDENTIFIER = NEWID()

INSERT INTO Familia (IdFamilia, Nombre, Descripcion) VALUES
(@IdFamGestionUsuarios, 'Gestión de Usuarios', 'Administración de usuarios del sistema'),
(@IdFamGestionPermisos, 'Gestión de Permisos', 'Administración de familias y patentes'),
(@IdFamConfiguracion, 'Configuración', 'Configuración del sistema');

PRINT '  ✓ Familias de permisos creadas'

-- =====================================================
-- PASO 3: Crear Patentes (Permisos Atómicos)
-- =====================================================
PRINT 'Creando Patentes...'

-- Patentes de Gestión de Usuarios
DECLARE @IdPatAltaUsuario UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatBajaUsuario UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatModUsuario UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatVerUsuarios UNIQUEIDENTIFIER = NEWID()

INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion) VALUES
(@IdPatAltaUsuario, 'frmGestionUsuarios', 'Alta de Usuario', 1, 'Crear nuevos usuarios'),
(@IdPatBajaUsuario, 'frmGestionUsuarios', 'Baja de Usuario', 2, 'Eliminar usuarios'),
(@IdPatModUsuario, 'frmGestionUsuarios', 'Modificar Usuario', 3, 'Editar usuarios existentes'),
(@IdPatVerUsuarios, 'frmGestionUsuarios', 'Ver Usuarios', 4, 'Ver listado de usuarios');

-- Patentes de Gestión de Permisos
DECLARE @IdPatAltaFamilia UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatBajaFamilia UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatModFamilia UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatAsignarPermisos UNIQUEIDENTIFIER = NEWID()

INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion) VALUES
(@IdPatAltaFamilia, 'frmGestionPermisos', 'Alta de Familia', 1, 'Crear grupos de permisos'),
(@IdPatBajaFamilia, 'frmGestionPermisos', 'Baja de Familia', 2, 'Eliminar grupos de permisos'),
(@IdPatModFamilia, 'frmGestionPermisos', 'Modificar Familia', 3, 'Editar grupos de permisos'),
(@IdPatAsignarPermisos, 'frmGestionPermisos', 'Asignar Permisos', 4, 'Asignar permisos a usuarios');

-- Patentes de Configuración
DECLARE @IdPatConfigSistema UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatVerLogs UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatBackup UNIQUEIDENTIFIER = NEWID()

INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion) VALUES
(@IdPatConfigSistema, 'frmConfiguracion', 'Configuración Sistema', 1, 'Configurar parámetros del sistema'),
(@IdPatVerLogs, 'frmVisorLogs', 'Ver Logs', 2, 'Ver logs del sistema'),
(@IdPatBackup, 'frmBackup', 'Backup/Restore', 3, 'Realizar backup y restore de BD');

PRINT '  ✓ Patentes creadas'

-- =====================================================
-- PASO 4: Asignar Patentes a Familias de Permisos
-- =====================================================
PRINT 'Asignando Patentes a Familias...'

-- Familia: Gestión de Usuarios
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdFamGestionUsuarios, @IdPatAltaUsuario),
(@IdFamGestionUsuarios, @IdPatBajaUsuario),
(@IdFamGestionUsuarios, @IdPatModUsuario),
(@IdFamGestionUsuarios, @IdPatVerUsuarios);

-- Familia: Gestión de Permisos
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdFamGestionPermisos, @IdPatAltaFamilia),
(@IdFamGestionPermisos, @IdPatBajaFamilia),
(@IdFamGestionPermisos, @IdPatModFamilia),
(@IdFamGestionPermisos, @IdPatAsignarPermisos);

-- Familia: Configuración
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdFamConfiguracion, @IdPatConfigSistema),
(@IdFamConfiguracion, @IdPatVerLogs),
(@IdFamConfiguracion, @IdPatBackup);

PRINT '  ✓ Patentes asignadas a Familias'

-- =====================================================
-- PASO 5: Construir Jerarquía de Roles (Composite)
-- =====================================================
PRINT 'Construyendo jerarquía de roles...'

-- ROL_Administrador contiene TODAS las familias de administración
INSERT INTO FamiliaFamilia (IdFamiliaPadre, IdFamiliaHijo) VALUES
(@IdRolAdmin, @IdFamGestionUsuarios),
(@IdRolAdmin, @IdFamGestionPermisos),
(@IdRolAdmin, @IdFamConfiguracion);

-- ROL_Docente sin permisos por ahora
-- Se agregarán permisos específicos para gestión de préstamos cuando se implementen

PRINT '  ✓ Jerarquía de roles construida'
PRINT '  → ROL_Docente creado sin permisos (pendiente implementación de módulo de préstamos)'

-- =====================================================
-- PASO 6: Crear Usuario Administrador por defecto
-- =====================================================
PRINT 'Creando usuario administrador...'

DECLARE @IdUsuarioAdmin UNIQUEIDENTIFIER = NEWID()
DECLARE @NombreAdmin NVARCHAR(100) = N'admin'
DECLARE @EmailAdmin NVARCHAR(150) = N'admin@biblioteca.edu'
DECLARE @PasswordTextoPlano NVARCHAR(100) = N'admin123' -- Cambiar en producción

-- Calcular hash SHA256 usando NVARCHAR (UTF-16) para coincidir con C# Encoding.Unicode
-- Hash SHA256 de N'admin123': 9D39DD891B174041B3488557421FAE0F8D551E1F612725717D820BDBB111530F
DECLARE @ClaveHasheada NVARCHAR(256) = CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', @PasswordTextoPlano), 2)

-- Calcular DVH
DECLARE @DVH NVARCHAR(64)
DECLARE @IdiomaAdmin NVARCHAR(10) = N'es-AR'
-- DVH = SHA256(IdUsuario|Nombre|Clave|Activo) - GUID en MAYÚSCULAS usando NVARCHAR
DECLARE @DatosParaDVH NVARCHAR(MAX) = UPPER(CAST(@IdUsuarioAdmin AS NVARCHAR(36))) + N'|' + @NombreAdmin + N'|' + @ClaveHasheada + N'|' + N'1'
SET @DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @DatosParaDVH), 2)

INSERT INTO Usuario (IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH) VALUES
(@IdUsuarioAdmin, @NombreAdmin, @EmailAdmin, @ClaveHasheada, 1, @IdiomaAdmin, @DVH);

-- Asignar ROL_Administrador al usuario admin
INSERT INTO UsuarioFamilia (idUsuario, idFamilia) VALUES
(@IdUsuarioAdmin, @IdRolAdmin);

PRINT '  ✓ Usuario admin creado (usuario: admin, email: admin@biblioteca.edu, password: admin123, idioma: es-AR)'
PRINT '    ADVERTENCIA: Cambiar la contraseña en producción'

PRINT ''
PRINT '=== Datos iniciales cargados exitosamente ==='
PRINT ''
PRINT 'RESUMEN:'
PRINT '  • Roles creados: 2 (Administrador, Docente*)'
PRINT '  • Familias de permisos: 3 (Gestión Usuarios, Gestión Permisos, Configuración)'
PRINT '  • Patentes: 11 (4 usuarios + 4 permisos + 3 configuración)'
PRINT '  • Usuario admin: admin / admin123'
PRINT '  • DVH (Dígito Verificador Horizontal) implementado'
PRINT ''
PRINT '  (*) Docente sin permisos - agregar cuando se implemente módulo de préstamos'
PRINT ''
GO
