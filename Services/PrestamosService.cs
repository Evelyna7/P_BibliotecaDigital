using BibliotecaDigital.Data;
using BibliotecaDigital.Models;
using MySql.Data.MySqlClient;

namespace BibliotecaDigital.Services
{
    public class PrestamoService
    {
        private readonly BibliotecaContext _ctx;
        public PrestamoService(BibliotecaContext ctx) => _ctx = ctx;

        // Registrar un préstamo
        public void RegistrarPrestamo(Prestamo prestamo)
        {
            using var conn = _ctx.GetConnection();
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                // 1. Insertar préstamo
                string insert = "INSERT INTO prestamos (libro_id, fecha_prestamo, fecha_devolucion) VALUES (@l,@fp,@fd)";
                using var cmd = new MySqlCommand(insert, conn, tx);
                cmd.Parameters.AddWithValue("@l", prestamo.LibroId);
                cmd.Parameters.AddWithValue("@fp", prestamo.FechaPrestamo);
                cmd.Parameters.AddWithValue("@fd", prestamo.FechaDevolucion);
                cmd.ExecuteNonQuery();

                // 2. Marcar libro como no disponible
                string update = "UPDATE libros SET disponible=false WHERE id=@id";
                using var cmd2 = new MySqlCommand(update, conn, tx);
                cmd2.Parameters.AddWithValue("@id", prestamo.LibroId);
                cmd2.ExecuteNonQuery();

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        // Listar préstamos registrados
        public List<Prestamo> ListarPrestamos()
        {
            var lista = new List<Prestamo>();
            using var conn = _ctx.GetConnection();
            conn.Open();

            string sql = "SELECT * FROM prestamos";
            using var cmd = new MySqlCommand(sql, conn);
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                lista.Add(new Prestamo
                {
                    Id = rd.GetInt32("id"),
                    LibroId = rd.GetInt32("libro_id"),
                    FechaPrestamo = rd.GetDateTime("fecha_prestamo"),
                    FechaDevolucion = rd.GetDateTime("fecha_devolucion")
                });
            }
            return lista;
        }

        // Devolver un libro
        public void DevolverLibro(int prestamoId)
        {
            using var conn = _ctx.GetConnection();
            conn.Open();

            // Buscar préstamo
            string query = "SELECT libro_id FROM prestamos WHERE id=@id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", prestamoId);
            var libroId = Convert.ToInt32(cmd.ExecuteScalar());

            if (libroId == 0)
                throw new Exception("Préstamo no encontrado");

            // Eliminar préstamo
            string delete = "DELETE FROM prestamos WHERE id=@id";
            using var cmd2 = new MySqlCommand(delete, conn);
            cmd2.Parameters.AddWithValue("@id", prestamoId);
            cmd2.ExecuteNonQuery();

            // Marcar libro disponible
            string update = "UPDATE libros SET disponible=true WHERE id=@id";
            using var cmd3 = new MySqlCommand(update, conn);
            cmd3.Parameters.AddWithValue("@id", libroId);
            cmd3.ExecuteNonQuery();
        }
    }
}