// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License

using System.Threading.Tasks;
using AutoRest.CSharp.Common.Input;
using AutoRest.CSharp.Generation.Types;
using Azure;
using Azure.ResourceManager;

namespace AutoRest.CSharp.Mgmt.Decorator
{
    internal static class TypeExtensions
    {

        /// <summary>
        /// Check whether two CSharpType instances equal or not
        /// This is not the same as left.Equals(right) because this function only checks the names
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool EqualsByName(this CSharpType left, CSharpType right)
        {
            if (left.Name != right.Name)
                return false;

            if (left.Arguments.Count != right.Arguments.Count)
                return false;

            for (int i = 0; i < left.Arguments.Count; i++)
            {
                if (left.Arguments[i].Name != right.Arguments[i].Name)
                    return false;
            }

            return true;
        }

        public static CSharpType WrapPageable(this CSharpType type, bool isAsync)
        {
            return isAsync ? new CSharpType(typeof(AsyncPageable<>), type) : new CSharpType(typeof(Pageable<>), type);
        }

        public static CSharpType WrapAsync(this CSharpType type, bool isAsync)
        {
            return isAsync ? new CSharpType(typeof(Task<>), type) : type;
        }

        public static CSharpType WrapResponse(this CSharpType type, bool isAsync, bool isNullable)
        {
            var response = new CSharpType(isNullable ? typeof(NullableResponse<>) : Configuration.ApiTypes.ResponseOfTType, type);
            return isAsync ? new CSharpType(typeof(Task<>), response) : response;
        }

        public static CSharpType WrapOperation(this CSharpType type, bool isAsync)
        {
            var response = new CSharpType(typeof(ArmOperation<>), type);
            return isAsync ? new CSharpType(typeof(Task<>), response) : response;
        }
    }
}
