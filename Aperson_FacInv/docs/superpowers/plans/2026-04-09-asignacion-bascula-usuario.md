# Asignación de Báscula por Usuario Desktop — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Extender `bodega_user` para soportar usuarios desktop y que el login asigne automáticamente la báscula desde BD (con fallback a ConfigSQL.xml).

**Architecture:** Se agrega `usuario_desktop_id` a la tabla pivot `bodega_user` existente. En el login, después de validar credenciales, se consulta la primera báscula activa asignada al usuario a través de `bodega_user → bascula_bodega → Basculas`. Si no hay asignación, se mantiene el valor cargado desde `ConfigSQL.xml`.

**Tech Stack:** SQL Server (T-SQL), VB.NET 4.8, Windows Forms

---

## Archivos a modificar

| Archivo | Cambio |
|---|---|
| `Forms/LogIn.vb` | Agregar consulta de báscula después de validar empresa (línea ~111) |
| BD `DbAperBascuCG` | Ejecutar script SQL para extender `bodega_user` |

Sin cambios en: `frmPesaje.vb`, `DatosGenerales.vb`, `LibreriaGeneral.vb`, `ConfigSQL.xml`, `RutinasConfig.vb`

---

## Task 1: Modificar la tabla `bodega_user` en la BD

**Files:**
- Execute SQL on: `DbAperBascuCG` (SQL Server)

Contexto: `bodega_user` actualmente tiene `user_id BIGINT NOT NULL` (FK a `users.id` web). Se debe hacer nullable y agregar `usuario_desktop_id INT NULL` (FK a `Usuario.id_usu`). Un CHECK garantiza que cada fila tenga exactamente uno de los dos.

- [ ] **Step 1: Ejecutar el script de migración en SQL Server Management Studio (o herramienta equivalente)**

Conectar a `DbAperBascuCG` y ejecutar:

```sql
USE [DbAperBascuCG]
GO

-- 1. Hacer user_id nullable (actualmente NOT NULL, solo para web)
ALTER TABLE dbo.bodega_user
  ALTER COLUMN user_id BIGINT NULL;
GO

-- 2. Agregar columna para usuario desktop
ALTER TABLE dbo.bodega_user
  ADD usuario_desktop_id INT NULL;
GO

-- 3. FK hacia tabla Usuario del desktop
ALTER TABLE dbo.bodega_user
  ADD CONSTRAINT FK_bodega_user_usuario
  FOREIGN KEY (usuario_desktop_id)
  REFERENCES dbo.Usuario(id_usu);
GO

-- 4. Check: exactamente un tipo de usuario por fila
ALTER TABLE dbo.bodega_user
  ADD CONSTRAINT CHK_bodega_user_one_user
  CHECK (
    (user_id IS NOT NULL AND usuario_desktop_id IS NULL) OR
    (user_id IS NULL AND usuario_desktop_id IS NOT NULL)
  );
GO

-- 5. Índice único para evitar duplicados en asignaciones desktop
CREATE UNIQUE INDEX UQ_bodega_user_desktop
  ON dbo.bodega_user(usuario_desktop_id, bodega_id)
  WHERE usuario_desktop_id IS NOT NULL;
GO
```

- [ ] **Step 2: Verificar que la migración no rompió filas existentes**

```sql
USE [DbAperBascuCG]
GO

-- Debe mostrar 0 filas (ninguna fila viola el CHECK)
SELECT COUNT(*) AS FilasInvalidas
FROM dbo.bodega_user
WHERE NOT (
  (user_id IS NOT NULL AND usuario_desktop_id IS NULL) OR
  (user_id IS NULL AND usuario_desktop_id IS NOT NULL)
);
```

Resultado esperado: `FilasInvalidas = 0`

- [ ] **Step 3: Insertar una fila de prueba para un usuario desktop**

Primero verificar qué `id_usu` y `Id_bod` existen:

```sql
SELECT TOP 3 id_usu, Log_usu FROM dbo.Usuario WHERE baja_usu = 'ACTIVO';
SELECT TOP 3 Id_bod, nombre_bod FROM dbo.Bodega WHERE baja_bod = 'ACTIVO';
SELECT TOP 3 idbascula_bodega, bascula_id, bodega_id FROM dbo.bascula_bodega WHERE activo = 1;
```

Luego insertar usando valores reales de las consultas anteriores (reemplazar `@ID_USU` y `@ID_BOD`):

```sql
INSERT INTO dbo.bodega_user (user_id, bodega_id, usuario_desktop_id, created_at, updated_at)
VALUES (NULL, @ID_BOD, @ID_USU, GETDATE(), GETDATE());
```

- [ ] **Step 4: Verificar que la fila de prueba se guardó correctamente**

```sql
SELECT idbodega_user, user_id, usuario_desktop_id, bodega_id
FROM dbo.bodega_user
WHERE usuario_desktop_id IS NOT NULL;
```

Resultado esperado: 1 fila con `user_id = NULL` y `usuario_desktop_id = @ID_USU`.

---

## Task 2: Modificar `Forms/LogIn.vb` — consulta de báscula post-login

**Files:**
- Modify: `Forms/LogIn.vb` (método `ValidaClave`, líneas 108–114)

Contexto: `LibreriaGeneral.nBascu` es `String` (valor actual: `"0"`, cargado desde ConfigSQL.xml como `"01"` en `DatosGenerales`). El campo `bascula_bas` en la tabla `Basculas` contiene el código de báscula ("01", "02", etc.) que es lo que `nBascu` espera.

