using IdentityApi.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApi.Extensions
{
    public class CustomBadRequest : ResponseModel
    {
        public CustomBadRequest(ActionContext context)
        {
            IsSuccess = false;
            Message = "Input validation failed!";
            Response = null;
            Errors = GetErrorMessages(context);
        }

        private static List<string> GetErrorMessages(ActionContext context)
        {
            List<string> Errors = new();

            foreach (var keyModelStatePair in context.ModelState)
            {
                string key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    for (var i = 0; i < errors.Count; i++)
                    {
                        Errors.Add(GetErrorMessage(errors[i], key));
                    }
                }
            }

            return Errors;
        }

        private static string GetErrorMessage(ModelError error, string key)
        {
            return string.IsNullOrEmpty(error.ErrorMessage) ? $"{key}: Invalid input" : error.ErrorMessage;
        }
    }
}
