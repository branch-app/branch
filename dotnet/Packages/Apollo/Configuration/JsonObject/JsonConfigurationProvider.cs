// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Apollo.Configuration.JsonObject
{
	/// <summary>
	/// A JSON object based <see cref="ConfigurationProvider"/>.
	/// </summary>
	public class JsonConfigurationProvider : ConfigurationProvider
	{
		/// <summary>
		/// Initializes a new instance of <see cref="JsonConfigurationProvider"/>.
		/// </summary>
		/// <param name="configObject">The JSON configuration object.</param>
		public JsonConfigurationProvider(object configObject, bool optional = false)
		{
			Object = configObject;
			Optional = optional;
		}

		/// <summary>
		/// Gets a value that determines if this instance of <see cref="JsonConfigurationProvider"/> is optional.
		/// </summary>
		public bool Optional { get; }

		/// <summary>
		/// The object backing up this instance of <see cref="JsonConfigurationProvider"/>.
		/// </summary>
		public object Object { get; }

		/// <summary>
		/// Loads the contents of the <see cref="Object"/>.
		/// </summary>
		public override void Load()
		{
			if (Object == null)
			{
				if (Optional)
				{
					Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}
				else
				{
					throw new InvalidOperationException("ConfigObject can not be null");
				}
			}
			else
			{
				var x = JsonConvert.SerializeObject(Object);

				using (var memStream = new MemoryStream())
				{
					using (var streamWriter = new StreamWriter(memStream))
					{
						streamWriter.Write(x);
						streamWriter.Flush();
						memStream.Position = 0;

						Load(memStream);
					}
				}
			}
		}

		internal void Load(Stream stream)
		{
			JsonConfigurationObjectParser parser = new JsonConfigurationObjectParser();
			try
			{
				Data = parser.Parse(stream);
			}
			catch(JsonReaderException)
			{
				throw new FormatException();
			}
		}
    }
}
