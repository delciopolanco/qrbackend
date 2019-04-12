using System.ComponentModel;

namespace qrbackend.Models.Enums
{
    public enum DocumentType
    {
        /// <summary>
        /// Tipo de Documento Cédula
        /// <para>C</para>
        /// </summary>
        [Description("Cedula")]
        C = 'C',

        /// <summary>
        /// Tipo de Documento Pasaporte
        /// <para>P</para>
        /// </summary>
        [Description("Pasaporte")]
        P = 'P'
    }
}
