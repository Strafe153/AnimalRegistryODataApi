namespace Domain.Shared;

public record ProblemDetails(
	string Type,
	string Title,
	int Status,
	string Instance,
	string? Detail,
	IEnumerable<Error>? ValidationErrors);
