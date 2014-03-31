namespace Branch.Models.Services.Halo4
{
	public enum ResponseCode
	{
		Okay = 0,
		PlayerFound = 1,
		InvalidGamertag = 3,
		PlayerHasNotPlayedHalo4 = 4
	}

	public enum CsrType
	{
		Team,
		Individual,
		Unknown
	}
}