namespace FamilySafety.Application.Common;

/// <summary>
/// Generic result wrapper for operation outcomes
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Error { get; }
    public IEnumerable<string> Errors { get; }

    private Result(bool isSuccess, T? data, string? error, IEnumerable<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
        Errors = errors ?? [];
    }

    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(string error) => new(false, default, error);
    public static Result<T> Failure(IEnumerable<string> errors) => new(false, default, errors.FirstOrDefault(), errors);
}

/// <summary>
/// Non-generic result wrapper
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public string? Error { get; }
    public IEnumerable<string> Errors { get; }

    private Result(bool isSuccess, string? error, IEnumerable<string>? errors = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        Errors = errors ?? [];
    }

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
    public static Result Failure(IEnumerable<string> errors) => new(false, errors.FirstOrDefault(), errors);
}

/// <summary>
/// Paginated list for query results
/// </summary>
public class PaginatedList<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedList(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public static PaginatedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
