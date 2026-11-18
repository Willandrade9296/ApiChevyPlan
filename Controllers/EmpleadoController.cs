using ApiDatalife.AccesoDatos;
using ApiDatalife.Config.Components;
using ApiDatalife.Config.DBContext;
using ApiDatalife.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;



namespace ApiDatalife.Controllers
{


   
    [ApiController]

    [Route("api/[controller]")]

    public class EmpleadoController : ControllerBase
    {

        private readonly StorageSettings _storageSettings;

        private readonly IDbContextFactory _dbContextFactory;
        private readonly IUserConnectionManager _connectionManager;

        public EmpleadoController(
            IDbContextFactory dbContextFactory,
            IUserConnectionManager connectionManager, IOptions<StorageSettings> storageSettings)
        {
            _dbContextFactory = dbContextFactory;
            _connectionManager = connectionManager;
            _storageSettings = storageSettings.Value;


        }



     /*   [Authorize]

        [HttpGet]
        public async Task<IActionResult> Get(string cedula = null, string compania = null)
        {
            try
            {

                #region Validar usuario
                // 1. Obtener el usuario autenticado
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized("Usuario no autenticado");

                // 2. Obtener la cadena de conexión del usuario
                var connectionString = _connectionManager.GetUserConnection(username);
                if (string.IsNullOrEmpty(connectionString))
                    return Unauthorized("No se encontró configuración de BD para el usuario");

                #endregion Validar usuario
                // 3. Crear contexto específico para esta solicitud
                await using var context = _dbContextFactory.CreateDbContext(connectionString);

                var datosEmpleado = new EmpleadoDALC();
                var empleados = await datosEmpleado.consultarEmpleados(context, cedula, compania);


                return Ok(empleados);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener empleados: {ex.Message}");
            }
        }

        */


        [Authorize]
        [HttpPost("Empleado_Nuevo")]
        public async Task<IActionResult> Post([FromBody] List<EmpleadoIng> ingresoE)
        {
            try {

                #region Validar usuario
                // 1. Obtener el usuario autenticado
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized("Usuario no autenticado");

                // 2. Obtener la cadena de conexión del usuario
                var connectionString = _connectionManager.GetUserConnection(username);
                if (string.IsNullOrEmpty(connectionString))
                    return Unauthorized("No se encontró configuración de BD para el usuario");

                #endregion Validar usuario
                // 3. Crear contexto específico para esta solicitud
                await using var context = _dbContextFactory.CreateDbContext(connectionString);
                
                var resultados = new List<MensajesConf>();

                var datosEmpleado = new EmpleadoDALC();

                foreach (var empleado in ingresoE)
                {
                    var resultado = await datosEmpleado.ProcesarEmpleadosNuevos(context, empleado, username);
                    resultados.Add(new MensajesConf
                    {
                        codigoTransaccion = empleado.codigoTransaccion,
                        
                        codigoRetorno = resultado.codigoRetorno,
                        mensajeRetorno = resultado.mensajeRetorno

                         
                    });  
                }


                return Ok(new { Success = true, Resultados = resultados });


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al ingresar Empleado: {ex.Message}");
            }
        }


