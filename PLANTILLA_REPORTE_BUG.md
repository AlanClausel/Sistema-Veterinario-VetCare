# 🐛 REPORTE DE BUG

> **Instrucciones:** Copiar esta plantilla para cada bug encontrado. Nombrar el archivo como `BUG-XXX_descripcion_corta.md`

---

## Información General

| Campo | Valor |
|-------|-------|
| **ID Bug** | BUG-XXX |
| **Fecha** | DD/MM/YYYY |
| **Reportado por** | [Tu nombre] |
| **Caso de Prueba** | TC-XXX |
| **Módulo** | [Login / Gestión Clientes / Gestión Citas / etc.] |
| **Versión** | [Versión del sistema si aplica] |

---

## 🔴 Severidad

Seleccionar una:

- [ ] **CRÍTICA** - Sistema no funciona / Pérdida de datos / Seguridad comprometida
- [ ] **ALTA** - Funcionalidad principal no funciona / Workaround difícil
- [ ] **MEDIA** - Funcionalidad secundaria no funciona / Workaround disponible
- [ ] **BAJA** - Problema estético / UX / Textos / No afecta funcionalidad

---

## 📝 Descripción del Bug

[Describir el problema en 1-2 oraciones claras]

**Ejemplo:**
> El sistema permite guardar clientes con DNI duplicado, violando la regla de negocio de que el DNI debe ser único por cliente.

---

## 🔄 Pasos para Reproducir

1. [Primer paso con detalle]
2. [Segundo paso con detalle]
3. [Tercer paso con detalle]
4. ...
5. [Último paso - lo que desencadena el bug]

**Ejemplo:**
```
1. Abrir Gestión de Clientes
2. Verificar que existe cliente "Juan Pérez" con DNI "12345678"
3. Click botón "Nuevo"
4. Ingresar datos:
   - Nombre: "Pedro"
   - Apellido: "García"
   - DNI: "12345678" (mismo DNI que Juan Pérez)
   - Teléfono: "1122334455"
   - Email: "pedro@email.com"
   - Dirección: "Calle Test 123"
   - Activo: ✓
5. Click botón "Guardar"
```

---

## ✅ Resultado Esperado

[Describir qué debería pasar si el sistema funcionara correctamente]

**Ejemplo:**
```
- Debe mostrar error: "Ya existe un cliente con DNI 12345678"
- NO debe guardarse el cliente en la base de datos
- El formulario debe permanecer con los datos ingresados
- El foco debe ir al campo DNI
```

---

## ❌ Resultado Actual

[Describir qué pasó realmente - el comportamiento incorrecto]

**Ejemplo:**
```
- Muestra mensaje: "Cliente registrado correctamente"
- El cliente SE GUARDA en la base de datos
- Ahora existen 2 clientes con DNI "12345678"
- El formulario se limpia normalmente
```

---

## 📸 Evidencia

### Capturas de Pantalla
- [ ] Adjuntas: [nombre_archivo_1.png, nombre_archivo_2.png]
- [ ] No aplica

**Descripción de las capturas:**
1. `captura1.png`: [Describe qué muestra]
2. `captura2.png`: [Describe qué muestra]

### Consultas SQL de Verificación

```sql
-- Verificar duplicados
SELECT DNI, COUNT(*) as Cantidad
FROM Cliente
WHERE DNI = '12345678'
GROUP BY DNI
HAVING COUNT(*) > 1;

-- Resultado: Retorna 1 fila con Cantidad = 2 (INCORRECTO)
-- Esperado: 0 filas (DNI único)
```

### Logs del Sistema
```
[Si hay logs relevantes, pegarlos aquí]
```

---

## 💻 Análisis Técnico

### Archivo Afectado
**Ruta:** `[Ruta completa al archivo con el bug]`
**Línea:** [Número de línea si se conoce]

**Ejemplo:**
```
Ruta: VetCareNegocio\BLL\ClienteBLL.cs
Línea: 41-64 (Método RegistrarCliente)
```

### Código Actual (Problemático)

```csharp
// Pegar aquí el código que tiene el bug
public Cliente RegistrarCliente(Cliente cliente)
{
    // Validaciones de negocio
    ValidarCliente(cliente);

    // ❌ PROBLEMA: Falta validación de DNI duplicado

    // Generar nuevo ID si no tiene
    if (cliente.IdCliente == Guid.Empty)
    {
        cliente.IdCliente = Guid.NewGuid();
    }

    cliente.FechaRegistro = DateTime.Now;
    cliente.Activo = true;

    // Persistir en base de datos
    return _clienteRepository.Crear(cliente);
}
```

### Causa Raíz
[Explicar POR QUÉ ocurre el bug]

**Ejemplo:**
> La validación de DNI único (`_clienteRepository.ExistePorDNI()`) no se está invocando en el método `RegistrarCliente()`. El método solo valida formato de campos pero no unicidad del DNI contra la base de datos.

---

## 🔧 Solución Propuesta

### Código Corregido

