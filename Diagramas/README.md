# Diagramas - Sistema VetCare

Este directorio contiene todos los diagramas UML del sistema organizados por tipo.

## 📁 Estructura de Carpetas

```
Diagramas/
├── Casos de Uso/              → Diagramas de casos de uso
│   ├── CU-A001_IniciarSesion.puml
│   └── CU-A001_IniciarSesion.xmi
│
├── Diagrama de Secuencia/     → Diagramas de secuencia
│   ├── CU-A001_DiagramaSecuencia.puml
│   └── README_DiagramaSecuencia.txt
│
└── Documentacion/             → Especificaciones y templates
    ├── CU-A001_INICIAR_SESION.md
    └── CU-A001_Template_EA.txt
```

## 📋 Casos de Uso Documentados

### CU-A001: Iniciar Sesión
**Estado:** ✅ Completo
**Archivos:**
- Diagrama de casos de uso (PlantUML): `Casos de Uso/CU-A001_IniciarSesion.puml`
- Modelo XMI para EA: `Casos de Uso/CU-A001_IniciarSesion.xmi`
- Diagrama de secuencia (PlantUML): `Diagrama de Secuencia/CU-A001_DiagramaSecuencia.puml`
- Especificación completa (Markdown): `Documentacion/CU-A001_INICIAR_SESION.md`
- Template para EA (TXT): `Documentacion/CU-A001_Template_EA.txt`

**Descripción:** Autenticación de usuarios con validación de credenciales, verificación DVH, carga de permisos (Composite), registro en bitácora y sincronización de veterinario.

**Patrones aplicados:**
- Composite (permisos jerárquicos)
- Singleton (servicios)
- Observer (multi-idioma)
- Repository (acceso a datos)

---

## 🛠️ Herramientas para Visualizar

### PlantUML (.puml)
**Opción 1 - VS Code:**
1. Instala extensión "PlantUML"
2. Abre archivo .puml
3. Presiona Alt+D

**Opción 2 - Online:**
- http://www.plantuml.com/plantuml/uml/

### Enterprise Architect (.xmi)
1. File → Import/Export → Import Package from XMI
2. Selecciona archivo .xmi
3. Import

### Markdown (.md)
- Cualquier editor de Markdown
- VS Code con extensión "Markdown Preview"
- GitHub/GitLab (vista automática)

### CU-A002: Gestionar Usuarios
**Estado:** ✅ Completo
**Archivos:**
- Especificación completa (TXT): `Documentacion/CU-A002_GESTIONAR_USUARIOS.txt`
- Diagrama de casos de uso (PlantUML): `Casos de Uso/CU-A002_GestionarUsuarios.puml`
- Diagrama de secuencia - Crear Usuario (PlantUML): `Diagrama de Secuencia/CU-A002_CrearUsuario_Secuencia.puml`
- Guía del diagrama de secuencia (TXT): `Diagrama de Secuencia/README_CU-A002_DiagramaSecuencia.txt`

**Descripción:** CRUD completo de usuarios del sistema. Incluye creación con hasheo SHA256, modificación con cambio de rol (Unit of Work), eliminación soft delete, búsqueda y listado. Validaciones: email formato regex, contraseña mínima 6 chars, nombre único, prevención auto-eliminación. Auditoría completa en Bitácora.

**Patrones aplicados:**
- Singleton (BLL y Repositories)
- Unit of Work (cambio de rol atómico)
- Adapter (DataTable → List<Usuario>)
- Repository (abstracción de datos)
- Exception Manager (manejo centralizado)

---

### CU-A003: Gestionar Permisos (Roles y Permisos)
**Estado:** ✅ Completo
**Archivos:**
- Especificación completa (TXT): `Documentacion/CU-A003_GESTIONAR_PERMISOS.txt`
- Template EA resumido (TXT): `Documentacion/CU-A003_Template_EA.txt`
- Diagrama de casos de uso (PlantUML): `Casos de Uso/CU-A003_GestionarPermisos.puml`
- Diagrama de secuencia - Actualizar Permisos (PlantUML): `Diagrama de Secuencia/CU-A003_ActualizarPermisosRol_Secuencia.puml`
- Guía del diagrama de secuencia (TXT): `Diagrama de Secuencia/README_CU-A003_DiagramaSecuencia.txt`

**Descripción:** Administración completa del sistema de autorización. 2 áreas funcionales: (1) Gestión de Roles - Crear/eliminar roles con prefijo ROL_, asignación masiva de permisos, protección de roles del sistema. (2) Gestión de Permisos de Usuarios - Cambio de rol atómico con Unit of Work, permisos individuales adicionales al rol. Navegación recursiva de jerarquías con Composite pattern. Validaciones: roles protegidos no eliminables, roles con usuarios no eliminables, nombres únicos, confirmación explícita. TODAS las operaciones críticas registradas en Bitácora.

