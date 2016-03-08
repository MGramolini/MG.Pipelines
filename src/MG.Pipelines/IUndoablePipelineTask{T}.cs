namespace MG.Pipelines
{
	public interface IUndoablePipelineTask<in T> : IPipelineTask<T>
	{
		/// <summary>
		/// Attempts to undo the pipeline task with the specified arguments
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns/>
		PipelineResult Undo(T args);
	}
}