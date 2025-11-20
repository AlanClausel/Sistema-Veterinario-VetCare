# Caso de Uso: CU-A001 - Iniciar Sesión

## INFORMACIÓN GENERAL

**Código:** CU-A001
**Nombre:** Iniciar Sesión (Login)
**Módulo:** Autenticación
**Actor Principal:** Usuario del Sistema (Administrador, Veterinario, Recepcionista)
**Actores Secundarios:** Sistema de Seguridad, Servicio de Bitácora
**Nivel:** Usuario (nivel sistema)
**Complejidad:** Alta

---

## DESCRIPCIÓN
Permite a un usuario autenticarse en el sistema mediante nombre de usuario y contraseña. El sistema valida las credenciales, verifica la integridad de los datos del usuario, carga sus permisos de forma recursiva utilizando el patrón Composite, registra el evento en la bitácora y sincroniza el veterinario si corresponde.

---

## PRECONDICIONES
1. El usuario debe estar registrado en la base de datos SecurityVet (tabla Usuario)
2. El usuario debe tener estado activo (no bloqueado)
3. La aplicación debe tener conexión a las bases de datos SecurityVet y VetCareDB
4. El sistema debe tener acceso a los servicios: LoginService, CryptographyService, Bitacora

---

## POSTCONDICIONES

### Postcondiciones de Éxito:
1. Usuario autenticado con sesión activa
2. Usuario almacenado en LoginService._usuarioLogueado (contexto estático)
3. Permisos del usuario cargados en memoria (Familias y Patentes) mediante patrón Composite
4. Login exitoso registrado en tabla Bitacora con:
   - Acción: "Login"
   - Criticidad: "Info"
   - Descripción: "Usuario '[nombre]' inició sesión exitosamente"
5. Idioma del sistema configurado según preferencia del usuario (o idioma seleccionado en login)
6. Si usuario tiene rol "Veterinario": registro sincronizado en tabla Veterinario de VetCareDB
7. Formulario Login oculto
8. Formulario Menu principal mostrado con opciones según permisos

### Postcondiciones de Fallo:
1. Login fallido registrado en tabla Bitacora con:
   - Acción: "LoginFallido"
   - Criticidad: "Advertencia"
   - Descripción: Motivo del fallo (usuario no encontrado / contraseña incorrecta)
2. Usuario permanece en pantalla de login
3. Campos de contraseña limpiados
4. Mensaje de error mostrado al usuario

---

## FLUJO PRINCIPAL (FLUJO EXITOSO)

### En Login.cs (UI Layer):

**Paso 1:** El usuario ingresa su Nombre de Usuario en el campo txtUsuario
**Paso 2:** El usuario ingresa su Contraseña en el campo txtContraseña
**Paso 3:** El usuario hace clic en el botón "Ingresar" (btnIngresar_Click)

**Paso 4:** El sistema invoca ValidationBLL.ValidarCredencialesLogin(usuario, contraseña)
- Valida que ambos campos no estén vacíos

**Paso 5:** El sistema invoca LoginService.Login(usuario.Trim(), contraseña)

### En LoginService.Login() (Service Layer):

**Paso 6:** El sistema invoca UsuarioRepository.Current.SelectOneByName(nombreUsuario)
- Ejecuta stored procedure: Usuario_SelectByUsername
- Parámetro: @NombreUsuario

**Paso 7:** El sistema verifica que el usuario existe

**Paso 8:** El sistema invoca CryptographyService.HashPassword(contraseñaIngresada)
- Algoritmo: SHA256
- Encoding: Unicode (UTF-16) para compatibilidad con NVARCHAR de SQL Server
- Retorna: String hexadecimal en mayúsculas

**Paso 9:** El sistema compara el hash generado con usuarioDB.Clave

**Paso 10:** El sistema invoca LoginService.CargarPermisosUsuario(usuario)
- **Paso 10.1:** Limpia usuario.Permisos.Clear()
- **Paso 10.2:** Carga Familias del usuario desde UsuarioFamilia (tabla intermedia)
- **Paso 10.3:** Para cada Familia, invoca CargarHijosDeFamilia() recursivamente:
  - Ejecuta SP: Familia_Familia_SelectParticular con @IdFamiliaPadre
  - Obtiene IDs de familias hijas
  - Para cada familia hija: carga recursivamente sus hijos ANTES de agregarla al padre (post-order)
  - Ejecuta FamiliaPatenteRepository.GetChildren() para cargar patentes (hojas del árbol)
  - Agrega familia al usuario.Permisos
