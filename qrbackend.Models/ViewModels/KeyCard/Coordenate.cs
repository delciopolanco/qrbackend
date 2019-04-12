using qrbackend.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace qrbackend.Models.ViewModels.KeyCard
{
    public class Coordenate: MQResponse
    {
        /// <summary>
        /// Coordenada de tarjetas de clave
        /// </summary>
        public string positionNumber { get; set; }

        public Coordenate() { }

        public Coordenate(string _coordenate)
        {
            positionNumber = _coordenate;
        }
    }
}
