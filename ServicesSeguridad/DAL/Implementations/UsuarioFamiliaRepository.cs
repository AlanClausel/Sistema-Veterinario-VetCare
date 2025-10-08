using ServicesSecurity.BLL;
using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DAL.Implementations.Adapter;
using ServicesSecurity.DAL.Tools;
using ServicesSecurity.DomainModel.Security;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Implementations
{
    internal class UsuarioFamiliaRepository : IGenericRepository<UsuarioFamilia>
    {
        #region Singleton
        private readonly static UsuarioFamiliaRepository _instance = new UsuarioFamiliaRepository();

        public static UsuarioFamiliaRepository Current
        {
            get
            {
                return _instance;
            }
        }

        private UsuarioFamiliaRepository()
        {
            //Implement here the initialization code
        }
        #endregion

        #region Statements
        private string InsertStatement
        {
            get => "INSERT INTO [dbo].[UsuarioFamilia] (idUsuario, idFamilia) VALUES (@IdUsuario, @IdFamilia)";
        }

        private string UpdateStatement
        {
            get => "UPDATE [dbo].[UsuarioFamilia] SET idFamilia = @IdFamilia WHERE idUsuario = @IdUsuario";
        }

        private string DeleteStatement
        {
            get => "DELETE FROM [dbo].[UsuarioFamilia] WHERE idUsuario = @IdUsuario AND idFamilia = @IdFamilia";
        }

        private string DeleteByUsuarioStatement
        {
            get => "DELETE FROM [dbo].[UsuarioFamilia] WHERE idUsuario = @IdUsuario";
        }

        private string GetOneStatement
        {
            get => "Select * from [dbo].[UsuarioFamilia] where idUsuario=@IdUsuario AND idFamilia=@IdFamilia";
        }

        private string GetOneByNameStatement
        {
            get => "[UsuarioFamilia_SelectByName]";
        }

        private string SelectAllStatement
        {
            get => "Select * from [dbo].[UsuarioFamilia] ";
        }
        #endregion

        public void Insert(UsuarioFamilia obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(InsertStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdUsuario", obj.idUsuario),
                    new SqlParameter("@IdFamilia", obj.idFamilia)
                });
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(DeleteByUsuarioStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdUsuario", id)
                });
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        public void DeleteRelacion(UsuarioFamilia obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(DeleteStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdUsuario", obj.idUsuario),
                    new SqlParameter("@IdFamilia", obj.idFamilia)
                });
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        public IEnumerable<UsuarioFamilia> GetAll(string sFilterExpression)
        {
            string sqlStatement = sFilterExpression ?? SelectAllStatement;

            sqlStatement = (sqlStatement == sFilterExpression) ? SelectAllStatement +
                            " where " + sFilterExpression : sqlStatement;

            using (var dr = SqlHelper.ExecuteReader(sqlStatement, System.Data.CommandType.Text))
            {
                Object[] values = new Object[dr.FieldCount];

                while (dr.Read())
                {
                    dr.GetValues(values);
                    yield return UsuarioFamiliaAdapter.Current.Adapt(values);
                }
            }
        }
        public IEnumerable<UsuarioFamilia> GetChildren(Usuario oUsuario)
        {
            string sqlStatement = SelectAllStatement + " WHERE idUsuario = @IdUsuario";

            using (var dr = SqlHelper.ExecuteReader(sqlStatement, System.Data.CommandType.Text,
                                                    new SqlParameter[] { new SqlParameter("@IdUsuario", oUsuario.IdUsuario) }))
            {
                Object[] values = new Object[dr.FieldCount];

                while (dr.Read())
                {
                    dr.GetValues(values);
                    yield return UsuarioFamiliaAdapter.Current.Adapt(values);
                }
            }
        }

        public UsuarioFamilia GetOne(Guid id)
        {
            UsuarioFamilia UsuarioFamiliaGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(GetOneStatement, System.Data.CommandType.Text,
                                                new SqlParameter[] { new SqlParameter("@IdUsuarioFamilia", id) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        UsuarioFamiliaGet = UsuarioFamiliaAdapter.Current.Adapt(values);
                    }
                }
            }
            catch (Exception ex)

            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);

            }

            return UsuarioFamiliaGet;
        }


        public UsuarioFamilia GetOneByName(String sNombre)
        {
            UsuarioFamilia UsuarioFamiliaGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(GetOneByNameStatement, System.Data.CommandType.StoredProcedure,
                                                new SqlParameter[] { new SqlParameter("@Nombre", sNombre) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        UsuarioFamiliaGet = UsuarioFamiliaAdapter.Current.Adapt(values);
                    }
                }
            }
            catch (Exception ex)

            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);

            }

            return UsuarioFamiliaGet;
        }

        public void Update(UsuarioFamilia obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(UpdateStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdUsuario", obj.idUsuario),
                    new SqlParameter("@IdFamilia", obj.idFamilia)
                });
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        public void Add(UsuarioFamilia obj)
        {
            Insert(obj);
        }

        public IEnumerable<UsuarioFamilia> SelectAll(string sFilterExpression)
        {
            return GetAll(sFilterExpression);
        }

        public IEnumerable<UsuarioFamilia> SelectAll()
        {
            return GetAll(null);
        }

        public UsuarioFamilia SelectOne(Guid id)
        {
            return GetOne(id);
        }

        public UsuarioFamilia SelectOneByName(string sName)
        {
            return GetOneByName(sName);
        }
    }
}
