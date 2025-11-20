================================================================================
DIAGRAMA DE SECUENCIA - CU-A002: GESTIONAR USUARIOS
Operación: CREAR USUARIO
Guía de uso
================================================================================

ARCHIVOS EN ESTA CARPETA:
- CU-A002_CrearUsuario_Secuencia.puml → Código PlantUML del diagrama

================================================================================
CÓMO VISUALIZAR EL DIAGRAMA
================================================================================

OPCIÓN 1: Visual Studio Code (Recomendado)
1. Instala extensión "PlantUML" de jebbs
2. Abre: CU-A002_CrearUsuario_Secuencia.puml
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
2. gestionUsuarios.cs (WinForm UI)
3. ValidationBLL (validaciones)
4. UsuarioBLL (lógica de negocio)
5. CryptographyService (hasheo SHA256)
6. UsuarioRepository (acceso a datos)
7. BD SecurityVet (base de datos)
8. ExceptionManager (manejo de excepciones)
9. Bitacora (auditoría)
10. FamiliaRepository (asignación de roles)

13 PASOS PRINCIPALES:
1. Ingreso de datos en formulario
2. Validación de campos obligatorios
3. Validación de formato de email (regex)
4. Validación de longitud de contraseña (mínimo 6 chars)
5. Verificación de unicidad de nombre de usuario
6. Hasheo de contraseña (SHA256 + Unicode)
7. Creación de objeto Usuario
8. Cálculo de DVH (Dígito Verificador Horizontal)
9. Inserción en base de datos (Usuario_Insert)
10. Asignación de rol (UsuarioFamilia_Insert)
11. Registro en Bitácora (auditoría)
12. Retorno al UI con confirmación
13. Actualización de grid con lista de usuarios

FRAGMENTOS COMBINADOS:
- 6 fragmentos "alt" (alternativas/condiciones)
- Validaciones con flujos de error y éxito

PATRONES VISIBLES:
- Singleton: UsuarioBLL.Current, UsuarioRepository.Current
- Adapter: UsuarioAdapter (DataRow → List<Usuario>)
- Repository: Abstracción de acceso a datos
- Exception Manager: Manejo centralizado de errores

================================================================================
FLUJOS ALTERNATIVOS EN EL DIAGRAMA
================================================================================

ALT 1: Campos vacíos o nulos
→ Lanza CampoRequeridoException
→ MessageBox: "Complete todos los campos"
→ Retorna al formulario
→ FIN

ALT 2: Email inválido (regex)
→ Lanza EmailInvalidoException
→ MessageBox: "Formato de email inválido"
→ Retorna al formulario
→ FIN

ALT 3: Contraseña < 6 caracteres
→ Lanza PasswordDemasiadoCortoException
→ MessageBox: "Contraseña mínimo 6 caracteres"
→ Retorna al formulario
→ FIN

ALT 4: Usuario ya existe (nombre duplicado)
→ Lanza UsuarioDuplicadoException
→ Log en Bitacora: "Intento crear usuario duplicado"
→ MessageBox: "Nombre de usuario ya existe"
→ Retorna al formulario
→ FIN

ALT 5: Error de base de datos
→ SQLException
→ Log en Bitacora: "Error al crear usuario"
→ MessageBox: "Error al crear usuario"
→ Retorna al formulario
→ FIN

ALT 6: Si idFamiliaRol != null
→ Ejecuta UsuarioFamilia_Insert
→ Asigna usuario al rol seleccionado

================================================================================
VALIDACIONES IMPLEMENTADAS
================================================================================

1. CAMPOS OBLIGATORIOS:
   - Nombre de usuario
   - Email
   - Contraseña

2. EMAIL:
   - Formato válido usando expresión regular
   - Patrón: nombre@dominio.extension

3. CONTRASEÑA:
   - Mínimo 6 caracteres
   - No hay restricción de complejidad en esta versión

4. UNICIDAD:
   - Nombre de usuario debe ser único en el sistema
   - Verificación vía SP: Usuario_VerificarExistenciaNombre

================================================================================
SEGURIDAD Y AUDITORÍA
================================================================================

HASHEO DE CONTRASEÑA:
- Algoritmo: SHA256 (256 bits)
- Encoding: Unicode (UTF-16) para compatibilidad SQL Server NVARCHAR
- Servicio: CryptographyService.HashPassword()
- Resultado: String hexadecimal de 64 caracteres
- NUNCA se almacena contraseña en texto plano

DVH (DÍGITO VERIFICADOR HORIZONTAL):
- Garantiza integridad de datos del usuario
- Hash SHA256 de todos los campos concatenados
- Auto-cálculo en INSERT/UPDATE
- Validación en SELECT detecta modificaciones no autorizadas

