namespace MG.Pipelines
{
	using System.Collections.Generic;

	public interface IPipeline<T>
	{
		/// <summary>
		/// Gets the tasks.
		/// </summary>
		/// <value>
		/// The tasks.
		/// </value>
		IList<IPipelineTask<T>> Tasks { get; }

		/// <summary>
		/// Executes the pipeline with the specified arguments
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns/>
		PipelineResult Execute(T args);
	}
}
