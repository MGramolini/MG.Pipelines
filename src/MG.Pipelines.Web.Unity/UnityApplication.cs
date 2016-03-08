namespace MG.Pipelines.Web.Unity
{
	using Microsoft.Practices.Unity;

	public class UnityApplication : System.Web.HttpApplication
	{
		public IUnityContainer Container { get; set; }

		public virtual void Application_Start()
		{
			Container = new UnityContainer();
			UnityStart.Initialize(Container);
		}
	}
}
