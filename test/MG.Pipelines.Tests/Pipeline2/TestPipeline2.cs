namespace MG.Pipelines.Attribute.Tests.Pipeline2
{
	using System;
	using System.Collections.Generic;

	using MG.Pipelines.Attribute;
	using MG.Pipelines.Attribute.Tests.Pipeline2.Tasks;

	[Pipeline("TestPipeline2", typeof(PipelineArguments2), typeof(Pipeline2Task1), typeof(Pipeline2Task2), typeof(Pipeline2Task3))]
	public class TestPipeline2 : Pipeline<PipelineArguments2>
	{
		protected override void Log(Exception caughtException, string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		public TestPipeline2(IList<IPipelineTask<PipelineArguments2>> tasks)
			: base(tasks) { }
	}
}
