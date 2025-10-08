-- =====================================================
-- Script: Crear Patentes para menuGeneral
-- Descripción: Inserta patentes para cada opción del menú general
--              y las asigna a los roles correspondientes
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Creando patentes para menuGeneral...'
PRINT ''

-- =====================================================
-- PASO 1: Crear las Patentes para cada opción del menú
-- =====================================================
PRINT 'Creando patentes de menú...'

-- Declarar IDs para las nuevas patentes
DECLARE @IdPatGestionUsuarios UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatGestionPermisos UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatGestionCatalogo UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatGestionAlumnos UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatGestionPrestamos UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatGestionDevoluciones UNIQUEIDENTIFIER = NEWID()
DECLARE @IdPatConsultarReportes UNIQUEIDENTIFIER = NEWID()

-- Insertar las patentes con los nombres que usa menuGeneral.cs
INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion) VALUES
(@IdPatGestionUsuarios, 'menuGeneral', 'GestionUsuarios', 1, 'Acceso al módulo de Gestión de Usuarios'),
(@IdPatGestionPermisos, 'menuGeneral', 'GestionPermisos', 2, 'Acceso al módulo de Gestión de Permisos y Roles'),
(@IdPatGestionCatalogo, 'menuGeneral', 'GestionCatalogo', 3, 'Acceso al módulo de Gestión de Catálogo de Materiales'),
(@IdPatGestionAlumnos, 'menuGeneral', 'GestionAlumnos', 4, 'Acceso al módulo de Gestión de Alumnos'),
(@IdPatGestionPrestamos, 'menuGeneral', 'GestionPrestamos', 5, 'Acceso al módulo de Gestión de Préstamos'),
(@IdPatGestionDevoluciones, 'menuGeneral', 'GestionDevoluciones', 6, 'Acceso al módulo de Gestión de Devoluciones'),
(@IdPatConsultarReportes, 'menuGeneral', 'ConsultarReportes', 7, 'Acceso al módulo de Consulta de Reportes');

PRINT '  ✓ 7 patentes de menú creadas'

-- =====================================================
-- PASO 2: Obtener IDs de los roles existentes
-- =====================================================
PRINT 'Obteniendo roles existentes...'

DECLARE @IdRolAdmin UNIQUEIDENTIFIER
DECLARE @IdRolDocente UNIQUEIDENTIFIER

SELECT @IdRolAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador'
SELECT @IdRolDocente = IdFamilia FROM Familia WHERE Nombre = 'ROL_Docente'

IF @IdRolAdmin IS NULL OR @IdRolDocente IS NULL
BEGIN
    PRINT '  ✗ ERROR: No se encontraron los roles. Ejecute primero 03_DatosIniciales.sql'
    RAISERROR('Roles no encontrados', 16, 1)
    RETURN
END

PRINT '  ✓ Roles encontrados'

-- =====================================================
-- PASO 3: Asignar patentes al ROL_Administrador
-- =====================================================
PRINT 'Asignando permisos al ROL_Administrador...'

-- El administrador tiene acceso a TODAS las opciones del menú
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdRolAdmin, @IdPatGestionUsuarios),
(@IdRolAdmin, @IdPatGestionPermisos),
(@IdRolAdmin, @IdPatGestionCatalogo),
(@IdRolAdmin, @IdPatGestionAlumnos),
(@IdRolAdmin, @IdPatGestionPrestamos),
(@IdRolAdmin, @IdPatGestionDevoluciones),
(@IdRolAdmin, @IdPatConsultarReportes);

PRINT '  ✓ ROL_Administrador: 7 permisos asignados (acceso completo)'

-- =====================================================
-- PASO 4: Asignar patentes al ROL_Docente
-- =====================================================
PRINT 'Asignando permisos al ROL_Docente...'

-- El docente tiene acceso solo a:
-- - Gestión de Catálogo (consulta y administración de libros)
-- - Gestión de Alumnos (consulta de alumnos)
-- - Gestión de Préstamos (realizar préstamos)
-- - Gestión de Devoluciones (registrar devoluciones)
-- - Consultar Reportes (ver reportes de préstamos)
INSERT INTO FamiliaPatente (idFamilia, idPatente) VALUES
(@IdRolDocente, @IdPatGestionCatalogo),
(@IdRolDocente, @IdPatGestionAlumnos),
(@IdRolDocente, @IdPatGestionPrestamos),
(@IdRolDocente, @IdPatGestionDevoluciones),
(@IdRolDocente, @IdPatConsultarReportes);

PRINT '  ✓ ROL_Docente: 5 permisos asignados (catálogo, alumnos, préstamos, devoluciones, reportes)'
PRINT '  → El docente NO tiene acceso a: Gestión de Usuarios ni Gestión de Permisos'

PRINT ''
PRINT '=== Patentes de menuGeneral configuradas exitosamente ==='
PRINT ''
PRINT 'RESUMEN:'
PRINT '  • Patentes creadas: 7'
PRINT '  • ROL_Administrador: Acceso completo (7/7 módulos)'
PRINT '  • ROL_Docente: Acceso limitado (5/7 módulos)'
PRINT ''
PRINT 'IMPORTANTE:'
PRINT '  - El administrador puede modificar estos permisos desde la interfaz gestionPermisos'
PRINT '  - Los nombres de las patentes coinciden con las constantes en menuGeneral.cs'
PRINT '  - Todos los usuarios verán solo las pestañas para las que tienen permiso'
PRINT ''
GO
