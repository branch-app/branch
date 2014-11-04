namespace Branch.Models.Sql
{
	public interface IGameSpecificIdentity
	{
		string PlayerModelUrl { get; set; }

		string ServiceTag { get; set; }
	}
}
