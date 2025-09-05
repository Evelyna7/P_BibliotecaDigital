using BibliotecaDigital.Data;
using BibliotecaDigital.Services;
using BibliotecaDigital.UI;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "server=localhost;database=biblioteca_digital;user=root;password=;";

        var context = new BibliotecaContext(connectionString);
        var libroService = new LibroService(context);
        var prestamoService = new PrestamoService(context);
        var menu = new Menu(libroService, prestamoService);

        menu.Mostrar();
    }
}