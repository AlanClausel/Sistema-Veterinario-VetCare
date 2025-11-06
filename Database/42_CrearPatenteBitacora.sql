USE SecurityVet;
GO

-- =============================================
-- Crear Patente para el Formulario de Bitácora
-- =============================================

DECLARE @IdPatenteNueva UNIQUEIDENTIFIER = NEWID();
DECLARE @IdRolAdmin UNIQUEIDENTIFIER;

-- Buscar el ID del ROL_Administrador
SELECT @IdRolAdmin = IdFamilia
FROM Familia
WHERE Nombre = 'ROL_Administrador';

IF @IdRolAdmin IS NULL
BEGIN
    PRINT 'ERROR: No se encontró el ROL_Administrador';
    PRINT 'Verifique que el rol existe en la tabla Familia';
END
ELSE
BEGIN
    -- Verificar si la patente ya existe
    IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'FormBitacora')
    BEGIN
        -- Crear la patente para el formulario de Bitácora
        INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
        VALUES (
            @IdPatenteNueva,
            'FormBitacora',
            'Bitácora del Sistema',
            900, -- Orden alto para que aparezca al final del menú
            'Permite consultar y exportar registros de auditoría del sistema'
        );

        PRINT 'Patente "FormBitacora" creada exitosamente';
        PRINT 'ID de Patente: ' + CAST(@IdPatenteNueva AS VARCHAR(36));

        -- Asignar la patente al ROL_Administrador
        IF NOT EXISTS (
            SELECT 1
            FROM FamiliaPatente
            WHERE idFamilia = @IdRolAdmin AND idPatente = @IdPatenteNueva
        )
        BEGIN
            INSERT INTO FamiliaPatente (idFamilia, idPatente)
            VALUES (@IdRolAdmin, @IdPatenteNueva);

            PRINT 'Patente asignada al ROL_Administrador exitosamente';
        END
        ELSE
        BEGIN
            PRINT 'La patente ya estaba asignada al ROL_Administrador';
        END
    END
    ELSE
    BEGIN
        PRINT 'La patente "FormBitacora" ya existe';

        -- Obtener el ID de la patente existente
        SELECT @IdPatenteNueva = IdPatente
        FROM Patente
        WHERE FormName = 'FormBitacora';

        -- Verificar si está asignada al administrador
        IF NOT EXISTS (
            SELECT 1
            FROM FamiliaPatente
            WHERE idFamilia = @IdRolAdmin AND idPatente = @IdPatenteNueva
        )
        BEGIN
            INSERT INTO FamiliaPatente (idFamilia, idPatente)
            VALUES (@IdRolAdmin, @IdPatenteNueva);

            PRINT 'Patente asignada al ROL_Administrador exitosamente';
        END
        ELSE
        BEGIN
            PRINT 'La patente ya está asignada al ROL_Administrador';
        END
    END
END

-- Verificar la creación
PRINT '';
PRINT 'Verificación:';
SELECT
    p.IdPatente,
    p.FormName,
    p.MenuItemName,
    p.Orden,
    p.Descripcion,
    CASE
        WHEN fp.idFamilia IS NOT NULL THEN 'Asignada a ROL_Administrador'
        ELSE 'NO asignada'
    END AS Estado
FROM Patente p
LEFT JOIN FamiliaPatente fp ON p.IdPatente = fp.idPatente AND fp.idFamilia = @IdRolAdmin
WHERE p.FormName = 'FormBitacora';

GO

PRINT '';
PRINT '========================================';
PRINT 'Script completado exitosamente';
PRINT 'La Patente de Bitácora está lista';
PRINT '========================================';