**Patrones aplicados:**
- Composite (jerarquía Familia composite + Patente leaf, recursión para herencia de permisos)
- Unit of Work (transacciones atómicas con ROLLBACK automático en errores)
- Singleton (BLL estático, Repositories .Current)
- Repository (abstracción de acceso a datos)
- Exception Manager (manejo centralizado de errores)

---

### CU-A004: Consultar Bitácora
**Estado:** ✅ Completo
**Archivos:**
- Template EA resumido (TXT): `Documentacion/CU-A004_Template_EA.txt`
- Diagrama de casos de uso (PlantUML): `Casos de Uso/CU-A004_ConsultarBitacora.puml`
- Diagrama de secuencia - Filtrar Bitácora (PlantUML): `Diagrama de Secuencia/CU-A004_FiltrarBitacora_Secuencia.puml`

**Descripción:** Consulta del historial de auditoría del sistema. Filtros combinables: rango de fechas (defecto últimos 7 días), módulo, acción, criticidad. Visualización con colores automáticos por criticidad (Crítico=rojo, Error=naranja, Advertencia=amarillo, Info=blanco). Exportación a Excel. ComboBoxes dinámicos actualizados según datos cargados. Límite de 1000 registros por performance. SOLO LECTURA, no modifica registros de bitácora. Permite monitorear actividad del sistema, detectar anomalías, auditar logins fallidos y cambios administrativos.

**Patrones aplicados:**
- Singleton (BitacoraBLL estático, BitacoraRepository .Current)
- Repository (abstracción de acceso a datos)
- Adapter (DataRow → Bitacora entity)

---

### CU-A005: Realizar Backup/Restore
**Estado:** ✅ Completo
**Archivos:**
- Template EA resumido (TXT): `Documentacion/CU-A005_A006_A007_Templates_EA.txt`

**Descripción:** Copias de seguridad y restauración de bases de datos SecurityVet y VetCareDB. Backup genera archivos .bak con timestamp en carpeta seleccionada. Restore requiere 2 confirmaciones (CRÍTICO: sobrescribe todos los datos). Validaciones: directorio destino válido, permisos de escritura, archivo .bak válido. Log en tiempo real con timestamps. Progress bar marquee durante operaciones. Permite seleccionar 1 o ambas BD para backup simultáneo. Garantiza continuidad del negocio y recuperación ante desastres.

**Patrones aplicados:**
- Observer (hereda BaseObservableForm para multi-idioma)
- Singleton (BackupRestoreService)

---

### CU-A006: Gestionar Mi Cuenta
**Estado:** ✅ Completo
**Archivos:**
- Template EA resumido (TXT): `Documentacion/CU-A005_A006_A007_Templates_EA.txt`

**Descripción:** Autogestión de cuenta para cualquier usuario autenticado. Cambio de contraseña con validación de actual (hash SHA256), nueva mínimo 6 caracteres, confirmación debe coincidir. Cambio de idioma (es-AR / en-GB) activa patrón Observer: LanguageManager notifica a TODOS los formularios abiertos que heredan de BaseObservableForm, cambian automáticamente sin reiniciar. Visualización de información de usuario (nombre, email, rol) en solo lectura. Campos de contraseña con opción "Mostrar contraseñas".

**Patrones aplicados:**
- Observer (cambio de idioma notifica a todos los formularios abiertos, actualización automática)
- Singleton (UsuarioBLL, LanguageManager)

---

### CU-A007: Cerrar Sesión (Logout)
**Estado:** ✅ Completo
**Archivos:**
- Template EA resumido (TXT): `Documentacion/CU-A005_A006_A007_Templates_EA.txt`

**Descripción:** Cierre seguro de sesión del usuario actual. Confirmación antes de cerrar. Registro de logout en Bitácora (módulo Login, acción Logout, criticidad Info). Cierra formulario menu y retorna control a pantalla de Login. Limpia variable _usuarioLogueado. Disponible para cualquier usuario autenticado (no requiere patente específica). Crítico para auditoría: cada logout queda registrado con usuario, fecha/hora exacta.

**Patrones aplicados:**
- Singleton (Bitacora service para registro de logout)

---

## 🏥 CASOS DE USO DE NEGOCIO

### CU-N001: Gestionar Clientes
**Estado:** ✅ Completo
**Archivos:**
- Template EA resumido (TXT): `Documentacion/CU-N001_Template_EA.txt`

**Descripción:** CRUD completo de clientes (dueños de mascotas) del sistema veterinario. Incluye creación con validaciones de DNI único y email formato válido, modificación, eliminación soft delete con cascada a mascotas, búsqueda flexible por nombre/apellido/DNI/email, y visualización de mascotas asociadas en grid maestro-detalle. 6 operaciones principales: Crear, Modificar, Eliminar, Buscar, Listar Todos, Visualizar Mascotas. Validaciones: DNI único (índice en BD), email formato válido (System.Net.Mail.MailAddress), nombre y apellido mínimo 2 chars, DNI mínimo 6 chars, teléfono mínimo 7 chars (opcional). Relación 1:N con Mascotas, eliminación cascada (soft delete Activo=0). Auditoría completa: Alta, Modificación, Baja en Bitácora.

