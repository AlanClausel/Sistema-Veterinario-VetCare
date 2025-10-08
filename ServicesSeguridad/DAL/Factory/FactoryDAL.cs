using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security; // Para clases de relación (UsuarioFamilia, etc.)
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using Usuario = ServicesSecurity.DomainModel.Security.Composite.Usuario; // Resolver ambigüedad
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Factory
{
    public static class FactoryDAL
    {
        /* repo nuevo
        readonly static string sRepository = ConfigurationManager.AppSettings.Get("RepositoryServices");
        readonly static string sRepositoryServices = ConfigurationManager.AppSettings.Get("RepositoryServices");
        */
        readonly static string sRepository = ConfigurationManager.AppSettings.Get("SecurityRepositoryServices");
        readonly static string sRepositoryServices = ConfigurationManager.AppSettings.Get("SecurityRepositoryServices");




        public static IGenericRepository<Usuario> UsuarioRepository { get; private set; }
        public static IGenericRepository<UsuarioFamilia> UsuarioFamiliaRepository { get; private set; }
        public static IGenericRepository<Patente> PatenteRepository { get; private set; }
        public static IGenericRepository<FamiliaPatente> FamiliaPatenteRepository { get; private set; }

        static FactoryDAL()
        {
            try
            {

                //string sRepositoryServices = ConfigurationManager.AppSettings.Get("RepositoryServices");
                //string sRepository = ConfigurationManager.AppSettings.Get("RepositoryServices");

                UsuarioRepository = (IGenericRepository<Usuario>)Activator.CreateInstance
                                        (Type.GetType(sRepository + ".UsuarioRepository"));

                UsuarioFamiliaRepository = (IGenericRepository<UsuarioFamilia>)Activator.CreateInstance
                                        (Type.GetType(sRepository + ".UsuarioFamiliaRepository"));
            }
            catch (Exception ex)
            {
                LoggerService.WriteLog(ex.Message, System.Diagnostics.Tracing.EventLevel.Critical, "FACTORY");
                //MessageBox.Show(ex.Message);
                throw new Exception();
            }

        }

    }
}
