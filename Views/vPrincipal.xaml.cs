using EntregaSegura.Models;
using EntregaSegura.Services;

namespace EntregaSegura.Views;

public partial class vPrincipal : ContentPage
{
    public vPrincipal()
    {
        InitializeComponent();
        lblSaludo.Text = "Hola, " + Sesion.Nombre;
        string fecha = DateTime.Now.ToString("dddd, dd 'de' MMMM", new System.Globalization.CultureInfo("es-EC"));
        lblFecha.Text = char.ToUpper(fecha[0]) + fecha[1..];

        var vehiculo = Sesion.Conductor?.vehiculo;
        lblVehiculo.Text = vehiculo == null ? "Sin vehículo asignado" : $"{vehiculo.modelo} · {vehiculo.placa}";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            var pendientes = await SupabaseService.ObtenerOrdenes("PENDIENTE");
            var realizadas = await SupabaseService.ObtenerOrdenes("REALIZADAS");
            lblPendientes.Text = pendientes.Count.ToString();
            lblRealizadas.Text = realizadas.Count.ToString();

            int total = pendientes.Count + realizadas.Count;
            double avance = total == 0 ? 0 : (double)realizadas.Count / total;
            await barAvance.ProgressTo(avance, 600, Easing.CubicOut);
            lblAvance.Text = $"{avance * 100:F0} %";
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", "No se pudo cargar el resumen: " + ex.Message, "Cerrar");
        }
    }

    private async void btnEntregas_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vEntregas());
    }

    private async void btnHistorial_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vHistorial());
    }

    private async void btnPerfil_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vPerfil());
    }

    private async void btnSalir_Clicked(object sender, EventArgs e)
    {
        bool salir = await DisplayAlert("Cerrar sesión", "¿Desea salir de la aplicación?", "Sí", "No");
        if (salir)
        {
            Sesion.Cerrar();
            await Navigation.PopToRootAsync();  
        }
    }
}
