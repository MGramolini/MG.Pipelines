namespace MG.Pipelines.Attribute
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Reflection;

	public static class Registration
	{
		/// <summary>
		/// The pipelines
		/// </summary>
		public static readonly ConcurrentDictionary<string, PipelineRegistration> Pipelines =
			new ConcurrentDictionary<string, PipelineRegistration>(StringComparer.Ordinal);

		/// <summary>
		/// Registers the pipelines.
		/// </summary>
		/// <exception cref="PipelineAttributeRegistrationException">
		/// Pipelines must have at least one task
		/// or
		/// Pipeline tasks must all have the same generic type agrument.
		/// </exception>
		public static void RegisterPipelines()
		{
			var types = TypeLocator.LocateTypes(typeof(IPipeline<>)).ToArray();

			foreach (var type in types)
			{
				var pipelineAttributes = type.GetCustomAttributes<PipelineAttribute>(false).ToArray();

				if (pipelineAttributes.Length == 0)
				{
					continue;
				}

				foreach (var attr in pipelineAttributes)
				{
					if (attr.PipelineTasks.Length == 0)
					{
						throw new PipelineAttributeRegistrationException("Pipelines must have at least one task");
					}

					// ReSharper disable once LoopCanBeConvertedToQuery
					foreach (var pipelineTask in attr.PipelineTasks)
					{
						if (!Reflection.DescendsFromAncestorType(pipelineTask, attr.TaskType))
						{
							throw new PipelineAttributeRegistrationException("Pipeline tasks must all have the same generic type agrument.");
						}
					}

					Pipelines.TryAdd(attr.Name, new PipelineRegistration{ PipelineType = type, Attribute = attr });
				}
			}
		}
	}
}
