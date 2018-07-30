// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Configuration;

namespace Apollo.Configuration.JsonObject
{
	public class JsonConfigurationSource : IConfigurationSource
	{
		public object JsonObject { get; set; }

		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new JsonConfigurationProvider(JsonObject);
		}
	}
}
