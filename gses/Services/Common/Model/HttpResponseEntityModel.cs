namespace Gses.Services.Common.Model
{
	public class HttpResponseEntityModel<TEntity>
		where TEntity : class
	{
		public bool Success { get; set; }

		public string? Message { get; set; }

		public TEntity? Entity { get; set; }
	}
}
