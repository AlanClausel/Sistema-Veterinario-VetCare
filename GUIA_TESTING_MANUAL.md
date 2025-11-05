# 📋 Guía de Testing Manual - Sistema VetCare

## 📄 Descripción

Este documento complementa el archivo **Testing_Manual_VetCare.csv** que contiene 120 casos de prueba organizados para validar todas las funcionalidades del sistema VetCare.

## 🎯 Objetivo del Testing Manual

Verificar que todas las funcionalidades del sistema funcionan correctamente:
- ✅ Validaciones de entrada
- ✅ Operaciones CRUD (Crear, Leer, Actualizar, Eliminar)
- ✅ Navegación entre pantallas
- ✅ Permisos por rol de usuario
- ✅ Integridad de datos
- ✅ Manejo de errores

---

## 📊 Estructura del CSV

El archivo CSV contiene las siguientes columnas:

| Columna | Descripción |
|---------|-------------|
| **ID** | Identificador único del caso de prueba (TC-001, TC-002...) |
| **Módulo** | Pantalla o módulo del sistema |
| **Funcionalidad** | Tipo de operación (Crear, Validación, Búsqueda, etc.) |
| **Caso de Prueba** | Nombre descriptivo del test |
| **Pre-Condición** | Estado del sistema antes de ejecutar |
| **Pasos** | Instrucciones paso a paso (numeradas) |
| **Resultado Esperado** | Lo que debe suceder si funciona correctamente |
| **Prioridad** | Crítico, Alto, Medio, Bajo |
| **Severidad** | Alta, Media, Baja |
| **Estado** | Pendiente / Ejecutado / Fallido |
| **Probado Por** | Nombre del tester |
| **Fecha** | Fecha de ejecución |
| **Comentarios** | Observaciones adicionales |

---

## 🚀 Cómo Usar Este Archivo

### Paso 1: Abrir en Excel
```
1. Abrir Excel
2. Archivo → Abrir → Seleccionar "Testing_Manual_VetCare.csv"
3. Importar como tabla
4. Activar filtros (Ctrl + Shift + L)
```

### Paso 2: Preparar Base de Datos
```sql
-- Ejecutar en SQL Server Management Studio
-- 1. Crear/Restaurar base de datos SecurityVet
-- 2. Crear/Restaurar base de datos VetCareDB
-- 3. Verificar que existen usuarios de prueba:

USE SecurityVet;
SELECT Nombre, Email FROM Usuario;

-- Debe existir al menos:
-- Usuario: admin / Contraseña: admin123 / Rol: Administrador
```

### Paso 3: Ejecutar Casos de Prueba

**Por Módulo:**
```
1. Filtrar columna "Módulo" por "Login"
2. Ejecutar casos TC-001 a TC-013
3. Marcar en columna "Estado": Ejecutado o Fallido
4. Si falla, documentar en "Comentarios"
5. Continuar con siguiente módulo
```

**Por Prioridad:**
```
1. Filtrar columna "Prioridad" por "Crítico"
2. Ejecutar todos los casos críticos primero
3. Luego "Alto", "Medio", "Bajo"
```

---

## 📝 Distribución de Casos de Prueba

### Por Módulo

| Módulo | Casos | IDs |
|--------|-------|-----|
| **Login** | 13 casos | TC-001 a TC-013 |
| **Menú Principal** | 6 casos | TC-014 a TC-019 |
| **Gestión Clientes** | 28 casos | TC-020 a TC-047 |
| **Gestión Mascotas** | 13 casos | TC-048 a TC-060 |
| **Gestión Citas** | 19 casos | TC-061 a TC-079 |
| **Gestión Medicamentos** | 7 casos | TC-080 a TC-086 |
| **Mis Citas (Veterinario)** | 2 casos | TC-087 a TC-088 |
| **Consulta Médica** | 7 casos | TC-089 a TC-095 |
| **Historial Clínico** | 3 casos | TC-096 a TC-098 |
| **Gestión Usuarios** | 9 casos | TC-099 a TC-107 |
| **Gestión Permisos** | 4 casos | TC-108 a TC-111 |
| **Integridad BD** | 3 casos | TC-111 a TC-113 |
| **Rendimiento** | 2 casos | TC-114 a TC-115 |
| **Usabilidad** | 3 casos | TC-116 a TC-118 |
| **Seguridad** | 2 casos | TC-119 a TC-120 |

### Por Prioridad

| Prioridad | Cantidad | Descripción |
|-----------|----------|-------------|
| **Crítico** | 45 casos | Funcionalidades core - Probar primero |
| **Alto** | 38 casos | Funcionalidades importantes |
| **Medio** | 26 casos | Funcionalidades secundarias |
| **Bajo** | 11 casos | Detalles de UI/UX |

---

## 🐛 Cómo Reportar Bugs Encontrados

Cuando un caso de prueba **FALLE**, documentar así en la columna "Comentarios":

