namespace MG.Pipelines.Attribute.Tests.Pipeline1
{
	using System;
	using System.Collections.Generic;

	using MG.Pipelines.Attribute;
	using MG.Pipelines.Attribute.Tests.Pipeline1.Tasks;

	[Pipeline("TestPipeline1", typeof(PipelineArguments1), typeof(Pipeline1Task1), typeof(Pipeline1Task2))]
	public class TestPipeline1 : Pipeline<PipelineArguments1>
	{
		protected override void Log(Exception caughtException, string message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		public TestPipeline1(IList<IPipelineTask<PipelineArguments1>> tasks)
			: base(tasks) {}
	}
}
