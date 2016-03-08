namespace MG.Pipelines.Attribute
{
	using System.Collections.Generic;
	using System.Linq;

	public class PipelineFactory : IPipelineFactory
	{
		/// <summary>
		/// The PipelineFactory instance
		/// </summary>
		public static IPipelineFactory Instance = new PipelineFactory();

		/// <summary>
		/// Gets all the pipline names for the specified argument type.
		/// </summary>
		/// <typeparam name="T">The type of the pipeline argument</typeparam>
		/// <returns/>
		public IEnumerable<string> AllPipelinesFor<T>()
		{
			return Registration.Pipelines.Where(kvp => kvp.Value.Attribute.ArgumentType == typeof(T)).Select(kvp => kvp.Key);
		}

		/// <summary>
		/// Creates the specified pipeline.
		/// </summary>
		/// <typeparam name="T">The type of the pipeline argument</typeparam>
		/// <param name="name">The name.</param>
		/// <returns/>
		public IPipeline<T> Create<T>(string name)
		{
			PipelineRegistration registration;
			if (!Registration.Pipelines.TryGetValue(name, out registration))
			{
				return null;
			}

			var arguments = new List<IPipelineTask<T>>();

			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var task in registration.Attribute.PipelineTasks)
			{
				var taskActivator = Reflection.GetActivator<IPipelineTask<T>>(task);
				var taskInstance = taskActivator();
				arguments.Add(taskInstance);
			}

			var pipelineActivator = Reflection.GetActivator<IPipeline<T>>(
				registration.PipelineType,
				typeof(IList<IPipelineTask<T>>));

			var pipeline = pipelineActivator(arguments);

			return pipeline;
		}
	}
}
