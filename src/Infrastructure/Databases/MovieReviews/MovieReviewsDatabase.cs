namespace CleanMinimalApi.Infrastructure.Databases.MovieReviews;

using Microsoft.Data.Sqlite;

/// <summary>
/// The connection details of the movie reviews database.
/// </summary>
/// <remarks>
/// A shared in-memory SQLite database, such as <c>Data Source=Movies;Mode=Memory;Cache=Shared</c>, only lives while a
/// connection to it is open, so this keeps one open for as long as the application runs.
/// </remarks>
internal sealed class MovieReviewsDatabase(string connectionString) : IDisposable
{
    private SqliteConnection? keepAlive;

    public string ConnectionString { get; } = connectionString;

    public bool IsInMemory => new SqliteConnectionStringBuilder(this.ConnectionString).Mode == SqliteOpenMode.Memory;

    public void KeepAlive()
    {
        if (this.IsInMemory && this.keepAlive is null)
        {
            this.keepAlive = new SqliteConnection(this.ConnectionString);
            this.keepAlive.Open();
        }
    }

    public void Dispose()
    {
        this.keepAlive?.Dispose();
        this.keepAlive = null;
    }
}
