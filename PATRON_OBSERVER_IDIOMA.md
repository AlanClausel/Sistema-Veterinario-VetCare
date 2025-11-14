# Patrón Observer para Cambio de Idioma

## Descripción

El sistema VetCare implementa el **patrón Observer** para el cambio dinámico de idioma. Cuando el usuario cambia el idioma desde `FormMiCuenta`, **todos los formularios abiertos se actualizan automáticamente** sin necesidad de cerrarlos y reabrirlos.

## Arquitectura

### Componentes del Patrón

```
┌─────────────────────────────────────────────────────────────┐
│                    LanguageManager                          │
│                      (Subject)                              │
│  - List<ILanguageObserver> _observers                       │
│  + Subscribe(observer)                                      │
│  + Unsubscribe(observer)                                    │
│  + CambiarIdioma(codigoCultura)                            │
│  - NotificarCambioIdioma()                                 │
└────────────┬────────────────────────────────────────────────┘
             │
             │ notifica
             ▼
┌────────────────────────────────┐
│    ILanguageObserver           │
│    (Interface)                 │
│  + ActualizarIdioma()          │
└───────────┬────────────────────┘
            │
            │ implementan
            ▼
┌───────────────────────────────────────────┐
│         BaseObservableForm                │
│         (Clase base abstracta)            │
│  + ActualizarIdioma()                     │
│  # abstract ActualizarTextos()            │
│  - Subscribe/Unsubscribe automático       │
└───────────┬───────────────────────────────┘
            │
            │ heredan
            ▼
┌────────────────────────────────────────────┐
│  FormMiCuenta, menu, etc.                  │
│  (Formularios concretos)                   │
│  # override ActualizarTextos()             │
└────────────────────────────────────────────┘
```

### Archivos Creados/Modificados

**Nuevos archivos:**
- `ServicesSeguridad/Services/ILanguageObserver.cs` - Interfaz para observers
- `UI/WinUi/BaseObservableForm.cs` - Clase base para formularios observables

**Archivos modificados:**
- `ServicesSeguridad/Services/LanguageManager.cs` - Refactorizado como Subject
- `UI/WinUi/Administración/FormMiCuenta.cs` - Hereda de BaseObservableForm
- `UI/WinUi/Administración/menu.cs` - Hereda de BaseObservableForm
- `UI/WinUi/Login.cs` - Usa LanguageManager.CambiarIdioma()

## Uso: Cómo Hacer un Formulario Observable

### Opción 1: Heredar de BaseObservableForm (Recomendado)

Esta es la forma más sencilla y recomendada para la mayoría de los formularios.

**Paso 1:** Cambiar la herencia del formulario

```csharp
// ANTES:
public partial class MiFormulario : Form

// DESPUÉS:
public partial class MiFormulario : BaseObservableForm
```

**Paso 2:** Implementar el método `ActualizarTextos()`

```csharp
/// <summary>
/// Actualiza todos los textos del formulario según el idioma actual.
/// Se invoca automáticamente cuando cambia el idioma.
/// </summary>
protected override void ActualizarTextos()
{
    // Actualizar título del formulario
    this.Text = LanguageManager.Translate("titulo_formulario");

    // Actualizar labels
    lblNombre.Text = LanguageManager.Translate("nombre");
    lblApellido.Text = LanguageManager.Translate("apellido");

    // Actualizar botones
    btnGuardar.Text = LanguageManager.Translate("guardar");
    btnCancelar.Text = LanguageManager.Translate("cancelar");

    // Actualizar GroupBox
    grpDatos.Text = LanguageManager.Translate("datos_personales");

    // Actualizar columnas de DataGridView
    if (dgvDatos.Columns.Count > 0)
    {
        dgvDatos.Columns["Nombre"].HeaderText = LanguageManager.Translate("nombre");
        dgvDatos.Columns["Apellido"].HeaderText = LanguageManager.Translate("apellido");
    }
}
```

