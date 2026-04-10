# Cambios: Sistema de Prefijo y Correlativo en Boletas

## Contexto
La BD `DbAperBascuCG` fue unificada en una instancia centralizada con múltiples empresas.
Se requirió agregar un prefijo de 3 letras a los números de boleta (ej. `DG1`, `RG1`) para distinguir
por tipo de operación y empresa, sin mostrar el prefijo en la UI — solo en BD.

---

## 1. Cambios en Base de Datos (SQL DDL)

### Tabla `dbo.correlactu` — nuevas columnas

```sql
ALTER TABLE dbo.correlactu ADD prefijo_cor  VARCHAR(3)  NULL;
ALTER TABLE dbo.correlactu ADD tipo_cor     VARCHAR(20) NULL;  -- 'DESPACHO' / 'RECEPCION'
ALTER TABLE dbo.correlactu ADD estatus_cor  VARCHAR(10) NULL;  -- 'activo' / 'inactivo'
```

### Nuevas filas (filas 3 y 4 para RECEPCION por empresa)

```sql
INSERT INTO correlactu (empresa_cor, nticketboleta_cor, prefijo_cor, tipo_cor, estatus_cor)
VALUES (1, 0, 'RG1', 'RECEPCION', 'activo'),
       (2, 0, 'RG2', 'RECEPCION', 'activo');
```

---

## 2. Archivos Modificados

### `Forms/frmPesaje.vb` (formulario principal — solo DESPACHO)

**Variables nuevas (nivel de clase):**
```vb
Private prefijoCor As String = ""
Private _ticketFullDB As String = ""  ' ticket completo con prefijo, para SALIDA
```

**`SumaTicket()`** — lee correlativo tipo DESPACHO:
```vb
Private Sub SumaTicket()
    Dim tipoCorrelativo As String = "DESPACHO"
    dTable = Datos.consulta_reader("SELECT prefijo_cor, CAST(nticketboleta_cor + 1 AS VARCHAR(10)) AS 'Corre'
        FROM correlactu WHERE empresa_cor = " & LibreriaGeneral.idEmpresa &
        " AND tipo_cor = '" & tipoCorrelativo & "' AND estatus_cor = 'activo'")
    If dTable.Rows.Count > 0 Then
        For Each dRow As DataRow In dTable.Rows
            prefijoCor = dRow.Item("prefijo_cor").ToString.Trim()
            txtTicket.Text = dRow.Item("Corre").ToString
        Next
    End If
End Sub
```

**`LimpiarNumero()`** — helper para campos numéricos vacíos (evita SQL inválido):
```vb
Private Function LimpiarNumero(valor As String) As String
    Dim limpio As String = valor.Replace(",", "").Replace(" ", "").Trim()
    If String.IsNullOrEmpty(limpio) Then Return "0"
    Dim dummy As Decimal
    If Not Decimal.TryParse(limpio, NumberStyles.Any,
                            CultureInfo.InvariantCulture, dummy) Then Return "0"
    Return limpio
End Function
```

**`Grabar()` — ENTRADA:**
- Null-check en `cmbClaseCarga.SelectedItem` antes del INSERT
- Ticket = `prefijoCor & txtTicket.Text`
- Campos numéricos usan `LimpiarNumero()` en lugar de `.Replace(",","")`
- Quitada columna `tipo_traslado_pes` (no existe en BD de producción)
- INSERT usa `Datos.EjecutarSQL()` que retorna Boolean y mensaje de error real

**`Grabar()` — SALIDA:**
- `Ticket = If(Not String.IsNullOrEmpty(_ticketFullDB), _ticketFullDB, txtTicket.Text)`
- Mismo null-check y `LimpiarNumero()`
- Mismo `EjecutarSQL()`

**`ControlEntrada()`** — carga ENTRADA para completar SALIDA:
```vb
_ticketFullDB = dRow.Item("ticketboleta_pes").ToString.Trim()
txtTicket.Text = New String(_ticketFullDB.SkipWhile(Function(c) Not Char.IsDigit(c)).ToArray())
```

