-- =====================================================
-- Script: Crear Tablas - Sistema VetCare
-- Descripción: Define todas las tablas del sistema
-- =====================================================

USE SeguridadBiblioteca;
GO

PRINT 'Creando tablas del sistema...'
PRINT ''

-- =====================================================
-- Tabla: Usuario
-- Descripción: Usuarios del sistema con DVH
-- =====================================================
CREATE TABLE Usuario (
    IdUsuario UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(150) NULL,
    Clave NVARCHAR(256) NOT NULL, -- Password hasheado
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaUltimoAcceso DATETIME NULL,
    Activo BIT DEFAULT 1,
    IdiomaPreferido NVARCHAR(10) NULL DEFAULT 'es-AR', -- Idioma preferido del usuario
    DVH NVARCHAR(64) NULL, -- Dígito Verificador Horizontal
    CONSTRAINT CHK_Usuario_Nombre CHECK (LEN(Nombre) >= 3)
);

PRINT '✓ Tabla Usuario creada (con DVH e IdiomaPreferido)'

-- =====================================================
-- Tabla: Familia
-- Descripción: Grupos de permisos (incluye ROLES)
-- Nota: Familias con nombre "ROL_*" son roles del sistema
-- =====================================================
CREATE TABLE Familia (
    IdFamilia UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255) NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT CHK_Familia_Nombre CHECK (LEN(Nombre) >= 3)
);

PRINT '✓ Tabla Familia creada'

-- =====================================================
-- Tabla: Patente
-- Descripción: Permisos atómicos del sistema
-- =====================================================
CREATE TABLE Patente (
    IdPatente UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    FormName NVARCHAR(100) NOT NULL, -- Nombre del formulario asociado
    MenuItemName NVARCHAR(100) NOT NULL, -- Nombre para mostrar en menú
    Orden INT DEFAULT 0, -- Orden de visualización
    Descripcion NVARCHAR(255) NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT CHK_Patente_FormName CHECK (LEN(FormName) >= 3)
);

PRINT '✓ Tabla Patente creada'

-- =====================================================
-- Tabla: UsuarioFamilia
-- Descripción: Relación muchos a muchos Usuario-Familia
-- Asigna roles y grupos de permisos a usuarios
-- =====================================================
CREATE TABLE UsuarioFamilia (
    idUsuario UNIQUEIDENTIFIER NOT NULL,
    idFamilia UNIQUEIDENTIFIER NOT NULL,
    FechaAsignacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT PK_UsuarioFamilia PRIMARY KEY (idUsuario, idFamilia),
    CONSTRAINT FK_UsuarioFamilia_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(IdUsuario) ON DELETE CASCADE,
    CONSTRAINT FK_UsuarioFamilia_Familia FOREIGN KEY (idFamilia) REFERENCES Familia(IdFamilia) ON DELETE CASCADE
);

PRINT '✓ Tabla UsuarioFamilia creada'

-- =====================================================
-- Tabla: UsuarioPatente
-- Descripción: Relación muchos a muchos Usuario-Patente
-- Permisos individuales asignados directamente a usuarios
-- =====================================================
CREATE TABLE UsuarioPatente (
    idUsuario UNIQUEIDENTIFIER NOT NULL,
    idPatente UNIQUEIDENTIFIER NOT NULL,
    FechaAsignacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT PK_UsuarioPatente PRIMARY KEY (idUsuario, idPatente),
    CONSTRAINT FK_UsuarioPatente_Usuario FOREIGN KEY (idUsuario) REFERENCES Usuario(IdUsuario) ON DELETE CASCADE,
    CONSTRAINT FK_UsuarioPatente_Patente FOREIGN KEY (idPatente) REFERENCES Patente(IdPatente) ON DELETE CASCADE
);

PRINT '✓ Tabla UsuarioPatente creada'

-- =====================================================
-- Tabla: FamiliaFamilia
-- Descripción: Jerarquía de Familias (Composite Pattern)
-- Permite que una Familia contenga otras Familias
-- =====================================================
CREATE TABLE FamiliaFamilia (
    IdFamiliaPadre UNIQUEIDENTIFIER NOT NULL,
    IdFamiliaHijo UNIQUEIDENTIFIER NOT NULL,
    FechaAsignacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT PK_FamiliaFamilia PRIMARY KEY (IdFamiliaPadre, IdFamiliaHijo),
    CONSTRAINT FK_FamiliaFamilia_Padre FOREIGN KEY (IdFamiliaPadre) REFERENCES Familia(IdFamilia),
    CONSTRAINT FK_FamiliaFamilia_Hijo FOREIGN KEY (IdFamiliaHijo) REFERENCES Familia(IdFamilia),
    CONSTRAINT CHK_FamiliaFamilia_NoSelfReference CHECK (IdFamiliaPadre != IdFamiliaHijo)
);

PRINT '✓ Tabla FamiliaFamilia creada'

-- =====================================================
-- Tabla: FamiliaPatente
-- Descripción: Relación muchos a muchos Familia-Patente
-- Define qué Patentes contiene cada Familia
-- =====================================================
CREATE TABLE FamiliaPatente (
    idFamilia UNIQUEIDENTIFIER NOT NULL,
    idPatente UNIQUEIDENTIFIER NOT NULL,
    FechaAsignacion DATETIME DEFAULT GETDATE(),
    CONSTRAINT PK_FamiliaPatente PRIMARY KEY (idFamilia, idPatente),
    CONSTRAINT FK_FamiliaPatente_Familia FOREIGN KEY (idFamilia) REFERENCES Familia(IdFamilia) ON DELETE CASCADE,
    CONSTRAINT FK_FamiliaPatente_Patente FOREIGN KEY (idPatente) REFERENCES Patente(IdPatente) ON DELETE CASCADE
);

PRINT '✓ Tabla FamiliaPatente creada'

-- =====================================================
-- Tabla: Logger
-- Descripción: Logs del sistema
-- =====================================================
CREATE TABLE Logger (
    IdLog UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Fecha DATETIME DEFAULT GETDATE(),
    Nivel NVARCHAR(20) NOT NULL, -- Critical, Error, Warning, Info
    Mensaje NVARCHAR(MAX) NOT NULL,
    Modulo NVARCHAR(100) NULL,
    IdUsuario UNIQUEIDENTIFIER NULL,
    CONSTRAINT FK_Logger_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);

PRINT '✓ Tabla Logger creada'

PRINT ''
PRINT '=== Todas las tablas creadas exitosamente ==='
PRINT ''
GO

-- =====================================================
-- Índices para mejorar performance
-- =====================================================
PRINT 'Creando índices...'

CREATE INDEX IX_Usuario_Nombre ON Usuario(Nombre);
CREATE INDEX IX_Usuario_Email ON Usuario(Email);
CREATE INDEX IX_Familia_Nombre ON Familia(Nombre);
CREATE INDEX IX_FamiliaFamilia_Padre ON FamiliaFamilia(IdFamiliaPadre);
CREATE INDEX IX_FamiliaFamilia_Hijo ON FamiliaFamilia(IdFamiliaHijo);
CREATE INDEX IX_Language_Culture ON Language(Culture);
CREATE INDEX IX_Logger_Fecha ON Logger(Fecha DESC);
CREATE INDEX IX_Logger_Nivel ON Logger(Nivel);

PRINT '✓ Índices creados'
PRINT ''
GO
