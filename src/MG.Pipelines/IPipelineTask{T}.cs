namespace MG.Pipelines
{
	public interface IPipelineTask<in T>
	{
		/// <summary>
		/// Executes the pipeline task with the specified arguments
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns/>
		PipelineResult Execute(T args);
	}
}
