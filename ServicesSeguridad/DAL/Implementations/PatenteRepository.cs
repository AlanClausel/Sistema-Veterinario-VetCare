using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DomainModel.Security.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesSecurity.DAL.Tools;
using System.Data.SqlClient;
using ServicesSecurity.Services.Extensions;
using ServicesSecurity.DAL.Implementations.Adapter;

namespace ServicesSecurity.DAL.Implementations
{

    internal sealed class PatenteRepository : IGenericRepository<Patente>
    {
        #region Singleton
        private readonly static PatenteRepository _instance = new PatenteRepository();

        public static PatenteRepository Current
        {
            get
            {
                return _instance;
            }
        }

        private PatenteRepository()
        {
            //Implement here the initialization code
        }
        #endregion


        #region Statements
        private string InsertStatement
        {
            get => "INSERT INTO [dbo].[Patente] (IdPatente, MenuItemName, FormName, Orden) VALUES (@IdPatente, @MenuItemName, @FormName, @Orden)";
        }

        private string UpdateStatement
        {
            get => "UPDATE [dbo].[Patente] SET MenuItemName = @MenuItemName, FormName = @FormName, Orden = @Orden WHERE IdPatente = @IdPatente";
        }

        private string DeleteStatement
        {
            get => "DELETE FROM [dbo].[Patente] WHERE IdPatente = @IdPatente";
        }

        private string SelectOneStatement
        {
            get => "Patente_Select";
        }

        private string SelectAllStatement
        {
            get => "Patente_SelectAll";
        }

        private string SelectOneByNameStatement
        {
            get => "SELECT * FROM [dbo].[Patente] WHERE FormName = @FormName";
        }
        #endregion

        public void Add(Patente obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(InsertStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdPatente", obj.IdComponent),
                    new SqlParameter("@MenuItemName", obj.MenuItemName),
                    new SqlParameter("@FormName", obj.FormName),
                    new SqlParameter("@Orden", obj.Orden ?? (object)DBNull.Value)
                });
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(DeleteStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdPatente", id)
                });
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        public IEnumerable<Patente> SelectAll()
        {
            List<Patente> patentes = new List<Patente>();
            Patente patenteGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectAllStatement, System.Data.CommandType.StoredProcedure))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        patenteGet = PatenteAdapter.Current.Adapt(values);
                        patentes.Add(patenteGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }

            return patentes;
        }

        public Patente SelectOne(Guid id)
        {
            Patente patenteGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectOneStatement, System.Data.CommandType.StoredProcedure,
                                                new SqlParameter[] { new SqlParameter("@IdPatente", id) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        patenteGet = PatenteAdapter.Current.Adapt(values);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }

            return patenteGet;
        }

        public void Update(Patente obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(UpdateStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdPatente", obj.IdComponent),
                    new SqlParameter("@MenuItemName", obj.MenuItemName),
                    new SqlParameter("@FormName", obj.FormName),
                    new SqlParameter("@Orden", obj.Orden ?? (object)DBNull.Value)
                });
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        public Patente SelectOneByName(string sName)
        {
            Patente patenteGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectOneByNameStatement, System.Data.CommandType.Text,
                                                new SqlParameter[] { new SqlParameter("@FormName", sName) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        patenteGet = PatenteAdapter.Current.Adapt(values);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }

            return patenteGet;
        }

        public Patente GetOneByName(string sName)
        {
            return SelectOneByName(sName);
        }

        public IEnumerable<Patente> SelectAll(string sFilterExpression)
        {
            List<Patente> patentes = new List<Patente>();
            Patente patenteGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectAllStatement, System.Data.CommandType.StoredProcedure))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        patenteGet = PatenteAdapter.Current.Adapt(values);
                        patentes.Add(patenteGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }

            return patentes;
        }
    }
}
