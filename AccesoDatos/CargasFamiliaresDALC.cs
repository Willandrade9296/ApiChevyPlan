using ApiDatalife.Config.Components;
using ApiDatalife.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ApiDatalife.AccesoDatos
{
    public class CargasFamiliaresDALC
    {


        public async Task<MensajesConf> IngresarCargasFamiliares(DynamicDbContext context, CargasFamiliaresIng carga, string username)
        {


            try
            {
                if (carga == null)
                    return new MensajesConf { codigoTransaccion = "*", codigoRetorno = -100, mensajeRetorno = "Carga Familiar no puede ser nula" };

                var msg = new MensajesConf();



                var parametros = new[] {
                     new SqlParameter("@i_CODTRANS", SqlDbType.VarChar) { Value = carga.codigoTransaccion.ToUpper() },
                     new SqlParameter("@i_CEDCIU", SqlDbType.VarChar) { Value = carga.cedulaTrabajador.ToUpper() },
                     new SqlParameter("@i_PARCFA", SqlDbType.VarChar) { Value = carga.codigoParentezco.ToUpper() },
                     new SqlParameter("@i_NOMCFA", SqlDbType.VarChar) { Value = carga.nombresApellidosCarga.ToUpper() },
                     new SqlParameter("@i_FECNAC", SqlDbType.DateTime) { Value = carga.fechaNacimientoCarga },
                     new SqlParameter("@i_FECINIREL", SqlDbType.DateTime) { Value = DateTime.Parse("1900/01/01") },
                     new SqlParameter("@i_FECFINREL", SqlDbType.DateTime) { Value = DateTime.Parse("1900/01/01") },
                     new SqlParameter("@i_CGAUTL", SqlDbType.VarChar) { Value = carga.indUtilidades.ToUpper() },
                     new SqlParameter("@i_CGAFAM", SqlDbType.VarChar) { Value = carga.indCargaFamiliar.ToUpper() },
                     new SqlParameter("@i_CGAEST", SqlDbType.VarChar) { Value = carga.indIRE.ToUpper() },
                     new SqlParameter("@i_SEXCFA", SqlDbType.VarChar) { Value = carga.codigoGenero.ToUpper() },
                     new SqlParameter("@i_ESTTRA", SqlDbType.VarChar) { Value = "S" },
                     new SqlParameter("@i_CEDFAM", SqlDbType.VarChar) { Value = carga.cedulaCarga.ToUpper() },
                     new SqlParameter("@i_DIRFAM", SqlDbType.VarChar) { Value = carga.direccionCarga.ToUpper() },
                     new SqlParameter("@i_CIUFAM", SqlDbType.VarChar) { Value = carga.nombreCiudadCarga.ToUpper() },
                     new SqlParameter("@i_TIPAFIF", SqlDbType.VarChar) { Value = carga.conDiscapacidad.ToUpper() },
                     new SqlParameter("@i_VINFAM", SqlDbType.VarChar) { Value = carga.cargaVinculo.ToUpper() },
                     new SqlParameter("@i_PORFAM", SqlDbType.VarChar) { Value = carga.gradoFamiliaridad.ToUpper() },
                     new SqlParameter("@i_CODCGO", SqlDbType.VarChar) { Value = carga.cargoCarga.ToUpper() },
                     new SqlParameter("@i_AS_USUARIO", SqlDbType.VarChar) { Value = username },
                     new SqlParameter("@i_FECMUE", SqlDbType.DateTime) { Value = carga.fechaMuerteCarga },
                     new SqlParameter("@i_INSCAR", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_TRACAR", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_APOCAR", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_OBSCAR", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_ENFCAR", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_TIPFAM", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_FAMCAR", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_TIPDIS", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_PORDIS", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_EXTCFA", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_ALCCFA", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_VIVEUD", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_NACCAR", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_ESTCAR", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_FECCADSUS", SqlDbType.DateTime) { Value = DateTime.Parse("1900/01/01") }

                    };

                var results = await context.Set<SpResult>()
                    .FromSqlRaw(
                        "EXEC spna_nodatfamsocQPH " +
                        "@i_CODTRANS, @i_CEDCIU, @i_PARCFA ,@i_NOMCFA ,@i_FECNAC , @i_FECINIREL, @i_FECFINREL," +
                        "@i_CGAUTL, @i_CGAFAM , @i_CGAEST , @i_SEXCFA , @i_ESTTRA , @i_CEDFAM , @i_DIRFAM,  " +
                        "@i_CIUFAM, @i_TIPAFIF, @i_VINFAM , @i_PORFAM , @i_CODCGO , @i_AS_USUARIO , @i_FECMUE," +
                        "@i_INSCAR, @i_TRACAR, @i_APOCAR, @i_OBSCAR, @i_ENFCAR, @i_TIPFAM, @i_FAMCAR, @i_TIPDIS," +
                        "@i_PORDIS, @i_EXTCFA, @i_ALCCFA, @i_VIVEUD, @i_NACCAR, @i_ESTCAR, @i_FECCADSUS"
                        ,
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
                    codigoTransaccion = "*",
                   // Success = false,
                    codigoRetorno = sqlEx.Number,
                    mensajeRetorno = $"Error SQL: {sqlEx.Message}"
                };
            }




        }








    }
}
