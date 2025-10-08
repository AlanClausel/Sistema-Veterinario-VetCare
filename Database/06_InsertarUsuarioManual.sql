-- =============================================
-- Script: Insertar Usuario Manualmente con DVH correcto
-- Descripción: Template para crear usuarios nuevos por SQL
-- =============================================

USE SeguridadBiblioteca;
GO

-- =============================================
-- PARÁMETROS A CONFIGURAR
-- =============================================
DECLARE @NombreUsuario NVARCHAR(100) = 'jperez'  -- ← CAMBIAR: Nombre de usuario (único)
DECLARE @Email NVARCHAR(100) = 'jperez@vetcare.com'  -- ← CAMBIAR: Email del usuario
DECLARE @PasswordTextoPlano NVARCHAR(100) = 'password123'  -- ← CAMBIAR: Contraseña en texto plano
DECLARE @Activo BIT = 1  -- ← CAMBIAR: 1 = activo, 0 = inactivo
DECLARE @IdiomaPreferido NVARCHAR(10) = 'es-AR'  -- ← CAMBIAR: 'es-AR' o 'en-GB'
DECLARE @RolNombre NVARCHAR(100) = 'ROL_Veterinario'  -- ← CAMBIAR: ROL_Administrador, ROL_Veterinario, ROL_Recepcionista

-- =============================================
-- PROCESO AUTOMÁTICO - NO MODIFICAR
-- =============================================

-- 1. Generar GUID único para el usuario
DECLARE @IdUsuario UNIQUEIDENTIFIER = NEWID()

-- 2. Hashear la contraseña (SHA256)
DECLARE @ClaveHasheada NVARCHAR(256)
SET @ClaveHasheada = CONVERT(NVARCHAR(256), HASHBYTES('SHA2_256', @PasswordTextoPlano), 2)

-- 3. Calcular DVH correctamente
-- Fórmula: SHA256(GUID_MAYUSCULAS|Nombre|ClaveHasheada|Activo)
DECLARE @DVH NVARCHAR(64)
DECLARE @DatosParaDVH NVARCHAR(MAX)

SET @DatosParaDVH = UPPER(CAST(@IdUsuario AS NVARCHAR(36))) + '|' +
                    @NombreUsuario + '|' +
                    @ClaveHasheada + '|' +
                    CASE WHEN @Activo = 1 THEN '1' ELSE '0' END

SET @DVH = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256', @DatosParaDVH), 2)

-- 4. Mostrar información antes de insertar
PRINT '========================================';
PRINT 'DATOS DEL NUEVO USUARIO';
PRINT '========================================';
PRINT 'IdUsuario: ' + CAST(@IdUsuario AS NVARCHAR(36));
PRINT 'Nombre: ' + @NombreUsuario;
PRINT 'Email: ' + @Email;
PRINT 'Password (texto plano): ' + @PasswordTextoPlano;
PRINT 'Password (hash SHA256): ' + @ClaveHasheada;
PRINT 'Activo: ' + CAST(@Activo AS NVARCHAR(1));
PRINT 'Idioma: ' + @IdiomaPreferido;
PRINT 'DVH: ' + @DVH;
PRINT 'Rol a asignar: ' + @RolNombre;
PRINT '';

-- 5. Verificar que el nombre de usuario no exista
IF EXISTS (SELECT 1 FROM Usuario WHERE Nombre = @NombreUsuario)
BEGIN
    PRINT '❌ ERROR: Ya existe un usuario con el nombre "' + @NombreUsuario + '"';
    PRINT 'Cambie el valor de @NombreUsuario y ejecute nuevamente.';
    RETURN;
END

-- 6. Insertar el usuario
BEGIN TRANSACTION;

BEGIN TRY
    -- Insertar usuario
    INSERT INTO Usuario (IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH)
    VALUES (@IdUsuario, @NombreUsuario, @Email, @ClaveHasheada, @Activo, @IdiomaPreferido, @DVH);

    PRINT '✓ Usuario insertado correctamente';

    -- 7. Buscar el rol y asignarlo
    DECLARE @IdRol UNIQUEIDENTIFIER
    SELECT @IdRol = IdFamilia FROM Familia WHERE Nombre = @RolNombre

    IF @IdRol IS NULL
    BEGIN
        PRINT '❌ ERROR: No existe el rol "' + @RolNombre + '"';
        PRINT 'Roles disponibles:';
        SELECT Nombre FROM Familia WHERE Nombre LIKE 'ROL_%';
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Asignar rol al usuario
    INSERT INTO UsuarioFamilia (idUsuario, idFamilia)
    VALUES (@IdUsuario, @IdRol);

    PRINT '✓ Rol "' + @RolNombre + '" asignado correctamente';

    COMMIT TRANSACTION;

    PRINT '';
    PRINT '========================================';
    PRINT '✓ USUARIO CREADO EXITOSAMENTE';
    PRINT '========================================';
    PRINT 'Puede iniciar sesión con:';
    PRINT '  Usuario: ' + @NombreUsuario;
    PRINT '  Password: ' + @PasswordTextoPlano;
    PRINT '';

    -- 8. Verificar que el DVH sea correcto
    DECLARE @DVH_Verificado NVARCHAR(64)
    SELECT @DVH_Verificado = CONVERT(NVARCHAR(64), HASHBYTES('SHA2_256',
        UPPER(CAST(IdUsuario AS NVARCHAR(36))) + '|' + Nombre + '|' + Clave + '|' + CASE WHEN Activo = 1 THEN '1' ELSE '0' END
    ), 2)
    FROM Usuario WHERE IdUsuario = @IdUsuario

    IF @DVH = @DVH_Verificado
    BEGIN
        PRINT '✓ Verificación DVH: CORRECTO';
    END
    ELSE
    BEGIN
        PRINT '❌ Verificación DVH: INCORRECTO';
        PRINT '  DVH insertado:  ' + @DVH;
        PRINT '  DVH verificado: ' + @DVH_Verificado;
    END

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '❌ ERROR al crear usuario:';
    PRINT ERROR_MESSAGE();
END CATCH

GO
