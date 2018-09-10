// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Configuration;

namespace Apollo.Configuration.JsonObject
{
	/// <summary>
	/// Extension methods for adding <see cref="JsonConfigurationProvider"/>.
	/// </summary>
	public static class JsonConfigurationExtensions
	{
		/// <summary>
		/// Adds the JSON configuration provider at <paramref name="path"/> to <paramref name="configurationBuilder"/>.
		/// </summary>
		/// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
		/// <param name="configObject">The object of the Json Configuration.</param>
		/// <param name="optional">Determines if loading the configuration provider is optional.</param>
		/// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
		/// <exception cref="ArgumentException">If <paramref name="configObject"/> is null or empty.</exception>
		public static IConfigurationBuilder AddJsonObject(
			this IConfigurationBuilder configurationBuilder,
			object configObject,
			bool optional = false)
		{
			if (configurationBuilder == null)
				throw new ArgumentNullException(nameof(configurationBuilder));

			if (configObject == null)
				throw new ArgumentNullException(nameof(configurationBuilder));

			configurationBuilder.Add(new JsonConfigurationSource { JsonObject = configObject });

			return configurationBuilder;
		}
	}
}