- **Paso 10.4:** Carga Patentes asignadas directamente al usuario desde UsuarioPatente
- **Implementa:** Patrón Composite (Familia = Composite, Patente = Leaf)

**Paso 11:** El sistema almacena el usuario en LoginService._usuarioLogueado (variable estática)

**Paso 12:** El sistema invoca Bitacora.Current.RegistrarLogin(usuario.IdUsuario, usuario.Nombre)
- Ejecuta BitacoraBLL.Registrar() que ejecuta SP: Bitacora_Insert
- Parámetros:
  - @IdUsuario: Guid del usuario
  - @NombreUsuario: string
  - @Modulo: "Sistema"
  - @Accion: "Login"
  - @Descripcion: "Usuario '[nombre]' inició sesión exitosamente"
  - @Criticidad: "Info"
  - @FechaHora: DateTime.Now
  - @Ip: null (opcional)

**Paso 13:** LoginService retorna el objeto Usuario al formulario Login.cs

### De vuelta en Login.cs:

**Paso 14:** El sistema invoca SincronizarVeterinarioSiCorresponde(usuario)
- **Paso 14.1:** Obtiene nombreRol con usuario.ObtenerNombreRol()
- **Paso 14.2:** Si nombreRol == "Veterinario":
  - Verifica si existe en VetCareDB con VeterinarioBLL.Current.EsVeterinario(IdUsuario)
  - Si no existe: VeterinarioBLL.Current.CrearDesdeUsuario(IdUsuario, Nombre)
    - Ejecuta SP: Veterinario_Insert en VetCareDB
    - Campos: IdUsuario (FK), Nombre, Apellido="", Matricula="", Especialidad="", Activo=1

**Paso 15:** El sistema invoca RedirigirPorRol(usuario)
- **Paso 15.1:** Determina idioma a usar: _idiomaSeleccionadoEnLogin ?? usuario.IdiomaPreferido ?? "es-AR"
- **Paso 15.2:** Invoca LanguageManager.CambiarIdioma(idiomaAUsar)
  - Carga archivo de recursos: idioma.es-AR o idioma.en-GB
  - Notifica a todos los observers (patrón Observer)
- **Paso 15.3:** Si el idioma seleccionado difiere del IdiomaPreferido almacenado:
  - Invoca UsuarioBLL.CambiarIdiomaPreferido(IdUsuario, idioma)
  - Ejecuta SP: Usuario_UpdateIdioma
  - Actualiza usuario.IdiomaPreferido en memoria
- **Paso 15.4:** Verifica que el usuario tenga rol asignado (nombreRol no vacío)
- **Paso 15.5:** Crea instancia del formulario menu: new menu(usuario)
- **Paso 15.6:** Muestra menuForm.Show()
- **Paso 15.7:** Oculta formulario Login: this.Hide()

**Paso 16:** El usuario está autenticado y en el menú principal

---

## FLUJOS ALTERNATIVOS

### FA-1: Campos vacíos (Validación)
**Punto de extensión:** Paso 4
**Condición:** Usuario o contraseña vacíos

**FA-1.1:** ValidationBLL.ValidarCredencialesLogin() lanza ValidacionException
**FA-1.2:** Login.cs captura ValidacionException en catch
**FA-1.3:** Sistema muestra MessageBox:
- Mensaje: vex.Message (mensaje de la excepción)
- Título: LanguageManager.Translate("error_validacion")
- Icono: Warning
**FA-1.4:** Usuario permanece en pantalla Login
**FA-1.5:** Fin del caso de uso

---

### FA-2: Usuario no encontrado
**Punto de extensión:** Paso 7
**Condición:** SelectOneByName retorna null

**FA-2.1:** Sistema invoca Bitacora.Current.RegistrarLoginFallido(nombreUsuario, "Usuario no encontrado")
- Ejecuta SP: Bitacora_Insert con:
  - @IdUsuario: NULL
  - @NombreUsuario: nombreUsuario ingresado
  - @Modulo: "Sistema"
  - @Accion: "LoginFallido"
  - @Descripcion: "Intento de login fallido para usuario '[nombre]': Usuario no encontrado"
  - @Criticidad: "Advertencia"

**FA-2.2:** LoginService lanza UsuarioNoEncontradoException(nombreUsuario)

**FA-2.3:** Login.cs captura UsuarioNoEncontradoException

