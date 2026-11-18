
using Newtonsoft.Json;
using System.Text;

namespace ApiDatalife.Models
{
    public class EmpleadoIng
    {

        public String codigoTransaccion { get; set; }

        public String cedulaTrabajador { get; set; }

        [JsonProperty("nombres")]
        public String nombres { get; set; }    //NOMBRE

        [JsonProperty("apellidos")]
        public String apellidos { get; set; }  //APELLI

        [JsonProperty("identificacion")]
        public String identificacion { get; set; }    //CEDCIU

        [JsonProperty("compania")]
        public String compania { get; set; }    //CODCIA

        [JsonProperty("cargo")]
        public String cargo { get; set; }    //CODCGO

        [JsonProperty("area")]

        public String area { get; set; }    //CODARE

        [JsonProperty("departamento")]
        public String departamento { get; set; }    //CODREG

        [JsonProperty("oficina")]
        public String oficina { get; set; }    //CODOFC

        [JsonProperty("pais")]
        public String pais { get; set; }    //NACTRA como el pais

        [JsonProperty("mailPersonal")]
        public String mailPersonal { get; set; }    //MAILJEF  como mail personal

        [JsonProperty("celular")]
        public String celular { get; set; }  // NUMSOC es numero de celular

        [JsonProperty("mailCorporativo")]
        public String mailCorporativo { get; set; }  // MAILTRA es el email corporativo

        [JsonProperty("fechaIngreso")]
        public DateTime fechaIngreso { get; set; }  //FECING

        [JsonProperty("tipoIdentificacion")]
        public String tipoIdentificacion { get; set; }  // tipo de intentificación para C es cédula y P es pasaporte

        [JsonProperty("idGenero")]
        public String idGenero { get; set; }  //SEXTRA

        [JsonProperty("idPaisOrigen")]
        public String idPaisOrigen { get; set; }  //FQNFIR como Pais de Origen

        [JsonProperty("centro")]
        public String centro { get; set; }  //CODCEN  como centro

        [JsonProperty("centro_costos")]
        public String centro_costos { get; set; }  //CODDEP  como centro de costos

        [JsonProperty("subcentro")]
        public String subcentro { get; set; }  //CODSCE  como sub centro

        [JsonProperty("subare")]
        public String subarea { get; set; }  //CODSAR  como SubArea

        [JsonProperty("nivelEstudios")]
        public String nivelEstudios { get; set; }  //CODCON  como nivel estudios

        [JsonProperty("codigoTurno")]
        public String codigoTurno { get; set; }  //CODTUR  como codigo turno


        [JsonProperty("ciudad")]
        public String ciudad { get; set; }  //NOMCIU  como codigo ciudad

        [JsonProperty("parroquia")]
        public String parroquia { get; set; }  //CODPAR  como codigo parroquia




        [JsonProperty("jefe")]
        public String jefe { get; set; }  //NOMREF  como Cedula del Jefe

        [JsonProperty("supervisor")]
        public String supervisor { get; set; }  // EMPREF cédula de supervidor

        [JsonProperty("tipoCargo")]
        public String tipoCargo { get; set; }  // INSTRU

        [JsonProperty("tipoContrato")]
        public String tipoContrato { get; set; } //TIPCTT


        [JsonProperty("NroContrato")]
        public String NroContrato { get; set; } //NUMCTT

        [JsonProperty("beneficioSeguro")]
        public String beneficioSeguro { get; set; } //TIPAFI es para el seguro medico

        [JsonProperty("sueldo")]
        public Decimal sueldo { get; set; } //SUEBAS 


        [JsonProperty("formaPago")]
        public String formaPago { get; set; } //COFOPA es para forma de pago

        public String direccion  { get; set; } // DIRTRA

        public String D14 { get; set; } // PACMPE

        public DateTime fechaNacimiento { get; set; } //FECNAC

        public DateTime fechaContrato { get; set; } //FECCTT
        public String tipoTrabajador { get; set; } // TIPTRA


        public String porcentajeDiscapacidad { get; set; } // CGOREF

        public String tipoDiscapacidad { get; set; } // TLFREF

        public String sectorDireccion { get; set; } // BARRIO


        public String estadoCivil { get; set; } // ESTCIV


        public String tipoCuenta { get; set; } // TIPCTA


        public String codigoBanco { get; set; } // COBATR


        public String numeroCuenta { get; set; } // NUMCTA

        public String nombreContactoEmergencia { get; set; } //NOMPAR


        public String parentescoContactoEmergencia { get; set; } //PARENT

        public String direccionContactoEmergencia { get; set; } //DIRPAR


        public String telefonoContactoEmergencia { get; set; } //TLFPAR


        public String telefono { get; set; } //TLFTRA

        public String codigoCiudadResidencia { get; set; } //CIUPAR


        public String TipoSangre { get; set; } //TIPSAN


        public String numeroCasa { get; set; } //TIPSAN


        public String tipoPosicion { get; set; }


        public String profesion { get; set; }  //PRFTRA






















        // PARA REGISTRAR SALIDA DEL EMPLEADO

        public DateTime fechaSalida { get; set; }

        public String causaSalida { get; set; }

        public String motivoSalida { get; set; }

        public String empresa {  get; set; }


















    }
}
