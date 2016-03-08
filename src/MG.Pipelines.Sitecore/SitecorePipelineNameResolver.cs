namespace MG.Pipelines.Sitecore
{
	using System;
	using System.Collections.Generic;

	public class SitecorePipelineNameResolver : IPipelineNameResolver
	{
		/// <summary>
		/// Resolves the possible pipeline names.
		/// </summary>
		/// <param name="localName">The local name.</param>
		/// <returns/>
		public IList<string> ResolveNames(string localName)
		{
			var siteName = global::Sitecore.Context.GetSiteName();

			var split = localName.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

			if (split.Length > 1)
			{
				return new[] { localName };
			}

			return new[] { string.Concat(localName, ":", siteName), localName };
		}
	}
}
