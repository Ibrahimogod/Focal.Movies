using System.Data;
using System.Globalization;
using System.Linq.Expressions;
using CsvHelper;
using Focal.Movies.API.Extensions;

namespace Focal.Movies.API.Services;

public abstract class CsvService<TModel> : IHostedService, IDisposable
    where TModel : class, new()
{
    private readonly string _filePath;
    private readonly DataTable _dataTable;
    private bool _fileWrote;

    protected CsvService(string filePath, IEnumerable<ColumnConfiguration> columnConfigurations)
    {
        _filePath = filePath;
        _dataTable = new DataTable();
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        using var dr = new CsvDataReader(csv);
        foreach (var configuration in columnConfigurations)
        {
            var column = _dataTable.Columns.Add(configuration.Name);
            column.DataType = configuration.DataType;
            column.AutoIncrement = configuration.AutoIncrement;
            column.AutoIncrementSeed = configuration.AutoIncrementSeed;
            column.AutoIncrementStep = configuration.AutoIncrementStep;
        }
        _dataTable.Load(dr);
    }

    public IEnumerable<TModel> GetAll<TProperty>(
        Expression<Func<TModel, bool>>? predicate = null,
        Expression<Func<TModel, TProperty>>? sortExpression = null,
        bool ascending = true)
    {
        var filterExpression = predicate?.ToFilterExpression();
        var sort = sortExpression?.ToSortExpression(ascending);
        return _dataTable
            .Select(filterExpression: filterExpression, sort: sort)
            .ToList<TModel>();
    }

    public TModel? FirstOrDefault<TProperty>(
        Expression<Func<TModel, bool>>? predicate = null,
        Expression<Func<TModel, TProperty>>? sortExpression = null,
        bool ascending = true)
    {
        var filterExpression = predicate?.ToFilterExpression();
        var sort = sortExpression?.ToSortExpression(ascending);
        return _dataTable
            .Select(filterExpression: filterExpression, sort: sort)
            .ToList<TModel>()
            .FirstOrDefault();
    }

    public void Add(TModel model)
    {
        _dataTable.AddRow(model);
    }
    

    public void Dispose()
    {
        WriteCsv();
    }
    
    private void WriteCsv()
    {
        if (_fileWrote)
        {
            return;
        }
        using var stream = new FileStream(_filePath, FileMode.Truncate);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        foreach (DataColumn column in _dataTable.Columns)
        {
            csv.WriteField(column.ColumnName);
        }

        csv.NextRecord();

        foreach (DataRow row in _dataTable.Rows)
        {
            foreach (DataColumn column in _dataTable.Columns)
            {
                csv.WriteField(row[column]);
            }

            csv.NextRecord();
        }

        _fileWrote = true;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        WriteCsv();
        return Task.CompletedTask;
    }
}

public class ColumnConfiguration
{
    public required string Name { get; init; }
    public required Type DataType { get; init; }
    public  required bool AutoIncrement { get; init; }
    public required int AutoIncrementSeed { get; init; }
    public required int AutoIncrementStep { get; init; }
}