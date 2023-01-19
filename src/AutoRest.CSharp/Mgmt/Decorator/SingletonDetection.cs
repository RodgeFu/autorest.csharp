// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.Mgmt.Models;

namespace AutoRest.CSharp.Mgmt.Decorator;

internal static class SingletonDetection
{
    private static string[] SingletonKeywords = { "default", "latest", "current" };

    private static ConcurrentDictionary<OperationSet, string?> _singletonResourceCache = new ConcurrentDictionary<OperationSet, string?>();

    public static bool IsSingletonResource(this OperationSet operationSet)
    {
        return operationSet.TryGetSingletonResourceSuffix(out _);
    }

    public static bool TryGetSingletonResourceSuffix(this OperationSet operationSet, [MaybeNullWhen(false)] out string singletonIdSuffix)
    {
        singletonIdSuffix = null;
        if (_singletonResourceCache.TryGetValue(operationSet, out singletonIdSuffix))
            return singletonIdSuffix != null;

        bool result = IsSingleton(operationSet, out singletonIdSuffix);
        _singletonResourceCache.TryAdd(operationSet, singletonIdSuffix);
        return result;
    }

    private static bool IsSingleton(OperationSet operationSet, [MaybeNullWhen(false)] out string singletonIdSuffix)
    {
        // we should first check the configuration for the singleton settings
        if (Configuration.MgmtConfiguration.RequestPathToSingletonResource.TryGetValue(operationSet.RequestPath, out singletonIdSuffix))
        {
            // ensure the singletonIdSuffix does not have a slash at the beginning
            singletonIdSuffix = singletonIdSuffix.TrimStart('/');
            return true;
        }

        // we cannot find the corresponding request path in the configuration, trying to deduce from the path
        // return false if this is not a resource
        if (!operationSet.IsResource())
            return false;
        // get the request path
        var currentRequestPath = operationSet.GetRequestPath();
        // if we are a singleton resource,
        // we need to find the suffix which should be the difference between our path and our parent resource
        var parentRequestPath = currentRequestPath.ParentRequestPath();
        var diff = parentRequestPath.TrimAncestorFrom(currentRequestPath);
        // if not all of the segment in difference are constant, we cannot be a singleton resource
        if (!diff.Any() || !diff.All(s => s.IsConstant))
            return false;
        // see if the configuration says that we need to honor the dictionary for singletons
        if (!Configuration.MgmtConfiguration.DoesSingletonRequiresKeyword)
        {
            singletonIdSuffix = string.Join('/', diff.Select(s => s.ConstantValue));
            return true;
        }
        // now we can ensure the last segment of the path is a constant
        var lastSegment = currentRequestPath.Last();
        if (lastSegment.Constant.Type.Equals(typeof(string)) && SingletonKeywords.Any(w => lastSegment.ConstantValue == w))
        {
            singletonIdSuffix = string.Join('/', diff.Select(s => s.ConstantValue));
            return true;
        }

        return false;
    }
}
