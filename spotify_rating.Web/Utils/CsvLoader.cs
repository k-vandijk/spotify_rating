using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using spotify_rating.Web.Models;

namespace spotify_rating.Web.Utils;

public static class CsvLoader
{
    public static List<Record> LoadRecords(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        });

        csv.Context.RegisterClassMap<RecordMap>();

        return new List<Record>(csv.GetRecords<Record>());
    }
}