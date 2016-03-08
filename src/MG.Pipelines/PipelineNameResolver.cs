namespace MG.Pipelines
{
	using System.Collections.Generic;

	public class PipelineNameResolver : IPipelineNameResolver
	{
		/// <summary>
		/// Resolves the possible pipeline names.
		/// </summary>
		/// <param name="localName">The local name.</param>
		/// <returns/>
		public IList<string> ResolveNames(string localName)
		{
			return new[] { localName };
		}
	}
}
