namespace MG.Pipelines
{
	using System.Collections.Generic;

	public interface IPipelineNameResolver
	{
		/// <summary>
		/// Resolves the possible pipeline names.
		/// </summary>
		/// <param name="localName">The local name.</param>
		/// <returns/>
		IList<string> ResolveNames(string localName);
	}
}
