using Dapper;
using Microsoft.Data.SqlClient;
using Presupuesto.Infraestructure;
using Presupuesto.Models;

namespace Presupuesto.Services
{
    public class TiposCuentasServices : ITiposCuentasServices
    {
        private readonly string connectionString;

        public TiposCuentasServices(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Connection string is null");

        }

        public void Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = connection
                .QuerySingle<int>($@"INSERT INTO TiposCuentas (Nombre, Usuario, Orden) 
                                Values(@Nombre, @UsuarioID,0)
                                SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        public async Task CrearAsync(TipoCuenta tipoCuenta)
        {

            using var connection = new SqlConnection(connectionString);
            var id = await connection
                .QuerySingleAsync<int>($@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden) 
                                Values(@Nombre, @UsuarioId,0)
                                SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {

            try
            {
                using var connection = new SqlConnection(connectionString);
                var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas
                            WHERE LOWER(Nombre) = LOWER(@Nombre) AND UsuarioId = @UsuarioId;", new { nombre, usuarioId });
                return existe == 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar la existencia de la cuenta.", ex);
            }
        }


        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId, int start, int length)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden 
                                                     FROM TiposCuentas
                                                     WHERE UsuarioId = @UsuarioId
                                                     ORDER BY Orden
                                                     OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY;",
                                                             new { usuarioId, start, length });
        }

        public async Task<int> ObtenerCantidadTotal(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.ExecuteScalarAsync<int>(@"SELECT COUNT(*) 
                                                      FROM TiposCuentas
                                                      WHERE UsuarioId = @UsuarioId;",
                                                              new { usuarioId });
        }

        //Funciones


        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre,
        Orden FROM TiposCuentas WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId });
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas SET
               Nombre=@Nombre WHERE Id = @Id", tipoCuenta);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE TiposCuentas WHERE Id=@Id", new { id });
        }

    }
}
