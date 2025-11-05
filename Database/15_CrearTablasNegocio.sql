-- =====================================================
-- Script: Crear Tablas de Negocio - VetCare
-- Descripción: Define tablas Cliente y Mascota
-- =====================================================

USE VetCareDB;
GO

PRINT 'Creando tablas de negocio VetCare...'
PRINT ''

-- =====================================================
-- Tabla: Cliente
-- Descripción: Clientes (dueños de mascotas)
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cliente')
BEGIN
    CREATE TABLE Cliente (
        IdCliente UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Nombre NVARCHAR(100) NOT NULL,
        Apellido NVARCHAR(100) NOT NULL,
        DNI NVARCHAR(20) NOT NULL UNIQUE,
        Telefono NVARCHAR(20) NULL,
        Email NVARCHAR(150) NULL,
        Direccion NVARCHAR(255) NULL,
        FechaRegistro DATETIME DEFAULT GETDATE(),
        Activo BIT DEFAULT 1,
        CONSTRAINT CHK_Cliente_Nombre CHECK (LEN(Nombre) >= 2),
        CONSTRAINT CHK_Cliente_Apellido CHECK (LEN(Apellido) >= 2),
        CONSTRAINT CHK_Cliente_DNI CHECK (LEN(DNI) >= 6)
    );

    PRINT '✓ Tabla Cliente creada'
END
ELSE
BEGIN
    PRINT '⚠ Tabla Cliente ya existe'
END
GO

-- Crear índice para búsquedas por DNI
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Cliente_DNI')
BEGIN
    CREATE INDEX IX_Cliente_DNI ON Cliente(DNI);
    PRINT '✓ Índice IX_Cliente_DNI creado'
END
GO

-- Crear índice para búsquedas por apellido
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Cliente_Apellido')
BEGIN
    CREATE INDEX IX_Cliente_Apellido ON Cliente(Apellido);
    PRINT '✓ Índice IX_Cliente_Apellido creado'
END
GO

-- =====================================================
-- Tabla: Mascota
-- Descripción: Mascotas de los clientes
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Mascota')
BEGIN
    CREATE TABLE Mascota (
        IdMascota UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        IdCliente UNIQUEIDENTIFIER NOT NULL,
        Nombre NVARCHAR(100) NOT NULL,
        Especie NVARCHAR(50) NOT NULL, -- Perro, Gato, Ave, etc.
        Raza NVARCHAR(100) NULL,
        FechaNacimiento DATE NOT NULL,
        Sexo NVARCHAR(10) NOT NULL, -- Macho, Hembra
        Peso DECIMAL(6,2) NULL, -- En kilogramos
        Color NVARCHAR(50) NULL,
        Observaciones NVARCHAR(500) NULL,
        FechaRegistro DATETIME DEFAULT GETDATE(),
        Activo BIT DEFAULT 1,
        CONSTRAINT FK_Mascota_Cliente FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente) ON DELETE CASCADE,
        CONSTRAINT CHK_Mascota_Nombre CHECK (LEN(Nombre) >= 2),
        CONSTRAINT CHK_Mascota_Especie CHECK (LEN(Especie) >= 2),
        CONSTRAINT CHK_Mascota_Sexo CHECK (Sexo IN ('Macho', 'Hembra')),
        CONSTRAINT CHK_Mascota_Peso CHECK (Peso IS NULL OR Peso >= 0),
        CONSTRAINT CHK_Mascota_FechaNacimiento CHECK (FechaNacimiento <= GETDATE())
    );

    PRINT '✓ Tabla Mascota creada'
END
ELSE
BEGIN
    PRINT '⚠ Tabla Mascota ya existe'
END
GO

-- Crear índice para búsquedas por cliente
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Mascota_IdCliente')
BEGIN
    CREATE INDEX IX_Mascota_IdCliente ON Mascota(IdCliente);
    PRINT '✓ Índice IX_Mascota_IdCliente creado'
END
GO

-- Crear índice para búsquedas por especie
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Mascota_Especie')
BEGIN
    CREATE INDEX IX_Mascota_Especie ON Mascota(Especie);
    PRINT '✓ Índice IX_Mascota_Especie creado'
END
GO

PRINT ''
PRINT '╔════════════════════════════════════════════════════╗'
PRINT '║   TABLAS DE NEGOCIO CREADAS EXITOSAMENTE         ║'
PRINT '╚════════════════════════════════════════════════════╝'
PRINT ''
PRINT 'Tablas creadas:'
PRINT '  • Cliente (con índices en DNI y Apellido)'
PRINT '  • Mascota (con índices en IdCliente y Especie)'
PRINT ''
PRINT 'Relaciones:'
PRINT '  • Mascota.IdCliente → Cliente.IdCliente (CASCADE DELETE)'
PRINT ''
GO
