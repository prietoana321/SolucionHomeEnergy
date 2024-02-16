using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.API.Utilidad;
using SistemaVenta.DTO;
using SistemaVenta.Models;


namespace SistemaVenta.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProspectoController : ControllerBase
    {
        private readonly IProspectoService _prospectoServicio;


        public ProspectoController(IProspectoService prospectoServicio)
        {
            _prospectoServicio = prospectoServicio;
        }
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<ProspectoDTO>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Lista();
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpPost]
        [Route("Guardar")]


        public async Task<IActionResult> Guardar([FromBody] ProspectoDTO prospecto)
        {
            var rsp = new Response<ProspectoDTO>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Crear(prospecto);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] ProspectoDTO prospecto)
        {
            var rsp = new Response<ProspectoDTO>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Crear(prospecto);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Eliminar(id);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }

        [HttpPost("GuardarImagen")]

        public async Task<string> GuardarImagen([FromForm]subirImagen fichero)
        {
            var ruta = String.Empty;

            if(fichero.Archivo.Length > 0) 
            {
                var nombreArchivo=Guid.NewGuid().ToString()+".jpg";
                ruta=$"Imagenes/{nombreArchivo}";
                //cogemos la ruta , le decimos create y creamos un archivo en fichero

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await fichero.Archivo.CopyToAsync(stream);
                }
            }

            return ruta;

        }

        [HttpPost("upload")]

        public IActionResult upload([FromForm]List<IFormFile>files) 
        {
            try
            {
                if(files.Count > 0)
                {
                    foreach(var file in files)
                    {
                        var filePath= $"Imagenes/"+file.FileName;

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            file.CopyToAsync(stream);
                        }
                    }
                }
                return Ok(files);
            }catch(Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UploadImage")]

        public async Task<ActionResult> UploadImage()
        {
            bool Result = false;
            var Files=Request.Form.Files;

            foreach(IFormFile source in Files)
            {
                string FileName=source.FileName;
                string imagepath = $"Imagenes/" + source.FileName;

                try
                {
                    if (!System.IO.Directory.Exists(imagepath)) System.IO.Directory.CreateDirectory(imagepath);

                    string Filepath = imagepath + "\\1.png";

                    if (System.IO.File.Exists(Filepath)) System.IO.File.Delete(Filepath);
                    using (FileStream stream = System.IO.File.Create(Filepath))
                    {
                        await source.CopyToAsync(stream);
                        Result = true;
                    }
                }catch(Exception ex) {
                    throw ex;
                }
            }
            return Ok(Result);
        }
        /*
        [NonAction]

        public string GetActualpath(string FileName)
        {//hostingEnv.WebRootPath+$"Imagenes/" + FileName;
            return WebRootPath + $"Imagenes/" + FileName;
        }*/

        [NonAction]

        private String GetImagebycode(int Code)
        {
            string hostUrl = "http://localhost:5167";
            string mainImagepath= $"Imagenes/"+(Code.ToString());
            string Filepath = mainImagepath + "\\1.png";

            if (System.IO.File.Exists(Filepath))
            {
                return hostUrl + $"Imagenes/" + Code + "/1.png";
            }
            else
            {
                return hostUrl + $"Imagenes/noimage.png";
            }
        }

        [HttpGet("removeImage/{Code}")]
        public async Task<IActionResult> removeImage(string Code)
        {
            var rsp = new Response<bool>();

            int id = Int32.Parse(Code);

            string Result=string.Empty;
            string FileName = Code;
            string imagepath= $"Imagenes/"+(FileName);

            try
            {
                string Filepath = imagepath + "/1.png";

                if (System.IO.File.Exists(Filepath)) System.IO.File.Delete(Filepath);
                Result = "pass";
            }
            catch(Exception ex) { throw ex; }

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Eliminar(id);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }

        [HttpPost]
        [Route("insertarImagen")]
        public async Task<IActionResult> insertarImagen([FromBody] ProspectoDTO prospecto)
        {
            /* var rsp = new Response<ProspectoDTO>();
             var Archivo = new ProspectoDTO
             {
                 IdProspecto = prospecto.IdProspecto,
                 Nombre = prospecto.Nombre,
                 Fachadaimg = prospecto.Fachadaimg,
                 Url=prospecto.Url,
                 Direccion=prospecto.Direccion,
                 Contacto=prospecto.Contacto,
                 RazonSocial=prospecto.RazonSocial,
                 Idauditor=prospecto.Idauditor,
                 Detalle=prospecto.Detalle,
                 EsActivo=prospecto.EsActivo
              };
             this.repo
             try
             {
                 rsp.Status = true;
                 rsp.Value = "";
             }

             catch (Exception ex)
             {
                 rsp.Status = false;
                 rsp.Msg = ex.Message;
             }
             //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
             return Ok(rsp);*/
            var rsp = new Response<ProspectoDTO>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Crear(prospecto);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpGet]
        [Route("imagenesSubidas")]

        public async Task<IActionResult> imagenesSubidas()
        {
            var rsp = new Response<List<ProspectoDTO>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Lista();
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
    }
}
