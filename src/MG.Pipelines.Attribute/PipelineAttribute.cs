namespace MG.Pipelines.Attribute
{
	using System;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class PipelineAttribute : Attribute
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the pipeline tasks.
		/// </summary>
		/// <value>
		/// The pipeline tasks.
		/// </value>
		public Type[] PipelineTasks { get; private set; }

		/// <summary>
		/// Gets the type of the argument.
		/// </summary>
		/// <value>
		/// The type of the argument.
		/// </value>
		public Type ArgumentType { get; private set; }

		/// <summary>
		/// Gets the type of the task.
		/// </summary>
		/// <value>
		/// The type of the task.
		/// </value>
		public Type TaskType { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PipelineAttribute"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="argumentType">Type of the argument.</param>
		/// <param name="pipelineTasks">The pipeline tasks.</param>
		public PipelineAttribute(string name, Type argumentType, params Type[] pipelineTasks)
		{
			Name = name;

			ArgumentType = argumentType;

			TaskType = typeof(IPipelineTask<>).MakeGenericType(argumentType);

			PipelineTasks = pipelineTasks;
		}
	}
}
