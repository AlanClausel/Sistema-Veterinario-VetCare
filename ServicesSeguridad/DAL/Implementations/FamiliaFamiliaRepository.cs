using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DAL.Tools;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesSecurity.DAL.Implementations;

namespace ServicesSecurity.DAL.Implementations
{

    public sealed class FamiliaFamiliaRepository : IJoinRepository<Familia>
    {
        #region Singleton
        private readonly static FamiliaFamiliaRepository _instance = new FamiliaFamiliaRepository();

        public static FamiliaFamiliaRepository Current
        {
            get
            {
                return _instance;
            }
        }

        private FamiliaFamiliaRepository()
        {
            //Implement here the initialization code
        }
        #endregion
        public void Add(Familia obj)
        {
            try
            {
                foreach (var item in obj.GetChildrens())
                {
                    // Verificar si los hijos son familia (no patente)
                    if (item.ChildrenCount() > 0)
                    {
                        Familia familiaHija = item as Familia;
                        SqlHelper.ExecuteNonQuery("Familia_Familia_Insert",
                            System.Data.CommandType.StoredProcedure,
                            new System.Data.SqlClient.SqlParameter[] {
                                new System.Data.SqlClient.SqlParameter("@IdFamiliaPadre", obj.IdComponent),
                                new System.Data.SqlClient.SqlParameter("@IdFamiliaHija", familiaHija.IdComponent)
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        public void Delete(Familia obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery("Familia_Familia_DeleteParticular",
                    System.Data.CommandType.StoredProcedure,
                    new System.Data.SqlClient.SqlParameter[] {
                        new System.Data.SqlClient.SqlParameter("@IdFamilia", obj.IdComponent)
                    });
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        public void GetChildren(Familia obj)
        {
            //1) Buscar en SP Familia_Familia_SelectParticular y retornar id de familias relacionados
            //2) Iterar sobre esos ids -> LLamar al Adaptador y cargar las familias...

            Familia familiaGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader("Familia_Familia_SelectParticular",
                                                        System.Data.CommandType.StoredProcedure,
                                                        new SqlParameter[] { new SqlParameter("@IdFamiliaPadre", obj.IdComponent) }))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        //Obtengo el id de familia relacionado a la familia principal...(obj)
                        Guid idFamiliaHija = Guid.Parse(values[0].ToString());

                        familiaGet = FamiliaRepository.Current.SelectOne(idFamiliaHija);

                        obj.Add(familiaGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }
        }

        /// <summary>
        /// Obtiene las relaciones FamiliaFamilia (usado por FamiliaBLL para navegación recursiva)
        /// </summary>
        public IEnumerable<ServicesSecurity.DomainModel.Security.FamiliaFamilia> GetChildrenRelations(Familia familia)
        {
            var relaciones = new List<ServicesSecurity.DomainModel.Security.FamiliaFamilia>();

            try
            {
                string sqlStatement = "SELECT IdFamiliaPadre, IdFamiliaHijo FROM [dbo].[FamiliaFamilia] WHERE IdFamiliaPadre = @IdFamiliaPadre";

                using (var dr = SqlHelper.ExecuteReader(sqlStatement, System.Data.CommandType.Text,
                                                        new SqlParameter[] { new SqlParameter("@IdFamiliaPadre", familia.IdComponent) }))
                {
                    while (dr.Read())
                    {
                        relaciones.Add(new ServicesSecurity.DomainModel.Security.FamiliaFamilia
                        {
                            idFamiliaPadre = (Guid)dr["IdFamiliaPadre"],
                            idFamiliaHijo = (Guid)dr["IdFamiliaHijo"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }

            return relaciones;
        }

        /// <summary>
        /// Obtiene las relaciones donde esta familia es hija (está contenida en otras familias)
        /// </summary>
        public IEnumerable<ServicesSecurity.DomainModel.Security.FamiliaFamilia> GetParentRelations(Familia familia)
        {
            var relaciones = new List<ServicesSecurity.DomainModel.Security.FamiliaFamilia>();

            try
            {
                string sqlStatement = "SELECT IdFamiliaPadre, IdFamiliaHijo FROM [dbo].[FamiliaFamilia] WHERE IdFamiliaHijo = @IdFamiliaHijo";

                using (var dr = SqlHelper.ExecuteReader(sqlStatement, System.Data.CommandType.Text,
                                                        new SqlParameter[] { new SqlParameter("@IdFamiliaHijo", familia.IdComponent) }))
                {
                    while (dr.Read())
                    {
                        relaciones.Add(new ServicesSecurity.DomainModel.Security.FamiliaFamilia
                        {
                            idFamiliaPadre = (Guid)dr["IdFamiliaPadre"],
                            idFamiliaHijo = (Guid)dr["IdFamiliaHijo"]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }

            return relaciones;
        }

        /// <summary>
        /// Elimina todas las relaciones FamiliaFamilia de una familia con soporte para Unit of Work
        /// </summary>
        public void Delete(Familia obj, IUnitOfWork unitOfWork)
        {
            try
            {
                if (unitOfWork != null && unitOfWork.Transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(
                        unitOfWork.Connection,
                        unitOfWork.Transaction,
                        "Familia_Familia_DeleteParticular",
                        System.Data.CommandType.StoredProcedure,
                        new SqlParameter[] {
                            new SqlParameter("@IdFamilia", obj.IdComponent)
                        });
                }
                else
                {
                    Delete(obj);
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        /// <summary>
        /// Elimina una relación específica FamiliaFamilia con soporte para Unit of Work
        /// </summary>
        public void DeleteRelacion(ServicesSecurity.DomainModel.Security.FamiliaFamilia obj, IUnitOfWork unitOfWork)
        {
            try
            {
                string deleteStatement = "DELETE FROM [dbo].[FamiliaFamilia] WHERE IdFamiliaPadre = @IdFamiliaPadre AND IdFamiliaHijo = @IdFamiliaHijo";

                if (unitOfWork != null && unitOfWork.Transaction != null)
                {
                    SqlHelper.ExecuteNonQuery(
                        unitOfWork.Connection,
                        unitOfWork.Transaction,
                        deleteStatement,
                        System.Data.CommandType.Text,
                        new SqlParameter[] {
                            new SqlParameter("@IdFamiliaPadre", obj.idFamiliaPadre),
                            new SqlParameter("@IdFamiliaHijo", obj.idFamiliaHijo)
                        });
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(deleteStatement, System.Data.CommandType.Text,
                        new SqlParameter[] {
                            new SqlParameter("@IdFamiliaPadre", obj.idFamiliaPadre),
                            new SqlParameter("@IdFamiliaHijo", obj.idFamiliaHijo)
                        });
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }
    }
}
