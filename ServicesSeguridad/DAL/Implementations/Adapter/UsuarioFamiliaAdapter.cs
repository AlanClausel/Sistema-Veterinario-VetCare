using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitacoraService = ServicesSecurity.Services.Bitacora;

namespace ServicesSecurity.DAL.Implementations.Adapter
{
    /// <summary>
    /// Adaptador para transformar filas de base de datos en objetos UsuarioFamilia (relación Usuario-Familia).
    /// Implementa el patrón Adapter y Singleton.
    /// </summary>
    public sealed class UsuarioFamiliaAdapter : IEntityAdapter<UsuarioFamilia>
    {
        #region singleton
        private readonly static UsuarioFamiliaAdapter _instance = new UsuarioFamiliaAdapter();

        /// <summary>
        /// Obtiene la instancia única del adaptador (patrón Singleton).
        /// </summary>
        public static UsuarioFamiliaAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Constructor privado para patrón Singleton.
        /// </summary>
        private UsuarioFamiliaAdapter()
        {
            //Implent here the initialization of your singleton
        }
        #endregion

        /// <summary>
        /// Adapta un array de valores de base de datos a un objeto UsuarioFamilia.
        /// Orden esperado: idUsuario, idFamilia
        /// </summary>
        public UsuarioFamilia Adapt(object[] values)
        {
            try
            {
                return new UsuarioFamilia() // Hidrata objeto UsuarioFamilia con lo que le tiró la DAL interna
                {
                    //idUsuarioFamilia = Guid.Parse(values[0].ToString()),
                    idUsuario = Guid.Parse(values[0].ToString()),
                    idFamilia = Guid.Parse(values[1].ToString()),
                };
            }
            catch (Exception ex)
            {
                BitacoraService.Current.LogException(ex);
                return null;
                throw;
            }

        }
    }
}
