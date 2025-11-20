using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Implementations.Adapter
{
    /// <summary>
    /// Adaptador para transformar filas de base de datos en objetos Patente.
    /// Implementa el patrón Adapter y Singleton.
    /// </summary>
    public sealed class PatenteAdapter : IAdapter<Patente>
    {
        #region Singleton
        private readonly static PatenteAdapter _instance = new PatenteAdapter();

        /// <summary>
        /// Obtiene la instancia única del adaptador (patrón Singleton).
        /// </summary>
        public static PatenteAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Constructor privado para patrón Singleton.
        /// </summary>
        private PatenteAdapter()
        {
            //Implement here the initialization code
        }
        #endregion

        /// <summary>
        /// Adapta un array de valores de base de datos a un objeto Patente.
        /// </summary>
        /// <param name="values">Array con valores: [0]=IdPatente(Guid), [1]=FormName, [2]=MenuItemName, [3]=Orden, [4]=Descripcion</param>
        /// <returns>Objeto Patente hidratado con los valores proporcionados</returns>
        public Patente Adapt(object[] values)
        {
            //Hidratar el objeto patente
            // El SP devuelve: IdPatente, FormName, MenuItemName, Orden, Descripcion
            Patente patente = new Patente()
            {
                IdComponent = Guid.Parse(values[0].ToString()),
                FormName = values[1].ToString(),
                MenuItemName = values[2].ToString(),
                Orden = values[3]?.ToString(),
                Descripcion = values.Length > 4 ? values[4]?.ToString() : string.Empty
            };

            return patente;
        }
    }

}
