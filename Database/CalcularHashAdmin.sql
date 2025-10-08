-- =====================================================
-- Script: Calcular Hash Correcto para Admin
-- Descripción: Calcula el hash SHA256 usando NVARCHAR (UTF-16)
-- =====================================================

USE SeguridadBiblioteca;
GO

DECLARE @Password NVARCHAR(100) = N'admin123'

-- Calcular hash usando NVARCHAR (UTF-16) para coincidir con C#
DECLARE @HashCalculado NVARCHAR(256) = CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', @Password), 2)

PRINT 'Password: ' + @Password
PRINT 'Hash calculado: ' + @HashCalculado
PRINT ''
PRINT 'Este hash debe coincidir con el que genera CryptographyService.HashPassword() en C#'

SELECT @HashCalculado AS HashGenerado

GO
