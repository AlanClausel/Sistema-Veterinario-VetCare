USE SecurityVet;
GO

-- Crear tabla Bitacora para auditoría del sistema
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Bitacora')
BEGIN
    CREATE TABLE Bitacora (
        IdBitacora UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        IdUsuario UNIQUEIDENTIFIER NULL, -- NULL para eventos del sistema sin usuario
        NombreUsuario VARCHAR(50) NULL, -- Denormalizado para preservar historial
        FechaHora DATETIME NOT NULL DEFAULT GETDATE(),
        Modulo VARCHAR(50) NOT NULL, -- Ej: "Clientes", "Citas", "Usuarios", "Sistema"
        Accion VARCHAR(50) NOT NULL, -- Ej: "Login", "Alta", "Baja", "Modificacion", "Consulta"
        Descripcion VARCHAR(500) NOT NULL,
        Tabla VARCHAR(50) NULL, -- Tabla afectada (opcional)
        IdRegistro VARCHAR(100) NULL, -- ID del registro afectado (opcional)
        Criticidad VARCHAR(20) NOT NULL, -- "Info", "Advertencia", "Error", "Critico"
        IP VARCHAR(45) NULL, -- Dirección IP (opcional, soporta IPv6)

        FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario) ON DELETE SET NULL
    );

    -- Índices para mejorar consultas
    CREATE INDEX IX_Bitacora_FechaHora ON Bitacora(FechaHora DESC);
    CREATE INDEX IX_Bitacora_IdUsuario ON Bitacora(IdUsuario);
    CREATE INDEX IX_Bitacora_Modulo ON Bitacora(Modulo);
    CREATE INDEX IX_Bitacora_Accion ON Bitacora(Accion);
    CREATE INDEX IX_Bitacora_Criticidad ON Bitacora(Criticidad);

    PRINT 'Tabla Bitacora creada exitosamente';
END
ELSE
BEGIN
    PRINT 'La tabla Bitacora ya existe';
END
GO