```
❌ FALLO: [Descripción breve]
Resultado actual: [Lo que pasó realmente]
Captura: [Opcional - nombre de archivo de imagen]
```

### Ejemplo:
```
Columna "Estado": Fallido
Columna "Comentarios":
❌ FALLO: Permite guardar cliente con DNI duplicado
Resultado actual: Se guardó cliente con DNI 12345678 que ya existía
Debería mostrar error pero no lo hizo
Captura: bug_dni_duplicado.png
```

---

## 📋 Plantilla de Reporte de Bug Detallado

Si encuentras bugs críticos, crear archivo separado:

```markdown
# BUG REPORT

## ID: BUG-001
**Fecha:** 23/10/2025
**Reportado por:** [Tu nombre]
**Módulo:** Gestión Clientes
**Caso de Prueba Relacionado:** TC-030

## Severidad
🔴 CRÍTICA - Permite datos inconsistentes

## Descripción
El sistema permite guardar clientes con DNI duplicado, violando la regla de negocio de DNI único.

## Pasos para Reproducir
1. Abrir Gestión de Clientes
2. Verificar que existe cliente con DNI "12345678"
3. Click "Nuevo"
4. Ingresar:
   - Nombre: "Pedro"
   - Apellido: "García"
   - DNI: "12345678" (duplicado)
   - Resto de campos completos
5. Click "Guardar"

## Resultado Esperado
- Error: "Ya existe un cliente con DNI 12345678"
- No debe guardarse en BD

## Resultado Actual
- Mensaje: "Cliente registrado correctamente"
- Se guarda en BD
- Ahora hay 2 clientes con mismo DNI

## Evidencia
- Captura de pantalla: bug_001_dni_duplicado.png
- Query SQL: SELECT * FROM Cliente WHERE DNI = '12345678'
  Retorna 2 filas (INCORRECTO)

## Análisis Técnico
**Archivo:** ClienteBLL.cs, línea 47
**Problema:** La validación `ExistePorDNI()` no se está ejecutando antes de guardar

**Código actual:**
```csharp
public Cliente RegistrarCliente(Cliente cliente)
{
    ValidarCliente(cliente);
    // FALTA: Validación de DNI duplicado
    return _clienteRepository.Crear(cliente);
}
```

## Solución Propuesta
Agregar validación antes de crear:
```csharp
public Cliente RegistrarCliente(Cliente cliente)
{
    ValidarCliente(cliente);

    // AGREGAR ESTA VALIDACIÓN:
    if (_clienteRepository.ExistePorDNI(cliente.DNI))
    {
        throw new InvalidOperationException($"Ya existe un cliente con DNI {cliente.DNI}");
    }

    return _clienteRepository.Crear(cliente);
}
```

## Impacto
- **Alto**: Datos inconsistentes en BD
- **Usuarios afectados**: Todos los que gestionan clientes
- **Workaround temporal**: Verificar manualmente en SQL antes de guardar
```

---

## 🎯 Plan de Ejecución Recomendado

### Día 1: Funcionalidades Críticas (4-6 horas)
```
☐ TC-001 a TC-013: Login (30 min)
☐ TC-014 a TC-019: Menú Principal (20 min)
☐ TC-020 a TC-047: Gestión Clientes (2 horas)
☐ TC-048 a TC-060: Gestión Mascotas (1.5 horas)
☐ TC-061 a TC-079: Gestión Citas (2 horas)
```

### Día 2: Módulos Especializados (3-4 horas)
```
☐ TC-080 a TC-086: Gestión Medicamentos (1 hora)
☐ TC-087 a TC-095: Consultas Médicas (1.5 horas)
☐ TC-096 a TC-098: Historial Clínico (30 min)
☐ TC-099 a TC-110: Administración (1.5 horas)
```

### Día 3: Testing Adicional (2-3 horas)
```
☐ TC-111 a TC-113: Integridad BD (1 hora)
☐ TC-114 a TC-115: Rendimiento (30 min)
☐ TC-116 a TC-120: Seguridad y Usabilidad (1 hora)
☐ Re-test de bugs encontrados
```

---

## 📊 Métricas de Testing

Al finalizar, calcular:

### Cobertura de Pruebas
```
Total casos ejecutados / Total casos (120) × 100 = ___%
```

### Tasa de Éxito
```
Casos exitosos / Casos ejecutados × 100 = ___%
```

### Bugs por Severidad
```
🔴 Críticos: ___
🟡 Medios: ___
🟢 Bajos: ___
Total: ___
```

---

## 🔧 Datos de Prueba Recomendados

### Usuarios de Prueba
```sql
-- Crear en SecurityVet si no existen

-- Usuario 1: Administrador
Usuario: admin
Contraseña: admin123
Rol: ROL_Administrador

-- Usuario 2: Recepcionista
Usuario: recepcionista
Contraseña: recep123
Rol: ROL_Recepcionista

-- Usuario 3: Veterinario
Usuario: veterinario
Contraseña: vet123
Rol: ROL_Veterinario
```

