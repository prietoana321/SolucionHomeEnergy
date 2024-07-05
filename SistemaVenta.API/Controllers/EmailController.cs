using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Models;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        private readonly IFileService _fileServicio;

        public EmailController(IEmailService emailService, IFileService fileServicio)
        {
            _emailService = emailService;
            _fileServicio = fileServicio;
        }

        [HttpPost]
        [Route("Enviar")]
        public IActionResult SendEmail(EmailDTO request)
        {

            _emailService.SendEmail(request);
            return Ok();
        }
        /*
        [HttpPost]
        [Route("Guardar")]

        public async Task<IActionResult> Guardar([FromBody] EmailAdjuntoDTO request)
        {



            _emailService.SendEmail(request);
            return Ok();
        }*/
        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromForm] EmailAdjuntoDTO request)
        {


            var imagen = new ImagenDTO();
            // var imagenModelo = _mapper.Map<Imagen>(imagen);
            // var fileModelo =new FileData();
            var rsp = new Response<ImagenDTO>();
            var fileData = new FileData();
            var imagenData = new Imagen();
            var path = "";

            var emailPath = new EmailPathDTO();

            

            imagen.Nombre = request.Asunto;
            imagen.ImageFile=request.Adjunto;

            if (!ModelState.IsValid)
            {
                rsp.Status = false;
                rsp.Msg = "Please pass the valid data";
                return Ok(rsp);
            }
            if (imagen.ImageFile != null)
            {
                var fileResult = _fileServicio.SaveImage(imagen.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    imagen.Nombre = fileResult.Item2;
                    path = fileResult.Item3;// getting name of image
                }
                FileRecordDTO file = new FileRecordDTO();
                file.Path = path;
                file.Name = imagen.Nombre;
                file.FileFormat = Path.GetExtension(imagen.ImageFile.FileName);
                file.ContentType = imagen.ImageFile.ContentType;
                fileData.Name = file.Name;
                fileData.Extension = file.FileFormat;
                fileData.MimeType = file.ContentType;
                fileData.Path = file.Path;
                imagenData.Nombre = imagen.Nombre;
                fileData.IdImagenNavigation = imagenData;
                var result = await _fileServicio.CrearFileData(fileData);

                emailPath.Para=request.Para;
                emailPath.Asunto = request.Asunto;
                emailPath.Contenido=request.Contenido;
                emailPath.PathAdjunto = path;

                _emailService.SendEmailAdjunto(emailPath);
                return Ok(rsp.Msg = "COPIA EL NOMBRE DE TU IMAGEN" + imagen.Nombre + imagen + fileResult);


            }
            //return Ok(rsp.Msg = "COPIA EL NOMBRE DE TU IMAGEN" + imagen.Nombre + imagen);
            return Ok();
        }
        /*
        [HttpPost]
        [Route("AgregarAdjunto")]
        public async Task<IActionResult> AgregarAdjunto([FromForm] EmailAdjuntoDTO imagen)
        {
            // var imagenModelo = _mapper.Map<Imagen>(imagen);
            // var fileModelo =new FileData();
            var emailPath = new EmailPathDTO();
            var rsp = new Response<ImagenDTO>();
            var fileData = new FileData();
            var imagenData = new Imagen();
            var path = "";
            if (!ModelState.IsValid)
            {
                rsp.Status = false;
                rsp.Msg = "Please pass the valid data";
                return Ok(rsp);
            }
            if (imagen.Adjunto != null)
            {
                var fileResult = _fileServicio.SaveImage(imagen.Adjunto);
                if (fileResult.Item1 == 1)
                {
                    imagen.Asunto = fileResult.Item2;
                    path = fileResult.Item3;// getting name of image
                }
                FileRecordDTO file = new FileRecordDTO();
                file.Path = path;
                file.Name = imagen.Asunto;
                file.FileFormat = Path.GetExtension(imagen.Adjunto.FileName);
                file.ContentType = imagen.Adjunto.ContentType;
                fileData.Name = file.Name;
                fileData.Extension = file.FileFormat;
                fileData.MimeType = file.ContentType;
                fileData.Path = file.Path;
                imagenData.Nombre = imagen.Asunto;
                fileData.IdImagenNavigation = imagenData;
                var result = await _fileServicio.CrearFileData(fileData);

                emailPath.Para = imagen.Para;
                emailPath.Asunto = emailPath.Asunto.ToString();
                emailPath.Contenido = emailPath.Contenido.ToString();
                emailPath.PathAdjunto = path;

                _emailService.SendEmailAdjunto(emailPath);



                return Ok(rsp.Msg = "COPIA EL NOMBRE DE TU IMAGEN" + imagen.Asunto + imagen + fileResult);
            }
            return Ok(rsp.Msg = "COPIA EL NOMBRE DE TU IMAGEN" + imagen.Asunto + imagen);


        }*/

        /*
        [HttpPost]
        [Route("SendEmailAdjunto")]

        public async Task<IActionResult> SendEmailAdjunto([FromBody] EmailAdjuntoDTO request)
        {
            

        }*/
        /*
        [HttpPost]
        [Route("SendEmailAdjunto")]
        public async Task<IActionResult> SendEmailAdjunto([FromForm]EmailAdjuntoDTO request)
        {
           

            var requestResult=new EmailPathDTO();
            var imagenDTO1= new ImagenDTO();
            var rsp = new Response<ImagenDTO>();


            var fileData = new FileData();
            var imagenData = new Imagen();


            imagenDTO1.IdImagen =+1;
            imagenDTO1.Nombre = request.Asunto;
            imagenDTO1.ImageFile = request.Adjunto;
            var path = "";
            if (!ModelState.IsValid)
            {
                rsp.Status = false;
                rsp.Msg = "Please pass the valid data";
                return Ok(rsp);
            }
            if (request.Adjunto != null)
            {
                var fileResult = _fileServicio.SaveImage(imagenDTO1.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    imagenDTO1.Nombre = fileResult.Item2;
                    path = fileResult.Item3;// getting name of image
                }
                FileRecordDTO file = new FileRecordDTO();
                file.Path = path;
                file.Name = imagenDTO1.Nombre;
                file.FileFormat = Path.GetExtension(imagenDTO1.ImageFile.FileName);
                file.ContentType = imagenDTO1.ImageFile.ContentType;
                fileData.Name = file.Name;
                fileData.Extension = file.FileFormat;
                fileData.MimeType = file.ContentType;
                fileData.Path = file.Path;
                imagenData.Nombre = imagenDTO1.Nombre;
                fileData.IdImagenNavigation = imagenData;
                var result = await _fileServicio.CrearFileData(fileData);

                requestResult.Para = request.Para;
                requestResult.Contenido = request.Contenido;
                requestResult.Asunto = request.Asunto;
                requestResult.PathAdjunto = file.Path;

                return Ok(rsp.Msg = "COPIA EL NOMBRE DE TU IMAGEN" + imagenDTO1.Nombre + imagenDTO1 + fileResult);
            }
            _emailService.SendEmailAdjunto(requestResult);
            return Ok(rsp.Msg = "Enviado con exito, revisar valores" + imagenDTO1.Nombre + imagenDTO1);
        }*/
        /*
        [HttpPost]
        [Route("EnviarAdjunto")]
        public async Task<IActionResult> Agregar([FromForm] ImagenDTO imagen)
        {
            // var imagenModelo = _mapper.Map<Imagen>(imagen);
            // var fileModelo =new FileData();
            var rsp = new Response<ImagenDTO>();
            var fileData = new FileData();
            var imagenData = new Imagen();
            var path = "";
            if (!ModelState.IsValid)
            {
                rsp.Status = false;
                rsp.Msg = "Please pass the valid data";
                return Ok(rsp);
            }
            if (imagen.ImageFile != null)
            {
                var fileResult = _fileServicio.SaveImage(imagen.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    imagen.Nombre = fileResult.Item2;
                    path = fileResult.Item3;// getting name of image
                }
                FileRecordDTO file = new FileRecordDTO();
                file.Path = path;
                file.Name = imagen.Nombre;
                file.FileFormat = Path.GetExtension(imagen.ImageFile.FileName);
                file.ContentType = imagen.ImageFile.ContentType;
                fileData.Name = file.Name;
                fileData.Extension = file.FileFormat;
                fileData.MimeType = file.ContentType;
                fileData.Path = file.Path;
                imagenData.Nombre = imagen.Nombre;
                fileData.IdImagenNavigation = imagenData;
                var result = await _fileServicio.CrearFileData(fileData);
                return Ok(rsp.Msg = "COPIA EL NOMBRE DE TU IMAGEN" + imagen.Nombre + imagen + fileResult);
            }
            return Ok(rsp.Msg = "COPIA EL NOMBRE DE TU IMAGEN" + imagen.Nombre + imagen);


        }*/
    }
}
