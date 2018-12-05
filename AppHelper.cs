using System;
using System.Collections.Generic;

namespace QuickerAccess {

	/// <summary>
	/// Helper functions to use across the application
	/// </summary>
	internal static class AppHelper { 

		/// <summary>
		/// Path to the internal resources folder, ended with '/'
		/// </summary>
		internal static string resourcesPath { get { return @"pack://application:,,,/QuickerAccess;component/Resources/"; } }


		internal static T[] SelectiveRemove<T>(this T[] a, Func<T, bool> func) {
			List<T> list = new List<T>(a);
			list.RemoveAll(new Predicate<T>(func));
			return list.ToArray();
		}
	}
}