### Clientes de Prueba
```
Cliente 1:
- Nombre: Juan
- Apellido: Pérez
- DNI: 12345678
- Teléfono: 1122334455
- Email: juan.perez@email.com

Cliente 2:
- Nombre: María
- Apellido: González
- DNI: 87654321
- Teléfono: 1155667788
- Email: maria.gonzalez@email.com
```

### Mascotas de Prueba
```
Mascota 1 (de Juan Pérez):
- Nombre: Max
- Especie: Perro
- Raza: Labrador
- Fecha Nac: 01/01/2020
- Sexo: Macho
- Peso: 25.5 kg

Mascota 2 (de María González):
- Nombre: Luna
- Especie: Gato
- Raza: Siamés
- Fecha Nac: 15/06/2021
- Sexo: Hembra
- Peso: 4.2 kg
```

### Medicamentos de Prueba
```
Medicamento 1:
- Nombre: Amoxicilina
- Presentación: Tableta
- Stock: 50
- Precio: 150.00

Medicamento 2:
- Nombre: Paracetamol Veterinario
- Presentación: Suspensión
- Stock: 30
- Precio: 80.00

Medicamento 3: (Stock bajo para testing)
- Nombre: Vacuna Antirrábica
- Presentación: Inyectable
- Stock: 3
- Precio: 350.00
```

---

## ✅ Checklist Pre-Testing

Antes de comenzar, verificar:

```
☐ Base de datos SecurityVet creada y poblada
☐ Base de datos VetCareDB creada y poblada
☐ Usuario admin con rol Administrador existe
☐ Usuario recepcionista con rol Recepcionista existe
☐ Usuario veterinario con rol Veterinario existe
☐ Conexión a SQL Server funcionando
☐ Aplicación compila sin errores
☐ UI.exe se ejecuta correctamente
☐ Archivo CSV abierto en Excel
☐ Columnas "Estado", "Probado Por", "Fecha" listas para editar
☐ Carpeta para capturas de pantalla creada
```

---

## 🎓 Tips para Testing Efectivo

### 1. No Apresurarse
- Leer cada caso completo antes de ejecutar
- Entender el resultado esperado
- Ejecutar paso a paso

### 2. Documentar TODO
- Marcar cada caso como Ejecutado o Fallido
- Escribir comentarios en casos fallidos
- Tomar capturas de pantalla de bugs

### 3. Probar Casos Borde
- Valores extremos (vacíos, muy largos, negativos)
- Fechas pasadas/futuras
- Caracteres especiales

### 4. Verificar en Base de Datos
```sql
-- Después de crear cliente, verificar:
SELECT * FROM Cliente ORDER BY FechaRegistro DESC;

-- Después de eliminar, verificar:
SELECT * FROM Cliente WHERE IdCliente = '[guid]';
-- Debe retornar 0 filas

-- Verificar cascada:
SELECT * FROM Mascota WHERE IdCliente = '[guid-eliminado]';
-- Debe retornar 0 filas
```

### 5. Limpiar Datos de Prueba
```sql
-- Al finalizar el día, limpiar datos de prueba:
DELETE FROM ConsultaMedicamento;
DELETE FROM ConsultaMedica;
DELETE FROM Cita;
DELETE FROM Mascota;
DELETE FROM Cliente WHERE DNI IN ('12345678', '87654321');
DELETE FROM Medicamento WHERE Nombre LIKE '%Prueba%';
```

---

## 📞 Contacto y Soporte

Si encuentras bugs críticos que bloquean el testing:
1. Documentar el bug con plantilla completa
2. Marcar caso como "Bloqueado" en CSV
3. Continuar con otros casos independientes
4. Reportar a equipo de desarrollo

---

## 📈 Reporte Final

Al completar todos los casos, generar reporte con:

```markdown
# REPORTE FINAL DE TESTING MANUAL
Fecha: _____________
Tester: _____________

## Resumen Ejecutivo
- Total casos de prueba: 120
- Casos ejecutados: ___
- Casos exitosos: ___
- Casos fallidos: ___
- Casos bloqueados: ___
- Cobertura: ___%

## Bugs Encontrados
### Críticos: ___
- BUG-001: [Descripción]
- BUG-002: [Descripción]

### Altos: ___
- BUG-003: [Descripción]

### Medios: ___
- BUG-004: [Descripción]

## Módulos con Mayor Cantidad de Bugs
1. [Módulo]: ___ bugs
2. [Módulo]: ___ bugs
3. [Módulo]: ___ bugs

## Recomendaciones
- [Recomendación 1]
- [Recomendación 2]
- [Recomendación 3]

## Estado del Sistema
☐ Listo para producción
☐ Requiere correcciones menores
☐ Requiere correcciones mayores
☐ No listo para producción

## Próximos Pasos
1. Corregir bugs críticos
2. Re-test de casos fallidos
3. Testing de regresión
```

---

**¡Éxito con el testing! 🎯**