**Paso 3:** NO llamar `ActualizarTextos()` manualmente en el `Load`

```csharp
// INCORRECTO - No hacer esto:
private void MiFormulario_Load(object sender, EventArgs e)
{
    ActualizarTextos(); // ❌ NO LLAMAR - BaseObservableForm ya lo hace
    CargarDatos();
}

// CORRECTO:
private void MiFormulario_Load(object sender, EventArgs e)
{
    // ActualizarTextos() se llama automáticamente
    CargarDatos();
}
```

### Ejemplo Completo

```csharp
using System;
using System.Windows.Forms;
using ServicesSecurity.Services;
using UI.WinUi;

namespace UI.WinUi.Negocio
{
    public partial class FormGestionProductos : BaseObservableForm
    {
        public FormGestionProductos()
        {
            InitializeComponent();
        }

        private void FormGestionProductos_Load(object sender, EventArgs e)
        {
            // BaseObservableForm ya llama ActualizarTextos() automáticamente
            CargarProductos();
        }

        /// <summary>
        /// Actualiza textos cuando cambia el idioma
        /// </summary>
        protected override void ActualizarTextos()
        {
            // Título
            this.Text = LanguageManager.Translate("gestion_productos");

            // Labels
            lblCodigo.Text = LanguageManager.Translate("codigo") + ":";
            lblNombre.Text = LanguageManager.Translate("nombre") + ":";
            lblPrecio.Text = LanguageManager.Translate("precio") + ":";

            // Botones
            btnNuevo.Text = LanguageManager.Translate("nuevo");
            btnEditar.Text = LanguageManager.Translate("editar");
            btnEliminar.Text = LanguageManager.Translate("eliminar");
            btnCerrar.Text = LanguageManager.Translate("cerrar");

            // GroupBox
            grpBusqueda.Text = LanguageManager.Translate("busqueda");
            grpListado.Text = LanguageManager.Translate("listado");

            // Columnas del grid
            if (dgvProductos.Columns.Count > 0)
            {
                dgvProductos.Columns["Codigo"].HeaderText =
                    LanguageManager.Translate("codigo");
                dgvProductos.Columns["Nombre"].HeaderText =
                    LanguageManager.Translate("nombre");
                dgvProductos.Columns["Precio"].HeaderText =
                    LanguageManager.Translate("precio");
                dgvProductos.Columns["Stock"].HeaderText =
                    LanguageManager.Translate("stock");
            }
        }

        private void CargarProductos()
        {
            // Lógica para cargar productos
        }
    }
}
```

### Opción 2: Implementar ILanguageObserver Manualmente

Para casos especiales donde no puedes heredar de `BaseObservableForm` (por ejemplo, si ya heredas de otra clase base).

```csharp
using System;
using System.Windows.Forms;
using ServicesSecurity.Services;

namespace UI.WinUi.Negocio
{
    public partial class FormEspecial : MiClaseBase, ILanguageObserver
    {
        public FormEspecial()
        {
            InitializeComponent();

            // Suscribirse al LanguageManager
            this.Load += FormEspecial_Load;
            this.FormClosing += FormEspecial_FormClosing;
        }

        private void FormEspecial_Load(object sender, EventArgs e)
        {
            // Suscribirse manualmente
            LanguageManager.Subscribe(this);

            // Actualizar textos inicial
            ActualizarTextos();
        }

        private void FormEspecial_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Desuscribirse manualmente para evitar memory leaks
            LanguageManager.Unsubscribe(this);
        }

        // Implementación de ILanguageObserver
        public void ActualizarIdioma()
        {
            // Ejecutar en el thread de la UI
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ActualizarTextos()));
            }
            else
            {
                ActualizarTextos();
            }
        }

        private void ActualizarTextos()
        {
            this.Text = LanguageManager.Translate("titulo");
            btnGuardar.Text = LanguageManager.Translate("guardar");
            // ... resto de controles
        }
    }
}
```

