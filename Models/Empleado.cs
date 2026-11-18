
using System.Text;

namespace ApiDatalife.Models
{
    public class Empleado
    {

       public String nombres { get; set; }    //NOMBRE
       public String apellidos { get; set; }  //APELLI

       public String cedula { get; set; }    //CEDCIU

       public String celular { get; set; }    //NUMSOC


       public String fecha_ingreso { get; set; }  //FECING

       public String fecha_nacimiento { get; set; } //FECNAC

       public String fecha_salida { get; set; }   //FECSAL

       public String email_personal { get; set; }   //MAILJEF

       public String email_corporativo { get; set; }     //MAILTRA

       public String cedulajefe { get; set; }    //NOMREF

       public String jefe { get; set; }   //NOMJEF

       public String  email_corp_jefe  { get; set; }    //MAILTRA de subquery

        public String pais { get; set; }    //CEDMIL

        public String cod_ciudad { get; set; }    //NOMCIU

        public String empresa { get; set; }    //CODCIA

        public String tipo_ingreso { get; set; }
        public String tipo_contrato { get; set; }  //TIPCTT

        public String departamento { get; set; }    //NOLDEP

        public String area { get; set; }   //NOLARE
        public String cargo { get; set; }  //NOLCGO

        public String sueldo { get; set; }   //SUEBAS






    }
}
