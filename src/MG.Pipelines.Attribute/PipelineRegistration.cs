namespace MG.Pipelines.Attribute
{
	using System;

	public class PipelineRegistration
	{
		/// <summary>
		/// Gets or sets the type of the pipeline.
		/// </summary>
		/// <value>
		/// The type of the pipeline.
		/// </value>
		public Type PipelineType { get; set; }

		/// <summary>
		/// Gets or sets the attribute.
		/// </summary>
		/// <value>
		/// The attribute.
		/// </value>
		public PipelineAttribute Attribute { get; set; }
	}
}