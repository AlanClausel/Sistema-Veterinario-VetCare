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
    }
}
