using ApiDatalife.Config.DBContext;
using ApiDatalife.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiDatalife.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class LoginController : ControllerBase
    {

       

        private readonly IConfiguration _config;
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IUserConnectionManager _userConnectionManager;

        public LoginController(IConfiguration config,
        IDbContextFactory dbContextFactory,
        IConnectionStringProvider connectionStringProvider,
        IUserConnectionManager userConnectionManager)
        {
            _config = config;
            _dbContextFactory = dbContextFactory;
            _connectionStringProvider = connectionStringProvider;
            _userConnectionManager = userConnectionManager;

            // Verifica si el modelo ya está configurado antes de configurarlo
            /*  if (!_context.IsEntityTypeRegistered(typeof(LoginRequest)))
              {
                  ConfigureLoginModel();
              }*/

        }

        private void ConfigureLoginModel()
        {
            // Configuración del modelo de Login si es necesario
            // _context.RegisterEntityType<LoginRequest>();
        }




        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequest loginRequest)
        {
            try
            {
                string usuario = loginRequest.username;
                string password = loginRequest.password;
                string baseDatos = loginRequest.baseDatos;

                Boolean usuarioValidado = false;

                // Obtener la cadena de conexión adecuada
                string connectionString = _connectionStringProvider.GetConnectionString(baseDatos);


                // Crear un nuevo contexto con la conexión específica
                using var context = _dbContextFactory.CreateDbContext(connectionString);


               

                var seguridad = new Seguridad();

                string Mensaje = "Inicial";

                int a = await seguridad.ValidarUsuarioSP(usuario, password, baseDatos, context);
                Mensaje = Mensaje + " Validar Usuario ";


                int valor =a;
                if (valor == 0 || valor == 1)
                {
                    Mensaje = Mensaje + " true o false ";
                    usuarioValidado = Convert.ToBoolean(valor);
                }
                else
                    usuarioValidado = false;



                if (usuarioValidado)
                {

                    Mensaje = Mensaje + " *********** Validado ";

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                    var claims = new[]
                                {
                                    new Claim(ClaimTypes.Name, usuario) // Esto es importante para obtener el usuario y pasarlo al middleware
                                   
                                };

                    var Sectoken = new JwtSecurityToken(
                        _config["Jwt:Issuer"],
                        _config["Jwt:Issuer"],
                        claims,
                        // expires: DateTime.Now.AddMinutes(1),
                        expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["Jwt:ExpireMinutes"])),
                        signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                    // Guardar la conexión para este usuario
                    _userConnectionManager.SetUserConnection(usuario, connectionString);

                    return Ok(token);
                }
                else
                {
                    return Unauthorized("Credenciales inválidas");
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }

     





    }
