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
        /// </summary>
        public static void ActualizarPatentesDeRol(Guid idFamilia, List<Patente> patentes)
        {
            try
            {
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
                        QuitarPatenteDeFamilia(idFamilia, patenteActual.IdComponent);
                    }
                }

                // Agregar patentes nuevas
                foreach (var patente in patentes)
                {
                    if (!patentesActuales.Any(p => p.IdComponent == patente.IdComponent))
                    {
                        AsignarPatenteAFamilia(idFamilia, patente.IdComponent);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Current.Handle(ex);
                throw new Exception("Error al actualizar patentes del rol", ex);
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
