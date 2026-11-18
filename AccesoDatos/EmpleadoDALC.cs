using ApiDatalife.Config.Components;
using ApiDatalife.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace ApiDatalife.AccesoDatos
{
    public class EmpleadoDALC
    {


        /*
        public async Task <List<Empleado>> consultarEmpleados(DynamicDbContext context,string cedula,string compania)
        {

            var empleados = new List<Empleado>();
            try
            {

                using var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();

                using var command = connection.CreateCommand();

                if (string.IsNullOrEmpty(cedula) && string.IsNullOrEmpty(compania))
                {
                    command.CommandText = "SELECT * FROM VI_EMPLEADO_QPH ORDER BY APELLIDOS";
                }
                else
                {
                    if (string.IsNullOrEmpty(cedula))
                    {
                        command.CommandText = "SELECT * FROM VI_EMPLEADO_QPH WHERE EMPRESA = @compania ORDER BY APELLIDOS";
                        command.Parameters.Add(new SqlParameter("@compania", string.IsNullOrEmpty(compania) ? DBNull.Value : compania));
                    }
                    else if (string.IsNullOrEmpty(compania))
                    {
                        command.CommandText = "SELECT * FROM VI_EMPLEADO_QPH WHERE CEDULA = @cedula ORDER BY APELLIDOS";
                        command.Parameters.Add(new SqlParameter("@cedula", string.IsNullOrEmpty(cedula) ? DBNull.Value : cedula));
                    }
                    else
                    {
                        command.CommandText = "SELECT * FROM VI_EMPLEADO_QPH WHERE CEDULA = @cedula AND EMPRESA = @compania ORDER BY APELLIDOS";
                        command.Parameters.Add(new SqlParameter("@cedula", string.IsNullOrEmpty(cedula) ? DBNull.Value : cedula));
                        command.Parameters.Add(new SqlParameter("@compania", string.IsNullOrEmpty(compania) ? DBNull.Value : compania));
                    }
                }

                using var reader = await command.ExecuteReaderAsync();


                while (await reader.ReadAsync())
                {
                    var empleado = new Empleado
                    {
                        nombres = reader.IsDBNull("NOMBRES") ? "*" : reader.GetString("NOMBRES"),
                        apellidos = reader.IsDBNull("APELLIDOS") ? "*" : reader.GetString("APELLIDOS"),
                        cedula = reader.IsDBNull("CEDULA") ? "*" : reader.GetString("CEDULA"),
                        celular = reader.IsDBNull("CELULAR") ? "*" : reader.GetString("CELULAR"),
                        fecha_ingreso = reader.IsDBNull("FECHA_INGRESO") ? "01/01/1900" : reader.GetDateTime("FECHA_INGRESO").ToString("dd/MM/yyyy"),
                        fecha_nacimiento = reader.IsDBNull("FECHA_NACIMIENTO") ? "01/01/1900" : reader.GetDateTime("FECHA_NACIMIENTO").ToString("dd/MM/yyyy"),
                        fecha_salida = reader.IsDBNull("FECHA_SALIDA") ? "01/01/1900" : reader.GetDateTime("FECHA_SALIDA").ToString("dd/MM/yyyy"),
                        email_personal = reader.IsDBNull("EMAIL_PERSONAL") ? "*" : reader.GetString("EMAIL_PERSONAL"),
                        email_corporativo = reader.IsDBNull("EMAIL_CORPORATIVO") ? "*" : reader.GetString("EMAIL_CORPORATIVO"),
                        cedulajefe = reader.IsDBNull("CEDULAJEFE") ? "*" : reader.GetString("CEDULAJEFE"),
                        jefe = reader.IsDBNull("JEFE") ? "*" : reader.GetString("JEFE"),
                        email_corp_jefe = reader.IsDBNull("EMAIL_CORP_JEFE") ? "*" : reader.GetString("EMAIL_CORP_JEFE"),
                        pais = reader.IsDBNull("PAIS") ? "*" : reader.GetString("PAIS"),
                        cod_ciudad = reader.IsDBNull("COD_CIUDAD") ? "*" : reader.GetString("COD_CIUDAD"),
                        empresa = reader.IsDBNull("EMPRESA") ? "*" : reader.GetString("EMPRESA"),
                        tipo_ingreso = "*",
                        tipo_contrato = reader.IsDBNull("TIPO_CONTRATO") ? "*" : reader.GetString("TIPO_CONTRATO"),
                        departamento = reader.IsDBNull("DEPARTAMENTO") ? "*" : reader.GetString("DEPARTAMENTO"),
                        area = reader.IsDBNull("AREA") ? "*" : reader.GetString("AREA"),
                        cargo = reader.IsDBNull("CARGO") ? "*" : reader.GetString("CARGO"),
                        sueldo = reader.IsDBNull("SUELDO") ? "0" : reader.GetDecimal("SUELDO").ToString()
                    };
                    empleados.Add(empleado);
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al consultar vacaciones: {ex.Message}");
                throw;
            }

            return empleados;
        }
        */
        public async Task<MensajesConf> ProcesarEmpleadosNuevos(DynamicDbContext context, EmpleadoIng empleado, string username)
        {
            try
            {
                if (empleado == null)
                return new MensajesConf { codigoTransaccion="*", codigoRetorno = -100, mensajeRetorno = "Empleado no puede ser nulo" };

            var msg = new MensajesConf();
            var parametros = new[] {

                #region Datos Generales

                new SqlParameter("@i_CODTRANS", SqlDbType.VarChar) { Value = empleado.codigoTransaccion.ToUpper() },
                new SqlParameter("@i_CEDCIU", SqlDbType.VarChar) { Value = empleado.identificacion.ToUpper() },
                new SqlParameter("@i_APELLI", SqlDbType.VarChar) { Value = empleado.apellidos.ToUpper() },
                new SqlParameter("@i_NOMBRE", SqlDbType.VarChar) { Value = empleado.nombres.ToUpper() },
                new SqlParameter("@i_FECNAC", SqlDbType.DateTime) { Value = empleado.fechaNacimiento },
                new SqlParameter("@i_SEXTRA", SqlDbType.VarChar) { Value = empleado.idGenero.ToUpper() },
                new SqlParameter("@i_ESTCIV", SqlDbType.VarChar) { Value = empleado.estadoCivil.ToUpper() },
                new SqlParameter("@i_ESTCIV", SqlDbType.VarChar) { Value = empleado.estadoCivil.ToUpper() },
                new SqlParameter("@i_DIRTRA", SqlDbType.VarChar) { Value = empleado.direccion.ToUpper() },
                new SqlParameter("@i_BARRIO", SqlDbType.VarChar) { Value = empleado.sectorDireccion.ToUpper()},
                new SqlParameter("@i_NACTRA", SqlDbType.VarChar) { Value = empleado.pais .ToUpper()},
                new SqlParameter("@i_PRFTRA", SqlDbType.VarChar) { Value = empleado.profesion.ToUpper() },
                new SqlParameter("@i_NUMSOC", SqlDbType.VarChar) { Value = empleado.celular.ToUpper() },
                new SqlParameter("@i_TLFTRA", SqlDbType.VarChar) { Value = empleado.telefono.ToUpper() }, 
                new SqlParameter("@i_FECING", SqlDbType.DateTime) { Value = empleado.fechaIngreso },
                new SqlParameter("@i_TIPTRA", SqlDbType.VarChar) { Value = empleado.tipoTrabajador.ToUpper() },
                new SqlParameter("@i_TIPCTT", SqlDbType.VarChar) { Value = empleado.tipoContrato.ToUpper() },
                new SqlParameter("@i_CGOREF", SqlDbType.VarChar) { Value = empleado.porcentajeDiscapacidad.ToUpper() },  
                new SqlParameter("@i_TLFREF", SqlDbType.VarChar) { Value = empleado.tipoDiscapacidad.ToUpper()},
                new SqlParameter("@i_TIPSAN", SqlDbType.VarChar) { Value = empleado.TipoSangre.ToUpper()},


                #endregion Datos Generales

                // new SqlParameter("@i_NOMTRA", SqlDbType.VarChar) { Value = "*" },  // Valor por defecto
                //  

                #region Estructura Organizacional
                new SqlParameter("@i_CODCGO", SqlDbType.VarChar) { Value = empleado.cargo.ToUpper() },
                new SqlParameter("@i_CODREG", SqlDbType.VarChar) { Value = empleado.departamento.ToUpper() },
                new SqlParameter("@i_CODOFC", SqlDbType.VarChar) { Value = empleado.oficina.ToUpper() },  
                new SqlParameter("@i_CODDEP", SqlDbType.VarChar) { Value = empleado.centro_costos.ToUpper() },
                new SqlParameter("@i_CODARE", SqlDbType.VarChar) { Value = empleado.area.ToUpper() },
                new SqlParameter("@i_CODSAR", SqlDbType.VarChar) { Value = empleado.subarea.ToUpper()},
                new SqlParameter("@i_CODCEN", SqlDbType.VarChar) { Value = empleado.centro.ToUpper()},
                new SqlParameter("@i_CODSCE", SqlDbType.VarChar) { Value = empleado.subcentro.ToUpper() },
                new SqlParameter("@i_CODCON", SqlDbType.VarChar) { Value = empleado.nivelEstudios.ToUpper() },
                new SqlParameter("@i_CODTUR", SqlDbType.VarChar) { Value = empleado.codigoTurno.ToUpper() },

                #endregion Estructura Organizacional

                #region Rubros del trabajador
                new SqlParameter("@i_SUEBAS", SqlDbType.Decimal) { Value = empleado.sueldo },

                #endregion Rubros del trabajador

                #region Condiciones de trabajo

                new SqlParameter("@i_NOMCIU", SqlDbType.VarChar) { Value = empleado.ciudad.ToUpper() },  
                new SqlParameter("@i_CODPAR", SqlDbType.VarChar) { Value = empleado.parroquia.ToUpper() },
                new SqlParameter("@i_NUMCTT", SqlDbType.VarChar) { Value = empleado.NroContrato.ToUpper() },
                new SqlParameter("@i_FECCTT", SqlDbType.VarChar) { Value = empleado.fechaContrato },

                #endregion Condiciones de trabajo

                #region Datos Forma de Pago

                 new SqlParameter("@i_PACMPE", SqlDbType.VarChar) { Value = empleado.D14.ToUpper() },
                 new SqlParameter("@i_COFOPA", SqlDbType.VarChar) { Value = empleado.formaPago.ToUpper() },
                 new SqlParameter("@i_COBATR", SqlDbType.VarChar) { Value = empleado.codigoBanco.ToUpper() },
                 new SqlParameter("@i_TIPCTA", SqlDbType.VarChar) { Value = empleado.tipoCuenta.ToUpper() },
                 new SqlParameter("@i_NUMCTA", SqlDbType.VarChar) { Value = empleado.numeroCuenta.ToUpper() },
                 new SqlParameter("@i_MAILTRA", SqlDbType.VarChar) { Value = empleado.mailCorporativo.ToUpper() },
                 new SqlParameter("@i_MAILJEF", SqlDbType.VarChar) { Value = empleado.mailPersonal.ToUpper() },


                #endregion Datos Forma de Pago

                #region Contactos de Emergencia

                 new SqlParameter("@i_NOMPAR", SqlDbType.VarChar) { Value = empleado.nombreContactoEmergencia.ToUpper() },
                 new SqlParameter("@i_PARENT", SqlDbType.VarChar) { Value = empleado.parentescoContactoEmergencia.ToUpper() },
                 new SqlParameter("@i_DIRPAR", SqlDbType.VarChar) { Value = empleado.direccionContactoEmergencia.ToUpper() }, 
                 new SqlParameter("@i_TLFPAR", SqlDbType.VarChar) { Value = empleado.telefonoContactoEmergencia.ToUpper() },


                #endregion Contactos de Emergencia


                #region Nivel de Jerarquia

                new SqlParameter("@i_NOMREF", SqlDbType.VarChar) { Value = empleado.jefe.ToUpper() },
                new SqlParameter("@i_EMPREF", SqlDbType.VarChar) { Value = empleado.supervisor.ToUpper() },
                new SqlParameter("@i_INSTRU", SqlDbType.VarChar) { Value = empleado.tipoCargo.ToUpper() },

                #endregion Nivel de Jerarquia


                #region Ubicación usuario

                new SqlParameter("@i_CODCIA", SqlDbType.VarChar) { Value = empleado.compania.ToUpper() },
                new SqlParameter("@i_AS_USUARIO", SqlDbType.VarChar) { Value = username.ToUpper() }

                #endregion Ubicación usuario
            };

                var results = await context.Set<SpResult>()
                    .FromSqlRaw(
                        "EXEC spnp_ingresarTrabajadoresNuevosChevy " +
                        "@i_CODTRANS, @i_NOMBRE, @i_APELLI, @i_CODCGO,@i_FECING," +
                        " @i_CODARE, @i_NUMSOC, @i_TIPCTT, @i_SEXTRA, @i_FQNFIR,"+
                        " @i_CEDCIU, @i_TIPAFI, @i_NACTRA,@i_CODDEP,@i_SUEBAS,@i_MAILTRA," +
                        "@i_MAILJEF,@i_NOMREF,@i_EMPREF,@i_CODCIA, @i_AS_USUARIO,@i_CODCEN,"+
                        "@i_TIPIDENT, @i_TIPOPOS",
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
                    codigoTransaccion ="*",
                    
                    codigoRetorno = sqlEx.Number,
                    mensajeRetorno = $"Error SQL: {sqlEx.Message}"
                };
            }




        }



       

        public async Task<MensajesConf> RegistrarFechaSalidaEmpleados(DynamicDbContext context, EmpleadoIng empleado, string username)
        {


            try
            {
                if (empleado == null)
                    return new MensajesConf { codigoTransaccion = "*", codigoRetorno = -100, mensajeRetorno = "Acción de salida no puede ser nula" };

                var msg = new MensajesConf();



                var parametros = new[] {
                     new SqlParameter("@i_CODTRANS", SqlDbType.VarChar) { Value = empleado.codigoTransaccion.ToUpper() },
                     new SqlParameter("@i_BANDER", SqlDbType.Int) { Value = 1 },
                     new SqlParameter("@a_CODTRA", SqlDbType.VarChar) { Value = empleado.cedulaTrabajador.ToUpper() },
                     new SqlParameter("@a_CODNOV", SqlDbType.VarChar) { Value = "SAL"},
                     new SqlParameter("@b_FECINV", SqlDbType.DateTime) { Value = empleado.fechaIngreso },
                     new SqlParameter("@i_CODTRA", SqlDbType.VarChar) { Value = empleado.cedulaTrabajador.ToUpper() },
                     new SqlParameter("@i_CODNOV", SqlDbType.VarChar) { Value = "SAL" },
                     new SqlParameter("@j_FECINV", SqlDbType.DateTime) { Value = empleado.fechaIngreso },
                     new SqlParameter("@j_FECFIV", SqlDbType.DateTime) { Value = empleado.fechaSalida },
                     new SqlParameter("@j_NOTNOV", SqlDbType.VarChar) { Value = "*"},
                     new SqlParameter("@i_AS_USUARIO", SqlDbType.VarChar) { Value = username.ToUpper() },
                     new SqlParameter("@i_VALOR1", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_VALOR2", SqlDbType.Float) { Value = 0 },
                     new SqlParameter("@i_CAMPO1", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_CAMPO2", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_NUMDOC", SqlDbType.VarChar) { Value = "0" },
                     new SqlParameter("@j_FECREG", SqlDbType.DateTime) { Value = DateTime.Now.ToShortDateString() },
                     new SqlParameter("@j_FECNOT", SqlDbType.DateTime) { Value = DateTime.Now.ToShortDateString() },
                     new SqlParameter("@i_OBSERV", SqlDbType.VarChar) { Value = "*" },
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
                     new SqlParameter("@i_DIANOV", SqlDbType.Decimal) { Value = 0 },
                     new SqlParameter("@i_BANDERCALC", SqlDbType.Decimal) { Value = 0 },
                     new SqlParameter("@i_CODCIA", SqlDbType.VarChar) { Value = empleado.empresa.ToUpper() },
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
                     new SqlParameter("@i_CAUSAL", SqlDbType.VarChar) { Value = empleado.causaSalida.ToUpper() },
                     new SqlParameter("@i_TIPPER", SqlDbType.VarChar) { Value = "*" },
                     new SqlParameter("@i_MOTSAL", SqlDbType.VarChar) { Value = empleado.motivoSalida.ToUpper()  }

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
                    codigoTransaccion = "*",
                    
                    codigoRetorno = sqlEx.Number,
                    mensajeRetorno = $"Error SQL: {sqlEx.Message}"
                };
            }




        }  




        }
}
