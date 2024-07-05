using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SistemaVenta.DTO;
using SistemaVenta.Models;

namespace SistemaVenta.DLL.Servicios.Contrato
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);

        //  void SendEmailAdjunto(EmailPathDTO emailPath);

        void SendEmailAdjunto(EmailPathDTO emailPath);

      //  Task<bool> SendEmailAdjunto(EmailPathDTO emailPath);
    }
}
