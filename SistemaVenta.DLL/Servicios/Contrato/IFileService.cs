using Microsoft.AspNetCore.Http;
using SistemaVenta.DTO;
using SistemaVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DLL.Servicios.Contrato
{
    public interface IFileService
    {
        Task<List<FileData>> Lista();

        Task<List<FileData>> ListaImagen();
        public Tuple<int, string, string> SaveImage(IFormFile imageFile);


        Task<bool> CrearFileData(FileData modelo);
        Task<FileRecordDTO> DeleteFileAsync(string imageFile);
        Task<List<FileRecordDTO>> GetAllFile();

        //   Task<bool> DownloadFile(string imageFileName);

        Task<bool> Eliminar(int id);

        Task<FileRecordDTO> SaveFileAsync1(IFormFile imageFile);

    }
}
