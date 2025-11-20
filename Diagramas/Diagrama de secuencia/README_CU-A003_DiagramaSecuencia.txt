================================================================================
DIAGRAMA DE SECUENCIA - CU-A003: GESTIONAR PERMISOS
Operación: ACTUALIZAR PERMISOS DE ROL
Guía de uso
================================================================================

ARCHIVOS EN ESTA CARPETA:
- CU-A003_ActualizarPermisosRol_Secuencia.puml → Código PlantUML del diagrama

================================================================================
CÓMO VISUALIZAR EL DIAGRAMA
================================================================================

OPCIÓN 1: Visual Studio Code (Recomendado)
1. Instala extensión "PlantUML" de jebbs
2. Abre: CU-A003_ActualizarPermisosRol_Secuencia.puml
3. Presiona Alt+D
4. Exporta como PNG/SVG si necesitas

OPCIÓN 2: Online (Sin instalación)
1. Abre: http://www.plantuml.com/plantuml/uml/
2. Copia/pega el contenido del archivo .puml
3. Ver diagrama renderizado

OPCIÓN 3: Enterprise Architect
1. Crea nuevo Sequence Diagram
2. Usa el archivo .puml como referencia
3. Crea manualmente los participantes y mensajes

================================================================================
ELEMENTOS PRINCIPALES DEL DIAGRAMA
================================================================================

10 PARTICIPANTES (LIFELINES):
1. Administrador (actor)
2. gestionPermisos.cs (WinForm UI)
3. FamiliaBLL (Static)
4. FamiliaRepository (Singleton)
5. SecurityUnitOfWork (transacción)
6. SqlConnection + SqlTransaction (BD connection)
7. FamiliaPatenteRepository (Singleton)
8. PatenteRepository (Singleton)
9. BD SecurityVet (base de datos)
10. Bitacora (Singleton)
11. ExceptionManager (Singleton)

13 PASOS PRINCIPALES:
1. Selección de rol en ComboBox
2. Carga recursiva de permisos (PATRÓN COMPOSITE)
3. Mostrar permisos en interfaz (CheckedListBox)
4. Modificación de checks por administrador
5. Obtener patentes seleccionadas del UI
6. Crear Unit of Work con BEGIN TRANSACTION
7. Verificar que familia existe y es rol válido
8. Obtener patentes actuales del rol
9. Eliminar patentes desmarcadas (DELETE)
10. Agregar patentes nuevas (INSERT)
11. COMMIT de transacción
12. Registro en Bitácora
13. Confirmación al usuario

FRAGMENTOS COMBINADOS:
- 3 fragmentos "alt" (alternativas/condiciones)
- 2 fragmentos "loop" (iteraciones)
- 1 fragmento "try-catch" (manejo de excepciones con ROLLBACK)

PATRONES VISIBLES:
- Composite: Recursión para navegar jerarquía Familia-Patente
- Unit of Work: Transacción atómica (BEGIN TRAN → COMMIT/ROLLBACK)
- Singleton: BLL (estático), Repositories (.Current)
- Repository: Abstracción de acceso a datos

================================================================================
FLUJOS EN EL DIAGRAMA
================================================================================

FLUJO PRINCIPAL (ÉXITO):
→ Administrador selecciona rol
→ Sistema carga patentes recursivamente (Composite)
→ Administrador modifica checks
→ Sistema inicia Unit of Work (BEGIN TRAN)
→ Sistema elimina patentes desmarcadas
→ Sistema agrega patentes nuevas
→ Sistema hace COMMIT
→ Sistema registra en Bitácora
→ Mensaje de éxito

FLUJO ALTERNATIVO (ERROR):
→ Durante Unit of Work ocurre SQLException
→ Sistema ejecuta ROLLBACK automático
→ TODAS las operaciones se revierten
→ ExceptionManager registra error
→ Mensaje de error al usuario
→ BD queda en estado ORIGINAL (sin cambios)

================================================================================
RECURSIÓN COMPOSITE (IMPORTANTE)
================================================================================

El método ObtenerPatentesRecursivo() navega la jerarquía completa:

1. Obtiene patentes DIRECTAS de la familia actual
2. Obtiene familias HIJAS de la familia actual
3. Por cada familia hija: LLAMADA RECURSIVA (vuelve al paso 1)
4. Acumula TODAS las patentes en una lista
5. Elimina duplicados usando GroupBy(IdComponent)

Ejemplo de jerarquía:
ROL_Administrador
  ├─ Patente: "Gestión Usuarios" (directa)
  ├─ Patente: "Realizar Backup" (directa)
  └─ ROL_Supervisor (familia hija)
      ├─ Patente: "Consultar Bitácora" (heredada)
      └─ ROL_Recepcionista (familia hija de Supervisor)
          ├─ Patente: "Gestión Clientes" (heredada)
          └─ Patente: "Agendar Citas" (heredada)

