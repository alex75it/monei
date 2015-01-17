using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Monei.MvcApplication.Helpers;

// "Extenders" was removed from namespace for comodity
namespace Monei.MvcApplication
{
	public static class FormCollectionExtender
	{
		//public static T GetValue<T>(this FormCollection collection, string name)
		//{ 
		//	if (!collection.AllKeys.Contains(name))
		//		throw new Exception(string.Format("Form collection does not contains the key \"{0}\".", name));

		//	try
		//	{
		//		var data = collection.GetValue(name); 

		//		if(data.RawValue == null)
		//			return default(T);
		//		else
		//		{
		//			if(T is DateTime)

		//			return (T)data.ConvertTo(typeof(T));
		//		}
		//	}
		//	catch (Exception exc)
		//	{
		//		object rawValue = collection.GetValue(name).RawValue;
		//		Utils.ThrowException(exc, "Fail to convert the value of \"{0}\" (\"{1}\") to the type \"{2}\".", rawValue, name, typeof(T));
		//		return default(T);
		//		//throw new Exception( string.Format("Fail to convert the value of \"{0}\" (\"{1}\") to the type \"{2}\".", rawValue, name, typeof(T).ToString(), exc);
		//	}

		//}

		public static bool ContainsKey(this FormCollection collection, string name)
		{
			return collection.AllKeys.Contains(name);
		}

		public static string GetStringValue(this FormCollection collection, string name)
		{
			if (!collection.AllKeys.Contains(name))
				throw new Exception(string.Format("Form collection does not contains the key \"{0}\".", name));

			return collection[name];
		}

		public static string GetStringValue(this FormCollection collection, string name, string defaultIfNotExists)
		{
			if (!collection.AllKeys.Contains(name))
				return defaultIfNotExists;

			return collection[name];
		}

		/// <remarks>Throw an Exception if the form collection dows not contains the key</remarks>
		public static int GetIntValue(this FormCollection collection, string name)
		{
			if (!collection.AllKeys.Contains(name))
				throw new Exception(string.Format("Form collection does not contains the key \"{0}\".", name));
			
			int value;
			if (!int.TryParse(collection[name], out value))
				throw new Exception(string.Format("Fail to parse as integer the Form collection key \"{0}\", value is \"{1}\".", name, collection[name]));
			return value;
		}

		/// <summary>
		/// Get the string value fron the form collection and try parse it to int.
		/// </summary>
		/// <remarks>Throw an Exception if the form collection dows not contains the key</remarks>
		/// <param name="collection"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static int? GetIntValue(this FormCollection collection, string name, int? defaultIfNotExists)
		{
			if (!collection.AllKeys.Contains(name))
				return defaultIfNotExists;

			if (collection[name] == "")
				return null;

			int value;
			if (!int.TryParse(collection[name], out value))
				throw new Exception(string.Format("Fail to parse as integer the Form collection key \"{0}\", value is \"{1}\".", name, collection[name]));

			return value;
		}

		/// <summary>
		/// Get the string values fron the form collection and try parse its values to integers.
		/// </summary>
		/// <remarks>Throw an Exception if the form collection dows not contains the key. Select with "multiple" attribute was not inclueded in for collectuion if there isn't a selection.</remarks>
		/// <param name="collection"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static IList<int> GetIntValues(this FormCollection collection, string name)
		{
			if (!collection.AllKeys.Contains(name))
				return new List<int>(0);

			List<int> values = new List<int>();
			foreach (string val in collection[name].Split(','))
			{
				int value;
				if (!int.TryParse(val, out value))
					throw new Exception("The form collection list of \"" + name + "\" contains a value that cannot be converted to int: " + value);
				values.Add(value);
			}

			return values;
		}

		public static IList<int> TryGetIntValues(this FormCollection collection, string name)
		{
			if (!collection.AllKeys.Contains(name))
				throw new Exception(string.Format("Form collection does not contains the key \"{0}\".", name));

			List<int> values = new List<int>();
			foreach (string val in collection[name].Split(','))
			{
				int value;
				if (!int.TryParse(collection[name], out value))
					throw new Exception("The form collection list of \"" + name + "\" contains a value that cannot be converted to int: " + value);
				values.Add(value);
			}

			return values;
		}


		/// <summary>
		/// Get the strig value fron the form collection and try parse it to decimal type.
		/// </summary>
		/// <remarks>Throw an Exception if the form collection dows not contains the key</remarks>
		/// <param name="collection"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static decimal GetDecimalValue(this FormCollection collection, string name)
		{
			if (collection[name] == "")
				throw new Exception(string.Format("The Form collection keys does not contains \"{0}\".", name));

			decimal value;
			if (!decimal.TryParse(collection[name], out value))
				throw new Exception(string.Format("Fail to parse as decimal the Form collection key \"{0}\", passed string value is \"{1}\".", name, collection[name]));

			return value;
		}

		/// <summary>
		/// Get the strig value fron the form collection and try parse it to decimal type.
		/// </summary>
		/// <remarks></remarks>
		/// <param name="collection"></param>
		/// <param name="name"></param>
		/// <param name="defaultValue">Decimal value returned if field is empty</param>
		/// <returns></returns>		
		public static decimal? GetDecimalValueIfPassed(this FormCollection collection, string name, decimal? defaultValue)
		{
			if (collection[name] == "" || string.IsNullOrWhiteSpace(collection[name]))
				return defaultValue;				

			decimal value;
			if (!decimal.TryParse(collection[name], out value))
				throw new Exception(string.Format("Fail to parse as decimal the Form collection key \"{0}\", passed string value is \"{1}\".", name, collection[name]));

			return value;
		}


	}//class
}