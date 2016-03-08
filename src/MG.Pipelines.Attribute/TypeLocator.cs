namespace MG.Pipelines.Attribute
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;

	public class TypeLocator
	{
		private static bool assembliesAlreadyForcedIntoAppDomain;

		/// <summary>
		/// Locates the types.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="includeAbstract">if set to <c>true</c> [include abstract].</param>
		/// <returns/>
		public static IEnumerable<Type> LocateTypes(Type type, bool includeAbstract = false)
		{
			if (!assembliesAlreadyForcedIntoAppDomain)
			{
				ForceAssembliesIntoAppDomain();
				assembliesAlreadyForcedIntoAppDomain = true;
			}

			var types = new List<Type>();
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					types.AddRange(assembly.GetTypes());
				}
				catch (ReflectionTypeLoadException e)
				{
					types.AddRange(e.Types.Where(t => t != null));
				}
			}

			return types.Where(t => (includeAbstract || !t.IsAbstract) && Reflection.DescendsFromAncestorType(t, type));
		}

		/// <summary>
		/// Locates the types.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="includeAbstract">if set to <c>true</c> [include abstract].</param>
		/// <returns/>
		public static IEnumerable<Type> LocateTypes<T>(bool includeAbstract = false) where T : class
		{
			return LocateTypes(typeof(T));
		}

		/// <summary>
		/// Forces the assemblies into application domain.
		/// </summary>
		private static void ForceAssembliesIntoAppDomain()
		{
			// Get all currently loaded paths
			var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			var loadedPaths = loadedAssemblies.Select(
				a =>
				{
					try
					{
						return a.Location;
					}
					catch
					{
						Trace.WriteLine(string.Format("Unable to discover location from assembly {0}", a.FullName));
						return null;
					}
				}).Where(a => a != null).ToArray();

			// Get the binary path of the application
			var currentAssembly = Assembly.GetExecutingAssembly();
			var currentAssemblyPath = currentAssembly.CodeBase;
			var binPath = (new Uri(Path.GetDirectoryName(currentAssemblyPath))).LocalPath;
			var referencedPaths = Directory.GetFiles(binPath, "*.dll", SearchOption.AllDirectories);

			// Get all assemblies in paths that are not already loaded
			var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
			toLoad.ForEach(
				path =>
				{
					try
					{
						loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path)));
					}
					catch (Exception ex)
					{
						Trace.WriteLine(string.Format("Unable to load assembly from {0}, Error Message: {1}", path, ex.Message));
					}
				});
		}
	}
}
