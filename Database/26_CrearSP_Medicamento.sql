-- =============================================
-- Script: 26_CrearSP_Medicamento.sql
-- Descripción: Stored Procedures para gestión de Medicamentos
-- =============================================

USE VetCareDB;
GO

-- =============================================
-- SP: Medicamento_Insert
-- =============================================
IF OBJECT_ID('Medicamento_Insert', 'P') IS NOT NULL
    DROP PROCEDURE Medicamento_Insert;
GO

CREATE PROCEDURE Medicamento_Insert
    @IdMedicamento UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(150),
    @Presentacion NVARCHAR(100),
    @Stock INT,
    @PrecioUnitario DECIMAL(10,2),
    @Observaciones NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Medicamento (IdMedicamento, Nombre, Presentacion, Stock, PrecioUnitario, Observaciones, FechaRegistro, Activo)
    VALUES (@IdMedicamento, @Nombre, @Presentacion, @Stock, @PrecioUnitario, @Observaciones, GETDATE(), 1);

    -- Retornar el registro insertado
    SELECT * FROM Medicamento WHERE IdMedicamento = @IdMedicamento;
END
GO

-- =============================================
-- SP: Medicamento_Update
-- =============================================
IF OBJECT_ID('Medicamento_Update', 'P') IS NOT NULL
    DROP PROCEDURE Medicamento_Update;
GO

CREATE PROCEDURE Medicamento_Update
    @IdMedicamento UNIQUEIDENTIFIER,
    @Nombre NVARCHAR(150),
    @Presentacion NVARCHAR(100),
    @Stock INT,
    @PrecioUnitario DECIMAL(10,2),
    @Observaciones NVARCHAR(500),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Medicamento
    SET Nombre = @Nombre,
        Presentacion = @Presentacion,
        Stock = @Stock,
        PrecioUnitario = @PrecioUnitario,
        Observaciones = @Observaciones,
        Activo = @Activo
    WHERE IdMedicamento = @IdMedicamento;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO

-- =============================================
-- SP: Medicamento_Delete (Soft Delete)
-- =============================================
IF OBJECT_ID('Medicamento_Delete', 'P') IS NOT NULL
    DROP PROCEDURE Medicamento_Delete;
GO

CREATE PROCEDURE Medicamento_Delete
    @IdMedicamento UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Medicamento
    SET Activo = 0
    WHERE IdMedicamento = @IdMedicamento;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO

-- =============================================
-- SP: Medicamento_SelectAll
-- =============================================
IF OBJECT_ID('Medicamento_SelectAll', 'P') IS NOT NULL
    DROP PROCEDURE Medicamento_SelectAll;
GO

CREATE PROCEDURE Medicamento_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Medicamento
    WHERE Activo = 1
    ORDER BY Nombre;
END
GO

-- =============================================
-- SP: Medicamento_SelectOne
-- =============================================
IF OBJECT_ID('Medicamento_SelectOne', 'P') IS NOT NULL
    DROP PROCEDURE Medicamento_SelectOne;
GO

CREATE PROCEDURE Medicamento_SelectOne
    @IdMedicamento UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Medicamento
    WHERE IdMedicamento = @IdMedicamento;
END
GO

-- =============================================
-- SP: Medicamento_Search
-- Busca medicamentos por nombre (LIKE)
-- =============================================
IF OBJECT_ID('Medicamento_Search', 'P') IS NOT NULL
    DROP PROCEDURE Medicamento_Search;
GO

CREATE PROCEDURE Medicamento_Search
    @Criterio NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Medicamento
    WHERE Activo = 1
      AND (Nombre LIKE '%' + @Criterio + '%' OR Presentacion LIKE '%' + @Criterio + '%')
    ORDER BY Nombre;
END
GO

-- =============================================
-- SP: Medicamento_SelectDisponibles
-- Retorna solo medicamentos con stock disponible
-- =============================================
IF OBJECT_ID('Medicamento_SelectDisponibles', 'P') IS NOT NULL
    DROP PROCEDURE Medicamento_SelectDisponibles;
GO

CREATE PROCEDURE Medicamento_SelectDisponibles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Medicamento
    WHERE Activo = 1
      AND Stock > 0
    ORDER BY Nombre;
END
GO

-- =============================================
-- SP: Medicamento_ActualizarStock
-- Actualiza el stock de un medicamento (incremento o decremento)
-- =============================================
IF OBJECT_ID('Medicamento_ActualizarStock', 'P') IS NOT NULL
    DROP PROCEDURE Medicamento_ActualizarStock;
GO

CREATE PROCEDURE Medicamento_ActualizarStock
    @IdMedicamento UNIQUEIDENTIFIER,
    @CantidadCambio INT  -- Positivo para incrementar, negativo para decrementar
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar que el stock resultante no sea negativo
    DECLARE @StockActual INT;
    SELECT @StockActual = Stock FROM Medicamento WHERE IdMedicamento = @IdMedicamento;

    IF (@StockActual + @CantidadCambio) < 0
    BEGIN
        RAISERROR('Stock insuficiente', 16, 1);
        RETURN;
    END

    UPDATE Medicamento
    SET Stock = Stock + @CantidadCambio
    WHERE IdMedicamento = @IdMedicamento;

    -- Retornar el registro actualizado
    SELECT * FROM Medicamento WHERE IdMedicamento = @IdMedicamento;
END
GO

PRINT 'Stored Procedures de Medicamento creados exitosamente';
GO
