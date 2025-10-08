using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Implementations.Adapter
{

    public sealed class PatenteAdapter : IAdapter<Patente>
    {
        #region Singleton
        private readonly static PatenteAdapter _instance = new PatenteAdapter();

        public static PatenteAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        private PatenteAdapter()
        {
            //Implement here the initialization code
        }
        #endregion

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