**FA-2.4:** Sistema muestra MessageBox:
- Mensaje: uex.Message
- Título: LanguageManager.Translate("error_autenticacion")
- Icono: Error

**FA-2.5:** Sistema limpia txtContraseña.Clear()

**FA-2.6:** Sistema mueve foco a txtUsuario.Focus()

**FA-2.7:** Usuario permanece en pantalla Login

**FA-2.8:** Fin del caso de uso

**NOTA DE SEGURIDAD:** El mensaje al usuario debe ser genérico ("Credenciales inválidas") para no revelar si el usuario existe o no, pero en el código actual muestra mensaje específico.

---

### FA-3: Contraseña incorrecta
**Punto de extensión:** Paso 9
**Condición:** hashGenerado != usuarioDB.Clave

**FA-3.1:** Sistema invoca Bitacora.Current.RegistrarLoginFallido(nombreUsuario, "Contraseña incorrecta")
- Ejecuta SP: Bitacora_Insert (igual estructura que FA-2.1)

**FA-3.2:** LoginService lanza ContraseñaInvalidaException()

**FA-3.3:** Login.cs captura ContraseñaInvalidaException

**FA-3.4:** Sistema muestra MessageBox:
- Mensaje: cex.Message
- Título: LanguageManager.Translate("error_autenticacion")
- Icono: Error

**FA-3.5:** Sistema limpia txtContraseña.Clear()

**FA-3.6:** Sistema mueve foco a txtContraseña.Focus()

**FA-3.7:** Usuario permanece en pantalla Login

**FA-3.8:** Fin del caso de uso

---

### FA-4: Violación de integridad (DVH inválido)
**Punto de extensión:** Durante SelectOneByName (Paso 6) o cualquier operación de BD
**Condición:** DVH del usuario no coincide (datos comprometidos)

**FA-4.1:** UsuarioRepository detecta IntegridadException al verificar DVH

**FA-4.2:** LoginService captura IntegridadException

**FA-4.3:** Sistema invoca Bitacora.Current.LogCritical() para archivo de log

**FA-4.4:** Sistema invoca Bitacora.Current.RegistrarViolacionDVH():
- Ejecuta SP: Bitacora_Insert con:
  - @Modulo: "Seguridad"
  - @Accion: "ViolacionDVH"
  - @Descripcion: "Intento de login con usuario comprometido: [nombre] - Detalle: [mensaje]"
  - @Criticidad: "Critico"

**FA-4.5:** LoginService re-lanza IntegridadException

**FA-4.6:** Login.cs captura Exception genérica (IntegridadException hereda de Exception)

**FA-4.7:** Sistema muestra MessageBox:
- Mensaje: LanguageManager.Translate("error_inesperado") + ": " + ex.Message
- Título: LanguageManager.Translate("error")
- Icono: Error

**FA-4.8:** Usuario permanece en pantalla Login

**FA-4.9:** Fin del caso de uso

**ACCIÓN DEL ADMINISTRADOR:** Debe ejecutar script 04_RecalcularDVH.sql para corregir integridad.

---

### FA-5: Error general inesperado
**Punto de extensión:** Cualquier paso
**Condición:** Excepción no controlada (ej: error de conexión BD, timeout, etc.)

**FA-5.1:** LoginService captura Exception genérica

**FA-5.2:** Sistema invoca Bitacora.Current.LogError() para registro en archivos

**FA-5.3:** Sistema invoca ExceptionManager.Current.Handle(ex)

**FA-5.4:** LoginService lanza AutenticacionException("Error al procesar la autenticación", ex)

**FA-5.5:** Login.cs captura Exception genérica

**FA-5.6:** Sistema muestra MessageBox:
- Mensaje: LanguageManager.Translate("error_inesperado") + ": " + ex.Message
- Título: LanguageManager.Translate("error")
- Icono: Error

**FA-5.7:** Usuario permanece en pantalla Login

**FA-5.8:** Fin del caso de uso

---

### FA-6: Usuario sin rol asignado
**Punto de extensión:** Paso 15.4
**Condición:** usuario.ObtenerNombreRol() retorna string vacío o null

**FA-6.1:** Sistema muestra MessageBox:
- Mensaje: LanguageManager.Translate("usuario_sin_rol")
- Título: LanguageManager.Translate("error_autorizacion")
- Icono: Error

**FA-6.2:** Sistema ejecuta return (no abre menú)

