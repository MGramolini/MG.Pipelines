namespace MG.Pipelines.Web.Unity
{
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.Practices.Unity;

	public class UnityPipelineFactory: IPipelineFactory
	{
		private readonly IUnityContainer container;

		private readonly IPipelineNameResolver nameResolver;

		public UnityPipelineFactory(IUnityContainer container, IPipelineNameResolver nameResolver)
		{
			this.container = container;
			this.nameResolver = nameResolver;
		}

		public IEnumerable<string> AllPipelinesFor<T>()
		{
			return container.Registrations.Where(r => r.RegisteredType == typeof(IPipeline<T>)).Select(r => r.Name);
		}

		public IPipeline<T> Create<T>(string name)
		{
			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var pipelineName in nameResolver.ResolveNames(name))
			{
				var pipeline = container.Resolve<IPipeline<T>>(pipelineName);
				if (pipeline != null)
				{
					return pipeline;
				}
			}

			return null;
		}
	}
}
