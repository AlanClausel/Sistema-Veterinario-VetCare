-- =====================================================
-- Script: Stored Procedures de Negocio - VetCare
-- Descripción: CRUD para Cliente y Mascota
-- =====================================================

USE VetCareDB;
GO

PRINT 'Creando Stored Procedures de Negocio...'
PRINT ''

-- =====================================================
-- STORED PROCEDURES: CLIENTE
-- =====================================================

-- SP: Cliente_Insert
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Cliente_Insert')
    DROP PROCEDURE Cliente_Insert;
GO

CREATE PROCEDURE Cliente_Insert
    @IdCliente UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DNI NVARCHAR(20),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(150),
    @Direccion NVARCHAR(255),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Cliente (IdCliente, Nombre, Apellido, DNI, Telefono, Email, Direccion, Activo)
    VALUES (@IdCliente, @Nombre, @Apellido, @DNI, @Telefono, @Email, @Direccion, @Activo);

    SELECT * FROM Cliente WHERE IdCliente = @IdCliente;
END
GO

PRINT '✓ SP Cliente_Insert creado'
GO

-- SP: Cliente_Update
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Cliente_Update')
    DROP PROCEDURE Cliente_Update;
GO

CREATE PROCEDURE Cliente_Update
    @IdCliente UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(100),
    @Apellido NVARCHAR(100),
    @DNI NVARCHAR(20),
    @Telefono NVARCHAR(20),
    @Email NVARCHAR(150),
    @Direccion NVARCHAR(255),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Cliente
    SET Nombre = @Nombre,
        Apellido = @Apellido,
        DNI = @DNI,
        Telefono = @Telefono,
        Email = @Email,
        Direccion = @Direccion,
        Activo = @Activo
    WHERE IdCliente = @IdCliente;

    SELECT * FROM Cliente WHERE IdCliente = @IdCliente;
END
GO

PRINT '✓ SP Cliente_Update creado'
GO

-- SP: Cliente_Delete
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Cliente_Delete')
    DROP PROCEDURE Cliente_Delete;
GO

CREATE PROCEDURE Cliente_Delete
    @IdCliente UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Esto eliminará en cascada las mascotas asociadas
    DELETE FROM Cliente WHERE IdCliente = @IdCliente;
END
GO

PRINT '✓ SP Cliente_Delete creado'
GO

-- SP: Cliente_SelectOne
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Cliente_SelectOne')
    DROP PROCEDURE Cliente_SelectOne;
GO

CREATE PROCEDURE Cliente_SelectOne
    @IdCliente UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Cliente WHERE IdCliente = @IdCliente;
END
GO

PRINT '✓ SP Cliente_SelectOne creado'
GO

-- SP: Cliente_SelectAll
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Cliente_SelectAll')
    DROP PROCEDURE Cliente_SelectAll;
GO

CREATE PROCEDURE Cliente_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Cliente ORDER BY Apellido, Nombre;
END
GO

PRINT '✓ SP Cliente_SelectAll creado'
GO

-- SP: Cliente_SelectByDNI
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Cliente_SelectByDNI')
    DROP PROCEDURE Cliente_SelectByDNI;
GO

CREATE PROCEDURE Cliente_SelectByDNI
    @DNI NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Cliente WHERE DNI = @DNI;
END
GO

PRINT '✓ SP Cliente_SelectByDNI creado'
GO

-- SP: Cliente_Search
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Cliente_Search')
    DROP PROCEDURE Cliente_Search;
GO

CREATE PROCEDURE Cliente_Search
    @Criterio NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Cliente
    WHERE Nombre LIKE '%' + @Criterio + '%'
       OR Apellido LIKE '%' + @Criterio + '%'
       OR DNI LIKE '%' + @Criterio + '%'
       OR Email LIKE '%' + @Criterio + '%'
    ORDER BY Apellido, Nombre;
END
GO

PRINT '✓ SP Cliente_Search creado'
GO

-- =====================================================
-- STORED PROCEDURES: MASCOTA
-- =====================================================

-- SP: Mascota_Insert
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Mascota_Insert')
    DROP PROCEDURE Mascota_Insert;
GO

