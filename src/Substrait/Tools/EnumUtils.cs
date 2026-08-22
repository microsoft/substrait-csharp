// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools;

/// <summary>
/// Utility class for enum operations.
/// </summary>
public static class EnumUtils
{
    /// <summary>
    /// Casts an enum of type TSource to an enum of type TTarget.
    /// </summary>
    /// <typeparam name="TSource">Enum type to cast from.</typeparam>
    /// <typeparam name="TTarget">Enum type to cast to.</typeparam>
    /// <param name="source">Value to cast.</param>
    /// <returns>Casted value.</returns>
    /// <exception cref="ArgumentException">Thrown when the enum type is unknown.</exception>
    public static TTarget Cast<TSource, TTarget>(TSource source)
        where TSource : Enum
        where TTarget : Enum
    {
        var sourceValue = Convert.ToInt32(source);
        foreach (TTarget target in Enum.GetValues(typeof(TTarget)))
        {
            if (Convert.ToInt32(target) == sourceValue)
            {
                return target;
            }
        }

        throw new ArgumentException("Unknown type: " + source);
    }

    /// <summary>
    /// Checks whether <paramref name="e"/> is set.
    /// </summary>
    /// <typeparam name="T">Enum type. Preferrably with flags annotation.</typeparam>
    /// <param name="e">Actual enum value.</param>
    /// <param name="flag">Flags to check.</param>
    /// <returns>true if <paramref name="e"/> has <paramref name="flag"/> set.</returns>
    public static bool IsOn<T>(this T e, T flag)
        where T : Enum
    {
        return (Convert.ToInt32(e) & Convert.ToInt32(flag)) != 0;
    }
}
