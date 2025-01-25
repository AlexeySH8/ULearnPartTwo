using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        Type type = typeof(T);

        public string GetApiDescription()
        {
            var attribute = type
                .GetCustomAttributes(true)
                .OfType<ApiDescriptionAttribute>()
                .FirstOrDefault();
            return attribute != null ? attribute.Description : null;
        }

        public string[] GetApiMethodNames()
        {
            var methodsInfo = type.GetMethods();
            return methodsInfo
                .Where(method => IsMethodHasApiMethodAttribute(method))
                .Select(el => el.Name)
                .ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            var method = type.GetMethod(methodName);
            if (method == null || !GetApiDescriptionsAttributes(method).Any())
                return null;
            var attribute = GetApiDescriptionsAttributes(method).FirstOrDefault();
            if (attribute == null)
                return null;
            return attribute.Description;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            var method = type.GetMethod(methodName);
            if (method == null || !GetApiDescriptionsAttributes(method).Any())
                return null;
            return method.GetParameters().Select(param => param.Name).ToArray();
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            var method = type.GetMethod(methodName);
            if (method == null || !GetApiDescriptionsAttributes(method).Any())
                return null;
            var parameter = method.GetParameters().Where(param => param.Name == paramName);
            if (!parameter.Any())
                return null;
            var attribute = GetApiDescriptionsAttributes(parameter.First()).FirstOrDefault();
            if (attribute == null)
                return null;
            return attribute.Description;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var result = new ApiParamDescription();
            result.ParamDescription = new CommonDescription(paramName);

            var method = type.GetMethod(methodName);
            if (method != null &&
                GetApiDescriptionsAttributes(method).Any())
            {
                var parameter = method
                    .GetParameters()
                    .FirstOrDefault(param => param.Name == paramName); ;
                if (parameter != null)
                {
                    SetParameterDescription(result, parameter);
                    SetValidationAttributes(result, parameter);
                    SetRequiredAttributes(result, parameter);
                }
            }

            return result;
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            var method = GetMethodInfo(typeof(T), methodName);
            var isApiMethod = method
                .GetCustomAttributes<ApiMethodAttribute>()
                .Any();

            var description = GetApiMethodDescription(methodName);
            var paramNames = GetApiMethodParamNames(methodName);
            var paramApiDesc = paramNames
                .Select(x => GetApiMethodParamFullDescription(methodName, x))
                .ToArray();

            var apiMethodDesc = new ApiMethodDescription
            {
                MethodDescription = new CommonDescription(methodName, description),
                ParamDescriptions = paramApiDesc,
                ReturnDescription = GetReturnTypeDescription(method)
            };

            return isApiMethod ? apiMethodDesc : null;
        }

        private ApiParamDescription GetReturnTypeDescription(MethodInfo method)
        {
            var retAttr = method.ReturnTypeCustomAttributes.GetCustomAttributes(false);
            var reqAttr = retAttr?.OfType<ApiRequiredAttribute>().FirstOrDefault();
            var intValAttr = retAttr?.OfType<ApiIntValidationAttribute>().FirstOrDefault();

            return retAttr.Length == 0 ? null : new ApiParamDescription
            {
                Required = reqAttr is null ? false : reqAttr.Required,
                MaxValue = intValAttr?.MaxValue,
                MinValue = intValAttr?.MinValue,
                ParamDescription = new CommonDescription()
            };
        }

        private MethodInfo GetMethodInfo(Type type, string methodName)
        {
            var methodsInfo = type.GetMethods();
            return methodsInfo.Where(mi => mi.Name == methodName).FirstOrDefault();
        }

        private void SetParameterDescription(ApiParamDescription result, ParameterInfo parameter)
        {
            var descriptionAttribute = GetApiDescriptionsAttributes(parameter).FirstOrDefault();
            if (descriptionAttribute != null)
            {
                result.ParamDescription.Description = descriptionAttribute.Description;
            }
        }

        private void SetValidationAttributes(ApiParamDescription result, ParameterInfo parameter)
        {
            var intValidationAttribute = parameter
                .GetCustomAttributes(true)
                .OfType<ApiIntValidationAttribute>()
                .FirstOrDefault();
            if (intValidationAttribute != null)
            {
                result.MinValue = intValidationAttribute.MinValue;
                result.MaxValue = intValidationAttribute.MaxValue;
            }
        }

        private void SetRequiredAttributes(ApiParamDescription result, ParameterInfo parameter)
        {
            var requiredAttribute = parameter
                .GetCustomAttributes(true)
                .OfType<ApiRequiredAttribute>()
                .FirstOrDefault();
            if (requiredAttribute != null)
            {
                result.Required = requiredAttribute.Required;
            }
        }

        private IEnumerable<ApiDescriptionAttribute> GetApiDescriptionsAttributes(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes(true).OfType<ApiDescriptionAttribute>();
        }

        private IEnumerable<ApiDescriptionAttribute> GetApiDescriptionsAttributes(ParameterInfo parametrInfo)
        {
            return parametrInfo.GetCustomAttributes(true).OfType<ApiDescriptionAttribute>();
        }

        private bool IsMethodHasApiMethodAttribute(MethodInfo method)
        {
            return method.GetCustomAttributes(true).OfType<ApiMethodAttribute>().Any();
        }
    }

    // Congratulations on completing the course 25.01.2025 <3
}