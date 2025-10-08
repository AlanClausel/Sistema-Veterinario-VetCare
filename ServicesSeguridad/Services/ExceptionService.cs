using ServicesSecurity.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.Services
{
    internal static class ExceptionService
    {
        public static void Handle(Exception ex, object sender)
        {
            BLL.ExceptionBLL.Handle(ex, sender);
        }
    }
}
