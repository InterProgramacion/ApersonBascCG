# Diseño: Asignación de Báscula por Usuario Desktop

**Fecha:** 2026-04-09  
**Proyecto:** AperBascu (Aperson_FacInv)

---

## Contexto

Actualmente la báscula activa del sistema se lee de `ConfigSQL.xml` (clave `<Bascula>`), lo que significa que todos los usuarios que inician sesión en la misma máquina comparten la misma báscula. Se necesita que la báscula se asigne por usuario de forma individual desde la BD, y que el sistema la seleccione automáticamente al hacer login.

Ya existen las tablas:
- `bodega_user` — relaciona usuarios web (`users.id`) con bodegas (`Bodega.Id_bod`)
- `bascula_bodega` — relaciona básculas (`Basculas.Id_bas`) con bodegas (`Bodega.Id_bod`)

La solución extiende `bodega_user` para soportar también usuarios desktop (`Usuario.id_usu`), aprovechando la cadena existente para derivar la báscula asignada.

---

## Arquitectura

### Cadena de relaciones

```
Usuario (desktop)
    ↓ usuario_desktop_id
bodega_user
    ↓ bodega_id
Bodega
    ↓ bodega_id
bascula_bodega
    ↓ bascula_id
Basculas
```

### Tablas involucradas

| Tabla | Rol |
|---|---|
| `Usuario` | Usuarios del desktop (login existente) |
| `bodega_user` | Tabla pivot extendida — web users y desktop users |
| `Bodega` | Bodegas/almacenes |
| `bascula_bodega` | Relación bodega ↔ báscula (ya existe) |
| `Basculas` | Básculas registradas |

---

## Cambios en Base de Datos

### 1. Extender `bodega_user`

```sql
-- Hacer user_id nullable (actualmente NOT NULL, solo para web)
ALTER TABLE bodega_user ALTER COLUMN user_id BIGINT NULL;

-- Agregar columna para usuario desktop
ALTER TABLE bodega_user ADD usuario_desktop_id INT NULL;

ALTER TABLE bodega_user ADD CONSTRAINT FK_bodega_user_usuario
  FOREIGN KEY (usuario_desktop_id) REFERENCES Usuario(id_usu);

-- Garantizar que exactamente un tipo de usuario esté en cada fila
ALTER TABLE bodega_user ADD CONSTRAINT CHK_bodega_user_one_user
  CHECK (
    (user_id IS NOT NULL AND usuario_desktop_id IS NULL) OR
    (user_id IS NULL AND usuario_desktop_id IS NOT NULL)
  );
```

**Impacto en filas existentes:** Las filas actuales tienen `user_id NOT NULL` y `usuario_desktop_id = NULL`, por lo que pasan el CHECK constraint sin cambios.

**Constraint única existente** `UQ_bodega_user (user_id, bodega_id)`: sigue siendo válida para filas web. Para filas desktop, la unicidad queda controlada naturalmente por el CHK.  
Si se requiere constraint única también para desktop:
```sql
CREATE UNIQUE INDEX UQ_bodega_user_desktop 
  ON bodega_user(usuario_desktop_id, bodega_id) 
  WHERE usuario_desktop_id IS NOT NULL;
```

---

## Cambios en Código VB.NET

### Archivo: `Forms/LogIn.vb`

**Punto de inserción:** Después de la validación exitosa de credenciales (donde ya se obtiene `id_usu`) y antes de abrir el formulario principal.

**Lógica a agregar:**

```vb
' 1. Intentar obtener la báscula asignada al usuario en BD
Dim sqlBascula As String = 
    "SELECT TOP 1 bb.bascula_id " &
    "FROM bodega_user bu " &
    "INNER JOIN bascula_bodega bb ON bb.bodega_id = bu.bodega_id " &
    "WHERE bu.usuario_desktop_id = " & LibreriaGeneral.id_usu & 
    " AND bb.activo = 1 " &
    "ORDER BY bu.idbodega_user ASC, bb.idbascula_bodega ASC"

Dim dtBascula As DataTable = ObjDatos.consulta_reader(sqlBascula)

If dtBascula IsNot Nothing AndAlso dtBascula.Rows.Count > 0 Then
    ' Usar báscula asignada desde BD
    LibreriaGeneral.nBascu = dtBascula.Rows(0)("bascula_id").ToString()
Else
    ' Fallback: usar valor de ConfigSQL.xml (comportamiento actual)
    ' LibreriaGeneral.nBascu ya fue cargado desde ConfigSQL en DatosGenerales
    ' No se necesita acción adicional
End If
```

**Nota:** `ObjDatos` es la instancia de `DatosGenerales` ya disponible en el form de login.

---

## Flujo de Login Completo (con el cambio)

```
Usuario escribe usuario + contraseña
    ↓
Validar credenciales en tabla Usuario         (sin cambios)
    ↓
Obtener empresa/bodega del ComboBox           (sin cambios)
    ↓
Consultar BD: bodega_user + bascula_bodega    ← NUEVO
    ├── Con resultado → LibreriaGeneral.nBascu = bascula asignada
    └── Sin resultado → LibreriaGeneral.nBascu = valor de ConfigSQL.xml (fallback)
    ↓
Abrir formulario principal                    (sin cambios)
```

---

## Administración de Asignaciones

Las asignaciones usuario-bascula se gestionan desde el portal web (DotVVM), no desde el desktop. El administrador web asignará:
1. Un usuario desktop (`usuario_desktop_id`) a una bodega (`bodega_id`) en `bodega_user`
2. Una báscula activa a esa bodega en `bascula_bodega` (ya existe esta funcionalidad)

---

## Casos de Borde

| Escenario | Comportamiento |
|---|---|
| Usuario sin asignación en BD | Fallback a `ConfigSQL.xml` |
| Bodega asignada sin báscula activa en `bascula_bodega` | Fallback a `ConfigSQL.xml` |
| Usuario con múltiples bodegas asignadas | Se usa la primera por `ORDER BY idbodega_user ASC` |
| Bodega con múltiples básculas activas | Se usa la primera por `ORDER BY idbascula_bodega ASC` en bascula_bodega (o agregar columna `orden` si se requiere control explícito) |

---

## Verificación

1. Crear una fila en `bodega_user` con `usuario_desktop_id = [id de un usuario test]` y `bodega_id = [bodega con báscula activa en bascula_bodega]`
2. Iniciar sesión con ese usuario en el desktop
3. Verificar que `LibreriaGeneral.nBascu` contiene el `bascula_id` esperado
4. Abrir `frmPesaje` y confirmar que carga la báscula correcta
5. Probar con un usuario SIN asignación: verificar que `nBascu` viene de `ConfigSQL.xml`

---

## Archivos a Modificar

| Archivo | Cambio |
|---|---|
| `Forms/LogIn.vb` | Agregar consulta post-login para obtener báscula asignada |
| BD `DbAperBascuCG` | Script SQL para extender `bodega_user` |

**Sin cambios en:** `frmPesaje.vb`, `DatosGenerales.vb`, `LibreriaGeneral.vb`, `ConfigSQL.xml`, `RutinasConfig.vb`
