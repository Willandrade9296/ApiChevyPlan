
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ApiDatalife.Models
{
    public class Seguridad
    {

        public string Usuario { get; set; }
        public string Password { get; set; }

        public string NombreBase { get; set; }

        public string Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public string CodificarClaveVB(string cadena)
        {
            Int32 Longitud;
            String Clave = "ADEFBBDFEHCCABDBBDACEFFEFGGAADCEG";
            Int32 Constante = 7;
            String Reducir = "A";
            Int32 MyNumber;
            Int32 Sumar;
            string Letra;
            string Palabra = "";
            String Mayuscula;
            Mayuscula = cadena.ToUpper();
            Longitud = Mayuscula.Length;
            for (int i = 1; i <= Longitud; i++)
            {
                Letra = Mayuscula.Substring(i - 1, 1);
                Sumar = Constante + Asc(Clave.Substring(i - 1, 1)) + i;
                MyNumber = Asc(Letra) + Sumar - Asc(Reducir);
                Palabra = Palabra + Chr(MyNumber);
            }

            return Palabra;

        }

        public static int Asc(string s)
        {
            return Encoding.ASCII.GetBytes(s)[0];
        }

        public static char Chr(int c)
        {
            return Convert.ToChar(c);
        }


        public async Task<Int32> ValidarUsuarioSP(string usuario, string password, string baseDatos, DynamicDbContext context)
        {
            try
            {

                string Clave = CodificarClaveVB(password);
                // Parámetros para el procedimiento almacenado
                var parametros = new[] {
                        new SqlParameter("@i_usuario", SqlDbType.VarChar) { Value = usuario },
                        new SqlParameter("@i_password", SqlDbType.VarChar) { Value = Clave},
                        new SqlParameter("@i_modulo", SqlDbType.VarChar) { Value = "*" },
                        new SqlParameter("@retorno", SqlDbType.Int) { Direction = ParameterDirection.Output }
                    };

                // Ejecutar el procedimiento almacenado
                await context.Database.ExecuteSqlRawAsync(
                    "EXEC ASPNET_VERIFICARUSUARIO @i_usuario, @i_password, @i_modulo, @retorno OUTPUT",
                    parametros);


                return Convert.ToInt32(parametros[3].Value);
            }
            catch (Exception ex)
            {
                // Loggear el error si es necesario
                Console.WriteLine($"Error al validar usuario: {ex.Message}");
                return -100;
            }
        }

    }
}
