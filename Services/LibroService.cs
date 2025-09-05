using BibliotecaDigital.Data;
using BibliotecaDigital.Models;
using MySql.Data.MySqlClient;

namespace BibliotecaDigital.Services
{
    public class LibroService
    {
        private readonly BibliotecaContext _context;

        public LibroService(BibliotecaContext context)
        {
            _context = context;
        }

        public void AgregarLibro(Libro libro)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string query = "INSERT INTO libros (titulo, autor, disponible) VALUES (@titulo, @autor, true)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@titulo", libro.Titulo);
            cmd.Parameters.AddWithValue("@autor", libro.Autor);
            cmd.ExecuteNonQuery();
        }

        public List<Libro> ListarLibros()
        {
            var libros = new List<Libro>();
            using var conn = _context.GetConnection();
            conn.Open();
            string query = "SELECT * FROM libros";
            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                libros.Add(new Libro
                {
                    Id = reader.GetInt32("id"),
                    Titulo = reader.GetString("titulo"),
                    Autor = reader.GetString("autor"),
                    Disponible = reader.GetBoolean("disponible")
                });
            }
            return libros;
        }
    }
}