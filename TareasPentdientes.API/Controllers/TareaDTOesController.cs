using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NB_JaimeJativa.Repositorio;
using System.Web.Http.Results;
using System.Web.Http;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using System.Threading;
using System.Data.Entity.Infrastructure;

namespace TareasPentdientes.API.Controllers
{
    [RoutePrefix("api/tareadtoes")]
    public class TareaDTOesController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TareaDTOes
        [HttpGet]
        [Route("ObtenerListado")]
        public async Task<IHttpActionResult> ObtenerListado()
        {
            var tareas = await db.Tareas.Where(t => t.Estado).ToListAsync();
            return Ok(tareas);
        }

        // GET: TareaDTOes/Details/5
        [HttpGet]
        [Route("DetalleTarea/{id}")]
        public async Task<IHttpActionResult> DetalleTarea(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("ID de tarea no proporcionado");
                }

                TareaDTO tareaDTO = await db.Tareas.FirstOrDefaultAsync(t => t.ID == id && t.Estado);

                if (tareaDTO == null)
                {
                    return BadRequest("Tarea no encontrada");
                }

                return Ok(tareaDTO); 
            }
            catch (DbUpdateException)
            {
                return InternalServerError(new Exception("Error al actualizar la BD"));
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error No Manejado"));
            }
        }


        [HttpPost]
        [Route("CrearTarea")]
        [ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> CrearTarea([Bind(Include = "ID,Titulo,Descripcion,FechaCreacion,FechaVencimiento,Completada")] TareaDTO tareaDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Tareas.Add(tareaDTO);
                    var result = await db.SaveChangesAsync();

                    return Ok(result);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (DbUpdateException)
            {
                return InternalServerError(new Exception("Error al actualizar la BD"));
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error No Manejado"));
            }

        }

        [HttpPost]
        [Route("ModificarTarea")]
        [ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> ModificarTarea([Bind(Include = "ID,Titulo,Descripcion,FechaCreacion,FechaVencimiento,Completada")] TareaDTO tareaDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tareaDTO).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok("Tarea actualizada exitosamente");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            catch (DbUpdateException)
            {
                return InternalServerError(new Exception("Error al actualizar la BD"));
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error No Manejado"));
            }
        }


        [HttpPost]
        [Route("EliminarTarea/{id}")]
        [ValidateAntiForgeryToken]

        public async Task<IHttpActionResult> EliminarTarea(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("ID de tarea no proporcionado");
                }

                TareaDTO tareaDTO = await db.Tareas.FirstOrDefaultAsync(t => t.ID == id && t.Estado);

                if (tareaDTO == null)
                {
                    return BadRequest("Tarea no encontrada");
                }
                else
                {
                    tareaDTO.Estado = false;
                    db.Entry(tareaDTO).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok("Tarea eliminada exitosamente");

                }

            }
            catch (DbUpdateException)
            {
                return InternalServerError(new Exception("Error al actualizar la BD"));
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Error No Manejado"));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}