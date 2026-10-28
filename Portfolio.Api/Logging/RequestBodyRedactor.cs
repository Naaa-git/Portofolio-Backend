using System.Text.Json;
using System.Text.Json.Nodes;

namespace Portfolio.Api.Logging;

// Generic, name-based redaction — not per-DTO — so a future DTO with a
// sensitive field is covered automatically as long as it uses one of these
// names, same as every sensitive field already in the codebase does
// (Password/CurrentPassword/NewPassword/Code/IdToken/PendingToken/Secret).
public static class RequestBodyRedactor
{
    private static readonly HashSet<string> SensitiveFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "password", "currentpassword", "newpassword", "code", "idtoken", "pendingtoken", "secret",
    };

    private const int MaxLength = 4000;

    public static string Redact(string rawJson)
    {
        JsonNode? node;
        try
        {
            node = JsonNode.Parse(rawJson);
        }
        catch (JsonException)
        {
            // Never log raw content we're not sure about the shape of —
            // safer to show nothing than risk a secret slipping through
            // because it didn't parse the way we expected.
            return "(unparseable body)";
        }

        RedactNode(node);
        var result = node?.ToJsonString() ?? "";

        return result.Length > MaxLength ? result[..MaxLength] + "...(truncated)" : result;
    }

    private static void RedactNode(JsonNode? node)
    {
        switch (node)
        {
            case JsonObject obj:
                foreach (var key in obj.Select(kv => kv.Key).ToList())
                {
                    if (SensitiveFields.Contains(key))
                        obj[key] = "***REDACTED***";
                    else
                        RedactNode(obj[key]);
                }
                break;
            case JsonArray arr:
                foreach (var item in arr)
                    RedactNode(item);
                break;
        }
    }
}
