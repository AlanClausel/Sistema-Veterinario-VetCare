# Módulo Veterinario - Guía de Instalación

## Descripción

Este módulo permite a los veterinarios gestionar sus consultas médicas, ver sus citas asignadas y registrar diagnósticos con recetas de medicamentos.

## Funcionalidades Implementadas

### 1. **Mis Citas** (Formulario MisCitas)
- Ver agenda de citas del día para el veterinario logueado
- Filtrar citas por fecha
- Iniciar consulta médica
- Marcar citas como completadas
- Cancelar citas
- Códigos de color por estado de cita

### 2. **Consulta Médica** (Formulario FormConsultaMedica)
- Visualizar datos de la cita, cliente y mascota
- Registrar síntomas y diagnóstico
- Buscar y seleccionar medicamentos
- Especificar cantidad e indicaciones por medicamento
- Finalizar consulta (actualiza estado de cita y reduce stock)
- Guardar consulta sin finalizar

### 3. **Gestión de Medicamentos** (Backend)
- CRUD completo de medicamentos
- Control de stock automático
- Búsqueda por nombre o presentación
- Alertas de stock bajo

## Instalación

### Paso 1: Ejecutar Scripts SQL

#### A) Base de Datos de Negocio (VetCareDB)

Ejecuta el script maestro que incluye todos los módulos:

```bash
sqlcmd -S localhost -i "Database\00_EJECUTAR_TODO_NEGOCIO.sql"
```

Este script creará:
- Tabla `Medicamento`
- Tabla `ConsultaMedica`
- Tabla `ConsultaMedicamento` (relación muchos a muchos)
- 8 Stored Procedures para Medicamento
- 13 Stored Procedures para ConsultaMedica

#### B) Base de Datos de Seguridad (SecurityVet)

Ejecuta el script de patentes del módulo veterinario:

```bash
sqlcmd -S localhost -d SecurityVet -i "Database\28_CrearPatentesModuloVeterinario.sql"
```

Este script:
- Crea la patente "Mis Citas" (MisCitas)
- Crea la familia ROL_Veterinario si no existe
- Asigna la patente al rol veterinario

### Paso 2: Compilar la Solución

```bash
msbuild "Sistema Veterinario VetCare.sln" /p:Configuration=Debug
```

### Paso 3: Verificar la Instalación

#### Verificar Tablas en VetCareDB:
```sql
USE VetCareDB;
SELECT * FROM Medicamento;
SELECT * FROM ConsultaMedica;
SELECT * FROM ConsultaMedicamento;
```

#### Verificar Patentes en SecurityVet:
```sql
USE SecurityVet;

-- Ver patente creada
SELECT * FROM Patente WHERE FormName = 'MisCitas';

-- Ver asignación al rol
SELECT f.Nombre AS Rol, p.MenuItemName, p.FormName
FROM FamiliaPatente fp
INNER JOIN Familia f ON fp.idFamilia = f.IdFamilia
INNER JOIN Patente p ON fp.idPatente = p.IdPatente
WHERE f.Nombre = 'ROL_Veterinario';
```

## Estructura de Base de Datos

### Medicamento
```sql
IdMedicamento    UNIQUEIDENTIFIER PRIMARY KEY
Nombre           NVARCHAR(150) NOT NULL
Presentacion     NVARCHAR(100)
Stock            INT NOT NULL DEFAULT 0
PrecioUnitario   DECIMAL(10,2) NOT NULL DEFAULT 0
Observaciones    NVARCHAR(500)
FechaRegistro    DATETIME NOT NULL DEFAULT GETDATE()
Activo           BIT NOT NULL DEFAULT 1
```

### ConsultaMedica
```sql
IdConsulta       UNIQUEIDENTIFIER PRIMARY KEY
IdCita           UNIQUEIDENTIFIER NOT NULL (FK → Cita)
IdVeterinario    UNIQUEIDENTIFIER NOT NULL (FK → Veterinario)
Sintomas         NVARCHAR(1000) NOT NULL
Diagnostico      NVARCHAR(1000) NOT NULL
Tratamiento      NVARCHAR(1000)
Observaciones    NVARCHAR(500)
FechaConsulta    DATETIME NOT NULL DEFAULT GETDATE()
Activo           BIT NOT NULL DEFAULT 1
```

### ConsultaMedicamento (Relación N:N)
```sql
IdConsulta       UNIQUEIDENTIFIER (PK, FK → ConsultaMedica)
IdMedicamento    UNIQUEIDENTIFIER (PK, FK → Medicamento)
Cantidad         INT NOT NULL DEFAULT 1
Indicaciones     NVARCHAR(500)
```

## Uso del Sistema

### Para Veterinarios

1. **Iniciar Sesión**
   - Login con usuario que tenga ROL_Veterinario asignado
   - El menú dinámico mostrará automáticamente la opción "Mis Citas"

2. **Ver Citas**
   - Click en "Mis Citas" en el menú
   - Seleccionar fecha (por defecto: hoy)
   - Ver lista de citas asignadas con colores por estado:
     - Amarillo: Agendada
     - Verde claro: Confirmada
     - Gris: Completada
     - Rojo claro: Cancelada
     - Naranja: No asistió

3. **Iniciar Consulta Médica**
   - Seleccionar una cita en estado Agendada o Confirmada
   - Click en "Iniciar Consulta Médica"
   - Se abre el formulario de consulta con:
     - Datos de la cita (solo lectura)
     - Datos del cliente y mascota (solo lectura)
     - Campos editables: Síntomas, Diagnóstico
     - Búsqueda y selección de medicamentos

