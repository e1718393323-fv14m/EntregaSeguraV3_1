using EntregaSegura.Services;

namespace EntregaSegura.Views;

public partial class vLogin : ContentPage
{
    public vLogin()
    {
        InitializeComponent();

        txtCorreo.Text = Preferences.Get("correo", "");
        chkRecordar.IsChecked = txtCorreo.Text != "";
    }

    private void chkMostrar_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        txtContrasena.IsPassword = !e.Value;
    }

    // Presionar "Enter" en la contraseña también inicia sesión
    private void txtContrasena_Completed(object sender, EventArgs e) => btnIngresar_Clicked(sender, e);

    private async void btnIngresar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string correo = txtCorreo.Text?.Trim() ?? "";
            string contrasena = txtContrasena.Text ?? "";

            if (correo == "" || contrasena == "")
            {
                await DisplayAlert("Aviso", "Ingrese su correo y su contraseña", "Aceptar");
                return;
            }
            if (!correo.Contains('@'))
            {
                await DisplayAlert("Aviso", "Ingrese un correo válido", "Aceptar");
                return;
            }

            btnIngresar.IsEnabled = false;
            indicador.IsRunning = true;

            bool correcto = await SupabaseService.IniciarSesion(correo, contrasena);

            if (correcto)
            {
                Models.Sesion.Conductor = await SupabaseService.ObtenerPerfil();
                if (Models.Sesion.Conductor == null)
                {
                    await DisplayAlert("Aviso", "Su usuario no está registrado en la tabla conductores", "Cerrar");
                    Models.Sesion.Cerrar();
                    return;
                }

                Preferences.Set("correo", chkRecordar.IsChecked ? correo : "");
                txtContrasena.Text = "";

                if (Models.Sesion.Conductor.EsSupervisor)
                    await Navigation.PushAsync(new vSupervisor());
                else
                    await Navigation.PushAsync(new vPrincipal());
            }
            else
            {
                await DisplayAlert("ERROR", "Usuario/Contraseña incorrectos", "Cerrar");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", "No se pudo conectar: " + ex.Message, "Cerrar");
        }
        finally
        {
            btnIngresar.IsEnabled = true;
            indicador.IsRunning = false;
        }
    }
}