**FA-6.3:** Usuario permanece en pantalla Login

**FA-6.4:** Fin del caso de uso

**NOTA:** El login técnicamente fue exitoso (se registró en bitácora), pero no se permite acceso al sistema.

---

### FA-7: Error al sincronizar veterinario
**Punto de extensión:** Paso 14
**Condición:** Falla VeterinarioBLL.CrearDesdeUsuario()

**FA-7.1:** Sistema captura Exception en SincronizarVeterinarioSiCorresponde()

**FA-7.2:** Sistema invoca LoggerService.WriteLog() con EventLevel.Warning

**FA-7.3:** Sistema NO interrumpe el flujo de login (error silencioso)

**FA-7.4:** Continúa con Paso 15 normalmente

**NOTA:** Este error no impide el login, solo registra advertencia en archivos de log.

---

## REGLAS DE NEGOCIO

**RN-A001:** Las contraseñas se almacenan hasheadas con algoritmo SHA256
**RN-A002:** El encoding del hash debe ser Unicode (UTF-16) para compatibilidad con NVARCHAR de SQL Server
**RN-A003:** Todos los intentos de login (exitosos y fallidos) se registran en tabla Bitacora
**RN-A004:** Los usuarios bloqueados o inactivos no pueden iniciar sesión (validado en BD)
**RN-A005:** Los permisos se cargan recursivamente mediante patrón Composite al momento del login
**RN-A006:** El contexto de sesión se almacena en LoginService._usuarioLogueado (variable estática)
**RN-A007:** Cada usuario debe tener al menos una Familia (rol) asignada para acceder al sistema
**RN-A008:** El idioma seleccionado en la pantalla de login tiene prioridad sobre el IdiomaPreferido almacenado
**RN-A009:** Si el usuario tiene rol "Veterinario", debe existir registro en tabla Veterinario de VetCareDB
**RN-A010:** El DVH (Dígito Verificador Horizontal) se valida en cada consulta de usuario desde BD
**RN-A011:** Una violación de DVH se registra como evento crítico y debe ser investigada por el administrador
**RN-A012:** Los mensajes de error deben usar LanguageManager para internacionalización

---

## ATRIBUTOS DE CALIDAD

### Seguridad:
- **Alta:** Contraseñas hasheadas, validación de integridad (DVH), registro de todos los intentos

### Auditabilidad:
- **Alta:** Todos los eventos registrados en Bitacora con timestamp, usuario, acción y criticidad

### Usabilidad:
- **Media:** Mensajes en idioma del usuario, foco automático en campos, tecla Enter para ingresar

### Performance:
- **Media:** Carga recursiva de permisos puede ser lenta con jerarquías profundas

### Mantenibilidad:
- **Alta:** Separación clara de capas (UI → Service → BLL → DAL → BD)

### Disponibilidad:
- **Media:** Requiere conexión a dos bases de datos (SecurityVet y VetCareDB)

---

## COMPONENTES Y CLASES INVOLUCRADAS

### Capa UI:
- **Login.cs:** Formulario principal de autenticación
  - Métodos: BtnIngresar_Click(), RedirigirPorRol(), SincronizarVeterinarioSiCorresponde()

### Capa Services:
- **LoginService.cs:** Servicio estático de autenticación
  - Métodos: Login(), CargarPermisosUsuario(), CargarHijosDeFamilia()
  - Propiedades: _usuarioLogueado (static), GetUsuarioLogueado()

- **CryptographyService.cs:** Servicio estático de criptografía
  - Métodos: HashPassword() → SHA256 con Encoding.Unicode

- **Bitacora.cs:** Servicio singleton de auditoría
  - Métodos: RegistrarLogin(), RegistrarLoginFallido(), RegistrarViolacionDVH()

- **LanguageManager.cs:** Servicio de internacionalización (patrón Observer)
  - Métodos: CambiarIdioma(), Translate()

### Capa BLL:
- **ValidationBLL.cs:** Validaciones de negocio
  - Métodos: ValidarCredencialesLogin()

- **UsuarioBLL.cs:** Lógica de negocio de usuarios
  - Métodos: CambiarIdiomaPreferido()

- **VeterinarioBLL.cs:** Lógica de negocio de veterinarios
  - Métodos: EsVeterinario(), CrearDesdeUsuario()

- **BitacoraBLL.cs:** Lógica de negocio de auditoría
  - Métodos: Registrar()