**Patrones aplicados:**
- Singleton (ClienteBLL estático, ClienteRepository .Current)
- Repository (abstracción de acceso a datos, IClienteRepository)
- Adapter (ClienteAdapter: DataRow → Cliente entity)
- Exception Manager (manejo centralizado de errores con Bitácora)

---

### CU-N002: Gestionar Mascotas
**Estado:** ✅ Completo
**Archivos:**
- Template EA resumido (TXT): `Documentacion/CU-N002_Template_EA.txt`

**Descripción:** CRUD completo de mascotas (pacientes veterinarios). Incluye creación con validación de dueño activo, modificación, eliminación con verificación de citas activas, búsqueda por nombre/especie/raza, y listado completo. REQUIERE cliente previo (no puede crear mascota sin dueño). Calcula edad automáticamente desde fecha de nacimiento. 5 operaciones principales: Crear, Modificar, Eliminar, Buscar, Listar Todas. Validaciones: Nombre mínimo 2 chars, especie mínimo 2 chars, sexo SOLO "Macho" o "Hembra", fecha nacimiento no futura (y no anterior a 1900), peso 0-1000 kg, dueño existente y activo. REGLA CRÍTICA: NO puede eliminar mascota con citas activas (Agendada/Confirmada) - debe cancelar/completar citas primero. Relación N:1 con Cliente (FK IdCliente obligatoria). Auditoría completa: Alta, Modificación, Baja, Transferencia en Bitácora. Soft delete preserva historial médico completo.

**Patrones aplicados:**
- Singleton (MascotaBLL estático, MascotaRepository .Current)
- Repository (abstracción de acceso a datos, IMascotaRepository)
- Adapter (MascotaAdapter: DataRow → Mascota entity)
- Exception Manager (manejo centralizado de errores con Bitácora)

---

### CU-N003: Gestionar Citas
**Estado:** ✅ Completo
**Archivos:**
- Template EA resumido (TXT): `Documentacion/CU-N003_Template_EA.txt`

**Descripción:** Gestión completa de citas veterinarias con máquina de estados, filtros avanzados, validación de conflictos de horario y código de colores. 8 operaciones principales: Listar/Filtrar (fecha/veterinario/estado), Agendar con validación horarios ±30 minutos, Modificar (solo Agendadas/Confirmadas), Cancelar, Actualizar Estado según flujo permitido, Ver Detalle, Limpiar Filtros. Estados: Agendada → Confirmada → Completada | Cancelada | No Asistió. REGLAS CRÍTICAS: (1) Conflicto Horario - NO permite agendar si veterinario o mascota tienen cita en ±30 minutos, (2) Modificación/Cancelación SOLO Agendadas o Confirmadas, (3) Completada NO cambia (terminal), Cancelada/NoAsistio solo reagendan a Agendada, (4) NO agendar en pasado. Filtros: Hoy/Semana/Mes/Todas/Fecha Específica, Veterinario, Estado. Colores automáticos: Agendada=amarillo, Confirmada=verde, Completada=azul, Cancelada=coral, NoAsistió=gris. Validación transiciones estado con State Machine pattern. Auditoría: Agendar (Info), Cancelar (Advertencia), Confirmar (Info).

**Patrones aplicados:**
- Singleton (CitaBLL thread-safe con lock, CitaRepository .Current)
- Repository (abstracción de acceso a datos, ICitaRepository)
- Adapter (CitaAdapter con JOINs: DataRow → Cita + Mascota + Cliente)
- State Machine (EstadoCita enum con validación de transiciones)
- Exception Manager (manejo centralizado de errores con Bitácora)

---

## 📊 Próximos Diagramas a Crear

- [ ] CU-N015: Registrar Consulta Médica
- [ ] CU-N009: Agendar Cita
- [ ] CU-A010: Asignar Patentes a Familia
- [ ] CU-A019: Realizar Backup
- [ ] Diagrama de Clases (Dominio de Negocio)
- [ ] Diagrama de Componentes (Arquitectura en Capas)
- [ ] Diagrama de Despliegue

---

## 📝 Convenciones de Nomenclatura

**Casos de Uso:**
- Arquitectura: `CU-A###` (ej: CU-A001)
- Negocio: `CU-N###` (ej: CU-N001)

**Archivos:**
- Diagramas PlantUML: `CU-[código]_[Nombre].puml`
- Modelos XMI: `CU-[código]_[Nombre].xmi`
- Especificaciones: `CU-[código]_[NOMBRE].md`
- Templates EA: `CU-[código]_Template_EA.txt`
- Diagramas de secuencia: `CU-[código]_DiagramaSecuencia.puml`

---

**Última actualización:** 2025-01-16
**Versión:** 2.3
