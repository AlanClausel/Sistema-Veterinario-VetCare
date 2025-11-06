/*
====================================================================================================
SCRIPT: Eliminar patente obsoleta "Visor Logs"
DESCRIPCIÓN: Elimina la patente frmVisorLogs ya que fue reemplazada por FormBitacora
AUTOR: Sistema
FECHA: 2025-01-05
====================================================================================================
*/

USE SecurityVet;
GO

PRINT '================================================='
PRINT 'Eliminando patente obsoleta Visor Logs...'
PRINT '================================================='
PRINT ''

-- Eliminar relaciones de la patente con familias
PRINT 'Eliminando relaciones FamiliaPatente...'
DELETE FROM FamiliaPatente
WHERE idPatente IN (SELECT IdPatente FROM Patente WHERE FormName = 'frmVisorLogs');

PRINT '  ✓ Relaciones eliminadas'
PRINT ''

-- Eliminar relaciones de la patente con usuarios
PRINT 'Eliminando relaciones UsuarioPatente...'
DELETE FROM UsuarioPatente
WHERE idPatente IN (SELECT IdPatente FROM Patente WHERE FormName = 'frmVisorLogs');

PRINT '  ✓ Relaciones eliminadas'
PRINT ''

-- Eliminar la patente
PRINT 'Eliminando patente frmVisorLogs...'
DELETE FROM Patente
WHERE FormName = 'frmVisorLogs';

PRINT '  ✓ Patente eliminada'
PRINT ''

PRINT '================================================='
PRINT 'Patente Visor Logs eliminada exitosamente'
PRINT 'Ahora solo existe FormBitacora como sistema de auditoría'
PRINT '================================================='
GO
