using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Presupuesto.Filters;
using Presupuesto.Infraestructure;
using Presupuesto.Models;

namespace Presupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        #region Codigo Temporal

        private readonly string connectionString;
        private readonly ITiposCuentasServices _tiposCuentasServices;
        private readonly IUsuariosServices _usuariosServices;

        public TiposCuentasController(ITiposCuentasServices tiposCuentasServices, IConfiguration configuration, IUsuariosServices usuariosServices)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration), "Connection string is null");
            _tiposCuentasServices = tiposCuentasServices;
            _usuariosServices = usuariosServices;
        }

        #endregion

        [ServiceFilter(typeof(GlobalExceptionFilter))]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = _usuariosServices.ObtenerUsuarioId();

            var ExistAccount = await _tiposCuentasServices.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);

            if (ExistAccount)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre),
                    $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }

            await _tiposCuentasServices.CrearAsync(tipoCuenta);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = _usuariosServices.ObtenerUsuarioId();
            var exist = await _tiposCuentasServices.Existe(nombre, usuarioId);
            if (exist)
            {
                return Json($"El nombre {nombre} ya existe");
            }
            return Json(true);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListaCuentas(int start, int length)
        {
            var usuarioId = _usuariosServices.ObtenerUsuarioId();
            var tiposCuentas = await _tiposCuentasServices.Obtener(usuarioId, start, length);

            var totalRecords = await _tiposCuentasServices.ObtenerCantidadTotal(usuarioId);

            return Json(new
            {
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = tiposCuentas
            });
        }

        [ServiceFilter(typeof(GlobalExceptionFilter))]
        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas SET
                            Nombre=@Nombre WHERE Id = @Id", tipoCuenta);

        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var tipoCuenta = await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Order FROM TiposCuentas WHERE Id = @Id AND UsuarioId = @UsuarioId",
                                        new { id, usuarioId });

            if (tipoCuenta == null)
                throw new Exception("Tipo de cuenta no encontrado");

            return tipoCuenta;
        }

        [HttpGet]
        [ServiceFilter(typeof(GlobalExceptionFilter))]
        public async Task<ActionResult> Editar(int id)
        {

            var usuarioid = _usuariosServices.ObtenerUsuarioId();
            var tipoCUenta = await _tiposCuentasServices.ObtenerPorId(id, usuarioid);
            if (tipoCUenta is null)
            {
                RedirectToAction("No Encontrado", "Home");
            }
            //pasamos la entidad como tal para editarla
            return View(tipoCUenta);
        }

        [HttpPost]
        [ServiceFilter(typeof(GlobalExceptionFilter))]
        public async Task<ActionResult> Editar(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            //obtenemos el identificador del usuario
            var usuarioId = _usuariosServices.ObtenerUsuarioId();
            var tipoCuentaExiste = await _tiposCuentasServices.ObtenerPorId(tipoCuenta.Id, usuarioId);
            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("No Encontrado", "Home");
            }
            await _tiposCuentasServices.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }
        #region Eliminacion Tipos Cuentas

        [HttpPost]

        [ServiceFilter(typeof(GlobalExceptionFilter))]

        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _usuariosServices.ObtenerUsuarioId();
            var tipoCuenta = await _tiposCuentasServices.ObtenerPorId(id, usuarioId);
            if(tipoCuenta is null)
            {
                return RedirectToAction("No Encontrado", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpPost]
        [ServiceFilter(typeof(GlobalExceptionFilter))]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = _usuariosServices.ObtenerUsuarioId();
            var tipoCuenta = await _tiposCuentasServices.ObtenerPorId(id, usuarioId);
            if (tipoCuenta is null)
            {
                return RedirectToAction("No Encontrado", "Home");
            }
            await _tiposCuentasServices.Borrar(id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
