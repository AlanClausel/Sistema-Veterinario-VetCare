-- ═══════════════════════════════════════════════════════════════════
--  EXPORT DDL - VETCAREDB (Solo CREATE TABLE para Enterprise Architect)
-- ═══════════════════════════════════════════════════════════════════
-- Este archivo contiene solo las definiciones de tablas (DDL)
-- sin datos, para importar en herramientas de modelado
-- ═══════════════════════════════════════════════════════════════════

USE VetCareDB
GO

-- ═══════════════════════════════════════════════════════════════════
--  TABLA: Cliente
-- ═══════════════════════════════════════════════════════════════════

CREATE TABLE Cliente (
    IdCliente UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DNI NVARCHAR(20) NOT NULL UNIQUE,
    Telefono NVARCHAR(50),
    Email NVARCHAR(100),
    Direccion NVARCHAR(200),
    Activo BIT NOT NULL DEFAULT 1
);

-- ═══════════════════════════════════════════════════════════════════
--  TABLA: Mascota
-- ═══════════════════════════════════════════════════════════════════

CREATE TABLE Mascota (
    IdMascota UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdCliente UNIQUEIDENTIFIER NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Especie NVARCHAR(50) NOT NULL,
    Raza NVARCHAR(100),
    FechaNacimiento DATETIME,
    Sexo NVARCHAR(1),
    Peso DECIMAL(10, 2),
    Color NVARCHAR(50),
    Observaciones NVARCHAR(MAX),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Mascota_Cliente FOREIGN KEY (IdCliente)
        REFERENCES Cliente(IdCliente)
);

-- ═══════════════════════════════════════════════════════════════════
--  TABLA: Veterinario
-- ═══════════════════════════════════════════════════════════════════

CREATE TABLE Veterinario (
    IdVeterinario UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(100) NOT NULL,
    Matricula NVARCHAR(50) NOT NULL UNIQUE,
    Telefono NVARCHAR(50),
    Email NVARCHAR(100),
    Observaciones NVARCHAR(MAX),
    FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1
);

-- ═══════════════════════════════════════════════════════════════════
--  TABLA: Cita
-- ═══════════════════════════════════════════════════════════════════

CREATE TABLE Cita (
    IdCita UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdMascota UNIQUEIDENTIFIER NOT NULL,
    IdVeterinario UNIQUEIDENTIFIER,
    Veterinario NVARCHAR(100), -- LEGACY - deprecated
    FechaHora DATETIME NOT NULL,
    TipoConsulta NVARCHAR(100),
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Agendada',
    Observaciones NVARCHAR(MAX),
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Cita_Mascota FOREIGN KEY (IdMascota)
        REFERENCES Mascota(IdMascota),
    CONSTRAINT FK_Cita_Veterinario FOREIGN KEY (IdVeterinario)
        REFERENCES Veterinario(IdVeterinario)
);

-- ═══════════════════════════════════════════════════════════════════
--  TABLA: Medicamento
-- ═══════════════════════════════════════════════════════════════════

CREATE TABLE Medicamento (
    IdMedicamento UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(200) NOT NULL,
    Presentacion NVARCHAR(200) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    PrecioUnitario DECIMAL(10, 2) NOT NULL DEFAULT 0,
    Observaciones NVARCHAR(MAX),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1
);

-- ═══════════════════════════════════════════════════════════════════
--  TABLA: ConsultaMedica
-- ═══════════════════════════════════════════════════════════════════

CREATE TABLE ConsultaMedica (
    IdConsultaMedica UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdCita UNIQUEIDENTIFIER NOT NULL UNIQUE,
    IdVeterinario UNIQUEIDENTIFIER NOT NULL,
    Diagnostico NVARCHAR(MAX) NOT NULL,
    Tratamiento NVARCHAR(MAX),
    Sintomas NVARCHAR(MAX),
    Observaciones NVARCHAR(MAX),
    FechaConsulta DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ConsultaMedica_Cita FOREIGN KEY (IdCita)
        REFERENCES Cita(IdCita),
    CONSTRAINT FK_ConsultaMedica_Veterinario FOREIGN KEY (IdVeterinario)
        REFERENCES Veterinario(IdVeterinario)
);

-- ═══════════════════════════════════════════════════════════════════
--  TABLA: ConsultaMedicamento (Relación N:M)
-- ═══════════════════════════════════════════════════════════════════

CREATE TABLE ConsultaMedicamento (
    IdConsultaMedicamento UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdConsulta UNIQUEIDENTIFIER NOT NULL,
    IdMedicamento UNIQUEIDENTIFIER NOT NULL,
    Cantidad INT NOT NULL,
    Indicaciones NVARCHAR(MAX),
    CONSTRAINT FK_ConsultaMedicamento_Consulta FOREIGN KEY (IdConsulta)
        REFERENCES ConsultaMedica(IdConsultaMedica),
    CONSTRAINT FK_ConsultaMedicamento_Medicamento FOREIGN KEY (IdMedicamento)
        REFERENCES Medicamento(IdMedicamento)
);

GO
