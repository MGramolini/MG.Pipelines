namespace MG.Pipelines.Attribute.Tests.Pipeline2.Tasks
{
	public class Pipeline2Task3 : IPipelineTask<PipelineArguments2>
	{
		public PipelineResult Execute(PipelineArguments2 args)
		{
			System.Diagnostics.Trace.WriteLine("Pipeline2Task3 - OK");
			return PipelineResult.Ok;
		}
	}
}