## Cambiar el Idioma de la Aplicación

### Desde FormMiCuenta (Ya implementado)

El usuario puede cambiar el idioma desde `FormMiCuenta`:
1. Seleccionar idioma del ComboBox
2. Click en "Guardar Idioma"
3. El idioma se guarda en BD y **todos los formularios abiertos se actualizan automáticamente**

Código relevante en `FormMiCuenta.cs`:

```csharp
private void btnGuardarIdioma_Click(object sender, EventArgs e)
{
    var idiomaSeleccionado = (IdiomaItem)cmbIdioma.SelectedItem;

    // 1. Actualizar en BD
    UsuarioBLL.ActualizarIdioma(_usuarioLogueado.IdUsuario, idiomaSeleccionado.Codigo);

    // 2. Actualizar en memoria
    _usuarioLogueado.IdiomaPreferido = idiomaSeleccionado.Codigo;

    // 3. PATRÓN OBSERVER: Notifica a todos los formularios abiertos
    LanguageManager.CambiarIdioma(idiomaSeleccionado.Codigo);

    MessageBox.Show(LanguageManager.Translate("idioma_actualizado_exito"));
}
```

### Desde Login (Ya implementado)

El usuario puede seleccionar idioma antes de hacer login:
1. Click en link "Español" o "English"
2. El formulario de login se actualiza inmediatamente
3. Al hacer login, el idioma seleccionado se aplica a toda la sesión

### Programáticamente (Para desarrolladores)

Si necesitas cambiar el idioma desde código:

```csharp
// Cambiar a español
LanguageManager.CambiarIdioma("es-AR");

// Cambiar a inglés
LanguageManager.CambiarIdioma("en-GB");

// Esto actualizará automáticamente todos los formularios suscritos
```

## Flujo de Ejecución

### Cuando un Formulario se Abre

```
1. Usuario abre FormGestionClientes
2. Constructor llama a InitializeComponent()
3. BaseObservableForm.Load se dispara
4. BaseObservableForm se suscribe a LanguageManager
5. BaseObservableForm llama a ActualizarTextos()
6. FormGestionClientes.ActualizarTextos() actualiza todos los controles
```

### Cuando el Usuario Cambia el Idioma

```
1. Usuario cambia idioma en FormMiCuenta
2. FormMiCuenta.btnGuardarIdioma_Click ejecuta:
   - UsuarioBLL.ActualizarIdioma() → BD actualizada
   - LanguageManager.CambiarIdioma("en-GB")
3. LanguageManager.CambiarIdioma():
   - Cambia Thread.CurrentCulture a "en-GB"
   - Llama a NotificarCambioIdioma()
4. LanguageManager.NotificarCambioIdioma():
   - Itera sobre todos los observers suscritos
   - Llama a observer.ActualizarIdioma() en cada uno
5. BaseObservableForm.ActualizarIdioma():
   - Verifica InvokeRequired (thread safety)
   - Llama a ActualizarTextos()
6. FormGestionClientes.ActualizarTextos():
   - Actualiza todos los controles con textos en inglés
7. Resultado: El formulario se actualiza automáticamente sin cerrarlo
```

### Cuando un Formulario se Cierra

```
1. Usuario cierra FormGestionClientes
2. BaseObservableForm.FormClosing se dispara
3. BaseObservableForm se desuscribe de LanguageManager
4. Formulario se libera de memoria (no hay memory leaks)
```

## Ventajas del Patrón Observer

1. **Actualización Automática:** Los formularios se actualizan sin intervención manual
2. **Desacoplamiento:** Los formularios no necesitan conocer cuándo cambia el idioma
3. **Escalabilidad:** Agregar nuevos formularios observables es trivial
4. **Sin Memory Leaks:** Desuscripción automática al cerrar formularios
5. **Thread-Safe:** Manejo correcto de threads UI/background
6. **Mantenibilidad:** Lógica centralizada en LanguageManager