La variable `id_usu` ya está disponible como campo privado en la clase `Login` (línea 13 de LogIn.vb), y se asigna en el bucle de credenciales (línea 87).

El bloque actual a modificar (líneas 95–114):

```vb
            Try

                dTable2 = Datos.consulta_reader("SELECT * FROM Empresa WHERE id_emp='" & LibreriaGeneral.gEmpresa & "'")

                If dTable2.Rows.Count = 0 Then
                    MessageBox.Show("❌ La Empresa no esta Registrada!!!!")
                    Me.Close()
                Else
                    'Setea Campos
                    For Each dRow As DataRow In dTable2.Rows
                        LibreriaGeneral.nBodega = dRow.Item("id_bod").ToString

                    Next
                End If
            Catch ex As Exception

            End Try

            _conectado = True
            Me.Close()
```

- [ ] **Step 1: Agregar la consulta de báscula después del bloque Try/Catch de empresa**

Reemplazar el bloque completo desde `Try` hasta `Me.Close()` (líneas 95–114) con el siguiente código:

```vb
            Try

                dTable2 = Datos.consulta_reader("SELECT * FROM Empresa WHERE id_emp='" & LibreriaGeneral.gEmpresa & "'")

                If dTable2.Rows.Count = 0 Then
                    MessageBox.Show("❌ La Empresa no esta Registrada!!!!")
                    Me.Close()
                Else
                    'Setea Campos
                    For Each dRow As DataRow In dTable2.Rows
                        LibreriaGeneral.nBodega = dRow.Item("id_bod").ToString

                    Next
                End If
            Catch ex As Exception

            End Try

            ' Obtener báscula asignada al usuario desde BD
            Try
                Dim sqlBascula As String =
                    "SELECT TOP 1 b.bascula_bas " &
                    "FROM dbo.bodega_user bu " &
                    "INNER JOIN dbo.bascula_bodega bb ON bb.bodega_id = bu.bodega_id " &
                    "INNER JOIN dbo.Basculas b ON b.Id_bas = bb.bascula_id " &
                    "WHERE bu.usuario_desktop_id = " & id_usu &
                    " AND bb.activo = 1 " &
                    "ORDER BY bu.idbodega_user ASC, bb.idbascula_bodega ASC"

                Dim dtBascula As DataTable = Datos.consulta_reader(sqlBascula)

                If dtBascula IsNot Nothing AndAlso dtBascula.Rows.Count > 0 Then
                    LibreriaGeneral.nBascu = dtBascula.Rows(0)("bascula_bas").ToString().Trim()
                End If
                ' Si no hay resultado, nBascu conserva el valor de ConfigSQL.xml (fallback)
            Catch ex As Exception
                ' Error no crítico: seguir con el valor de ConfigSQL.xml
            End Try

            _conectado = True
            Me.Close()
```

- [ ] **Step 2: Compilar el proyecto para verificar que no hay errores de sintaxis**

En Visual Studio: `Build → Build Solution` (o `Ctrl+Shift+B`).

Resultado esperado: `Build succeeded` sin errores. Advertencias son aceptables.

- [ ] **Step 3: Commit del cambio**

```bash
git add "Forms/LogIn.vb"
git commit -m "feat: asignar bascula desde BD al login segun usuario desktop"
```

---

## Task 3: Verificación end-to-end

- [ ] **Step 1: Verificar usuario CON asignación**

1. Ejecutar la app (`F5` en Visual Studio o desde `bin/Debug/AperBascu.exe`)
2. Iniciar sesión con el usuario desktop al que se le insertó la fila en Task 1, Step 3
3. Abrir el formulario principal, luego `frmPesaje` (o el form que use `LibreriaGeneral.nBascu`)
4. Confirmar que la báscula cargada corresponde a la asignada en `bascula_bodega`

Para verificar el valor de `nBascu` sin abrir `frmPesaje`, agregar temporalmente en `frmPesaje_Load` o usar el debugger para inspeccionar `LibreriaGeneral.nBascu` inmediatamente después del login.

- [ ] **Step 2: Verificar usuario SIN asignación**

1. Iniciar sesión con un usuario que NO tenga fila en `bodega_user.usuario_desktop_id`
2. Confirmar que la app abre normalmente
3. Confirmar que `LibreriaGeneral.nBascu` tiene el valor de `ConfigSQL.xml` (por defecto `"01"`)

Verificar el valor en `ConfigSQL.xml`:

```xml
<!-- Ruta: bin/Debug/ConfigSQL.xml -->
<add key="Bascula" value="01"/>
```

El `nBascu` debe coincidir con ese valor.

- [ ] **Step 3: Eliminar la fila de prueba si no se necesita en producción**

```sql
DELETE FROM dbo.bodega_user
WHERE usuario_desktop_id = @ID_USU AND bodega_id = @ID_BOD;
```

(Solo si la fila de prueba del Task 1 Step 3 no debe quedar en producción.)

---

## Notas para el administrador web (DotVVM)

Para asignar una báscula a un usuario desktop desde el portal web, el flujo es:

1. Insertar fila en `bodega_user` con `user_id = NULL` y `usuario_desktop_id = [id_usu del Usuario desktop]` y `bodega_id = [Id_bod]`
2. Asegurarse que esa bodega tenga al menos una báscula activa en `bascula_bodega` (`activo = 1`)
3. El desktop usará la primera báscula activa (`ORDER BY idbodega_user ASC, idbascula_bodega ASC`)
