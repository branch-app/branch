using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Schema;

namespace Branch.Packages.Crpc.Registration
{
	public enum AuthenticationType
	{
		UnsafeNoAuthentication,
		AllowInternalAuthentication,
	}

	public class CrpcRegistrationOptions
	{
		private static readonly Regex _endpointRegex = new Regex(@"[a-z]{1}[a-z0-9_]+[a-z]{1}", RegexOptions.Compiled);

		internal Type ServerType { get; set; }

		internal Dictionary<string, Dictionary<string, CrpcVersionRegistration>> Registrations { get; set; }

		public AuthenticationType Authentication { get; set; }

		/// <summary>
		/// Registers the server type with the middleware.
		/// </summary>
		/// <typeparam name="T">The type of the server layer.</typeparam>
		public void RegisterServer<T>()
		{
			if (ServerType != null)
				throw new InvalidOperationException("server type already registered");

			ServerType = typeof(T);
			Registrations = new Dictionary<string, Dictionary<string, CrpcVersionRegistration>>();
		}

		public void RegisterMethod<TReq, TRes>(string endpoint, string methodName, string date)
		{
			if (ServerType == null)
				throw new NullReferenceException("server type must be registered first");

			validateDate(date);
			validateEndpoint(endpoint);
			var method = ServerType.GetMethod(methodName);
			var schema = ServerType.GetField($"{methodName}Schema");

			if (method == null)
				throw new Exception("no method could be found with that name");

			if (schema == null)
				throw new Exception("no schema could be found");

			if (!schema.IsStatic)
				throw new Exception("schema must be static");

			if (schema.FieldType != typeof(string))
				throw new Exception("schema field must be a string");

			Dictionary<string, CrpcVersionRegistration> registration;
			if (Registrations.ContainsKey(endpoint))
				registration = Registrations[endpoint];
			else
				registration = new Dictionary<string, CrpcVersionRegistration>();

			if (registration.ContainsKey(date))
				throw new Exception("duplicate date version found");

			registration.Add(date, new CrpcVersionRegistration
			{
				RequestType = typeof(TReq),
				ResponseType = typeof(TRes),
				Method = method,
				Date = date,
				Schema = JSchema.Parse(schema.GetValue(null) as string),
			});

			Registrations[endpoint] = registration;
		}

		/// <summary>
		/// Validates the format of the input date. Dates have to be either formatted as
		/// yyyy-MM-dd or be "preview".
		/// </summary>
		/// <param name="date">The date to validate.</param>
		private void validateDate(string date)
		{
			if (date == "preview")
				return;

			DateTime.ParseExact(date, "yyyy-MM-dd", null);
		}

		/// <summary>
		/// Validates the format of the input endpoint. Endpoints have to be lowercase
		/// and alphanumeric (with underscores allowed). They also have to start and end
		/// with a letter.
		/// </summary>
		/// <param name="endpoint"></param>
		private void validateEndpoint(string endpoint)
		{
			if (!_endpointRegex.Match(endpoint).Success)
				throw new FormatException("endpoint format incorrect");
		}
	}
}
