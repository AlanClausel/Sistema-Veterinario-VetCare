using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DAL.Implementations.Adapter;
using ServicesSecurity.DAL.Tools;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.DomainModel.Exceptions;
using ServicesSecurity.Services;

namespace ServicesSecurity.DAL.Implementations
{
    internal class UsuarioRepository : IGenericRepository<Usuario>
    {

        #region Singleton
        private readonly static UsuarioRepository _instance = new UsuarioRepository();

        public static UsuarioRepository Current
        {
            get
            {
                return _instance;
            }
        }

        private UsuarioRepository()
        {
            //Implement here the initialization code
        }
        #endregion

        #region DVH - Dígito Verificador Horizontal

        /// <summary>
        /// Calcula el DVH (Dígito Verificador Horizontal) para un usuario
        /// Fórmula: SHA256(IdUsuario|Nombre|Clave|Activo)
        /// </summary>
        /// <param name="usuario">Usuario para calcular DVH</param>
        /// <returns>Hash SHA256 del usuario</returns>
        private string CalcularDVH(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            // Formato: IdUsuario|Nombre|Clave|Activo (1 o 0)
            // IMPORTANTE: GUID en MAYÚSCULAS para coincidir con CAST de SQL Server
            string guidMayusculas = usuario.IdUsuario.ToString().ToUpper();
            string activo = usuario.Activo ? "1" : "0";
            string datos = $"{guidMayusculas}|{usuario.Nombre}|{usuario.Clave}|{activo}";

            return CryptographyService.HashPassword(datos);
        }

        /// <summary>
        /// Valida que el DVH del usuario coincida con el calculado
        /// Si no coincide, lanza IntegridadException
        /// </summary>
        /// <param name="usuario">Usuario a validar</param>
        /// <exception cref="IntegridadException">Cuando el DVH no coincide</exception>
        private void ValidarDVH(Usuario usuario)
        {
            if (usuario == null)
                return;

            // Si no tiene DVH, no validar (compatibilidad con datos antiguos)
            if (string.IsNullOrWhiteSpace(usuario.DVH))
            {
                Bitacora.Current.LogWarning($"Usuario '{usuario.Nombre}' no tiene DVH. Se recomienda recalcular.");
                return;
            }

            string dvhCalculado = CalcularDVH(usuario);

            if (usuario.DVH != dvhCalculado)
            {
                // Log crítico de seguridad
                Bitacora.Current.LogCritical($"DVH INVÁLIDO para usuario '{usuario.Nombre}' (ID: {usuario.IdUsuario}). " +
                                            $"Posible alteración directa en base de datos. " +
                                            $"DVH esperado: {dvhCalculado}, DVH en BD: {usuario.DVH}");

                throw new IntegridadException($"Los datos del usuario '{usuario.Nombre}' han sido alterados. " +
                                              "Contacte al administrador del sistema.");
            }
        }

        #endregion

        #region Statements
        private string InsertStatement
        {
            get => "INSERT INTO [dbo].[Usuario] (IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH) VALUES (@IdUsuario, @Nombre, @Email, @Clave, @Activo, @IdiomaPreferido, @DVH)";
        }

        private string UpdateStatement
        {
            get => "UPDATE [dbo].[Usuario] SET Nombre = @Nombre, Email = @Email, Clave = @Clave, Activo = @Activo, IdiomaPreferido = @IdiomaPreferido, DVH = @DVH WHERE IdUsuario = @IdUsuario";
        }

        private string DeleteStatement
        {
            get => "DELETE FROM [dbo].[Usuario] WHERE IdUsuario = @IdUsuario";
        }

        private string SelectOneStatement
        {
            get => "SELECT IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH FROM [dbo].[Usuario] WHERE IdUsuario = @IdUsuario";
        }

        private string SelectOneByNameStatement
        {
            get => "SELECT IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH FROM [dbo].[Usuario] WHERE Nombre = @Nombre";
        }

        private string SelectAllStatement
        {
            get => "SELECT IdUsuario, Nombre, Email, Clave, Activo, IdiomaPreferido, DVH FROM [dbo].[Usuario]";
        }
        #endregion

