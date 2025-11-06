/*
====================================================================================================
SCRIPT: Eliminar patente obsoleta "Configuración"
DESCRIPCIÓN: Elimina la patente frmConfiguracion ya que no está implementada
AUTOR: Sistema
FECHA: 2025-01-05
====================================================================================================
*/

USE SecurityVet;
GO

PRINT '================================================='
PRINT 'Eliminando patente obsoleta Configuración...'
PRINT '================================================='
PRINT ''

-- Eliminar relaciones de la patente con familias
PRINT 'Eliminando relaciones FamiliaPatente...'
DELETE FROM FamiliaPatente
WHERE idPatente IN (SELECT IdPatente FROM Patente WHERE FormName = 'frmConfiguracion');

PRINT '  ✓ Relaciones eliminadas'
PRINT ''

-- Eliminar relaciones de la patente con usuarios
PRINT 'Eliminando relaciones UsuarioPatente...'
DELETE FROM UsuarioPatente
WHERE idPatente IN (SELECT IdPatente FROM Patente WHERE FormName = 'frmConfiguracion');

PRINT '  ✓ Relaciones eliminadas'
PRINT ''

-- Eliminar la patente
PRINT 'Eliminando patente frmConfiguracion...'
DELETE FROM Patente
WHERE FormName = 'frmConfiguracion';

PRINT '  ✓ Patente eliminada'
PRINT ''

PRINT '================================================='
PRINT 'Patente Configuración eliminada exitosamente'
PRINT '================================================='
GO
