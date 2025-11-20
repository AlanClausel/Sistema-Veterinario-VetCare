using ServicesSecurity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DomainModel.Security.Composite
{
    /// <summary>
    /// Representa un usuario del sistema con autenticación y autorización basada en permisos.
    /// Integra el patrón Composite para gestionar permisos mediante Familias (roles) y Patentes (permisos individuales).
    /// Incluye verificación de integridad de datos mediante DVH (Dígito Verificador Horizontal).
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario (GUID)
        /// </summary>
        public Guid IdUsuario { get; set; }

        /// <summary>
        /// Nombre de usuario para autenticación (login)
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Dirección de correo electrónico del usuario
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Contraseña del usuario (uso interno, no se persiste directamente).
        /// Se utiliza para generar el hash almacenado en Clave.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Contraseña hasheada almacenada en la base de datos.
        /// Alias para compatibilidad con el esquema de BD (campo Clave en tabla Usuario).
        /// </summary>
        public string Clave { get; set; }

        /// <summary>
        /// Indica si el usuario está activo en el sistema (para baja lógica)
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Código de idioma preferido del usuario para la interfaz (ej: "es-AR", "en-GB").
        /// Se utiliza para cargar los recursos de idioma correspondientes.
        /// </summary>
        public string IdiomaPreferido { get; set; }

        /// <summary>
        /// Dígito Verificador Horizontal - Hash para verificar integridad de los datos del usuario.
        /// Se calcula con todos los campos del usuario para detectar modificaciones no autorizadas.
        /// </summary>
        public string DVH { get; set; }

        /// <summary>
        /// Calcula el hash de integridad combinando Nombre + Password.
        /// Se utiliza para verificar la integridad del registro del usuario (DVH).
        /// </summary>
        /// <returns>Hash SHA256 de Nombre + Password</returns>
        public string HashDH
        {
            get
            {
                return CryptographyService.HashPassword(Nombre + Password);
            }
        }

        /// <summary>
        /// Calcula el hash de la contraseña actual.
        /// Utilizado para comparar con la contraseña almacenada (Clave) durante la autenticación.
        /// </summary>
        /// <returns>Hash SHA256 de Password</returns>
        public string HashPassword
        {
            get
            {
                return CryptographyService.HashPassword(this.Password);
            }
        }

        /// <summary>
        /// Lista de permisos del usuario (Familias y Patentes).
        /// Se carga dinámicamente desde la BD al autenticarse mediante LoginService.
        /// Incluye roles (Familias con "ROL_"), familias de permisos y patentes individuales.
        /// </summary>
        public List<Component> Permisos { get; set; }

        /// <summary>
        /// Constructor por defecto. Inicializa la lista de permisos vacía.
        /// </summary>
        public Usuario()
        {
            Permisos = new List<Component>();
        }

        /// <summary>
        /// Obtiene la Familia que representa el Rol del usuario.
        /// Busca en la lista de permisos la primera Familia cuyo nombre comienza con "ROL_".
        /// En el sistema, cada usuario debe tener asignado exactamente un rol.
        /// </summary>
        /// <returns>Familia que representa el rol del usuario, o null si no tiene rol asignado</returns>
        /// <example>
        /// var rol = usuario.ObtenerFamiliaRol();
        /// if (rol != null)
        ///     Console.WriteLine($"Rol: {rol.NombreRol}");
        /// </example>
        public Familia ObtenerFamiliaRol()
        {
            foreach (var permiso in Permisos)
            {
                if (permiso is Familia familia && familia.EsRol)
                {
                    return familia;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtiene el nombre del rol del usuario sin el prefijo "ROL_".
        /// Retorna un nombre amigable del rol para mostrar en la interfaz.
        /// </summary>
        /// <returns>Nombre del rol sin prefijo (ej: "Administrador", "Veterinario"), o null si no tiene rol</returns>
        /// <example>
        /// string rol = usuario.ObtenerNombreRol();
        /// // Si el usuario tiene ROL_Administrador, retorna "Administrador"
        /// </example>
        public string ObtenerNombreRol()
        {
            var familiaRol = ObtenerFamiliaRol();
            return familiaRol?.ObtenerNombreRol();
        }

        /// <summary>
        /// Verifica si el usuario tiene asignado un rol específico.
        /// La comparación es case-insensitive.
        /// </summary>
        /// <param name="nombreRol">Nombre del rol sin prefijo "ROL_" (ej: "Administrador", "Veterinario")</param>
        /// <returns>True si el usuario tiene ese rol, false en caso contrario</returns>
        /// <example>
        /// if (usuario.TieneRol("Administrador"))
        /// {
        ///     // Código para administradores
        /// }
        /// </example>
        public bool TieneRol(string nombreRol)
        {
            var rolActual = ObtenerNombreRol();
            return rolActual != null && rolActual.Equals(nombreRol, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Retorna todas las patentes (permisos) únicas del usuario.
        /// Recorre recursivamente la jerarquía de permisos (Familias y Patentes) y extrae todas las patentes sin duplicados.
        /// Se utiliza principalmente para construir el menú dinámico de la aplicación según los permisos del usuario.
        /// </summary>
        /// <returns>Lista de patentes únicas que el usuario puede acceder</returns>
        /// <example>
        /// var patentes = usuario.GetPatentesAll();
        /// foreach (var patente in patentes)
        /// {
        ///     // Construir menú con patente.MenuItemName y patente.FormName
        /// }
        /// </example>
        public List<Patente> GetPatentesAll()
        {
            List<Patente> patentesDistinct = new List<Patente>();

            RecorrerComposite(patentesDistinct, Permisos, "-");

            return patentesDistinct;
        }

        /// <summary>
        /// Método auxiliar recursivo que recorre la estructura Composite de permisos.
        /// Extrae todas las patentes de la jerarquía evitando duplicados.
        /// Imprime información de depuración en consola durante el recorrido.
        /// </summary>
        /// <param name="patentes">Lista acumuladora donde se agregan las patentes encontradas (sin duplicados)</param>
        /// <param name="components">Lista de componentes a recorrer en este nivel</param>
        /// <param name="tab">Tabulación para el formato de salida de depuración (aumenta en cada nivel de recursión)</param>
        private static void RecorrerComposite(List<Patente> patentes, List<Component> components, string tab)
        {
            foreach (var item in components)
            {
                if (item.ChildrenCount() == 0)
                {
                    //Estoy ante un elemento de tipo Patente
                    Patente patente1 = item as Patente;
                    Console.WriteLine($"{tab} Patente: {patente1.FormName}");

                    if (!patentes.Exists(o => o.FormName == patente1.FormName))
                        patentes.Add(patente1);

                    //bool existe = false;

                    //foreach (var item2 in patentes)
                    //{
                    //    if(item2.FormName == patente1.FormName)
                    //    {
                    //        existe = true;
                    //        break;
                    //    }
                    //}

                    //if(!existe)
                    //    patentes.Add(patente1);
                }
                else
                {
                    Familia familia = item as Familia;
                    Console.WriteLine($"{tab} Familia: {familia.Nombre}");
                    RecorrerComposite(patentes, familia.GetChildrens(), tab + "-");
                }
            }
        }

    }
}
