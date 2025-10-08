-- =====================================================
-- Script: Limpiar Usuarios Viejos
-- Descripción: Elimina usuarios con hashes UTF-8 antiguos
--              y deja solo el usuario admin correcto
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT '========================================';
PRINT 'Limpiando usuarios viejos';
PRINT '========================================';
PRINT '';

-- Mostrar usuarios actuales
PRINT 'Usuarios actuales:';
SELECT
    Nombre,
    'Hash' = LEFT(Clave, 16) + '...',
    'DVH' = LEFT(DVH, 16) + '...'
FROM Usuario
ORDER BY Nombre;
PRINT '';

-- Eliminar usuarios viejos (todos excepto el más reciente 'admin')
PRINT 'Eliminando usuarios viejos...';

DECLARE @IdUsuariosAEliminar TABLE (IdUsuario UNIQUEIDENTIFIER, Nombre NVARCHAR(100))

-- Buscar usuarios que NO son el admin más reciente
INSERT INTO @IdUsuariosAEliminar
SELECT TOP 100 IdUsuario, Nombre
FROM Usuario
WHERE Nombre != 'admin'
   OR IdUsuario != (
       SELECT TOP 1 IdUsuario
       FROM Usuario
       WHERE Nombre = 'admin'
       ORDER BY CAST(IdUsuario AS NVARCHAR(36)) DESC
   )

-- Mostrar lo que se va a eliminar
SELECT 'Usuarios a eliminar:' AS Accion, Nombre FROM @IdUsuariosAEliminar

-- Eliminar relaciones
DELETE uf
FROM UsuarioFamilia uf
INNER JOIN @IdUsuariosAEliminar temp ON uf.idUsuario = temp.IdUsuario;

DELETE up
FROM UsuarioPatente up
INNER JOIN @IdUsuariosAEliminar temp ON up.idUsuario = temp.IdUsuario;

-- Eliminar usuarios
DELETE u
FROM Usuario u
INNER JOIN @IdUsuariosAEliminar temp ON u.IdUsuario = temp.IdUsuario;

DECLARE @CantidadEliminada INT = (SELECT COUNT(*) FROM @IdUsuariosAEliminar)

PRINT '';
PRINT '  ✓ ' + CAST(@CantidadEliminada AS NVARCHAR) + ' usuario(s) eliminado(s)';
PRINT '';

-- Verificar resultado
PRINT 'Usuarios restantes:';
SELECT
    Nombre,
    Email,
    'Hash' = LEFT(Clave, 16) + '...',
    'DVH Válido' = CASE
        WHEN DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
            UPPER(CAST(IdUsuario AS NVARCHAR(36))) + N'|' + Nombre + N'|' + Clave + N'|' + CASE WHEN Activo = 1 THEN N'1' ELSE N'0' END
        ), 2) THEN 'SÍ ✓'
        ELSE 'NO ✗'
    END
FROM Usuario
ORDER BY Nombre;

PRINT '';
PRINT '========================================';
PRINT 'Limpieza completada';
PRINT '========================================';
PRINT '';
PRINT 'Usuario activo:';
PRINT '  Usuario: admin';
PRINT '  Password: admin123';
PRINT '';

GO
