using ApiDatalife.Config.Components;
using ApiDatalife.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;

namespace ApiDatalife.AccesoDatos
{
    public class VacacionesDALC
    {

        public async Task<List<Vacaciones>> consultarVacaciones(DynamicDbContext context, string cedula, string compania)
        {
            var vacaciones = new List<Vacaciones>();

            try
            {

                 using var connection = context.Database.GetDbConnection();
                // 4. Abrir conexión y ejecutar consulta
                await connection.OpenAsync();


                 using var command = connection.CreateCommand();

                if (string.IsNullOrEmpty(cedula) && string.IsNullOrEmpty(compania))
                {
                    command.CommandText = "SELECT * FROM VIPERIODOS_QPH ORDER BY NOMBRE_TRABAJADOR";

                }
                else
                {
                    if (string.IsNullOrEmpty(cedula))
                    {

                        command.CommandText = "SELECT * FROM VIPERIODOS_QPH WHERE  CODIGO_COMPANIA = @compania  ORDER BY NOMBRE_TRABAJADOR";

                        command.Parameters.Add(new SqlParameter("@compania", string.IsNullOrEmpty(compania) ? DBNull.Value : compania));


                    }
                    else if (string.IsNullOrEmpty(compania))
                    {
                        command.CommandText = "SELECT * FROM VIPERIODOS_QPH WHERE  CEDULA_TRABAJADOR = @cedula  ORDER BY NOMBRE_TRABAJADOR";

                        command.Parameters.Add(new SqlParameter("@cedula", string.IsNullOrEmpty(cedula) ? DBNull.Value : cedula));
                    }
                    else
                    {
                        command.CommandText = "SELECT * FROM VIPERIODOS_QPH WHERE CEDULA_TRABAJADOR = @cedula AND CODIGO_COMPANIA = @compania  ORDER BY NOMBRE_TRABAJADOR";

                        command.Parameters.Add(new SqlParameter("@cedula", string.IsNullOrEmpty(cedula) ? DBNull.Value : cedula));
                        command.Parameters.Add(new SqlParameter("@compania", string.IsNullOrEmpty(compania) ? DBNull.Value : compania));

                    }
                }

                 using var reader = await command.ExecuteReaderAsync();




                while (await reader.ReadAsync())
                {
                    var vacacion = new Vacaciones
                    {
                        codigo_trabajador = reader.IsDBNull(reader.GetOrdinal("CODIGO_TRABAJADOR")) ? "*" : reader.GetString(reader.GetOrdinal("CODIGO_TRABAJADOR")),
                        cedula_trabajador = reader.IsDBNull(reader.GetOrdinal("CEDULA_TRABAJADOR")) ? "*" : reader.GetString(reader.GetOrdinal("CEDULA_TRABAJADOR")),
                        nombre_trabajador = reader.IsDBNull(reader.GetOrdinal("NOMBRE_TRABAJADOR")) ? "*" : reader.GetString(reader.GetOrdinal("NOMBRE_TRABAJADOR")),
                        fecha_ingreso = reader.IsDBNull(reader.GetOrdinal("FECHA_INGRESO")) ? "01/01/1900" : reader.GetDateTime(reader.GetOrdinal("FECHA_INGRESO")).ToString("dd/MM/yyyy"),
                        fecha_salida = reader.IsDBNull(reader.GetOrdinal("FECHA_SALIDA")) ? "01/01/1900" : reader.GetDateTime(reader.GetOrdinal("FECHA_SALIDA")).ToString("dd/MM/yyyy"),
                        anio_vacaciones = reader.IsDBNull(reader.GetOrdinal("ANIO_VACACIONES")) ? "*" : reader.GetString(reader.GetOrdinal("ANIO_VACACIONES")),
                        fecha_inicial_periodo = reader.IsDBNull(reader.GetOrdinal("FECHA_INICIAL_PERIODO")) ? "01/01/1900" : reader.GetDateTime(reader.GetOrdinal("FECHA_INICIAL_PERIODO")).ToString("dd/MM/yyyy"),
                        fecha_final_periodo = reader.IsDBNull(reader.GetOrdinal("FECHA_FINAL_PERIODO")) ? "01/01/1900" : reader.GetDateTime(reader.GetOrdinal("FECHA_FINAL_PERIODO")).ToString("dd/MM/yyyy"),
                        fecha_inicio_vac_tomados = reader.IsDBNull(reader.GetOrdinal("FECHA_INICIO_VAC_TOMADOS")) ? "01/01/1900" : reader.GetDateTime(reader.GetOrdinal("FECHA_INICIO_VAC_TOMADOS")).ToString("dd/MM/yyyy"),
                        fecha_fin_vac_tomados = reader.IsDBNull(reader.GetOrdinal("FECHA_FIN_VAC_TOMADOS")) ? "01/01/1900" : reader.GetDateTime(reader.GetOrdinal("FECHA_FIN_VAC_TOMADOS")).ToString("dd/MM/yyyy"),
                        dias_vacaciones_tomados = reader.IsDBNull(reader.GetOrdinal("DIAS_VACACIONES_TOMADOS")) ? "0.00" : reader.GetDecimal(reader.GetOrdinal("DIAS_VACACIONES_TOMADOS")).ToString("0.00", CultureInfo.InvariantCulture),
                        valor_descontado = reader.IsDBNull(reader.GetOrdinal("VALOR_DESCONTADO")) ? "0.00" : reader.GetDecimal(reader.GetOrdinal("VALOR_DESCONTADO")).ToString("0.00", CultureInfo.InvariantCulture),
                        dias_normales = reader.IsDBNull(reader.GetOrdinal("DIAS_NORMALES")) ? "0.00" : reader.GetDecimal(reader.GetOrdinal("DIAS_NORMALES")).ToString("0.00", CultureInfo.InvariantCulture),
                        dias_excedentes = reader.IsDBNull(reader.GetOrdinal("DIAS_EXCEDENTES")) ? "0.00" : reader.GetDecimal(reader.GetOrdinal("DIAS_EXCEDENTES")).ToString("0.00", CultureInfo.InvariantCulture),
                        dias_totales = reader.IsDBNull(reader.GetOrdinal("DIAS_TOTALES")) ? "0.00" : reader.GetDecimal(reader.GetOrdinal("DIAS_TOTALES")).ToString("0.00", CultureInfo.InvariantCulture),
                        dias_tomados = reader.IsDBNull(reader.GetOrdinal("DIAS_TOMADOS")) ? "0.00" : reader.GetDecimal(reader.GetOrdinal("DIAS_TOMADOS")).ToString("0.00", CultureInfo.InvariantCulture),
                        
                        dias_saldo = reader.IsDBNull(reader.GetOrdinal("DIAS_SALDO")) ? "0.00" : reader.GetDecimal(reader.GetOrdinal("DIAS_SALDO")).ToString("0.00", CultureInfo.InvariantCulture),
                        dias_pagados = reader.IsDBNull(reader.GetOrdinal("DIAS_PAGADOS")) ? "0.00" : reader.GetDecimal(reader.GetOrdinal("DIAS_PAGADOS")).ToString("0.00", CultureInfo.InvariantCulture),
                        codigo_compania = reader.IsDBNull(reader.GetOrdinal("CODIGO_COMPANIA")) ? "*" : reader.GetString(reader.GetOrdinal("CODIGO_COMPANIA")),
                        valor_periodo = reader.IsDBNull(reader.GetOrdinal("VALOR_PERIODO")) ? "*" : reader.GetString(reader.GetOrdinal("VALOR_PERIODO")),
                        motivo= reader.IsDBNull(reader.GetOrdinal("MOTIVO")) ? "*" : reader.GetString(reader.GetOrdinal("MOTIVO")),
                        observacion = reader.IsDBNull(reader.GetOrdinal("OBSERVACION")) ? "*" : reader.GetString(reader.GetOrdinal("OBSERVACION")),
                    };
                    vacaciones.Add(vacacion);
                }

                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al consultar vacaciones: {ex.Message}");
                throw;
            }

            return vacaciones;

        }