Si seleccionas ROL_Administrador, el método recursivo obtiene:
→ 2 patentes directas de Administrador
→ 1 patente de Supervisor (recursión nivel 1)
→ 2 patentes de Recepcionista (recursión nivel 2)
TOTAL: 5 patentes mostradas en CheckedListBox

================================================================================
UNIT OF WORK - ATOMICIDAD GARANTIZADA
================================================================================

FLUJO DE TRANSACCIÓN:

1. CREATE: new SecurityUnitOfWork()
   → Abre SqlConnection
   → Crea SqlTransaction

2. BEGIN: unitOfWork.BeginTransaction()
   → connection.BeginTransaction()
   → Todas las operaciones siguientes usan esta transacción

3. OPERACIONES DENTRO DE TRANSACCIÓN:
   → DELETE FamiliaPatente (patentes removidas)
   → DELETE FamiliaPatente (patentes removidas)
   → DELETE FamiliaPatente (patentes removidas)
   → INSERT FamiliaPatente (patentes nuevas)
   → INSERT FamiliaPatente (patentes nuevas)

4a. ÉXITO: unitOfWork.Commit()
   → transaction.Commit()
   → Cambios permanentes en BD
   → connection.Close()

4b. ERROR: unitOfWork.Rollback()
   → transaction.Rollback()
   → TODOS los cambios revertidos
   → BD queda en estado ORIGINAL
   → connection.Close()

VENTAJAS:
- Atomicidad: Todo o nada
- Consistencia: No deja relaciones huérfanas
- Rollback automático en bloque catch
- Sin estados intermedios inconsistentes

================================================================================
STORED PROCEDURES UTILIZADOS
================================================================================

1. Familia_SelectOne
   - Parámetros: @IdFamilia
   - Retorna: DataRow con datos de la familia/rol

2. FamiliaPatente_GetChildrenRelations
   - Parámetros: @IdFamilia
   - Retorna: DataTable con patentes directas de la familia

3. FamiliaFamilia_GetChildrenRelations
   - Parámetros: @IdFamilia
   - Retorna: DataTable con familias hijas (para recursión)

4. Patente_SelectOne
   - Parámetros: @IdPatente
   - Retorna: DataRow con datos de la patente

5. FamiliaPatente_DeleteRelacion (DENTRO DE TRANSACCIÓN)
   - Parámetros: @IdFamilia, @IdPatente
   - Efecto: Elimina relación Familia-Patente
   - Usa: SqlConnection + SqlTransaction del Unit of Work

6. FamiliaPatente_Insert (DENTRO DE TRANSACCIÓN)
   - Parámetros: @IdFamilia, @IdPatente
   - Efecto: Crea relación Familia-Patente
   - Usa: SqlConnection + SqlTransaction del Unit of Work

7. Bitacora_Insert
   - Parámetros: @IdUsuario, @NombreUsuario, @Modulo, @Accion,
                 @Detalle, @Criticidad, @TipoEntidad, @IdEntidad, @Fecha
   - Efecto: Registra operación en auditoría

================================================================================
VALIDACIONES IMPLEMENTADAS
================================================================================

1. Familia existe: FamiliaRepository.SelectOne(idFamilia) != null
2. Es rol válido: familia.EsRol == true (tiene prefijo "ROL_")
3. Rol seleccionado antes de guardar: _familiaSeleccionada != null

Si alguna validación falla:
→ Lanza ValidacionException
→ NO inicia Unit of Work
→ Retorna al formulario sin cambios

================================================================================
AUDITORÍA EN BITÁCORA
================================================================================

Registro después de COMMIT exitoso:

Campos registrados:
- IdUsuario: ID del administrador que ejecuta la acción
- NombreUsuario: Nombre del administrador
- Modulo: "Permisos"
- Accion: "ActualizarPatentes"
- Detalle: "Patentes actualizadas para rol {nombre}: {cantidad} patentes
           asignadas ({primeras 5 patentes})"
- Criticidad: CriticidadBitacora.Advertencia
- TipoEntidad: "FamiliaPatente"
- IdEntidad: IdFamilia del rol modificado
- Fecha: GETDATE() automático

Ejemplo de detalle:
"Patentes actualizadas para rol ROL_Veterinario: 8 patentes asignadas
(gestionClientes, gestionMascotas, agendarCitas, consultaMedica,
gestionMedicamentos (+3 más))"

================================================================================
ARCHIVOS DEL CÓDIGO FUENTE
================================================================================

UI:
- UI/WinUi/Administración/gestionPermisos.cs (líneas 376-449)
- Métodos: CboRoles_SelectedIndexChanged(), BtnGuardarRol_Click(),
           CargarPatentesDelRol()

