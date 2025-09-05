using BibliotecaDigital.Models;
using BibliotecaDigital.Services;

namespace BibliotecaDigital.UI
{
    public class Menu
    {
        private readonly LibroService _libroService;
        private readonly PrestamoService _prestamoService;

        public Menu(LibroService libroService, PrestamoService prestamoService)
        {
            _libroService = libroService;
            _prestamoService = prestamoService;
        }

        public void Mostrar()
        {
            int opcion = 0;
            do
            {
                Console.WriteLine("\n--- Biblioteca Digital ---");
                Console.WriteLine("1. Registrar libro");
                Console.WriteLine("2. Listar libros");
                Console.WriteLine("3. Registrar préstamo");
                Console.WriteLine("4. Listar préstamos");
                Console.WriteLine("5. Devolver libro");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                opcion = int.Parse(Console.ReadLine() ?? "0");

                switch (opcion)
                {
                    case 1:
                        Console.Write("Título: ");
                        string titulo = Console.ReadLine()!;
                        Console.Write("Autor: ");
                        string autor = Console.ReadLine()!;
                        _libroService.AgregarLibro(new Libro { Titulo = titulo, Autor = autor });
                        Console.WriteLine("✅ Libro registrado con éxito.");
                        break;

                    case 2:
                        var libros = _libroService.ListarLibros();
                        foreach (var l in libros)
                            Console.WriteLine($"{l.Id} - {l.Titulo} ({l.Autor}) - Disponible: {l.Disponible}");
                        break;

                    case 3:
                        Console.Write("ID libro a prestar: ");
                        int libroId = int.Parse(Console.ReadLine()!);
                        var prestamo = new Prestamo
                        {
                            LibroId = libroId,
                            FechaPrestamo = DateTime.Now,
                            FechaDevolucion = DateTime.Now.AddDays(7)
                        };
                        _prestamoService.RegistrarPrestamo(prestamo);
                        Console.WriteLine("✅ Préstamo registrado.");
                        break;

                    case 4:
                        var prestamos = _prestamoService.ListarPrestamos();
                        foreach (var p in prestamos)
                            Console.WriteLine($"{p.Id} | Libro: {p.LibroId} | Desde: {p.FechaPrestamo:dd/MM/yyyy} hasta {p.FechaDevolucion:dd/MM/yyyy}");
                        break;

                    case 5:
                        Console.Write("ID del préstamo a devolver: ");
                        int idPrestamo = int.Parse(Console.ReadLine()!);
                        _prestamoService.DevolverLibro(idPrestamo);
                        Console.WriteLine("✅ Libro devuelto.");
                        break;
                }

            } while (opcion != 0);
        }
    }
}