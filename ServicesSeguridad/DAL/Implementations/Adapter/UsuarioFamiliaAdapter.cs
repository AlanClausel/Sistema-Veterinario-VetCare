using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Implementations.Adapter
{
    public sealed class UsuarioFamiliaAdapter : IEntityAdapter<UsuarioFamilia>
    {
        #region singleton
        private readonly static UsuarioFamiliaAdapter _instance = new UsuarioFamiliaAdapter();

        public static UsuarioFamiliaAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        private UsuarioFamiliaAdapter()
        {
            //Implent here the initialization of your singleton
        }
        #endregion 
        public UsuarioFamilia Adapt(object[] values)
        {
            try
            {
                return new UsuarioFamilia() ///HIDRATA OBJETO UsuarioFamilia con lo que le tiró la DAL interna 
                {
                    //idUsuarioFamilia = Guid.Parse(values[0].ToString()),
                    idUsuario = Guid.Parse(values[0].ToString()),
                    idFamilia = Guid.Parse(values[1].ToString()),
                };
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                return null;
                throw;
            }

        }
    }
}
