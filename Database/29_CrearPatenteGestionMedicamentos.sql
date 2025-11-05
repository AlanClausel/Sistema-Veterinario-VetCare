/*
 * Script: 29_CrearPatenteGestionMedicamentos.sql
 * Descripción: Crea la patente "Gestión de Medicamentos" y la asigna a los roles correspondientes
 * Fecha: 2025
 */

USE SecurityVet;
GO

PRINT 'Creando patente: Gestión de Medicamentos...';

-- Verificar si la patente ya existe
IF NOT EXISTS (SELECT 1 FROM Patente WHERE FormName = 'gestionMedicamentos')
BEGIN
    DECLARE @IdPatente UNIQUEIDENTIFIER = NEWID();

    INSERT INTO Patente (IdPatente, FormName, MenuItemName, Orden, Descripcion)
    VALUES (
        @IdPatente,
        'gestionMedicamentos',
        'Gestión de Medicamentos',
        12,
        'Permite administrar el catálogo de medicamentos y controlar el stock'
    );

    PRINT 'Patente "Gestión de Medicamentos" creada exitosamente.';

    -- Asignar a ROL_Administrador
    IF EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'ROL_Administrador')
    BEGIN
        DECLARE @IdFamiliaAdmin UNIQUEIDENTIFIER;
        SELECT @IdFamiliaAdmin = IdFamilia FROM Familia WHERE Nombre = 'ROL_Administrador';

        IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaAdmin AND idPatente = @IdPatente)
        BEGIN
            INSERT INTO FamiliaPatente (idFamilia, idPatente)
            VALUES (@IdFamiliaAdmin, @IdPatente);

            PRINT 'Patente asignada a ROL_Administrador.';
        END
    END
    ELSE
    BEGIN
        PRINT 'ADVERTENCIA: ROL_Administrador no existe.';
    END

    -- Asignar también a ROL_Veterinario (para que puedan ver medicamentos disponibles)
    IF EXISTS (SELECT 1 FROM Familia WHERE Nombre = 'ROL_Veterinario')
    BEGIN
        DECLARE @IdFamiliaVet UNIQUEIDENTIFIER;
        SELECT @IdFamiliaVet = IdFamilia FROM Familia WHERE Nombre = 'ROL_Veterinario';

        IF NOT EXISTS (SELECT 1 FROM FamiliaPatente WHERE idFamilia = @IdFamiliaVet AND idPatente = @IdPatente)
        BEGIN
            INSERT INTO FamiliaPatente (idFamilia, idPatente)
            VALUES (@IdFamiliaVet, @IdPatente);

            PRINT 'Patente asignada a ROL_Veterinario.';
        END
    END
END
ELSE
BEGIN
    PRINT 'La patente "Gestión de Medicamentos" ya existe.';
END

GO

-- Verificar la creación
SELECT
    p.MenuItemName AS 'Patente',
    p.FormName AS 'Formulario',
    p.Orden AS 'Orden',
    f.Nombre AS 'Asignada a Rol'
FROM Patente p
LEFT JOIN FamiliaPatente fp ON p.IdPatente = fp.idPatente
LEFT JOIN Familia f ON fp.idFamilia = f.IdFamilia
WHERE p.FormName = 'gestionMedicamentos'
ORDER BY f.Nombre;

PRINT '';
PRINT 'Script ejecutado exitosamente.';
PRINT 'La opción "Gestión de Medicamentos" aparecerá en el menú para Administradores y Veterinarios.';
GO
