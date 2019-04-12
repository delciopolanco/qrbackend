using Microsoft.AspNetCore.Mvc.ModelBinding;
using qrbackend.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qrbackend.Models.ViewModels
{
    public class GenericResponse<T>
    {
        public T data { get; set; }
        public bool success { get; set; }
        public string error { get; set; }

        public GenericResponse<T> withError(string _error)
        {
            success = false;
            error = _error;
            return this;
        }
        public GenericResponse<T> withError()
        {
            success = false;
            return this;
        }
        public GenericResponse<T> withSuccess(T _data)
        {
            data = _data;
            success = true;
            return this;
        }
        public GenericResponse<T> withSuccess()
        {
            success = true;
            return this;
        }
    }

    public class GenericResponse
    {
        public bool validated { get; set; }
    }

    public class FrontStatusCode
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public FrontStatusCode(string message, string code = "01")
        {
            Message = message;
            Code = code;
        }

        public FrontStatusCode(ModelStateDictionary modelState)
        {
            Code = "01";

            var errorList = modelState.Keys
                    .SelectMany(key => modelState[key].Errors)
                    .ToList();

            Message = $"Debe validar los datos enviados, debe enviar todos los campos requeridos: {Environment.NewLine}";

            foreach (var item in errorList)
            {
                Message += item.ErrorMessage;
            }

        }
    }

    public class Response : MQResponse
    {

    }
}
