using NB_JaimeJativa.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Sockets;

namespace NB_JaimeJativa.Controllers
{
    public class TareaController : Controller
    {
        private readonly string apiUrl = "https://localhost:44380";

        public ActionResult Index()
        {
            ViewBag.Message = "Este es tu index";

            return View();
        }

        public async Task<ActionResult> MostrarListadoTareas()
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44380/api/TareaDTOes/ObtenerListado");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var x = await response.Content.ReadAsStringAsync();
            Console.WriteLine(x);
            var tareas = JsonConvert.DeserializeObject<List<TareaDTO>>(x);
            return View("ListadoTareas", tareas);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Crear()
        {
            ViewBag.FechaVencimientoDefault = DateTime.Now.AddMonths(1).ToString("yyyy-MM-ddTHH:mm"); // Establece la fecha de vencimiento por defecto como la fecha actual + 1 mes
            //ViewBag.FechaActual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm"); // Establece la hora actual como preselección
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(TareaDTO tareaDTO)
        {
            try
            {
                tareaDTO.FechaCreacion = DateTime.Now;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string jsonContent = JsonConvert.SerializeObject(tareaDTO);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("api/TareaDTOes/CrearTarea", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Tarea creada exitosamente";
                        return RedirectToAction("MostrarListadoTareas", "Tarea");
                    }
                    else
                    {
                        // Error al crear la tarea
                        TempData["ErrorMessage"] = "Error al crear la tarea. Por favor, inténtelo de nuevo más tarde.";
                        return View(tareaDTO); // Volver a la vista de creación con los datos ingresados por el usuario
                    }
                }
            }
            catch (Exception)
            {
                // Error de conexión o servidor
                TempData["ErrorMessage"] = "Error de conexión al intentar crear la tarea. Por favor, inténtelo de nuevo más tarde.";
                return View(tareaDTO); // Volver a la vista de creación con los datos ingresados por el usuario
            }
        }
        public async Task<ActionResult> Editar(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/TareaDTOes/DetalleTarea/{id}");
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var tarea = await response.Content.ReadAsStringAsync();
                    var tareaDTO = JsonConvert.DeserializeObject<TareaDTO>(tarea);
                    return View("Editar", tareaDTO);
                }
                else
                {
                    return RedirectToAction("MostrarListadoTareas", "Tarea");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(TareaDTO tareaDTO)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsync("api/TareaDTOes/ModificarTarea", new StringContent(JsonConvert.SerializeObject(tareaDTO), Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("MostrarListadoTareas", "Tarea");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error al editar la tarea");
                        return View(tareaDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al editar la tarea: " + ex.Message);
                return View(tareaDTO);
            }
        }

        //[HttpPost]
        //public async Task<ActionResult> Eliminar(int Id)
        //{
        //    try
        //    {

        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(apiUrl);
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            HttpResponseMessage response = await client.PostAsync($"api/TareaDTOes/EliminarTarea/{Id}", null);
        //            response.EnsureSuccessStatusCode();

        //            if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, "Error al eliminar la tarea");
        //                return View("Editar", new { ID = Id });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Error al eliminar la tarea: " + ex.Message);
        //        return View("Editar", new { ID = Id });
        //    }
        //}
        [HttpPost]
        public async Task<JsonResult> Eliminar(int Id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsync($"api/TareaDTOes/EliminarTarea/{Id}", null);
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Tarea eliminada exitosamente" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error al eliminar la tarea" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al eliminar la tarea: " + ex.Message });

            }
        }
    }
}