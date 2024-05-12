using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using SistemaVenta.API.Utilidad;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Models;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileManagerController : ControllerBase
    {
        private readonly IGenericRepository<FileData> _fileDataRepositorio;

        private IWebHostEnvironment environment;

        private readonly IFileService _fileServicio;

        private readonly IImageService _imageServicio;
        public FileManagerController(IFileService fileService, IMapper mapper, IGenericRepository<FileData> fileDataRepositorio, IImageService imageServicio, IWebHostEnvironment env)
        {
            _fileServicio = fileService;
            _fileDataRepositorio = fileDataRepositorio;
            _imageServicio = imageServicio;
            this.environment = env;
        }
        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rsp = new Response<bool>();

            var prospectoEncontrado = await _fileDataRepositorio.Obtener(u => u.IdFile == id);

            var imagenDowload = new ImagenDownloadDTO();

            imagenDowload.IdFile = prospectoEncontrado.IdFile;
            imagenDowload.Nombre = prospectoEncontrado.Name;
            imagenDowload.IdImagen = prospectoEncontrado.IdFile;

            if (!ModelState.IsValid)
            {
                rsp.Status = false;
                rsp.Msg = "Please pass the valid data";
                return Ok(rsp);
            }
            if (imagenDowload.Nombre != null)
            {
                var file = await _fileServicio.DeleteFileAsync(imagenDowload.Nombre);
                if (file != null)
                {
                    rsp.Msg = "Archivo eliminado";
                }

                // var id = imagenDowload.IdImagen;
                var idFile = imagenDowload.IdFile;
                var fileData = await _fileServicio.Eliminar(idFile);

                if (fileData == true)
                {
                    rsp.Msg = "imagen  eliminado";
                }
                var imagenData = await _imageServicio.Eliminar(id);
                if (imagenData == true)
                {
                    rsp.Msg = "imagen  eliminado";
                }

                return Ok("exito");
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }/*
        [HttpPost]
        //[Route("DeletePrueba")]
        [Route("DeletePrueba/,{id:int}")]
        public async Task<IActionResult> DeletePrueba(int id)
        {

            var rsp = new Response<Imagen>();

            var prospectoEncontrado = await _fileDataRepositorio.Obtener(u => u.IdFile == id);

            var imagenDowload = new ImagenDownloadDTO();

            imagenDowload.IdFile = prospectoEncontrado.IdFile;
            imagenDowload.Nombre = prospectoEncontrado.Name;
            imagenDowload.IdImagen = prospectoEncontrado.IdFile;

            if (!ModelState.IsValid)
            {
                rsp.Status = false;
                rsp.Msg = "Please pass the valid data";
                return Ok(rsp);
            }
            if (imagenDowload.Nombre != null)
            {
                var file = await _fileServicio.DeleteFileAsync(imagenDowload.Nombre);
                if (file != null)
                {
                    rsp.Msg = "Archivo eliminado";
                }

               // var id = imagenDowload.IdImagen;
                var idFile = imagenDowload.IdFile;
                var fileData = await _fileServicio.Eliminar(idFile);

                if (fileData == true)
                {
                    rsp.Msg = "imagen  eliminado";
                }
                var imagenData = await _imageServicio.Eliminar(id);
                if (imagenData == true)
                {
                    rsp.Msg = "imagen  eliminado";
                }

                return Ok("exito");
            }
            return Ok(rsp.Msg = "fallido");
        }

        */

        [HttpGet]
        [Route("GetAllFile")]

        public async Task<IActionResult> GetAllFile()
        {
            var rsp = new Response<List<FileRecordDTO>>();

            try
            {
                rsp.Status = true;

                rsp.Value = await _fileServicio.GetAllFile();
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        /*
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var fileEncontrado = await _fileDataRepositorio.Obtener(u => u.IdFile == id);
            var wwwPath = fileEncontrado.Path;
            //getting file from inmemory obj
            //var file = fileDB?.Where(n => n.Id == id).FirstOrDefault();
            //getting file from DB

            var memory = new MemoryStream();
            using (var stream = new FileStream(wwwPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(wwwPath);

            return File(memory, contentType, fileName);
        }*/
        [HttpDelete("Download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            var fileEncontrado = await _fileDataRepositorio.Obtener(u => u.IdFile == id);
            var wwwPath = this.environment.WebRootPath;
            if (fileEncontrado.Path == null)
            {
                return Ok("fallido, ruta no encontrada");
            }
            var path = fileEncontrado.Path;
            
            //getting file from inmemory obj
            //var file = fileDB?.Where(n => n.Id == id).FirstOrDefault();
            //getting file from DB

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(path);

            return File(memory, contentType, fileName);
        }
        [HttpGet]
        [Route("ListaFileImagenes")]

        public async Task<IActionResult> ListaFileImagenes()
        {
            var rsp = new Response<List<FileData>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _fileServicio.ListaImagen();
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpPost]
        [Route("Agregar")]
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


        }
    }
}
