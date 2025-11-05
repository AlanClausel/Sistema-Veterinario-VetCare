using System;
using System.Collections.Generic;
using System.Linq;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.DomainModel.Exceptions;
using ServicesSecurity.Services;

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
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al quitar patente de la familia", ex);
            }
        }
    }
}
