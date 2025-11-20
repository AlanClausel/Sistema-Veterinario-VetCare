using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using ServicesSecurity.DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitacoraService = ServicesSecurity.Services.Bitacora;

namespace ServicesSecurity.DAL.Implementations.Adapter
{
    /// <summary>
    /// Adaptador para transformar filas de base de datos en objetos Usuario.
    /// Implementa el patrón Adapter y Singleton.
    /// Realiza hidratación en dos niveles: datos básicos y permisos (Familias) asociados.
    /// </summary>
    public sealed class UsuarioAdapter : IAdapter<Usuario>
    {
        #region Singleton
        private readonly static UsuarioAdapter _instance = new UsuarioAdapter();

        /// <summary>
        /// Obtiene la instancia única del adaptador (patrón Singleton).
        /// </summary>
        public static UsuarioAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Constructor privado para patrón Singleton.
        /// </summary>
        private UsuarioAdapter()
        {
            //Implement here the initialization code
        }
        #endregion

        /// <summary>
        /// Adapta un array de valores de base de datos a un objeto Usuario.
        /// Realiza hidratación en dos niveles:
        /// Nivel 1: Datos básicos del usuario (Id, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH)
        /// Nivel 2: Carga de permisos (Familias) asociados al usuario desde UsuarioFamilia
        /// </summary>
        public Usuario Adapt(object[] values)
        {
            //Hidratar el objeto usuario -> Nivel 1
            // Orden: IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH
            Usuario usuario = new Usuario()
            {
                IdUsuario = Guid.Parse(values[0].ToString()),
                Nombre = values[1].ToString(),
                Email = values.Length > 2 && values[2] != DBNull.Value ? values[2].ToString() : null,
                Clave = values[3].ToString(),
                Activo = values.Length > 4 && values[4] != DBNull.Value ? Convert.ToBoolean(values[4]) : true,
                IdiomaPreferido = values.Length > 5 && values[5] != DBNull.Value ? values[5].ToString() : "es-AR",
                DVH = values.Length > 6 && values[6] != DBNull.Value ? values[6].ToString() : null
            };

            //Nivel 2 de hidratación...
            try
            {
                List<Component> components = new List<Component>();
                var veremos = UsuarioFamiliaRepository.Current.GetChildren(usuario);

                foreach (var item in veremos)
                {
                    Familia familia = LoginService.SelectOneFamilia(item.idFamilia);
                    usuario.Permisos.Add(familia);
                }
            }
            catch (Exception ex)
            {
                BitacoraService.Current.LogError($"Error en hidratación de permisos para usuario {usuario.Nombre}: {ex.GetType().Name} - {ex.Message}");
                throw;
            }

            return usuario;
        }
    }
}
