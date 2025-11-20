================================================================================
DIAGRAMA DE SECUENCIA - CU-A001: INICIAR SESIÓN
Guía de uso
================================================================================

ARCHIVOS EN ESTA CARPETA:
- CU-A001_DiagramaSecuencia.puml → Código PlantUML del diagrama

================================================================================
CÓMO VISUALIZAR EL DIAGRAMA
================================================================================

OPCIÓN 1: Visual Studio Code (Recomendado)
1. Instala extensión "PlantUML" de jebbs
2. Abre: CU-A001_DiagramaSecuencia.puml
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

14 PARTICIPANTES:
1. Usuario (actor)
2. Login.cs (UI)
3. ValidationBLL
4. LoginService
5. UsuarioRepository
6. BD SecurityVet
7. CryptographyService
8. Bitacora
9. FamiliaRepository
10. PatenteRepository
11. LanguageManager
12. VeterinarioBLL
13. BD VetCareDB
14. Menu.cs (UI)

20 PASOS PRINCIPALES:
1. Ingreso de credenciales
2. Validación de campos
3. Inicio de autenticación
4-6. Búsqueda y validación de usuario
7-8. Validación de contraseña
9-11. Carga de permisos (Patrón Composite - RECURSIVO)
12. Almacenamiento de sesión
13. Registro en bitácora
14. Retorno al UI
15. Sincronización de veterinario
16. Configuración de idioma
17. Verificación de rol
18-20. Mostrar menú

FRAGMENTOS COMBINADOS:
- 5 fragmentos "alt" (alternativas/condiciones)
- 2 fragmentos "loop" (iteraciones)

PATRONES VISIBLES:
- Composite: Recursión en CargarHijosDeFamilia()
- Singleton: Servicios (LoginService, Bitacora)
- Observer: LanguageManager notifica cambios
- Repository: Acceso a datos abstracto

================================================================================
FLUJOS ALTERNATIVOS EN EL DIAGRAMA
================================================================================

ALT 1: Usuario NO encontrado
→ Bitacora registra login fallido
→ Lanza UsuarioNoEncontradoException
→ Muestra MessageBox
→ FIN

ALT 2: Contraseña INCORRECTA
→ Bitacora registra login fallido
→ Lanza ContraseñaInvalidaException
→ Muestra MessageBox
→ FIN

ALT 3: Si rol == "Veterinario"
→ Verifica existencia en VetCareDB
→ Crea registro si no existe

ALT 4: Idioma diferente
→ Actualiza idioma en BD

ALT 5: Usuario SIN rol
→ Muestra error
→ NO abre menú

================================================================================
RECURSIÓN IMPORTANTE
================================================================================

El método CargarHijosDeFamilia() se llama a sí mismo recursivamente:
- Permite jerarquías de roles ilimitadas
- Familias pueden contener otras Familias (composites)
- Patentes son las hojas del árbol

Ejemplo de jerarquía:
ROL_Administrador
  ├─ ROL_Supervisor
  │   ├─ ROL_Usuario
  │   │   └─ Patentes (hojas)
  │   └─ Patentes
  └─ Patentes

================================================================================
NOTAS TÉCNICAS
================================================================================

- Hash SHA256 con Encoding.Unicode (UTF-16)
- DVH validado automáticamente en cada lectura de usuario
- Todas las operaciones registradas en Bitacora
- Sincronización de veterinario NO interrumpe login si falla
- Variable estática _usuarioLogueado mantiene sesión

================================================================================
