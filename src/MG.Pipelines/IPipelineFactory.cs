namespace MG.Pipelines
{
	using System.Collections.Generic;

	public interface IPipelineFactory
	{
		/// <summary>
		/// Gets all the piplines names for the specified argument type.
		/// </summary>
		/// <typeparam name="T">The type of the pipeline argument</typeparam>
		/// <returns/>
		IEnumerable<string> AllPipelinesFor<T>();

		/// <summary>
		/// Creates the specified pipeline.
		/// </summary>
		/// <typeparam name="T">The type of the pipeline argument</typeparam>
		/// <param name="name">The name.</param>
		/// <returns/>
		IPipeline<T> Create<T>(string name);
	}
}
