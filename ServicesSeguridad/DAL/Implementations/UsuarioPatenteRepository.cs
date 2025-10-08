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
    internal class UsuarioPatenteRepository : IGenericRepository<UsuarioPatente>
    {
        #region Singleton
        private readonly static UsuarioPatenteRepository _instance = new UsuarioPatenteRepository();

        public static UsuarioPatenteRepository Current
        {
            get
            {
                return _instance;
            }
        }

        private UsuarioPatenteRepository()
        {
            //Implement here the initialization code
        }
        #endregion

        #region Statements
        private string InsertStatement
        {
            get => "INSERT INTO [dbo].[UsuarioPatente] (idUsuario, idPatente) VALUES (@IdUsuario, @IdPatente)";
        }

        private string UpdateStatement
        {
            get => "UPDATE [dbo].[UsuarioPatente] SET idPatente = @IdPatente WHERE idUsuario = @IdUsuario";
        }

        private string DeleteStatement
        {
            get => "DELETE FROM [dbo].[UsuarioPatente] WHERE idUsuario = @IdUsuario AND idPatente = @IdPatente";
        }

        private string DeleteByUsuarioStatement
        {
            get => "DELETE FROM [dbo].[UsuarioPatente] WHERE idUsuario = @IdUsuario";
        }

        private string GetOneStatement
        {
            get => "Select * from [dbo].[UsuarioPatente] where idUsuario=@IdUsuario AND idPatente=@IdPatente";
        }

        private string GetOneByNameStatement
        {
            get => "[UsuarioPatente_SelectByName]";
        }

        private string SelectAllStatement
        {
            get => "Select * from [dbo].[UsuarioPatente] ";
        }
        #endregion

        public void Insert(UsuarioPatente obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(InsertStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdUsuario", obj.idUsuario),
                    new SqlParameter("@IdPatente", obj.idPatente)
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

        public void DeleteRelacion(UsuarioPatente obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(DeleteStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdUsuario", obj.idUsuario),
                    new SqlParameter("@IdPatente", obj.idPatente)
                });
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        public IEnumerable<UsuarioPatente> GetAll(string sFilterExpression)
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
                    yield return UsuarioPatenteAdapter.Current.Adapt(values);
                }
            }
        }
        public IEnumerable<UsuarioPatente> GetChildren(Usuario oUsuario)
        {
            string sqlStatement = SelectAllStatement + " WHERE idUsuario = @IdUsuario";

            using (var dr = SqlHelper.ExecuteReader(sqlStatement, System.Data.CommandType.Text,
                                                    new SqlParameter[] { new SqlParameter("@IdUsuario", oUsuario.IdUsuario) }))
            {
                Object[] values = new Object[dr.FieldCount];

                while (dr.Read())
                {
                    dr.GetValues(values);
                    yield return UsuarioPatenteAdapter.Current.Adapt(values);
                }
            }
        }

        public UsuarioPatente GetOne(Guid id)
        {
            UsuarioPatente UsuarioPatenteGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(GetOneStatement, System.Data.CommandType.Text,
                                                new SqlParameter[] { new SqlParameter("@IdUsuarioPatente", id) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        UsuarioPatenteGet = UsuarioPatenteAdapter.Current.Adapt(values);
                    }
                }
            }
            catch (Exception ex)

            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);

            }

            return UsuarioPatenteGet;
        }


        public UsuarioPatente GetOneByName(String sNombre)
        {
            UsuarioPatente UsuarioPatenteGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(GetOneByNameStatement, System.Data.CommandType.StoredProcedure,
                                                new SqlParameter[] { new SqlParameter("@Nombre", sNombre) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        UsuarioPatenteGet = UsuarioPatenteAdapter.Current.Adapt(values);
                    }
                }
            }
            catch (Exception ex)

            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);

            }

            return UsuarioPatenteGet;
        }

        public void Update(UsuarioPatente obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(UpdateStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdUsuario", obj.idUsuario),
                    new SqlParameter("@IdPatente", obj.idPatente)
                });
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);
                throw;
            }
        }

        public void Add(UsuarioPatente obj)
        {
            Insert(obj);
        }

        public IEnumerable<UsuarioPatente> SelectAll(string sFilterExpression)
        {
            return GetAll(sFilterExpression);
        }

        public IEnumerable<UsuarioPatente> SelectAll()
        {
            return GetAll(null);
        }

        public UsuarioPatente SelectOne(Guid id)
        {
            return GetOne(id);
        }

        public UsuarioPatente SelectOneByName(string sName)
        {
            return GetOneByName(sName);
        }
    }
}
