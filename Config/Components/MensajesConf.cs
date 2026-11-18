using ApiDatalife.Models;
using Microsoft.AspNetCore.Http;

namespace ApiDatalife.Config.Components
{
    public class MensajesConf
    {

       
      //  public bool Success { get; set; }
       
        public string codigoTransaccion { get; set; }
        public int codigoRetorno { get; set; }
        public string mensajeRetorno { get; set; }

       // public string codigoColaborador { get; set; }



        public  MensajesConf mostrarMensajeSP(IEnumerable<SpResult> results)
        {
            var result = results.FirstOrDefault();

            if (result != null)
            {
                codigoTransaccion = result.iCodTran.ToString();
                //Success = true;
                codigoRetorno = result.iRetCode;
                mensajeRetorno = result.sErrMsg;
            }
            else
            {
                codigoTransaccion = "*";
               // Success = false;
                codigoRetorno = -100;
                mensajeRetorno = "No se recibió respuesta del procedimiento";
            }

            return this;
        }
    }
}
