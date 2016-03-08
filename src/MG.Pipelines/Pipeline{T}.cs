namespace MG.Pipelines
{
	using System;
	using System.Collections.Generic;

	public abstract class Pipeline<T> : IPipeline<T>
	{
		protected Pipeline(IList<IPipelineTask<T>> tasks)
		{
			Tasks = tasks;
		}

		#region IPipeline<T> Members

		/// <summary>
		/// Gets the tasks.
		/// </summary>
		/// <value>
		/// The tasks.
		/// </value>
		public IList<IPipelineTask<T>> Tasks { get; private set; }

		/// <summary>
		/// Executes the pipeline with the specified arguments
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns/>
		/// <exception cref="PipelineException">Thrown if there was an unhandled exception during pipeline processing</exception>
		public PipelineResult Execute(T args)
		{
			var isAborted = false;
			Exception exception = null;
			var pipelineResult = PipelineResult.Ok;
			var executedTasks = new List<IPipelineTask<T>>();
			foreach (var pipelineTask in Tasks)
			{
				try
				{
					executedTasks.Add(pipelineTask);

					var taskResult = pipelineTask.Execute(args);
					if (taskResult == PipelineResult.Warn)
					{
						pipelineResult = taskResult;
					}
					else if (taskResult > PipelineResult.Warn)
					{
						pipelineResult = taskResult;
						isAborted = true;
						break;
					}
				}
				catch (Exception ex)
				{
					exception = ex;
					var str = string.Format(
						"Exception occoured while processing pipeline '{0}'. See inner exception for details.",
						this.GetType().FullName);
					this.Log(ex, str);
					isAborted = true;
					break;
				}
			}

			if (isAborted)
			{
				try
				{
					this.Undo(executedTasks, args);
				}
				catch (Exception ex)
				{
					exception = ex;
					var str1 = string.Format(
						"Exception occoured while undoing pipeline '{0}'. See inner exception for details.",
						this.GetType().FullName);
					this.Log(exception, str1);
				}
			}

			if (exception != null)
			{
				var str2 = string.Format(
					"Exception occoured while processing pipeline '{0}'. See inner exception for details.",
					this.GetType().FullName);
				throw new PipelineException(str2, exception);
			}

			return pipelineResult;
		}

		#endregion

		/// <summary>
		/// Logs the specified caught exception.
		/// </summary>
		/// <param name="caughtException">The caught exception.</param>
		/// <param name="message">The message.</param>
		protected abstract void Log(Exception caughtException, string message);

		/// <summary>
		/// Attempts to undo the specified tasks.
		/// </summary>
		/// <param name="executedTasks">The executed tasks.</param>
		/// <param name="args">The arguments.</param>
		protected virtual void Undo(IList<IPipelineTask<T>> executedTasks, T args)
		{
			for (var i = executedTasks.Count - 1; i >= 0; i--)
			{
				var task = executedTasks[i] as IUndoablePipelineTask<T>;
				if (task != null)
				{
					task.Undo(args);
				}
			}
		}
	}
}