        public void Insert(Usuario obj)
        {
            // SIEMPRE calcular DVH antes de insertar
            obj.DVH = CalcularDVH(obj);

            SqlHelper.ExecuteNonQuery(InsertStatement, System.Data.CommandType.Text, new SqlParameter[] {
                new SqlParameter("@IdUsuario", obj.IdUsuario),
                new SqlParameter("@Nombre", obj.Nombre),
                new SqlParameter("@Email", obj.Email ?? (object)DBNull.Value),
                new SqlParameter("@Clave", obj.Clave),
                new SqlParameter("@Activo", obj.Activo),
                new SqlParameter("@IdiomaPreferido", obj.IdiomaPreferido ?? "es-AR"),
                new SqlParameter("@DVH", obj.DVH)  // Ya no puede ser null
            });
        }

        public void Delete(Guid id)
        {
            SqlHelper.ExecuteNonQuery(DeleteStatement, System.Data.CommandType.Text, new SqlParameter[] {
                new SqlParameter("@IdUsuario", id)
            });
        }


        public void Update(Usuario obj)
        {
            // SIEMPRE recalcular DVH antes de actualizar
            obj.DVH = CalcularDVH(obj);

            SqlHelper.ExecuteNonQuery(UpdateStatement, System.Data.CommandType.Text, new SqlParameter[] {
                new SqlParameter("@IdUsuario", obj.IdUsuario),
                new SqlParameter("@Nombre", obj.Nombre),
                new SqlParameter("@Email", obj.Email ?? (object)DBNull.Value),
                new SqlParameter("@Clave", obj.Clave),
                new SqlParameter("@Activo", obj.Activo),
                new SqlParameter("@IdiomaPreferido", obj.IdiomaPreferido ?? "es-AR"),
                new SqlParameter("@DVH", obj.DVH)  // Ya no puede ser null
            });
        }

        public IEnumerable<Usuario> SelectAll()
        {
            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectAllStatement, System.Data.CommandType.Text))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        Usuario usuario = UsuarioAdapter.Current.Adapt(values);

                        // Validar DVH de cada usuario
                        ValidarDVH(usuario);

                        usuarios.Add(usuario);
                    }
                }
            }
            catch (IntegridadException)
            {
                // Re-lanzar excepciones de integridad sin procesar
                throw;
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);
            }

            return usuarios;
        }

        public Usuario SelectOne(Guid id)
        {
            Usuario usuarioGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectOneStatement, System.Data.CommandType.Text,
                                                new SqlParameter[] { new SqlParameter("@IdUsuario", id) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        usuarioGet = UsuarioAdapter.Current.Adapt(values);

                        // Validar DVH del usuario
                        ValidarDVH(usuarioGet);
                    }
                }
            }
            catch (IntegridadException)
            {
                // Re-lanzar excepciones de integridad sin procesar
                throw;
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);
            }

            return usuarioGet;
        }

        public Usuario SelectOneByName(string sName)
        {
            Usuario usuarioGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader(SelectOneByNameStatement, System.Data.CommandType.Text,
                                                new SqlParameter[] { new SqlParameter("@Nombre", sName) }))
                {
                    object[] values = new object[reader.FieldCount];

                    if (reader.Read())
                    {
                        reader.GetValues(values);
                        usuarioGet = UsuarioAdapter.Current.Adapt(values);

                        // Validar DVH del usuario
                        ValidarDVH(usuarioGet);
                    }
                }
            }
            catch (IntegridadException)
            {
                // Re-lanzar excepciones de integridad sin procesar
                throw;
            }
            catch (Exception ex)
            {
                Bitacora.Current.LogException(ex);
                ExceptionManager.Current.Handle(ex);
            }

            return usuarioGet;
        }

        public void Add(Usuario obj)
        {
            Insert(obj);
        }

        public IEnumerable<Usuario> SelectAll(string sFilterExpression)
        {
            // Por ahora retorna todos, se puede implementar filtros después
            return SelectAll();
        }

        public Usuario GetOneByName(string sName)
        {
            return SelectOneByName(sName);
        }
    }
}
