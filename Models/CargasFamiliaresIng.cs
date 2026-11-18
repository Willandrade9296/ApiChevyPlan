namespace ApiDatalife.Models
{
    public class CargasFamiliaresIng
    {


        public String codigoTransaccion { get; set; }   //CODTRANS
        public String cedulaTrabajador { get; set; }    //CEDULA

        public String cedulaCarga { get; set; }  //CEDFAM

        public String codigoParentezco { get; set; }  //PARCFA

        public String nombresApellidosCarga { get; set; }  //NOMCFA

        public String codigoGenero { get; set; }  //SEXCFA

        public DateTime fechaNacimientoCarga { get; set; } //FECNAC

        public String conDiscapacidad { get; set; } //TIPAFIF

        public String indUtilidades { get; set; } //CGAUTL

        public String indCargaFamiliar { get; set; } //CGAFAM

        public String indIRE { get; set; } //CGAEST

        public DateTime fechaMuerteCarga { get; set; } //FECMUE

        public String nombreCiudadCarga { get; set; } //CIUFAM

        public String direccionCarga { get; set; } //DIRFAM

        public String cargaVinculo { get; set; }  //VINFAM

        public String gradoFamiliaridad { get; set; } //PORFAM

        public String cargoCarga { get; set; } //CODCGO




    }
}
