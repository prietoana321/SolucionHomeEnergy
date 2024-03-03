using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DLL.Servicios
{
    public class FileService : IFileService
    {
        private IWebHostEnvironment environment;


        private readonly IGenericRepository<FileData> _fileDataRepositorio;

        private readonly IGenericRepository<FileRecordDTO> _fileRecordRepositorio;

        private readonly IFileService _fileServicio;

        private static List<FileRecordDTO> fileDB = new List<FileRecordDTO>();

        private static List<FileData> fileData = new List<FileData>();

        private readonly IGenericRepository<Imagen> _imagenRepositorio;


        private readonly IMapper _mapper;
        public FileService(IWebHostEnvironment env, IGenericRepository<FileData> fileRepositorio, IMapper mapper, IGenericRepository<Imagen> imagenRepositorio)
        {
            this.environment = env;
            _fileDataRepositorio = fileRepositorio;
            _mapper = mapper;
            _imagenRepositorio = imagenRepositorio;
        }

        public async Task<List<FileData>> ListaImagen()
        {
            try
            {
                var queryProducto = await _fileDataRepositorio.Consultar();
                return queryProducto.ToList();
            }
            catch
            {
                throw;
            }
        }

        public Tuple<int, string, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var contentPath = this.environment.ContentRootPath;
                // path = "c://projects/productminiapi/uploads" ,not exactly something like that
                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions

                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg", ".pdf", ".xls", ".xlsx", ".docx" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string, string>(0, msg, "failed");
                }
                string uniqueString = Guid.NewGuid().ToString();
                // we are trying to create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string, string>(1, newFileName, fileWithPath);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string, string>(0, "Error has occured", "failed");
            }
        }

        public async Task<FileRecordDTO> DeleteFileAsync(string imageFile)
        {
            FileRecordDTO file = new FileRecordDTO();

            try
            {
                var contentPath = this.environment.ContentRootPath;
                // path = "c://projects/productminiapi/uploads" ,not exactly something like that
                var path = Path.Combine(contentPath, "Uploads", imageFile);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                // Check the allowed extenstions


                file.Path = path;
                file.Name = imageFile;
                file.FileFormat = Path.GetExtension(imageFile);
                file.ContentType = imageFile;
                //aqui termina
                return file;
            }
            catch (Exception ex)
            {
                return file;
            }
        }/*
        public async Task<bool> DownloadFile(string imageFileName)
        {
            var contentPath = this.environment.ContentRootPath;
            // path = "c://projects/productminiapi/uploads" ,not exactly something like that

            bool respuesta = true;
            var memoryStream = new MemoryStream();
            var fileNameStream = "";
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    //getting file from DB
                    var prospectoEncontrado = await _imagenRepositorio.Obtener(u => u.Nombre == imageFileName);
                    //donde usuario encontrado en la segunda parte no lleva await 

                    if (prospectoEncontrado == null)
                    {
                        throw new TaskCanceledException("El prospecto no existe");
                    }
                    var file = prospectoEncontrado;
                    var fileWithPath = Path.Combine(path, prospectoEncontrado.Nombre);
                    var memory = new MemoryStream();

                    using (var stream = new FileStream(fileWithPath, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                        stream.Close();
                    }
                    memory.Position = 0;

                    // You could use an object initializer instead of a separate statement, of course.
                    //FileStreamResult =Stream fileStream, string contentType, string fileDownloadName;
                    var contentType = "APPLICATION/octet-stream";
                    var fileName = Path.GetFileName(path);
                    memoryStream = memory;
                    fileNameStream = fileName;
                    var fileStreamResult = new FileStreamResult(memory, fileName);
                    if (fileStreamResult != null)
                    {
                        respuesta = true;
                    }
                    return respuesta;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }*/
        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var prospectoEncontrado = await _fileDataRepositorio.Obtener(u => u.IdFile == id);
                //donde usuario encontrado en la segunda parte no lleva await 

                if (prospectoEncontrado == null)
                {
                    throw new TaskCanceledException("El prospecto no existe");
                }

                bool respuesta = await _fileDataRepositorio.Eliminar(prospectoEncontrado);

                if (respuesta == false)
                {
                    throw new TaskCanceledException("No se pudo eliminar");

                }
                return respuesta;

            }
            catch
            {
                throw;
            }
        }
        /*
        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.GetAttributes(imageFileName);
                    FileInfo oFileInfo = new FileInfo(path);
                    Console.WriteLine("myFile Extension: " + oFileInfo.Extension);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /*
        public bool CrearFile(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(path);

                if (System.IO.File.Exists(path))
                {
                    // Enumerate all files in the Pictures library.
                    var folder = Windows.Storage.KnownFolders.PicturesLibrary;
                    var query = folder.CreateFileQuery();
                    var files = await query.GetFilesAsync();

                    foreach (Windows.Storage.StorageFile file in files)
                    {
                        StringBuilder fileProperties = new StringBuilder();

                        // Get top-level file properties.
                        fileProperties.AppendLine("File name: " + file.Name);
                        fileProperties.AppendLine("File type: " + file.FileType);
                    }
                    string filePath = @"C:\temp\example.docx";
                    Console.WriteLine("File: " + myFileVersionInfo.FileDescription + '\n' +
           "Version number: " + myFileVersionInfo.FileVersion+myFileVersionInfo.FileMajorPart+myFileVersionInfo.ProductVersion+myFileVersionInfo);
                    myFileVersionInfo.Properties.System.Author.Value = new string[] { "Author #1", "Author #2" };
                    file.Properties.System.Title.Value = "Example Title";
                    StringBuilder fileProperties = new StringBuilder();

                    // Get top-level file properties.
                    fileProperties.AppendLine("File name: " + file.Name);
                    fileProperties.AppendLine("File type: " + file.FileType);

                    var versionActual = file.Properties.System.Document.Version.Value;
                    file.Properties.System.Document.Version.Value = "123";
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        */
        public async Task<List<FileData>> Lista()
        {
            try
            {
                /* var queryProducto = await _servicioRepositorio.Consultar();
                 var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();
                 return _mapper.Map<List<ServicioDTO>>(listaProductos).ToList();*/
                var queryProducto = await _fileDataRepositorio.Consultar();
                return queryProducto.ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> CrearFileData(FileData modelo)
        {
            try
            {
                var prospectoCreado = await _fileDataRepositorio.Crear(modelo);
                if (prospectoCreado.IdFile == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el Prospecto");
                }

                return true;

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<FileRecordDTO>> GetAllFile()
        {
            try
            {
                /* var queryProducto = await _servicioRepositorio.Consultar();
                 var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();
                 return _mapper.Map<List<ServicioDTO>>(listaProductos).ToList();*/
                var queryProducto = await _fileDataRepositorio.Consultar();
                return _mapper.Map<List<FileRecordDTO>>(queryProducto).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<FileRecordDTO> SaveFileAsync1(IFormFile imageFile)
        {
            FileRecordDTO file = new FileRecordDTO();
            if (imageFile != null)
            {
                var contentPath = this.environment.ContentRootPath;
                // path = "c://projects/productminiapi/uploads" ,not exactly something like that
                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    var fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(imageFile.FileName);
                    var path2 = Path.Combine(path, fileName);

                    file.Path = path;
                    file.Name = fileName;
                    file.FileFormat = Path.GetExtension(imageFile.FileName);
                    file.ContentType = imageFile.ContentType;
                    var stream = new FileStream(path2, FileMode.Create);
                    using (stream = new FileStream(path2, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    imageFile.CopyTo(stream);
                    stream.Close();
                    return file;
                }
                return file;

            }
            return file;
        }
    }
}
