namespace MG.Pipelines.Attribute.Tests
{
	using MG.Pipelines.Attribute.Tests.Pipeline1;
	using MG.Pipelines.Attribute.Tests.Pipeline2;

	using NUnit.Framework;

	[SetUpFixture]
	public class SetUp
	{
		[SetUp]
		public void Setup()
		{
			Registration.RegisterPipelines();
		}
	}

	[TestFixture]
	public class PipelineTests
	{
		[Test]
		public void TestPipeline1()
		{
			var pipeline = PipelineFactory.Instance.Create<PipelineArguments1>("TestPipeline1");
			var args = new PipelineArguments1();
			pipeline.Execute(args);
		}

		[Test]
		public void TestPipeline2()
		{
			var pipeline = PipelineFactory.Instance.Create<PipelineArguments2>("TestPipeline2");
			var args = new PipelineArguments2();
			pipeline.Execute(args);
		}

		[Test]
		public void Create100PipelinesTime()
		{
			for (var i = 0; i < 100; i++)
			{
				var pipeline = PipelineFactory.Instance.Create<PipelineArguments1>("TestPipeline1");
			}
		}
	}
}
