using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesSecurity.DomainModel.Security;
using BitacoraService = ServicesSecurity.Services.Bitacora;

namespace ServicesSecurity.DAL.Implementations.Adapter
{
    /// <summary>
    /// Adaptador para transformar filas de base de datos en objetos UsuarioPatente (relación Usuario-Patente).
    /// Implementa el patrón Adapter y Singleton.
    /// </summary>
    public sealed class UsuarioPatenteAdapter : IEntityAdapter<UsuarioPatente>
    {
        #region singleton
        private readonly static UsuarioPatenteAdapter _instance = new UsuarioPatenteAdapter();

        /// <summary>
        /// Obtiene la instancia única del adaptador (patrón Singleton).
        /// </summary>
        public static UsuarioPatenteAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Constructor privado para patrón Singleton.
        /// </summary>
        private UsuarioPatenteAdapter()
        {
            //Implent here the initialization of your singleton
        }
        #endregion

        /// <summary>
        /// Adapta un array de valores de base de datos a un objeto UsuarioPatente.
        /// Orden esperado: idUsuario, idPatente
        /// </summary>
        public UsuarioPatente Adapt(object[] values)
        {
            try
            {
                return new UsuarioPatente() // Hidrata objeto UsuarioPatente con lo que le tiró la DAL interna
                {
                    //idUsuarioPatente = Guid.Parse(values[0].ToString()),
                    idUsuario = Guid.Parse(values[0].ToString()),
                    idPatente = Guid.Parse(values[1].ToString()),
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