## Mejores Prácticas

### ✅ Hacer

1. **Heredar de BaseObservableForm** para nuevos formularios
2. **Implementar ActualizarTextos()** actualizando TODOS los controles
3. **Usar LanguageManager.Translate()** para obtener textos
4. **Actualizar columnas de DataGridView** dentro de ActualizarTextos()
5. **Actualizar items de ComboBox** si contienen textos traducibles

### ❌ No Hacer

1. **NO llamar ActualizarTextos() manualmente** en el Load (BaseObservableForm lo hace)
2. **NO olvidar desuscribirse** si implementas ILanguageObserver manualmente
3. **NO hardcodear textos** en los controles (usar LanguageManager.Translate())
4. **NO cachear traducciones** en variables privadas (siempre llamar Translate())
5. **NO ignorar el InvokeRequired** si implementas manualmente

## Debugging y Testing

### Verificar Observers Suscritos

```csharp
// Obtener cantidad de observers actualmente suscritos
int cantidadObservers = LanguageManager.ContarObservers();
Console.WriteLine($"Observers suscritos: {cantidadObservers}");
```

### Probar Cambio de Idioma

1. Ejecutar la aplicación
2. Login con usuario admin
3. Abrir varios formularios (Gestión Clientes, Citas, etc.)
4. Ir a "Mi Cuenta"
5. Cambiar idioma
6. Verificar que TODOS los formularios abiertos se actualicen inmediatamente

### Debugging de Notificaciones

Si un formulario no se actualiza:

1. **Verificar herencia:** ¿Hereda de `BaseObservableForm`?
2. **Verificar override:** ¿El método `ActualizarTextos()` tiene `override`?
3. **Verificar textos:** ¿Usa `LanguageManager.Translate()` en ActualizarTextos()?
4. **Verificar logs:** Revisar `Bitacora` para ver errores de notificación

## Troubleshooting

### Problema: El formulario no se actualiza al cambiar idioma

**Causa:** El formulario no hereda de `BaseObservableForm`

**Solución:** Cambiar herencia a `BaseObservableForm` e implementar `ActualizarTextos()`

---

### Problema: Error "abstract member not implemented"

**Causa:** No se implementó el método `ActualizarTextos()`

**Solución:** Agregar método con override:
```csharp
protected override void ActualizarTextos()
{
    // Implementación aquí
}
```

---

### Problema: Algunos controles no se actualizan

**Causa:** No todos los controles están en `ActualizarTextos()`

**Solución:** Asegurarse de actualizar TODOS los controles con texto

---

### Problema: Memory leak - observers no se liberan

**Causa:** Si implementas manualmente `ILanguageObserver`, no te desuscribiste

**Solución:** Llamar `LanguageManager.Unsubscribe(this)` en `FormClosing` o usar `BaseObservableForm`

---

## Idiomas Disponibles

Actualmente el sistema soporta:
- **es-AR:** Español (Argentina)
- **en-GB:** English (United Kingdom)

Para agregar nuevos idiomas:
1. Crear archivo de recursos en `UI/Resources/I18n/idioma/[codigo].txt`
2. Agregar opción en `FormMiCuenta.CargarIdiomasDisponibles()`
3. Las traducciones se cargan automáticamente vía `LanguageRepository`

## Referencias

- **Patrón Observer:** Gang of Four - Design Patterns
- **ILanguageObserver:** `ServicesSeguridad/Services/ILanguageObserver.cs`
- **LanguageManager:** `ServicesSeguridad/Services/LanguageManager.cs`
- **BaseObservableForm:** `UI/WinUi/BaseObservableForm.cs`
- **Ejemplo completo:** `UI/WinUi/Administración/FormMiCuenta.cs`

---

**Implementado:** 2025-01-13
**Autor:** Claude Code
**Patrón:** Observer (Gang of Four)