**UPDATE correlactu** — incrementa solo fila DESPACHO:
```vb
Datos.consulta_non_queryDeta("UPDATE correlactu SET nticketboleta_cor = (...) 
    WHERE empresa_cor = " & LibreriaGeneral.idEmpresa & 
    " AND tipo_cor = 'DESPACHO' AND estatus_cor = 'activo'")
```

**Validación de ticket duplicado** — usa ticket COMPLETO con prefijo para no colisionar
con registros viejos sin prefijo de otras instancias:
```vb
Dim ticketParaValidar As String = prefijoCor & txtTicket.Text
-- WHERE ticketboleta_pes = 'DG118160'
-- AND (empresa_pes IS NULL OR empresa_pes = 'X')
```

**`LeerData(grdData)`** — muestra tickets ENTRADA sin SALIDA:
- Reemplazado `HAVING count = 1` por `NOT EXISTS` (más robusto con registros duplicados de prueba)
- Quita prefijo en display con `CASE WHEN LEFT(...,1) LIKE '[A-Z]' THEN SUBSTRING(...,4,...)`
- Filtra por báscula: `Idbas_entrada_pes = LibreriaGeneral.nBascu`

**Impresión** — envuelta en `Try/Catch`:
```vb
Try
    Datos.ImprBoleta(Ticket, EstaPesando)  ' o Imprime_Ticket
Catch ex As Exception
    MessageBox.Show("El pesaje se guardó correctamente, pero ocurrió un error al imprimir: " & ex.Message, ...)
End Try
```

**Consultas de búsqueda/validación** — en todas se usa CASE WHEN para comparar número sin prefijo:
```sql
CASE WHEN LEFT(ticketboleta_pes,1) LIKE '[A-Z]'
     THEN SUBSTRING(ticketboleta_pes, 4, LEN(ticketboleta_pes))
     ELSE ticketboleta_pes
END = '<txtTicket.Text>'
```

---

### `Forms/frmPesadoEspecial.vb` (siempre RECEPCION)

- `SumaTicket()` usa `tipo_cor = 'RECEPCION'`
- UPDATE correlactu usa `tipo_cor = 'RECEPCION'`
- ENTRADA: `Ticket = prefijoCor & txtTicket.Text`

---

### `Forms/frmCorrealtivo.vb` (admin de correlativos)

- Variable `TipoCorActual As String` para capturar tipo al cargar
- `Consulta()` carga `prefijo_cor`, `tipo_cor`, `estatus_cor` en nuevos controles
- `Grabar()` UPDATE incluye `prefijo_cor`, `estatus_cor`; WHERE incluye `AND tipo_cor = TipoCorActual`
- `ValidaCampos()` valida que `txtPrefijo` y `txtEstatus` no estén vacíos

---

### `Forms/frmCorrealtivo.Designer.vb`

- Controles nuevos: `txtPrefijo` (MaxLength=3), `txtTipo` (ReadOnly, fondo gris), `txtEstatus`
- Labels: `LblPrefijo`, `LblTipo`, `LblEstatus`
- GroupBox2 alto: 127 → 192; GroupBox3 movido y2: 186 → 249
- Form ClientSize: (409,317) → (409,385)

---

### `Forms/frmAnulaBoleta.vb`, `frmConsultaBoleta.vb`, `frmConsultaBoletaII.vb`, `frmPesajeModifica.vb`

En todos: SUBSTRING/CASE WHEN para quitar prefijo en SELECT y WHERE de búsqueda.

---

### `Librerias/DatosGenerales.vb`

**Nueva función `EjecutarSQL`:**
```vb
Public Function EjecutarSQL(ByVal consulta As String, ByRef mensajeError As String) As Boolean
    ' Ejecuta INSERT/UPDATE/DELETE
    ' Devuelve True si tuvo éxito, False + mensajeError si falló
    ' Finally garantiza que la conexión siempre se cierra
End Function
```