### Capa DAL:
- **UsuarioRepository.cs:** Acceso a datos de usuarios
  - Métodos: SelectOneByName() → SP: Usuario_SelectByUsername

- **UsuarioFamiliaRepository.cs:** Relación Usuario-Familia
  - Métodos: SelectAll()

- **FamiliaRepository.cs:** Acceso a datos de familias (roles)
  - Métodos: SelectOne()

- **FamiliaPatenteRepository.cs:** Relación Familia-Patente
  - Métodos: GetChildren()

- **PatenteRepository.cs:** Acceso a datos de patentes (permisos)
  - Métodos: SelectOne()

- **UsuarioPatenteRepository.cs:** Relación Usuario-Patente directa
  - Métodos: SelectAll()

### Capa Database:
- **SecurityVet:** Base de datos de seguridad
  - Tablas: Usuario, Familia, Patente, UsuarioFamilia, UsuarioPatente, FamiliaPatente, FamiliaFamilia, Bitacora

- **VetCareDB:** Base de datos de negocio
  - Tablas: Veterinario

---

## STORED PROCEDURES UTILIZADOS

| Stored Procedure | Parámetros | Base de Datos | Descripción |
|-----------------|------------|---------------|-------------|
| Usuario_SelectByUsername | @NombreUsuario (NVARCHAR) | SecurityVet | Busca usuario por nombre, valida DVH |
| Familia_Familia_SelectParticular | @IdFamiliaPadre (UNIQUEIDENTIFIER) | SecurityVet | Obtiene familias hijas de una familia |
| Bitacora_Insert | @IdUsuario, @NombreUsuario, @Modulo, @Accion, @Descripcion, @Criticidad, @Tabla, @IdRegistro, @Ip, @FechaHora | SecurityVet | Registra evento en bitácora |
| Usuario_UpdateIdioma | @IdUsuario, @IdiomaPreferido | SecurityVet | Actualiza idioma preferido del usuario |
| Veterinario_Insert | @IdUsuario, @Nombre, @Apellido, @Matricula, @Especialidad, @Telefono, @Email | VetCareDB | Crea registro de veterinario |

---

## EXCEPCIONES PERSONALIZADAS

| Excepción | Namespace | Descripción | Cuándo se lanza |
|-----------|-----------|-------------|-----------------|
| ValidacionException | ServicesSecurity.DomainModel.Exceptions | Errores de validación de entrada | Campos vacíos o inválidos |
| UsuarioNoEncontradoException | ServicesSecurity.DomainModel.Exceptions | Usuario no existe en BD | SelectOneByName retorna null |
| ContraseñaInvalidaException | ServicesSecurity.DomainModel.Exceptions | Contraseña incorrecta | Hash no coincide |
| AutenticacionException | ServicesSecurity.DomainModel.Exceptions | Error genérico de autenticación | Otros errores de login |
| IntegridadException | ServicesSecurity.DomainModel.Exceptions | Violación de DVH | DVH no coincide |

---

## PATRONES DE DISEÑO APLICADOS

### 1. Patrón Composite (Permisos)
**Estructura:**
- **Component:** Clase abstracta Component (interfaz común)
- **Composite:** Familia (puede contener otras Familias y Patentes)
- **Leaf:** Patente (permiso atómico, sin hijos)

**Implementación:**
- CargarHijosDeFamilia() es recursivo (post-order traversal)
- Primero carga familias hijas recursivamente, luego patentes de la familia actual
- Permite jerarquías ilimitadas: ROL_Admin → ROL_Supervisor → ROL_Usuario

**Beneficio:** Permisos heredados automáticamente por jerarquía de roles

---

### 2. Patrón Singleton (Servicios)
**Clases:**
- Bitacora.Current (sealed class con instancia privada)

**Implementación:**
```csharp
private static readonly Bitacora _instance = new Bitacora();
public static Bitacora Current { get { return _instance; } }
private Bitacora() { }
```

**Beneficio:** Una sola instancia de servicios compartida en toda la aplicación

---

### 3. Patrón Static Service (LoginService)
**Implementación:**
- LoginService es static class (no instancias)
- _usuarioLogueado es static variable (contexto global de sesión)

**Beneficio:** Acceso global al usuario logueado desde cualquier parte: LoginService.GetUsuarioLogueado()

---

### 4. Patrón Observer (LanguageManager)
**Estructura:**
- **Subject:** LanguageManager
- **Observer:** ILanguageObserver (formularios)
- **Notify:** CambiarIdioma() notifica a todos los observers

