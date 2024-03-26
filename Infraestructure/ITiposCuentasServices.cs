using Presupuesto.Models;

namespace Presupuesto.Infraestructure
{
    public interface ITiposCuentasServices
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        void Crear(TipoCuenta tipoCuenta);
        Task CrearAsync(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId, int start, int length);
        Task<int> ObtenerCantidadTotal(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
    }
}
