using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace qrbackend.Models.Enums
{
    public enum ApiMessages
    {
        [Description("Hubo un error al ejecutar esta acción: ")]
        DefaultError,

        [Description("Los sistemas externos no se encuentran disponibles")]
        BrokerNoResponse,

        [Description("Usuario y/o contraseña inválidos")]
        InvalidUserAndPassWord
    }
}
