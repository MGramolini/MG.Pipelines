namespace MG.Pipelines.Attribute.Tests.Pipeline1.Tasks
{
	public class Pipeline1Task1 : IPipelineTask<PipelineArguments1>
	{
		public PipelineResult Execute(PipelineArguments1 args)
		{
			System.Diagnostics.Trace.WriteLine("Pipeline1Task1 - OK");
			return PipelineResult.Ok;
		}
	}
}
