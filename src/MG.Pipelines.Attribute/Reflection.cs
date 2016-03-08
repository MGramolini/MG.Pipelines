namespace MG.Pipelines.Attribute
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	internal static class Reflection
	{
		public delegate T CompiledActivator<out T>(params object[] args);

		/// <summary>
		/// The activators
		/// </summary>
		public static readonly ConcurrentDictionary<Type, CompiledActivator<object>> Activators =
			new ConcurrentDictionary<Type, CompiledActivator<object>>();

		/// <summary>
		/// Creates the activator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="constructor">The constructor.</param>
		/// <returns/>
		public static CompiledActivator<T> CreateActivator<T>(ConstructorInfo constructor)
		{
			var parameters = constructor.GetParameters();
			var parameterExpression = Expression.Parameter(typeof(object[]), "args");
			var expressionArray = new Expression[parameters.Length];
			for (var index1 = 0; index1 < parameters.Length; ++index1)
			{
				Expression index2 = Expression.Constant(index1);
				var parameterType = parameters[index1].ParameterType;
				Expression expression = Expression.Convert(Expression.ArrayIndex(parameterExpression, index2), parameterType);
				expressionArray[index1] = expression;
			}
			return
				(CompiledActivator<T>)
				Expression.Lambda(typeof(CompiledActivator<T>), Expression.New(constructor, expressionArray), parameterExpression)
					.Compile();
		}

		/// <summary>
		/// Creates a generic type with the specific arguments and parameters.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="arguments">The arguments.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns/>
		public static object CreateGenericType(Type type, Type[] arguments, params object[] parameters)
		{
			var forType = type.MakeGenericType(arguments);
			object obj;
			if (parameters != null && parameters.Any())
			{
				var parameterTypes = parameters.Select(p => p.GetType()).ToArray();
				obj = GetActivator(forType, parameterTypes)(parameters);
			}
			else
			{
				obj = GetActivator(forType)();
			}
			return obj;
		}

		/// <summary>
		///     Determines if the Type descends from the supplied ancestor Type.
		/// </summary>
		/// <param name="type">The Type to test.</param>
		/// <param name="ancestor">The Type that must be inherited.</param>
		/// <returns>True if the Type inherits from the ancestor Type.</returns>
		public static bool DescendsFromAncestorType(Type type, Type ancestor)
		{
			while (true)
			{
				if (type == null || type == typeof(object))
				{
					return false;
				}

				if (ancestor.IsAssignableFrom(type))
				{
					return true;
				}

				if (ancestor.IsGenericType && DescendsFromGeneric(type, ancestor))
				{
					return true;
				}

				type = type.BaseType;
			}
		}

		/// <summary>
		/// Gets the activator.
		/// </summary>
		/// <typeparam name="T">The result Type from invoking the activator</typeparam>
		/// <param name="type">The type.</param>
		/// <param name="args">The arguments.</param>
		/// <returns/>
		public static CompiledActivator<T> GetActivator<T>(Type type, params Type[] args)
		{
			var constructors = type.GetConstructors();
			var constructor = args == null
								? constructors.FirstOrDefault(ci => !ci.GetParameters().Any())
								: constructors.FirstOrDefault(
									ci =>
									SequenceEqual(
										ci.GetParameters().Select(p => p.ParameterType),
										args,
										(type1, type2) => type1.IsAssignableFrom(type2)));

			return constructor != null ? CreateActivator<T>(constructor) : null;
		}

		/// <summary>
		///     Gets the activator.
		/// </summary>
		/// <param name="forType">For type.</param>
		/// <param name="parameterTypes">The parameter types.</param>
		/// <returns />
		internal static CompiledActivator<object> GetActivator(Type forType, Type[] parameterTypes = null)
		{
			return Activators.GetOrAdd(forType, type => GetActivator<object>(type, parameterTypes));
		}

		/// <summary>
		///     Determines if the Type implements the ancestor Generic.
		/// </summary>
		/// <param name="type">The Type to test.</param>
		/// <param name="ancestor">The Generic type that must be implemented.</param>
		/// <returns>True if the Type implements the ancestor Type.</returns>
		private static bool DescendsFromGeneric(Type type, Type ancestor)
		{
			if (type == null || type == typeof(object))
			{
				return false;
			}

			if (type.IsGenericType && type.GetGenericTypeDefinition() == ancestor)
			{
				return true;
			}

			var interfaceTypes = type.GetInterfaces();

			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var i in interfaceTypes)
			{
				if (i.IsGenericType && i.GetGenericTypeDefinition() == ancestor)
				{
					return true;
				}
			}

			return DescendsFromAncestorType(type.BaseType, ancestor);
		}

		/// <summary>
		/// Determines if the two sequences are equal
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="second">The second.</param>
		/// <param name="comparer">The comparer.</param>
		/// <returns>True if the sequences are equal</returns>
		private static bool SequenceEqual<T>(this IEnumerable<T> source, IEnumerable<T> second, Func<T, T, bool> comparer)
		{
			return source.SequenceEqual(second, new LambdaComparer<T>(comparer));
		}
	}
}