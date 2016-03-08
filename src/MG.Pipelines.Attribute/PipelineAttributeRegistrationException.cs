namespace MG.Pipelines.Attribute
{
	using System;

	public class PipelineAttributeRegistrationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PipelineAttributeRegistrationException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public PipelineAttributeRegistrationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PipelineAttributeRegistrationException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public PipelineAttributeRegistrationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
