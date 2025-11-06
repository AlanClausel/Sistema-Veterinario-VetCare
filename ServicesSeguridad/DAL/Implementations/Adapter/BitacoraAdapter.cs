using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security;
using System;

namespace ServicesSecurity.DAL.Implementations.Adapter
{
    public sealed class BitacoraAdapter : IAdapter<Bitacora>
    {
        #region Singleton
        private readonly static BitacoraAdapter _instance = new BitacoraAdapter();

        public static BitacoraAdapter Current
        {
            get
            {
                return _instance;
            }
        }

        private BitacoraAdapter()
        {
            // Initialization code
        }
        #endregion

        /// <summary>
        /// Adapta un array de valores de la base de datos a una entidad Bitacora
        /// Orden esperado: IdBitacora, IdUsuario, NombreUsuario, FechaHora, Modulo, Accion,
        /// Descripcion, Tabla, IdRegistro, Criticidad, IP
        /// </summary>
        public Bitacora Adapt(object[] values)
        {
            if (values == null || values.Length < 11)
                throw new ArgumentException("El array de valores debe contener al menos 11 elementos");

            Bitacora bitacora = new Bitacora
            {
                IdBitacora = Guid.Parse(values[0].ToString()),
                IdUsuario = values[1] != DBNull.Value ? (Guid?)Guid.Parse(values[1].ToString()) : null,
                NombreUsuario = values[2] != DBNull.Value ? values[2].ToString() : null,
                FechaHora = Convert.ToDateTime(values[3]),
                Modulo = values[4].ToString(),
                Accion = values[5].ToString(),
                Descripcion = values[6].ToString(),
                Tabla = values[7] != DBNull.Value ? values[7].ToString() : null,
                IdRegistro = values[8] != DBNull.Value ? values[8].ToString() : null,
                Criticidad = values[9].ToString(),
                IP = values[10] != DBNull.Value ? values[10].ToString() : null
            };

            return bitacora;
        }
    }
}