    /*   
        [Authorize]
        [HttpPost("Imagen")]
        public async Task<IActionResult> UploadImage([FromForm] List<IFormFile> fotos, [FromForm] string datos_Empleado)
        {
            try
            {

                #region Validar usuario
                // 1. Obtener el usuario autenticado
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized("Usuario no autenticado");

                // 2. Obtener la cadena de conexión del usuario
                var connectionString = _connectionManager.GetUserConnection(username);
                if (string.IsNullOrEmpty(connectionString))
                    return Unauthorized("No se encontró configuración de BD para el usuario");

                #endregion Validar usuario

                // Validar que hay al menos un archivo
                if (fotos == null || fotos.Count == 0)
                    return BadRequest("No se enviaron archivos.");

                // Deserializar los datos de empleados
                List<Empleado> empleados;
                try
                {
                    empleados = JsonConvert.DeserializeObject<List<Empleado>>(datos_Empleado);
                }
                catch (JsonException)
                {
                    return BadRequest("Formato JSON inválido para datos_Empleado");
                }

                // Validar que hay datos de empleados
                if (empleados == null || empleados.Count == 0)
                    return BadRequest("No se enviaron datos de empleados.");

                // Validar que coincida el número de archivos con empleados
                if (fotos.Count != empleados.Count)
                    return BadRequest("El número de archivos no coincide con el número de empleados.");

                // Asegurar que el directorio existe
                if (!Directory.Exists(_storageSettings.ImageUploadPath))
                    Directory.CreateDirectory(_storageSettings.ImageUploadPath);

                var results = new List<object>();

                for (int i = 0; i < fotos.Count; i++)
                {
                    var file = fotos[i];
                    var empleado = empleados[i];

                    // Validar extensión
                    var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!_storageSettings.AllowedExtensions.Contains(fileExtension))
                    {
                        results.Add(new
                        {
                            cedula = empleado.cedula,
                            Success = false,
                            Mensaje = $"Extensión no permitida: {fileExtension}"
                        });
                        continue;
                    }

                    // Validar tamaño
                    var maxFileSizeBytes = _storageSettings.MaxFileSizeMB * 1024 * 1024;
                    if (file.Length > maxFileSizeBytes)
                    {
                        results.Add(new
                        {
                            cedula = empleado.cedula,
                            Success = false,
                            Mensaje = $"Archivo excede el tamaño máximo de {_storageSettings.MaxFileSizeMB}MB"
                        });
                        continue;
                    }

                    // Generar nombre único para el archivo
                    var uniqueFileName = $"{empleado.cedula}{fileExtension}";
                    var fullPath = Path.Combine(_storageSettings.ImageUploadPath, uniqueFileName);

                    // Guardar el archivo
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    results.Add(new
                    {
                        cedula = empleado.cedula,
                        Success = true,
                        Mensaje = "Archivo subido correctamente",
                        FileName = uniqueFileName,
                        OriginalName = file.FileName,
                        Path = fullPath,
                        Size = file.Length
                    });
                }

                return Ok(new
                {
                    TotalFiles = fotos.Count,
                    SuccessfulUploads = results.Count(r => (bool)r.GetType().GetProperty("Success").GetValue(r)),
                    FailedUploads = results.Count(r => !(bool)r.GetType().GetProperty("Success").GetValue(r)),
                    Results = results
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar foto: {ex.Message}");
            }
        }
*/

        /*
        [Authorize]
        [HttpPost("Desvinculacion" +
            "" +
            "")]
        public async Task<IActionResult> PostFechaSalida([FromBody] List<EmpleadoIng> ingresoE)
        {
            try
            {  

                #region Validar usuario
                // 1. Obtener el usuario autenticado
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized("Usuario no autenticado");

                // 2. Obtener la cadena de conexión del usuario
                var connectionString = _connectionManager.GetUserConnection(username);
                if (string.IsNullOrEmpty(connectionString))
                    return Unauthorized("No se encontró configuración de BD para el usuario");

                #endregion Validar usuario
                // 3. Crear contexto específico para esta solicitud
                await using var context = _dbContextFactory.CreateDbContext(connectionString);

                var resultados = new List<MensajesConf>();

                var datosEmpleado = new EmpleadoDALC();

                foreach (var empleado in ingresoE)
                {
                    var resultado = await datosEmpleado.RegistrarFechaSalidaEmpleados(context, empleado, username);
                    resultados.Add(new MensajesConf
                    {
                        codigoTransaccion = empleado.codigoTransaccion,

                        codigoRetorno = resultado.codigoRetorno,
                        mensajeRetorno = resultado.mensajeRetorno


                    });
                }


                return Ok(new { Success = true, Resultados = resultados });


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al ingresar Fecha de Salida: {ex.Message}");
            }
        }
         */
    }
















}