```csharp
// Código con la corrección aplicada
public Cliente RegistrarCliente(Cliente cliente)
{
    // Validaciones de negocio
    ValidarCliente(cliente);

    // ✅ AGREGAR: Validar DNI único
    if (_clienteRepository.ExistePorDNI(cliente.DNI))
    {
        throw new InvalidOperationException($"Ya existe un cliente con DNI {cliente.DNI}");
    }

    // Generar nuevo ID si no tiene
    if (cliente.IdCliente == Guid.Empty)
    {
        cliente.IdCliente = Guid.NewGuid();
    }

    cliente.FechaRegistro = DateTime.Now;
    cliente.Activo = true;

    // Persistir en base de datos
    return _clienteRepository.Crear(cliente);
}
```

### Archivos a Modificar
1. `ClienteBLL.cs` - Agregar validación de DNI único
2. [Otro archivo si aplica]

### Tests para Validar la Corrección
- [ ] TC-030: DNI duplicado debe mostrar error
- [ ] TC-021: Cliente válido debe guardarse correctamente
- [ ] TC-032: Editar sin cambiar DNI debe funcionar

---

## 📊 Impacto

### Usuarios Afectados
- [ ] Todos los usuarios
- [ ] Solo administradores
- [ ] Solo recepcionistas
- [ ] Solo veterinarios
- [ ] Otro: _____________

### Frecuencia
- [ ] Ocurre siempre (100%)
- [ ] Ocurre frecuentemente (> 50%)
- [ ] Ocurre a veces (< 50%)
- [ ] Ocurre raramente (< 10%)

### Datos Afectados
- [ ] **SÍ** - Datos inconsistentes en BD
- [ ] **NO** - Solo afecta UI

**Descripción del impacto en datos:**
> [Si SÍ, explicar: qué tablas, qué registros, cómo limpiar]

**Ejemplo:**
```
Tablas afectadas: Cliente
Cantidad de registros afectados: 5 clientes con DNI duplicado
Script de limpieza:
  -- Identificar duplicados
  SELECT DNI, COUNT(*) FROM Cliente GROUP BY DNI HAVING COUNT(*) > 1;

  -- Eliminar duplicados manteniendo el más antiguo
  [Script según lógica de negocio]
```

---

## 🔄 Workaround Temporal

[Si existe una manera de evitar el bug mientras se corrige]

**Ejemplo:**
```
Antes de crear un cliente:
1. Buscar por DNI en la grilla
2. Verificar visualmente que no existe
3. Si no existe, proceder a crear

O ejecutar en SQL:
SELECT * FROM Cliente WHERE DNI = '[dni-a-crear]';
Si retorna filas, NO crear el cliente desde la aplicación.
```

Si no hay workaround:
```
❌ No existe workaround - Bug bloqueante
```

---

## 🔗 Bugs Relacionados

- BUG-XXX: [Descripción si hay bugs similares]
- BUG-YYY: [Descripción si hay bugs similares]
- Ninguno

---

## ✅ Verificación de la Corrección

Una vez corregido el bug, verificar:

### Tests de Regresión
- [ ] TC-021: Crear cliente válido (debe seguir funcionando)
- [ ] TC-030: DNI duplicado rechazado (debe fallar con error correcto)
- [ ] TC-032: Editar cliente sin cambiar DNI (debe funcionar)
- [ ] TC-031: Editar cliente y cambiar DNI a uno nuevo válido (debe funcionar)

### Verificación Manual
```
1. Crear cliente con DNI nuevo: ✓ Funciona
2. Intentar crear otro con mismo DNI: ✓ Muestra error
3. Editar cliente existente sin tocar DNI: ✓ Funciona
4. Editar cliente cambiando a DNI duplicado: ✓ Muestra error
```

### Verificación en BD
```sql
-- No deben existir duplicados
SELECT DNI, COUNT(*) as Cantidad
FROM Cliente
GROUP BY DNI
HAVING COUNT(*) > 1;

-- Resultado esperado: 0 filas
```

---

## 📝 Notas Adicionales

[Cualquier información adicional relevante]

**Ejemplos:**
- Este bug solo ocurre en [condiciones específicas]
- Posible relación con [otra funcionalidad]
- Usuario reportó que [información del usuario]
- Verificado en versión anterior: [SÍ/NO]

---

## 🏷️ Estado del Bug

- [ ] **Abierto** - Reportado, pendiente de análisis
- [ ] **En Análisis** - Equipo revisando
- [ ] **Confirmado** - Bug validado, pendiente de corrección
- [ ] **En Desarrollo** - Desarrollador trabajando en la corrección
- [ ] **En Testing** - Corrección implementada, pendiente de verificación
- [ ] **Cerrado** - Corregido y verificado
- [ ] **Rechazado** - No es un bug / Funciona como diseño
- [ ] **Duplicado** - Ya reportado en BUG-XXX

---

## 📅 Historial

| Fecha | Usuario | Acción |
|-------|---------|--------|
| DD/MM/YYYY | [Nombre] | Bug reportado |
| DD/MM/YYYY | [Nombre] | Bug confirmado |
| DD/MM/YYYY | [Nombre] | Corrección implementada |
| DD/MM/YYYY | [Nombre] | Verificado y cerrado |

---

**Firma del Tester:** ___________________
**Fecha:** ___________________
