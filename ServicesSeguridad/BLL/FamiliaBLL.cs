using System;
using System.Collections.Generic;
using System.Linq;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.DomainModel.Exceptions;
using ServicesSecurity.Services;
using ServicesSecurity.DomainModel.Security;
using BitacoraService = ServicesSecurity.Services.Bitacora;

namespace ServicesSecurity.BLL
{
    public static class FamiliaBLL
    {
        /// <summary>
        /// Actualiza las patentes de un rol (Familia)
        /// Elimina todas las patentes actuales y asigna las nuevas
        /// Usa Unit of Work para garantizar atomicidad
        /// </summary>
        public static void ActualizarPatentesDeRol(Guid idFamilia, List<Patente> patentes)
        {
            // Usar Unit of Work para garantizar que todas las operaciones se completen o se reviertan
            using (var unitOfWork = new ServicesSecurity.DAL.Implementations.SecurityUnitOfWork())
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    // Verificar que la familia existe y es un rol
                    var familia = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                    if (familia == null)
                    {
                        throw new ValidacionException("La familia seleccionada no existe");
                    }

                    if (!familia.EsRol)
                    {
                        throw new ValidacionException("La familia seleccionada no es un rol válido");
                    }

                    // Obtener patentes actuales directas de la familia
                    var patentesActuales = ObtenerPatentesDirectasDeFamilia(idFamilia);

                    // Eliminar patentes que ya no están en la lista
                    foreach (var patenteActual in patentesActuales)
                    {
                        if (!patentes.Any(p => p.IdComponent == patenteActual.IdComponent))
                        {
                            var familiaPatente = new ServicesSecurity.DomainModel.Security.FamiliaPatente
                            {
                                idFamilia = idFamilia,
                                idPatente = patenteActual.IdComponent
                            };
                            ServicesSecurity.DAL.Implementations.FamiliaPatenteRepository.Current.DeleteRelacion(familiaPatente, unitOfWork);
                        }
                    }

                    // Agregar patentes nuevas
                    foreach (var patente in patentes)
                    {
                        if (!patentesActuales.Any(p => p.IdComponent == patente.IdComponent))
                        {
                            var familiaPatente = new ServicesSecurity.DomainModel.Security.FamiliaPatente
                            {
                                idFamilia = idFamilia,
                                idPatente = patente.IdComponent
                            };
                            ServicesSecurity.DAL.Implementations.FamiliaPatenteRepository.Current.Insert(familiaPatente, unitOfWork);
                        }
                    }

                    // Confirmar la transacción
                    unitOfWork.Commit();

                    // Registrar en bitácora (después del commit exitoso)
                    var usuarioLogueado = LoginService.GetUsuarioLogueado();
                    if (usuarioLogueado != null)
                    {
                        var patentesNombres = string.Join(", ", patentes.Select(p => p.FormName).Take(5));
                        if (patentes.Count > 5) patentesNombres += $" (+{patentes.Count - 5} más)";

                        BitacoraService.Current.Registrar(
                            usuarioLogueado.IdUsuario,
                            usuarioLogueado.Nombre,
                            "Permisos",
                            "ActualizarPatentes",
                            $"Patentes actualizadas para rol {familia.Nombre}: {patentes.Count} patentes asignadas ({patentesNombres})",
                            CriticidadBitacora.Advertencia,
                            "FamiliaPatente",
                            idFamilia.ToString()
                        );
                    }
                }
                catch (Exception ex)
                {
                    // En caso de error, revertir la transacción
                    unitOfWork.Rollback();
                    ExceptionManager.Current.Handle(ex);
                    throw new Exception("Error al actualizar patentes del rol", ex);
                }
            }
        }

        /// <summary>
        /// Obtiene las patentes asignadas directamente a una familia (no recursivo)
        /// </summary>
        public static IEnumerable<Patente> ObtenerPatentesDirectasDeFamilia(Guid idFamilia)
        {
            try
            {
                var familia = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                if (familia == null)
                {
                    throw new ValidacionException("La familia no existe");
                }

                var familiaPatentes = ServicesSecurity.DAL.Implementations.FamiliaPatenteRepository.Current
                    .GetChildrenRelations(familia);

                List<Patente> patentes = new List<Patente>();
                foreach (var fp in familiaPatentes)
                {
                    var patente = ServicesSecurity.DAL.Implementations.PatenteRepository.Current.SelectOne(fp.idPatente);
                    if (patente != null)
                    {
                        patentes.Add(patente);
                    }
                }

                return patentes;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener patentes de la familia", ex);
            }
        }

        /// <summary>
        /// Obtiene TODAS las patentes de una familia incluyendo las de sus familias hijas (recursivo)
        /// </summary>
        public static IEnumerable<Patente> ObtenerTodasLasPatentesDeRol(Guid idFamilia)
        {
            try
            {
                var familia = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                if (familia == null)
                {
                    throw new ValidacionException("La familia no existe");
                }

                var todasLasPatentes = new List<Patente>();

                ObtenerPatentesRecursivo(familia, todasLasPatentes);

                // Eliminar duplicados usando IdComponent
                return todasLasPatentes.GroupBy(p => p.IdComponent).Select(g => g.First());
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al obtener patentes del rol", ex);
            }
        }

        private static void ObtenerPatentesRecursivo(Familia familia, List<Patente> acumulador)
        {
            // 1. Obtener patentes directas de esta familia
            var patentesDirectas = ServicesSecurity.DAL.Implementations.FamiliaPatenteRepository.Current
                .GetChildrenRelations(familia);

            foreach (var fp in patentesDirectas)
            {
                var patente = ServicesSecurity.DAL.Implementations.PatenteRepository.Current.SelectOne(fp.idPatente);
                if (patente != null && !acumulador.Any(p => p.IdComponent == patente.IdComponent))
                {
                    acumulador.Add(patente);
                }
            }

            // 2. Obtener familias hijas y procesar recursivamente
            var familiasHijas = ServicesSecurity.DAL.Implementations.FamiliaFamiliaRepository.Current
                .GetChildrenRelations(familia);

            foreach (var ff in familiasHijas)
            {
                var familiaHija = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(ff.idFamiliaHijo);
                if (familiaHija != null)
                {
                    ObtenerPatentesRecursivo(familiaHija, acumulador);
                }
            }
        }

        /// <summary>
        /// Asigna una patente a una familia
        /// </summary>
        public static void AsignarPatenteAFamilia(Guid idFamilia, Guid idPatente)
        {
            try
            {
                var familiaPatente = new ServicesSecurity.DomainModel.Security.FamiliaPatente
                {
                    idFamilia = idFamilia,
                    idPatente = idPatente
                };

                ServicesSecurity.DAL.Implementations.FamiliaPatenteRepository.Current.Insert(familiaPatente);

                // Registrar en bitácora
                var usuarioLogueado = LoginService.GetUsuarioLogueado();
                if (usuarioLogueado != null)
                {
                    var familia = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                    var patente = ServicesSecurity.DAL.Implementations.PatenteRepository.Current.SelectOne(idPatente);

                    BitacoraService.Current.Registrar(
                        usuarioLogueado.IdUsuario,
                        usuarioLogueado.Nombre,
                        "Permisos",
                        "AsignarPatenteAFamilia",
                        $"Patente asignada a familia {familia?.Nombre}: {patente?.FormName}",
                        CriticidadBitacora.Advertencia,
                        "FamiliaPatente",
                        idFamilia.ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al asignar patente a la familia", ex);
            }
        }

        /// <summary>
        /// Quita una patente de una familia
        /// </summary>
        public static void QuitarPatenteDeFamilia(Guid idFamilia, Guid idPatente)
        {
            try
            {
                var familiaPatente = new ServicesSecurity.DomainModel.Security.FamiliaPatente
                {
                    idFamilia = idFamilia,
                    idPatente = idPatente
                };

                ServicesSecurity.DAL.Implementations.FamiliaPatenteRepository.Current.DeleteRelacion(familiaPatente);

                // Registrar en bitácora
                var usuarioLogueado = LoginService.GetUsuarioLogueado();
                if (usuarioLogueado != null)
                {
                    var familia = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                    var patente = ServicesSecurity.DAL.Implementations.PatenteRepository.Current.SelectOne(idPatente);

                    BitacoraService.Current.Registrar(
                        usuarioLogueado.IdUsuario,
                        usuarioLogueado.Nombre,
                        "Permisos",
                        "QuitarPatenteDeFamilia",
                        $"Patente removida de familia {familia?.Nombre}: {patente?.FormName}",
                        CriticidadBitacora.Advertencia,
                        "FamiliaPatente",
                        idFamilia.ToString()
                    );
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al quitar patente de la familia", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo rol (Familia) en el sistema usando el patrón Composite.
        /// El nombre del rol debe empezar con "ROL_" para identificarlo como tal.
        /// </summary>
        /// <param name="nombreRol">Nombre descriptivo del rol (ej: "Veterinario", "Recepcionista")</param>
        /// <returns>La familia (rol) creada</returns>
        /// <exception cref="ValidacionException">Si el nombre es inválido o ya existe</exception>
        public static Familia CrearRol(string nombreRol)
        {
            try
            {
                // Validar nombre
                if (string.IsNullOrWhiteSpace(nombreRol))
                {
                    throw new ValidacionException("El nombre del rol no puede estar vacío");
                }

                // Limpiar el nombre (quitar espacios extra)
                nombreRol = nombreRol.Trim();

                // Validar longitud
                if (nombreRol.Length < 3)
                {
                    throw new ValidacionException("El nombre del rol debe tener al menos 3 caracteres");
                }

                if (nombreRol.Length > 50)
                {
                    throw new ValidacionException("El nombre del rol no puede superar 50 caracteres");
                }

                // Construir nombre completo con prefijo ROL_
                string nombreCompleto = nombreRol.StartsWith("ROL_", StringComparison.OrdinalIgnoreCase)
                    ? nombreRol
                    : $"ROL_{nombreRol}";

                // Verificar que no exista un rol con ese nombre
                var rolesExistentes = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectAll();
                if (rolesExistentes.Any(r => r.Nombre.Equals(nombreCompleto, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ValidacionException($"Ya existe un rol con el nombre '{nombreRol}'");
                }

                // Crear la familia (usando el patrón Composite)
                var nuevaFamilia = new Familia
                {
                    IdComponent = Guid.NewGuid(),
                    Nombre = nombreCompleto
                };

                // Insertar en la base de datos
                ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.Add(nuevaFamilia);

                // Registrar en bitácora
                var usuarioLogueado = LoginService.GetUsuarioLogueado();
                if (usuarioLogueado != null)
                {
                    BitacoraService.Current.Registrar(
                        usuarioLogueado.IdUsuario,
                        usuarioLogueado.Nombre,
                        "Permisos",
                        "CrearRol",
                        $"Nuevo rol creado: {nombreCompleto}",
                        CriticidadBitacora.Advertencia,
                        "Familia",
                        nuevaFamilia.IdComponent.ToString()
                    );
                }

                return nuevaFamilia;
            }
            catch (ValidacionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al crear el rol", ex);
            }
        }

        /// <summary>
        /// Verifica si existe un rol con el nombre especificado
        /// </summary>
        public static bool ExisteRol(string nombreRol)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreRol))
                    return false;

                string nombreCompleto = nombreRol.StartsWith("ROL_", StringComparison.OrdinalIgnoreCase)
                    ? nombreRol
                    : $"ROL_{nombreRol}";

                var roles = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectAll();
                return roles.Any(r => r.Nombre.Equals(nombreCompleto, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                return false;
            }
        }

        /// <summary>
        /// Verifica si un rol tiene usuarios asignados
        /// </summary>
        /// <param name="idFamilia">ID del rol a verificar</param>
        /// <returns>True si hay usuarios asignados, False si no hay</returns>
        public static bool TieneUsuariosAsignados(Guid idFamilia)
        {
            try
            {
                var familia = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                if (familia == null)
                    return false;

                // Obtener todas las relaciones UsuarioFamilia y filtrar por esta familia
                var todasLasRelaciones = ServicesSecurity.DAL.Implementations.UsuarioFamiliaRepository.Current.SelectAll();
                var usuariosFamilia = todasLasRelaciones.Where(uf => uf.idFamilia == idFamilia);

                return usuariosFamilia != null && usuariosFamilia.Any();
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                return true; // Por seguridad, retornar true en caso de error
            }
        }

        /// <summary>
        /// Cuenta cuántos usuarios tienen asignado un rol específico
        /// </summary>
        /// <param name="idFamilia">ID del rol</param>
        /// <returns>Cantidad de usuarios con ese rol</returns>
        public static int ContarUsuariosConRol(Guid idFamilia)
        {
            try
            {
                var familia = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                if (familia == null)
                    return 0;

                // Obtener todas las relaciones UsuarioFamilia y filtrar por esta familia
                var todasLasRelaciones = ServicesSecurity.DAL.Implementations.UsuarioFamiliaRepository.Current.SelectAll();
                var usuariosFamilia = todasLasRelaciones.Where(uf => uf.idFamilia == idFamilia);

                return usuariosFamilia?.Count() ?? 0;
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                return 0;
            }
        }

        /// <summary>
        /// Elimina un rol del sistema con validaciones de seguridad.
        /// No permite eliminar roles protegidos ni roles con usuarios asignados.
        /// Usa Unit of Work para garantizar atomicidad.
        /// </summary>
        /// <param name="idFamilia">ID del rol a eliminar</param>
        /// <exception cref="ValidacionException">Si el rol no puede ser eliminado</exception>
        public static void EliminarRol(Guid idFamilia)
        {
            using (var unitOfWork = new ServicesSecurity.DAL.Implementations.SecurityUnitOfWork())
            {
                try
                {
                    // Verificar que la familia existe
                    var familia = ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.SelectOne(idFamilia);
                    if (familia == null)
                    {
                        throw new ValidacionException("El rol seleccionado no existe");
                    }

                    // Verificar que es un rol (tiene prefijo ROL_)
                    if (!familia.EsRol)
                    {
                        throw new ValidacionException("Solo se pueden eliminar roles (familias con prefijo ROL_)");
                    }

                    // Verificar que no es un rol protegido del sistema
                    string[] rolesProtegidos = { "ROL_Administrador", "ROL_ADMINISTRADOR", "ROL_Admin", "ROL_ADMIN" };
                    if (rolesProtegidos.Any(r => familia.Nombre.Equals(r, StringComparison.OrdinalIgnoreCase)))
                    {
                        throw new ValidacionException($"El rol '{familia.NombreRol}' es un rol del sistema y no puede ser eliminado");
                    }

                    // Verificar que no hay usuarios asignados
                    var cantidadUsuarios = ContarUsuariosConRol(idFamilia);
                    if (cantidadUsuarios > 0)
                    {
                        throw new ValidacionException(
                            $"No se puede eliminar el rol '{familia.NombreRol}' porque tiene {cantidadUsuarios} usuario(s) asignado(s).\n\n" +
                            $"Primero debe reasignar o eliminar los usuarios con este rol.");
                    }

                    unitOfWork.BeginTransaction();

                    // 1. Eliminar todas las patentes asignadas a este rol (FamiliaPatente)
                    var patentesDelRol = ServicesSecurity.DAL.Implementations.FamiliaPatenteRepository.Current
                        .GetChildrenRelations(familia);

                    foreach (var fp in patentesDelRol)
                    {
                        ServicesSecurity.DAL.Implementations.FamiliaPatenteRepository.Current.DeleteRelacion(fp, unitOfWork);
                    }

                    // 2. Eliminar relaciones con otras familias (si las hay)
                    // FamiliaFamilia donde esta familia es padre
                    var familiasHijas = ServicesSecurity.DAL.Implementations.FamiliaFamiliaRepository.Current
                        .GetChildrenRelations(familia);
                    foreach (var ff in familiasHijas)
                    {
                        ServicesSecurity.DAL.Implementations.FamiliaFamiliaRepository.Current.DeleteRelacion(ff, unitOfWork);
                    }

                    // FamiliaFamilia donde esta familia es hija
                    var familiasPadres = ServicesSecurity.DAL.Implementations.FamiliaFamiliaRepository.Current
                        .GetParentRelations(familia);
                    foreach (var ff in familiasPadres)
                    {
                        ServicesSecurity.DAL.Implementations.FamiliaFamiliaRepository.Current.DeleteRelacion(ff, unitOfWork);
                    }

                    // 3. Eliminar la familia (rol) - IMPORTANTE: usar el UnitOfWork para evitar deadlock
                    ServicesSecurity.DAL.Implementations.FamiliaRepository.Current.Delete(idFamilia, unitOfWork);

                    // Confirmar transacción
                    unitOfWork.Commit();

                    // Registrar en bitácora
                    var usuarioLogueado = LoginService.GetUsuarioLogueado();
                    if (usuarioLogueado != null)
                    {
                        BitacoraService.Current.Registrar(
                            usuarioLogueado.IdUsuario,
                            usuarioLogueado.Nombre,
                            "Permisos",
                            "EliminarRol",
                            $"Rol eliminado: {familia.Nombre}",
                            CriticidadBitacora.Advertencia,
                            "Familia",
                            idFamilia.ToString()
                        );
                    }
                }
                catch (ValidacionException)
                {
                    unitOfWork.Rollback();
                    throw;
                }
                catch (Exception ex)
                {
                    unitOfWork.Rollback();
                    ExceptionManager.Current.Handle(ex);
                    throw new Exception("Error al eliminar el rol", ex);
                }
            }
        }
    }
}