---

## 3. Bugs Resueltos

| Bug | Causa | Fix |
|-----|-------|-----|
| Ticket siempre muestra "1" | `SumaTicket()` no filtraba por empresa ni tipo | Agregado `WHERE empresa_cor = X AND tipo_cor = 'DESPACHO'` |
| `ArgumentOutOfRangeException` línea 2031 | `ORDER BY CONVERT(int, ticketboleta_pes)` falla con "DG1..." | Cambiado a `TRY_CONVERT(BIGINT, SUBSTRING(...))` |
| Correlativo no incrementaba | UPDATE apuntaba a `tipo_cor='RECEPCION'` en frmPesaje | Cambiado a `tipo_cor='DESPACHO'` |
| SALIDA se guardaba sin prefijo | `Ticket = txtTicket.Text` (solo número) | Agregado `_ticketFullDB`; SALIDA usa el ticket completo de BD |
| Validación no detectaba ENTRADA existente | `WHERE ticketboleta_pes = '16697'` vs BD tiene `'DG116697'` | Cambiado a SUBSTRING/CASE WHEN en WHERE |
| ENTRADA no aparecía en `grdData` | `HAVING count = 1` excluía tickets con registros de prueba duplicados | Reemplazado por `NOT EXISTS` buscando SALIDA |
| Grid mostraba primer dígito del prefijo | `PATINDEX('%[0-9]%',...)` detecta "1" en "DG**1**16697" | Cambiado a `CASE WHEN LEFT()='[A-Z]' THEN SUBSTRING(...,4,...)` |
| Error duplicado en BD unificada | Validación comparaba número sin prefijo → "8160" viejo colisionaba con "DG18160" nuevo | Validación ahora usa ticket completo con prefijo; `(empresa_pes IS NULL OR empresa_pes = X)` |
| SQL Error 207: `tipo_traslado_pes` | Columna no existe en BD de producción | Eliminada del INSERT en ambas ramas ENTRADA y SALIDA |
| INSERT fallaba sin mensaje | `consulta_non_query` tragaba excepciones; `Paso="Si"` igual | Reemplazado por `EjecutarSQL()` que retorna Boolean y error real |
| NullReferenceException en `cmbClaseCarga` | `.SelectedItem("id_cla")` sin null-check | Agregado TryCast + fallback a `"0"` |
| Campos numéricos vacíos → SQL inválido | `.Replace(",","")` devuelve `""` | Reemplazado por `LimpiarNumero()` que devuelve `"0"` |
| Error de impresión sin contexto | Sin Try/Catch en impresión | Envuelto en Try/Catch con mensaje distinguiendo "guardó OK pero falló impresión" |

---

## 4. Patrón SQL para quitar prefijo (referencia)

```sql
-- Quitar exactamente 3 chars si empieza con letra (prefijo "DG1", "RG2", etc.)
CASE WHEN LEFT(ticketboleta_pes, 1) LIKE '[A-Z]'
     THEN SUBSTRING(ticketboleta_pes, 4, LEN(ticketboleta_pes))
     ELSE ticketboleta_pes
END AS ticketboleta_pes

-- ORDER BY numérico seguro
ORDER BY TRY_CONVERT(BIGINT,
    CASE WHEN LEFT(ticketboleta_pes, 1) LIKE '[A-Z]'
         THEN SUBSTRING(ticketboleta_pes, 4, LEN(ticketboleta_pes))
         ELSE ticketboleta_pes
    END)
```

---

## 5. Pendiente (no implementado en código)

- SP `UPSERT_PESAJE_COGRANSA_DESDE_XML` — actualizar para usar nuevo sistema de correlativo con `tipo_cor` y `prefijo_cor`
- SP `PROCESAR_INBOX_PESAJE_QUEUE_BY_BOLETA` — sección 2.2 necesita usar `correlactu` con los nuevos campos