        public async Task<MensajesConf> IngresarVacaciones(DynamicDbContext context, VacacionesIng vacaciones, string username)
        {

            try {  
            if (vacaciones == null)
                return new MensajesConf { codigoTransaccion = "*", codigoRetorno = -100, mensajeRetorno = "Vacación no puede ser nula" };

            var msg = new MensajesConf();



                var parametros = new[] {
                     new SqlParameter("@i_CODTRANS", SqlDbType.VarChar) { Value = vacaciones.codigoTransaccion.ToUpper() },
                     new SqlParameter("@i_BANDER", SqlDbType.Int) { Value = 1 },
                     new SqlParameter("@a_CODTRA", SqlDbType.VarChar) { Value = vacaciones.cedulaTrabajador.ToUpper() },
                     new SqlParameter("@a_CODNOV", SqlDbType.VarChar) { Value = "VAC"},
                     new SqlParameter("@b_FECINV", SqlDbType.DateTime) { Value = vacaciones.fechaInicialNovedad },
                     new SqlParameter("@i_CODTRA", SqlDbType.VarChar) { Value = vacaciones.cedulaTrabajador },
                     new SqlParameter("@i_CODNOV", SqlDbType.VarChar) { Value = "VAC" },
                     new SqlParameter("@j_FECINV", SqlDbType.DateTime) { Value = vacaciones.fechaInicialNovedad },
                     new SqlParameter("@j_FECFIV", SqlDbType.DateTime) { Value = vacaciones.fechaFinalNovedad },
                     new SqlParameter("@j_NOTNOV", SqlDbType.VarChar) { Value = vacaciones.motivo.ToUpper() },
                     new SqlParameter("@i_AS_USUARIO", SqlDbType.VarChar) { Value = username.ToUpper() },
                     new SqlParameter("@i_VALOR1", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_VALOR2", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO1", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO2", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_NUMDOC", SqlDbType.VarChar) { Value = "0" },
                     new SqlParameter("@j_FECREG", SqlDbType.DateTime) { Value = DateTime.Now.ToShortDateString() },
                     new SqlParameter("@j_FECNOT", SqlDbType.DateTime) { Value = DateTime.Now.ToShortDateString() },
                     new SqlParameter("@i_OBSERV", SqlDbType.VarChar) { Value = vacaciones.observacion.ToUpper() },
                     new SqlParameter("@i_CAMPO3", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO4", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CODREG", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CODOFC", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CODDEP", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_DIARES", SqlDbType.Decimal) { Value = 0 },
                     new SqlParameter("@i_NOTSAL", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_HORTRS", SqlDbType.Decimal) { Value = 0 },
                     new SqlParameter("@i_ACUTRS", SqlDbType.Decimal) { Value = 0 },
                     new SqlParameter("@i_VALTRS", SqlDbType.Decimal) { Value = 0 },
                     new SqlParameter("@i_DIAPAG", SqlDbType.Decimal) { Value = 0 },
                     new SqlParameter("@i_FECINIP", SqlDbType.DateTime) { Value = DateTime.Parse("1900/01/01") },
                     new SqlParameter("@i_FECFINP", SqlDbType.DateTime) { Value = DateTime.Parse("1900/01/01") },
                     new SqlParameter("@i_DIANOV", SqlDbType.Decimal) { Value = vacaciones.dias },
                     new SqlParameter("@i_BANDERCALC", SqlDbType.Decimal) { Value = 0 },
                     new SqlParameter("@i_CODCIA", SqlDbType.VarChar) { Value = vacaciones.codigoCompania.ToUpper() },
                     new SqlParameter("@i_INDICNOV", SqlDbType.VarChar) { Value = "A" },
                     new SqlParameter("@i_CAMPO5", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO6", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO7", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO8", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO9", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO10", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO11", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO12", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO13", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO14", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO15", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO16", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO17", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO18", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO19", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO20", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO21", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO22", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO23", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO24", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO25", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO26", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO27", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO28", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO29", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO30", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO31", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO32", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAUSAL", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_TIPPER", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_MOTSAL", SqlDbType.VarChar) { Value = "*" }

                    };

                var results = await context.Set<SpResult>()
                    .FromSqlRaw(
                        "EXEC spna_notranvdTW_QPH " +
                        "@i_CODTRANS, @i_BANDER, @a_CODTRA, @a_CODNOV, @b_FECINV, @i_CODTRA, @i_CODNOV, " +
                        "@j_FECINV, @j_FECFIV, @j_NOTNOV, @i_AS_USUARIO, @i_VALOR1, @i_VALOR2, @i_CAMPO1, " +
                        "@i_CAMPO2, @i_NUMDOC, @j_FECREG, @j_FECNOT, @i_OBSERV, @i_CAMPO3, @i_CAMPO4, @i_CODREG, " +
                        "@i_CODOFC, @i_CODDEP, @i_DIARES, @i_NOTSAL, @i_HORTRS, @i_ACUTRS, @i_VALTRS,@i_DIAPAG, @i_FECINIP," +
                        " @i_FECFINP, @i_DIANOV, @i_BANDERCALC, @i_CODCIA, @i_INDICNOV, @i_CAMPO5, @i_CAMPO6, @i_CAMPO7," +
                        " @i_CAMPO8, @i_CAMPO9, @i_CAMPO10, @i_CAMPO11, @i_CAMPO12, @i_CAMPO13, @i_CAMPO14, @i_CAMPO15, " +
                        "@i_CAMPO16, @i_CAMPO17, @i_CAMPO18, @i_CAMPO19, @i_CAMPO20, @i_CAMPO21, @i_CAMPO22, @i_CAMPO23, " +
                        "@i_CAMPO24, @i_CAMPO25, @i_CAMPO26, @i_CAMPO27, @i_CAMPO28, @i_CAMPO29, @i_CAMPO30, @i_CAMPO31, " +
                        "@i_CAMPO32, @i_CAUSAL, @i_TIPPER, @i_MOTSAL ",
                        parametros
                    )
                    .AsNoTracking()
                    .ToListAsync();



                return msg.mostrarMensajeSP(results);

            }
            catch (SqlException sqlEx)
            {
                return new MensajesConf
                { 
                    codigoTransaccion="*",
                    
                    codigoRetorno = sqlEx.Number,
                    mensajeRetorno = $"Error SQL: {sqlEx.Message}"
                };
             }

        }






        }
}
