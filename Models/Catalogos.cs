namespace EntregaSegura.Models
{
    public class Cliente
    {
        public long id { get; set; }
        public string nombre { get; set; } = "";
        public string? ruc { get; set; }
        public string direccion { get; set; } = "";
        public string? telefono { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
    }

    public class Producto
    {
        public long id { get; set; }
        public string codigo { get; set; } = "";
        public string descripcion { get; set; } = "";
        public decimal precio { get; set; }
    }

    public class DetalleOrden
    {
        public int cantidad { get; set; }
        public Producto producto { get; set; } = new Producto();

        public decimal Subtotal => cantidad * producto.precio;
        public string Texto => $"{cantidad} x {producto.descripcion}";
        public string SubtotalTexto => $"${Subtotal:F2}";
    }

    public class Vehiculo
    {
        public string placa { get; set; } = "";
        public string modelo { get; set; } = "";
        public int capacidad_kg { get; set; }
    }

    public class Rol
    {
        public string nombre { get; set; } = "CONDUCTOR";
    }

    public class Conductor
    {
        public long id { get; set; }
        public string email { get; set; } = "";
        public string nombres { get; set; } = "";
        public string cedula { get; set; } = "";
        public string? telefono { get; set; }
        public Vehiculo? vehiculo { get; set; }
        public Rol? rol { get; set; }

        public bool EsSupervisor => rol?.nombre == "SUPERVISOR";
        public string RolTexto => EsSupervisor ? "Supervisor" : "Conductor";
    }
    public class Novedad
    {
        public long id { get; set; }
        public string descripcion { get; set; } = "";
        public DateTime fecha { get; set; }
        public bool revisada { get; set; }
        public string? respuesta { get; set; }
        public string conductor_email { get; set; } = "";
        public MotivoNovedad motivo { get; set; } = new MotivoNovedad();
        public Orden orden { get; set; } = new Orden();

        public string Titulo => $"{orden.numero} - {orden.cliente.nombre}";
        public string Detalle => $"{motivo.descripcion}: {descripcion}";
        public string Info => $"{conductor_email} · {fecha.ToLocalTime():dd/MM HH:mm}";
        public string EstadoTexto => revisada ? "Respuesta: " + (respuesta ?? "") : "Toque para registrar la revisión";
        public Color ColorEstado => revisada ? Color.FromArgb("#1E7B34") : Color.FromArgb("#B3261E");
        public Color FondoEstado => revisada ? Color.FromArgb("#E3F4E7") : Color.FromArgb("#FCE8E6");
        public string Chip => revisada ? "Revisada" : "Por revisar";
    }
    public class MotivoNovedad
    {
        public long id { get; set; }
        public string descripcion { get; set; } = "";

        public override string ToString() => descripcion;
    }
}