CREATE PROCEDURE Mascota_Insert
    @IdMascota UNIQUEIDENTIFIER,
    @IdCliente UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(100),
    @Especie NVARCHAR(50),
    @Raza NVARCHAR(100),
    @FechaNacimiento DATE,
    @Sexo NVARCHAR(10),
    @Peso DECIMAL(6,2),
    @Color NVARCHAR(50),
    @Observaciones NVARCHAR(500),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Mascota (IdMascota, IdCliente, Nombre, Especie, Raza, FechaNacimiento, Sexo, Peso, Color, Observaciones, Activo)
    VALUES (@IdMascota, @IdCliente, @Nombre, @Especie, @Raza, @FechaNacimiento, @Sexo, @Peso, @Color, @Observaciones, @Activo);

    SELECT * FROM Mascota WHERE IdMascota = @IdMascota;
END
GO

PRINT '✓ SP Mascota_Insert creado'
GO

-- SP: Mascota_Update
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Mascota_Update')
    DROP PROCEDURE Mascota_Update;
GO

CREATE PROCEDURE Mascota_Update
    @IdMascota UNIQUEIDENTIFIER,
    @IdCliente UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(100),
    @Especie NVARCHAR(50),
    @Raza NVARCHAR(100),
    @FechaNacimiento DATE,
    @Sexo NVARCHAR(10),
    @Peso DECIMAL(6,2),
    @Color NVARCHAR(50),
    @Observaciones NVARCHAR(500),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Mascota
    SET IdCliente = @IdCliente,
        Nombre = @Nombre,
        Especie = @Especie,
        Raza = @Raza,
        FechaNacimiento = @FechaNacimiento,
        Sexo = @Sexo,
        Peso = @Peso,
        Color = @Color,
        Observaciones = @Observaciones,
        Activo = @Activo
    WHERE IdMascota = @IdMascota;

    SELECT * FROM Mascota WHERE IdMascota = @IdMascota;
END
GO

PRINT '✓ SP Mascota_Update creado'
GO

-- SP: Mascota_Delete
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Mascota_Delete')
    DROP PROCEDURE Mascota_Delete;
GO

CREATE PROCEDURE Mascota_Delete
    @IdMascota UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Mascota WHERE IdMascota = @IdMascota;
END
GO

PRINT '✓ SP Mascota_Delete creado'
GO

-- SP: Mascota_SelectOne
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Mascota_SelectOne')
    DROP PROCEDURE Mascota_SelectOne;
GO

CREATE PROCEDURE Mascota_SelectOne
    @IdMascota UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Mascota WHERE IdMascota = @IdMascota;
END
GO

PRINT '✓ SP Mascota_SelectOne creado'
GO

-- SP: Mascota_SelectAll
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Mascota_SelectAll')
    DROP PROCEDURE Mascota_SelectAll;
GO

CREATE PROCEDURE Mascota_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Mascota ORDER BY Nombre;
END
GO

PRINT '✓ SP Mascota_SelectAll creado'
GO

-- SP: Mascota_SelectByCliente
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Mascota_SelectByCliente')
    DROP PROCEDURE Mascota_SelectByCliente;
GO

CREATE PROCEDURE Mascota_SelectByCliente
    @IdCliente UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Mascota
    WHERE IdCliente = @IdCliente AND Activo = 1
    ORDER BY Nombre;
END
GO

PRINT '✓ SP Mascota_SelectByCliente creado'
GO

-- SP: Mascota_Search
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Mascota_Search')
    DROP PROCEDURE Mascota_Search;
GO

CREATE PROCEDURE Mascota_Search
    @Criterio NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM Mascota
    WHERE Nombre LIKE '%' + @Criterio + '%'
       OR Especie LIKE '%' + @Criterio + '%'
       OR Raza LIKE '%' + @Criterio + '%'
    ORDER BY Nombre;
END
GO

PRINT '✓ SP Mascota_Search creado'
GO

PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║   STORED PROCEDURES CREADOS EXITOSAMENTE         ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Cliente: 7 SPs (Insert, Update, Delete, SelectOne, SelectAll, SelectByDNI, Search)'
PRINT 'Mascota: 7 SPs (Insert, Update, Delete, SelectOne, SelectAll, SelectByCliente, Search)'
PRINT ''
GO
