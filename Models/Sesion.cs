namespace EntregaSegura.Models
{
    public static class Sesion
    {
        public static string Token { get; set; } = "";
        public static string UsuarioId { get; set; } = "";
        public static string Correo { get; set; } = "";

        public static Conductor? Conductor { get; set; }

        public static string Nombre => Conductor?.nombres ?? (Correo.Contains('@') ? Correo.Split('@')[0] : Correo);

        public static void Cerrar()
        {
            Token = "";
            UsuarioId = "";
            Correo = "";
            Conductor = null;
        }
    }
}