**Beneficio:** Todos los formularios abiertos se actualizan automáticamente al cambiar idioma

---

### 5. Patrón Repository
**Implementación:**
- Cada entidad tiene su repository: UsuarioRepository, FamiliaRepository, PatenteRepository
- Todos implementan Singleton: Repository.Current

**Beneficio:** Abstracción del acceso a datos, facilita testing y cambios de BD

---

## DIAGRAMA DE SECUENCIA (TEXTUAL)

```
Usuario -> Login.cs: Ingresa credenciales y hace clic en "Ingresar"
Login.cs -> ValidationBLL: ValidarCredencialesLogin(user, pass)
ValidationBLL --> Login.cs: OK (o ValidacionException)

Login.cs -> LoginService: Login(user, pass)
LoginService -> UsuarioRepository: SelectOneByName(user)
UsuarioRepository -> BD_SecurityVet: SP: Usuario_SelectByUsername
BD_SecurityVet --> UsuarioRepository: DataRow con usuario y DVH
UsuarioRepository --> LoginService: Usuario objeto (o null)

alt Usuario no encontrado
    LoginService -> Bitacora: RegistrarLoginFallido(user, "Usuario no encontrado")
    Bitacora -> BD_SecurityVet: SP: Bitacora_Insert (acción LoginFallido)
    LoginService --> Login.cs: throw UsuarioNoEncontradoException
    Login.cs --> Usuario: MessageBox error
end

LoginService -> CryptographyService: HashPassword(pass)
CryptographyService --> LoginService: hashGenerado (SHA256)

alt Contraseña incorrecta
    LoginService -> Bitacora: RegistrarLoginFallido(user, "Contraseña incorrecta")
    LoginService --> Login.cs: throw ContraseñaInvalidaException
    Login.cs --> Usuario: MessageBox error
end

LoginService -> LoginService: CargarPermisosUsuario(usuario)
LoginService -> UsuarioFamiliaRepository: SelectAll() filtrado por IdUsuario
loop Para cada familia del usuario
    LoginService -> FamiliaRepository: SelectOne(idFamilia)
    LoginService -> LoginService: CargarHijosDeFamilia(familia) [RECURSIVO]
    LoginService -> BD_SecurityVet: SP: Familia_Familia_SelectParticular
    loop Para cada familia hija
        LoginService -> LoginService: CargarHijosDeFamilia(familiaHija) [RECURSIÓN]
    end
    LoginService -> FamiliaPatenteRepository: GetChildren(familia)
    LoginService -> usuario.Permisos: Add(familia)
end

LoginService -> LoginService: _usuarioLogueado = usuario (static)

LoginService -> Bitacora: RegistrarLogin(IdUsuario, NombreUsuario)
Bitacora -> BD_SecurityVet: SP: Bitacora_Insert (acción Login, criticidad Info)

LoginService --> Login.cs: return usuario

Login.cs -> Login.cs: SincronizarVeterinarioSiCorresponde(usuario)
alt Usuario tiene rol Veterinario
    Login.cs -> VeterinarioBLL: EsVeterinario(IdUsuario)
    alt No existe en VetCareDB
        Login.cs -> VeterinarioBLL: CrearDesdeUsuario(IdUsuario, Nombre)
        VeterinarioBLL -> BD_VetCareDB: SP: Veterinario_Insert
    end
end

Login.cs -> Login.cs: RedirigirPorRol(usuario)
Login.cs -> LanguageManager: CambiarIdioma(idioma)
LanguageManager -> Observers: NotificarCambioIdioma() [OBSERVER PATTERN]

alt Idioma diferente al preferido
    Login.cs -> UsuarioBLL: CambiarIdiomaPreferido(IdUsuario, idioma)
    UsuarioBLL -> BD_SecurityVet: SP: Usuario_UpdateIdioma
end

Login.cs -> menu: new menu(usuario)
Login.cs -> menu: Show()
Login.cs -> Login: Hide()

Usuario -> menu: Ve menú principal con opciones según permisos
```

---

## DATOS DE PRUEBA

### Usuario Válido:
- **Nombre de Usuario:** admin
- **Contraseña:** admin123
- **Hash (SHA256-Unicode):** [calculado por el sistema]
- **Rol:** ROL_Administrador
- **Estado:** Activo
- **Idioma Preferido:** es-AR

### Casos de Prueba:

| Caso | Usuario | Contraseña | Resultado Esperado |
|------|---------|------------|-------------------|
| CP-001 | admin | admin123 | Login exitoso, redirige a menu |
| CP-002 | admin | incorrecta | ContraseñaInvalidaException, foco en txtContraseña |
| CP-003 | noexiste | cualquiera | UsuarioNoEncontradoException, foco en txtUsuario |
| CP-004 | (vacío) | admin123 | ValidacionException, mensaje "Campo requerido" |
| CP-005 | admin | (vacío) | ValidacionException, mensaje "Campo requerido" |
| CP-006 | usuarioConDVHInvalido | correcta | IntegridadException, evento crítico en bitácora |
| CP-007 | usuarioSinRol | correcta | Login exitoso pero sin acceso, mensaje "usuario_sin_rol" |

---

## NOTAS TÉCNICAS

1. **Seguridad del hash:**
   - El sistema usa SHA256 (256 bits)
   - Encoding.Unicode (UTF-16) es crítico para coincidir con NVARCHAR de SQL
   - No se usa salt (mejora futura recomendada)

2. **Performance de carga de permisos:**
   - La recursión puede ser profunda si hay muchas jerarquías
   - Cada familia ejecuta múltiples SPs
   - Optimización futura: cargar todo en una consulta con CTE recursivo

3. **Sincronización de Veterinario:**
   - Es un hack temporal para mantener consistencia entre SecurityVet y VetCareDB
   - Idealmente, debería usarse una vista o FK directa

4. **Contexto estático:**
   - _usuarioLogueado es static (singleton de sesión)
   - Problema: no soporta múltiples sesiones en misma instancia de app
   - OK para aplicación WinForms de escritorio (una sesión por proceso)

5. **Manejo de errores:**
   - Bitacora silencia sus propias excepciones para evitar loops infinitos
   - Errores de sincronización de veterinario no interrumpen login

---

## TRAZABILIDAD

### Requisitos relacionados:
- REQ-SEC-001: Autenticación de usuarios
- REQ-SEC-002: Gestión de permisos por roles
- REQ-SEC-003: Auditoría de accesos
- REQ-SEC-004: Integridad de datos (DVH)
- REQ-FUN-001: Soporte multi-idioma

### Casos de uso relacionados:
- **<<include>>** CU-A016: Verificar DVH
- **<<include>>** CU-A015: Registrar Evento en Bitácora
- **<<extend>>** CU-A002: Cerrar Sesión
- **<<extend>>** CU-A003: Cambiar Contraseña

---

## HISTORIAL DE CAMBIOS

| Fecha | Versión | Autor | Cambio |
|-------|---------|-------|--------|
| 2025-01-15 | 1.0 | Sistema | Documentación inicial basada en análisis de código real |

---

**FIN DEL DOCUMENTO**

---

## PARA ENTERPRISE ARCHITECT

### Elementos a modelar:

1. **Actores:**
   - Usuario del Sistema (generalización de Administrador, Veterinario, Recepcionista)
   - Sistema de Seguridad (actor secundario)

2. **Caso de Uso Principal:**
   - CU-A001: Iniciar Sesión

3. **Casos de Uso Incluidos (<<include>>):**
   - Validar Credenciales
   - Hashear Contraseña
   - Cargar Permisos (Composite)
   - Registrar Login en Bitácora
   - Sincronizar Veterinario
   - Configurar Idioma

4. **Casos de Uso Extendidos (<<extend>>):**
   - Registrar Login Fallido
   - Registrar Violación DVH

5. **Relaciones:**
   - Usuario del Sistema -----> CU-A001: Iniciar Sesión
   - CU-A001 <<include>> Validar Credenciales
   - CU-A001 <<include>> Hashear Contraseña
   - CU-A001 <<include>> Cargar Permisos
   - CU-A001 <<include>> Registrar Login en Bitácora
   - CU-A001 <<include>> Sincronizar Veterinario
   - CU-A001 <<include>> Configurar Idioma
   - Registrar Login Fallido <<extend>> CU-A001 (punto: usuario no encontrado / contraseña incorrecta)
   - Registrar Violación DVH <<extend>> CU-A001 (punto: DVH inválido)

6. **Puntos de Extensión:**
   - Extension Point "validación fallida" en CU-A001
   - Extension Point "autenticación fallida" en CU-A001
   - Extension Point "integridad comprometida" en CU-A001