4. **Agregar Medicamentos**
   - Escribir en el campo "Buscar" (nombre o presentación)
   - Seleccionar medicamento de los resultados
   - Click en "Añadir >"
   - Especificar cantidad e indicaciones
   - El medicamento se agrega a "Seleccionados"
   - Para quitar: seleccionar y click en "< Quitar"

5. **Finalizar Consulta**
   - Completar síntomas y diagnóstico (obligatorios)
   - Agregar medicamentos si es necesario
   - Click en "Finalizar Consulta"
   - El sistema:
     - Guarda la consulta
     - Actualiza el estado de la cita a "Completada"
     - Reduce el stock de los medicamentos recetados
   - Si hay stock insuficiente, muestra error y no finaliza

### Para Administradores

1. **Crear Usuario Veterinario**
   - Ir a "Gestión de Usuarios"
   - Crear nuevo usuario
   - Asignar rol "ROL_Veterinario"
   - El sistema automáticamente crea el registro en la tabla Veterinario de VetCareDB

2. **Gestionar Medicamentos** (Pendiente de UI)
   - Actualmente solo disponible por SQL
   - Futuro: formulario de gestión de medicamentos

## Archivos Creados

### Base de Datos (SQL)
- `Database/24_CrearTablaMedicamento.sql`
- `Database/25_CrearTablaConsultaMedica.sql`
- `Database/26_CrearSP_Medicamento.sql`
- `Database/27_CrearSP_ConsultaMedica.sql`
- `Database/28_CrearPatentesModuloVeterinario.sql`
- `Database/00_EJECUTAR_TODO_NEGOCIO.sql` (actualizado)

### Domain Model
- `VetCareNegocio/DomainModel/Medicamento.cs`
- `VetCareNegocio/DomainModel/ConsultaMedica.cs`
- `VetCareNegocio/DomainModel/Exceptions/ValidacionException.cs`

### Data Access Layer (DAL)
- `VetCareNegocio/DAL/Contracts/IMedicamentoRepository.cs`
- `VetCareNegocio/DAL/Contracts/IConsultaMedicaRepository.cs`
- `VetCareNegocio/DAL/Implementations/MedicamentoRepository.cs`
- `VetCareNegocio/DAL/Implementations/ConsultaMedicaRepository.cs`
- `VetCareNegocio/DAL/Adapters/MedicamentoAdapter.cs`
- `VetCareNegocio/DAL/Adapters/ConsultaMedicaAdapter.cs`

### Business Logic Layer (BLL)
- `VetCareNegocio/BLL/MedicamentoBLL.cs`
- `VetCareNegocio/BLL/ConsultaMedicaBLL.cs`

### User Interface (UI)
- `UI/WinUi/Negocio/MisCitas.cs` + Designer + resx
- `UI/WinUi/Negocio/FormConsultaMedica.cs` + Designer + resx

## Notas Técnicas

### Sincronización Automática
- Cuando se asigna ROL_Veterinario a un usuario, se crea automáticamente en la tabla Veterinario de VetCareDB
- Cuando se actualiza el nombre del usuario veterinario, se sincroniza automáticamente en VetCareDB
- La sincronización se maneja en `ServicesSecurity/BLL/UsuarioBLL.cs`

### Arquitectura
- **Menú Dinámico**: El menú carga automáticamente las opciones según las patentes del usuario
- **Repositorio Pattern**: Singletons con interfaces específicas
- **Stored Procedures**: Toda la lógica de datos usa SPs
- **Transacciones**: `ConsultaMedica_Finalizar` usa transacción para atomicidad

### Validaciones
- Síntomas: mínimo 10 caracteres
- Diagnóstico: mínimo 10 caracteres
- Stock: no permite valores negativos
- Medicamentos: verifica stock disponible antes de finalizar consulta

## Próximos Pasos Sugeridos

1. **Formulario de Gestión de Medicamentos**
   - Crear patente "Gestión de Medicamentos"
   - Asignar a ROL_Administrador o crear ROL_Farmacia
   - Implementar CRUD completo con UI

2. **Reportes**
   - Historial de consultas por veterinario
   - Medicamentos más recetados
   - Control de stock y alertas

3. **Mejoras**
   - Historial clínico completo de la mascota
   - Imprimir receta médica
   - Notificaciones de stock bajo
   - Exportar consultas a PDF

## Troubleshooting

### Error: "No hay usuario logueado"
- Verificar que LoginService esté inicializado correctamente
- Revisar que el usuario haya iniciado sesión antes de abrir MisCitas

### Error: "El usuario actual no es veterinario"
- Verificar que el usuario tenga ROL_Veterinario asignado
- Ejecutar script 28 para crear la patente
- Revisar que el registro exista en tabla Veterinario de VetCareDB

### Error: "Stock insuficiente"
- Verificar stock disponible en tabla Medicamento
- Agregar stock con SP: `Medicamento_ActualizarStock`
- Revisar que no se hayan recetado medicamentos sin stock

### La patente no aparece en el menú
- Ejecutar: `SELECT * FROM Patente WHERE FormName = 'MisCitas'`
- Verificar asignación: `SELECT * FROM FamiliaPatente WHERE idFamilia = (SELECT IdFamilia FROM Familia WHERE Nombre = 'ROL_Veterinario')`
- Reiniciar la aplicación y volver a iniciar sesión

## Contacto y Soporte

Para reportar errores o solicitar nuevas funcionalidades, contactar al equipo de desarrollo.
