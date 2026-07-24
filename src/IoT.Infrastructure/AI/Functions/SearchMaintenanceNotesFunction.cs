// IoT.Infrastructure/AI/Functions/SearchMaintenanceNotesFunction.cs

using System.Text.Json;
using IoT.Interfaces.Repositories;
using IoT.Interfaces.Services;

namespace IoT.Infrastructure.AI.Functions;

public class SearchMaintenanceNotesFunction(
    IMaintenanceRecordRepository maintenanceRecordRepository,
    IEmbeddingService embeddingService) : AIFunctionBase
{
    public override string Name => "search_maintenance_notes";
    public override string Description => "Searches maintenance records by semantic similarity to the query. Use when asked about device problems, maintenance history, or technician notes.";
    public override BinaryData Parameters => BinaryData.FromString("""
                                                                   {
                                                                       "type": "object",
                                                                       "properties": {
                                                                           "query": {
                                                                               "type": "string",
                                                                               "description": "The search query to find relevant maintenance notes"
                                                                           },
                                                                           "limit": {
                                                                               "type": "integer",
                                                                               "description": "Maximum number of results to return (default 5)"
                                                                           }
                                                                       },
                                                                       "required": ["query"]
                                                                   }
                                                                   """);

    public override async Task<string> ExecuteAsync(string arguments, CancellationToken ct = default)
    {
        using var doc = JsonDocument.Parse(arguments);
        var query = doc.RootElement.GetProperty("query").GetString()!;
        var limit = doc.RootElement.TryGetProperty("limit", out var limitProp) 
            ? limitProp.GetInt32() 
            : 5;

        var queryEmbedding = await embeddingService.GenerateEmbeddingAsync(query, ct);
        var records = await maintenanceRecordRepository.SearchByEmbeddingAsync(queryEmbedding, limit, ct);

        var result = records.Select(r => new
        {
            deviceName = r.Device.Name,
            notes = r.Notes,
            performedAt = r.PerformedAt,
            technicianId = r.TechnicianId
        });

        return SerializeResult(result);
    }
}