﻿namespace Roblox.ApplicationContext;

using System;
using System.IO;
using System.Reflection;

/// <inheritdoc cref="IApplicationContext"/>
public class ApplicationContext : IApplicationContext
{
    private static readonly string _AppName = Environment.GetEnvironmentVariable("AppName");

    private string _AppNameOverride;

    /// <inheritdoc cref="IApplicationContext.Name"/>
    public string Name
    {
        get
        {
            if (!string.IsNullOrEmpty(_AppNameOverride))
                return _AppNameOverride;

            if (!string.IsNullOrWhiteSpace(_AppName))
                return _AppName;

            var path = Assembly?.Location;
            if (string.IsNullOrWhiteSpace(path))
                return null;

            return Path.GetFileNameWithoutExtension(path);
        }
        set { _AppNameOverride = value; }
    }

    /// <inheritdoc cref="IApplicationContext.Assembly"/>
    public Assembly Assembly { get; private set; }

    /// <summary>
    /// The singleton <see cref="ApplicationContext"/>.
    /// </summary>
    public static readonly ApplicationContext Singleton = new();

    private ApplicationContext()
    {
        Assembly = Assembly.GetEntryAssembly();
    }

    /// <summary>
    /// Sets the entry class <see cref="Type"/> to base the <see cref="ApplicationContext"/> on.
    /// </summary>
    /// <param name="entryClass">The entry class <see cref="Type"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="entryClass"/>
    /// </exception>
    /// <exception cref="ArgumentException">
    /// - <paramref name="entryClass"/>.<see cref="Type.IsClass"/> is <c>false</c>.
    /// </exception>
    public static void SetEntryClass(Type entryClass)
    {
        if (entryClass == null)
            throw new ArgumentNullException(nameof(entryClass));

        if (!entryClass.IsClass)
            throw new ArgumentException($"{nameof(entryClass)} expected to be class type.", nameof(entryClass));

        Singleton.Assembly = entryClass.Assembly;
    }
}
