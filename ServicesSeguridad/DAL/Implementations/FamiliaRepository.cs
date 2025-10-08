using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DAL.Implementations.Adapter;
using ServicesSecurity.DAL.Tools;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Implementations
{

    internal sealed class FamiliaRepository : IGenericRepository<Familia>
    {

        #region Statements
        private string InsertStatement
        {
            get => "INSERT INTO [dbo].[Familia] (IdFamilia, Nombre) VALUES (@IdFamilia, @Nombre)";
        }

        private string UpdateStatement
        {
            get => "UPDATE [dbo].[Familia] SET Nombre = @Nombre WHERE IdFamilia = @IdFamilia";
        }

        private string DeleteStatement
        {
            get => "DELETE FROM [dbo].[Familia] WHERE IdFamilia = @IdFamilia";
        }

        private string SelectOneStatement
        {
            get => "Familia_Select";
        }

        private string SelectAllStatement
        {
            get => "Familia_SelectAll";
        }

        private string SelectOneByNameStatement
        {
            get => "SELECT * FROM [dbo].[Familia] WHERE Nombre = @Nombre";
        }

        #endregion

        #region Singleton
        private readonly static FamiliaRepository _instance = new FamiliaRepository();

        public static FamiliaRepository Current
        {
            get
            {
                return _instance;
            }
        }

        private FamiliaRepository()
        {
            //Implement here the initialization code
        }
        #endregion
        public void Add(Familia obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(InsertStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdFamilia", obj.IdComponent),
                    new SqlParameter("@Nombre", obj.Nombre)
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
                    new SqlParameter("@IdFamilia", id)
                });
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        public IEnumerable<Familia> SelectAll(string sFilter)
        {
            // Por ahora retorna todos, se puede implementar filtros después
            return SelectAll();
        }

        public IEnumerable<Familia> SelectAll()
        {
            List<Familia> familias = new List<Familia>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectAllStatement, System.Data.CommandType.StoredProcedure))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        familias.Add(FamiliaAdapter.Current.Adapt(values));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }

            return familias;
        }

        public Familia SelectOne(Guid id)
        {
            Familia patenteGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectOneStatement, System.Data.CommandType.StoredProcedure,
                                                new SqlParameter[] { new SqlParameter("@IdFamilia", id) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        patenteGet = FamiliaAdapter.Current.Adapt(values);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }

            return patenteGet;
        }

        public void Update(Familia obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(UpdateStatement, System.Data.CommandType.Text, new SqlParameter[] {
                    new SqlParameter("@IdFamilia", obj.IdComponent),
                    new SqlParameter("@Nombre", obj.Nombre)
                });
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        public Familia SelectOneByName(string sName)
        {
            Familia familiaGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectOneByNameStatement, System.Data.CommandType.Text,
                                                new SqlParameter[] { new SqlParameter("@Nombre", sName) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        familiaGet = FamiliaAdapter.Current.Adapt(values);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }

            return familiaGet;
        }

        public Familia GetOneByName(string sName)
        {
            return SelectOneByName(sName);
        }
    }

}
