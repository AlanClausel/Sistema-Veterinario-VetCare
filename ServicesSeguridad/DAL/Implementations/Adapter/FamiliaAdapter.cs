using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Implementations.Adapter
{
    /// <summary>
    /// Adaptador para transformar filas de base de datos en objetos Familia.
    /// Implementa el patrón Adapter y Singleton.
    /// Realiza hidratación recursiva de hijos (Familias y Patentes).
    /// </summary>
    public sealed class FamiliaAdapter : IAdapter<Familia>
    {
        #region Singleton
        private readonly static FamiliaAdapter _instance = new FamiliaAdapter();

        /// <summary>
        /// Obtiene la instancia única del adaptador (patrón Singleton).
        /// </summary>
        public static FamiliaAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Constructor privado para patrón Singleton.
        /// </summary>
        private FamiliaAdapter()
        {
            //Implement here the initialization code
        }
        #endregion

        /// <summary>
        /// Adapta un array de valores de base de datos a un objeto Familia.
        /// Realiza hidratación en dos niveles:
        /// Nivel 1: Datos básicos de la familia (Id, Nombre)
        /// Nivel 2: Carga recursiva de familias hijas y patentes hijas (patrón Composite)
        /// </summary>
        /// <param name="values">Array con valores: [0]=IdFamilia(Guid), [1]=Nombre</param>
        /// <returns>Objeto Familia hidratado con sus componentes hijos cargados recursivamente</returns>
        public Familia Adapt(object[] values)
        {
            //Hidratar el objeto familia -> Nivel 1
            Familia familia = new Familia()
            {
                IdComponent = Guid.Parse(values[0].ToString()),
                Nombre = values[1].ToString()
            };


            //Nivel 2 de hidratación...
            FamiliaFamiliaRepository.Current.GetChildren(familia);
            FamiliaPatenteRepository.Current.GetChildren(familia);

            return familia;
        }
    }
}
