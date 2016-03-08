namespace MG.Pipelines.Web.Unity
{
	using Microsoft.Practices.Unity;

	public static class UnityStart
	{
		public static void Initialize(IUnityContainer container)
		{
			container.RegisterType<IPipelineFactory, UnityPipelineFactory>(new ContainerControlledLifetimeManager());
		}
	}
}