BLL:
- ServicesSeguridad/BLL/FamiliaBLL.cs (líneas 13-101)
- Método principal: ActualizarPatentesDeRol(idFamilia, patentes)
- Métodos auxiliares:
  * ObtenerTodasLasPatentesDeRol(idFamilia) - líneas 138-163
  * ObtenerPatentesRecursivo(familia, acumulador) - líneas 165-192
  * ObtenerPatentesDirectasDeFamilia(idFamilia) - líneas 103-136

DAL:
- ServicesSeguridad/DAL/Implementations/FamiliaRepository.cs
- Método: SelectOne(idFamilia)

- ServicesSeguridad/DAL/Implementations/FamiliaPatenteRepository.cs
- Métodos: GetChildrenRelations(familia), DeleteRelacion(fp, uow),
           Insert(fp, uow)

- ServicesSeguridad/DAL/Implementations/PatenteRepository.cs
- Método: SelectOne(idPatente)

- ServicesSeguridad/DAL/Implementations/FamiliaFamiliaRepository.cs
- Método: GetChildrenRelations(familia)

UNIT OF WORK:
- ServicesSeguridad/DAL/Implementations/SecurityUnitOfWork.cs
- Métodos: BeginTransaction(), Commit(), Rollback(), Dispose()

SERVICES:
- ServicesSeguridad/Services/Bitacora.cs
- Método: Registrar(idUsuario, nombreUsuario, modulo, accion, detalle,
          criticidad, tipoEntidad, idEntidad)

- ServicesSeguridad/Services/ExceptionManager.cs
- Método: Handle(Exception ex)

- ServicesSeguridad/Services/LoginService.cs
- Método estático: GetUsuarioLogueado()

================================================================================
DIFERENCIA ENTRE MÉTODOS DE CARGA
================================================================================

ObtenerPatentesDirectasDeFamilia(idFamilia):
→ NO recursivo
→ Obtiene SOLO patentes directas de la familia
→ No navega familias hijas
→ Usado para operaciones de actualización

ObtenerTodasLasPatentesDeRol(idFamilia):
→ RECURSIVO (llama a ObtenerPatentesRecursivo)
→ Obtiene patentes directas + heredadas de familias hijas
→ Navega jerarquía completa (Composite pattern)
→ Usado para visualización en interfaz

Ejemplo:
Familia: ROL_Supervisor
- Directas: 2 patentes
- Heredadas de hijas: 5 patentes

ObtenerPatentesDirectasDeFamilia() → 2 patentes
ObtenerTodasLasPatentesDeRol() → 7 patentes (2 + 5)

================================================================================
RELACIÓN CON OTROS DIAGRAMAS
================================================================================

ESPECIFICACIÓN COMPLETA:
- Diagramas/Documentacion/CU-A003_GESTIONAR_PERMISOS.txt

DIAGRAMA DE CASOS DE USO:
- Diagramas/Casos de Uso/CU-A003_GestionarPermisos.puml
- Muestra 7 operaciones principales divididas en 2 tabs

OTROS DIAGRAMAS RELACIONADOS:
- Diagramas/Diagrama de Secuencia/CU-A001_DiagramaSecuencia.puml
  (Login - muestra carga de permisos recursiva durante autenticación)

- Diagramas/Diagrama de Secuencia/CU-A002_CrearUsuario_Secuencia.puml
  (Crear Usuario - muestra asignación de rol)

================================================================================
ESCENARIOS DE PRUEBA
================================================================================

ESCENARIO 1: Actualizar permisos exitosamente
----------------------------------------------
1. Seleccionar "ROL_Veterinario"
2. Sistema carga 8 patentes actuales (marcadas)
3. Desmarcar 2 patentes, marcar 3 nuevas
4. Guardar cambios
5. Unit of Work: BEGIN TRAN → 2 DELETE → 3 INSERT → COMMIT
6. Bitácora registra: "9 patentes asignadas"
7. Mensaje: "Permisos actualizados exitosamente"

Resultado: ROL_Veterinario tiene 9 patentes (6 antiguas + 3 nuevas)

ESCENARIO 2: Error de BD con Rollback automático
-------------------------------------------------
1. Seleccionar "ROL_Recepcionista"
2. Sistema carga 5 patentes actuales
3. Marcar 10 nuevas patentes
4. Guardar cambios
5. Unit of Work: BEGIN TRAN → 10 INSERT
6. En el INSERT #7: SQLException (timeout, deadlock)
7. Unit of Work: ROLLBACK automático
8. Todos los 6 INSERT previos revertidos
9. Mensaje: "Error al actualizar patentes del rol"

Resultado: ROL_Recepcionista mantiene sus 5 patentes ORIGINALES (sin cambios)

================================================================================