AUDITORÍA (BITÁCORA):
- Se registra TODA operación de creación de usuario
- Campos registrados:
  * IdUsuario (quien ejecuta la acción)
  * NombreUsuario (quien ejecuta)
  * Módulo: "Usuarios"
  * TipoOperacion: "Alta"
  * TipoEntidad: "Usuario"
  * IdEntidad: IdUsuario del nuevo usuario
  * Detalle: "Usuario creado: {nombre}"
  * Fecha: GETDATE() automático

SOFT DELETE:
- Usuarios no se eliminan físicamente
- Campo Activo = 1 (activo) / 0 (inactivo)
- Preserva integridad referencial
- Permite auditoría histórica

================================================================================
STORED PROCEDURES UTILIZADOS
================================================================================

1. Usuario_VerificarExistenciaNombre
   - Parámetros: @NombreUsuario NVARCHAR(100)
   - Retorna: COUNT(*) > 0 si existe

2. Usuario_Insert
   - Parámetros: @IdUsuario, @Nombre, @Email, @Clave,
                 @IdiomaPreferido, @DVH, @Activo
   - Crea registro en tabla Usuario

3. UsuarioFamilia_Insert
   - Parámetros: @IdUsuario, @IdFamilia
   - Asigna usuario a rol (Familia)

4. Bitacora_Insert
   - Parámetros: @IdUsuario, @NombreUsuario, @Modulo,
                 @TipoOperacion, @TipoEntidad, @IdEntidad,
                 @Detalle, @Fecha
   - Registra operación en auditoría

5. Usuario_SelectAll
   - Sin parámetros
   - Retorna: Todos los usuarios con Activo = 1

================================================================================
EXCEPCIONES MANEJADAS
================================================================================

EXCEPCIONES PERSONALIZADAS:
- CampoRequeridoException
- EmailInvalidoException
- PasswordDemasiadoCortoException
- UsuarioDuplicadoException
- RepositoryException

EXCEPCIONES DEL SISTEMA:
- SQLException (errores de base de datos)

MANEJO:
- ExceptionManager.HandleException(ex)
- Log automático en Bitacora
- Mensajes user-friendly al usuario
- No expone detalles técnicos sensibles

================================================================================
ARCHIVOS DEL CÓDIGO FUENTE
================================================================================

UI:
- UI/WinUi/Administración/gestionUsuarios.cs (líneas 150-250 aprox.)
- Método: btnAgregar_Click(), GuardarNuevoUsuario()

BLL:
- ServicesSeguridad/BLL/UsuarioBLL.cs (líneas 50-120 aprox.)
- Método: CrearUsuario(nombre, email, password, idFamiliaRol, idioma)

DAL:
- ServicesSeguridad/DAL/Implementations/UsuarioRepository.cs
- Métodos: VerificarExistenciaNombre(), Insert()
- ServicesSeguridad/DAL/Implementations/Adapter/UsuarioAdapter.cs

SERVICES:
- ServicesSeguridad/Services/CryptographyService.cs
- Método: HashPassword(textPlainPass)
- ServicesSeguridad/Services/Bitacora.cs
- Método: RegistrarAlta()
- ServicesSeguridad/Services/ExceptionManager.cs
- Método: HandleException(Exception ex)

VALIDATION:
- ServicesSeguridad/BLL/ValidationBLL.cs
- Métodos: ValidarCamposObligatorios(), ValidarFormatoEmail(),
           ValidarLongitudPassword()

================================================================================
NOTAS ADICIONALES
================================================================================

PATRÓN SINGLETON:
- UsuarioBLL.Current (instancia única)
- UsuarioRepository.Current (instancia única)
- Garantiza consistencia de estado
- Thread-safe mediante static readonly

PATRÓN ADAPTER:
- UsuarioAdapter.AdaptarLista(DataTable dt)
- Convierte DataTable → List<Usuario>
- Mapeo manual de columnas a propiedades
- Encapsula lógica de conversión

TRANSACCIONES:
- Esta operación NO usa Unit of Work
- El SP Usuario_Insert es atómico por sí mismo
- La asignación de rol es independiente (puede fallar sin afectar usuario)

IDIOMA:
- Campo IdiomaPreferido: "es-AR" o "en-GB"
- Define idioma de la interfaz para ese usuario
- Patrón Observer: LanguageManager notifica cambios

================================================================================
RELACIÓN CON OTROS DIAGRAMAS
================================================================================

ESPECIFICACIÓN COMPLETA:
- Diagramas/Documentacion/CU-A002_GESTIONAR_USUARIOS.txt

DIAGRAMA DE CASOS DE USO:
- Diagramas/Casos de Uso/CU-A002_GestionarUsuarios.puml
- Muestra las 5 operaciones CRUD: Crear, Modificar, Eliminar, Buscar, Listar

OTROS DIAGRAMAS DE SECUENCIA:
- Diagramas/Diagrama de Secuencia/CU-A001_DiagramaSecuencia.puml
  (Login - muestra carga de permisos jerárquica con Composite pattern)

================================================================================

