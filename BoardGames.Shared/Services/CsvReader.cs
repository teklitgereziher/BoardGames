using BoardGames.Shared.Interfaces;
using BoardGames.Shared.Models.Csv;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.Reflection;

namespace BoardGames.Shared.Services
{
  public class CsvReader : ICsvReader
  {
    private readonly IWebHostEnvironment _env;

    public CsvReader(IWebHostEnvironment env)
    {
      _env = env;
    }

    public IEnumerable<BggRecord> Read()
    {
      var config = new CsvConfiguration(CultureInfo.InvariantCulture)
      {
        HasHeaderRecord = true,
        Delimiter = ",",
      };

      string filePath = Path.Combine(
        Path.GetDirectoryName(
          Assembly.GetExecutingAssembly().Location),
        @"Data/bgg_dataset.csv");
      //string filePath = "Data/bgg_dataset.csv";
      var reader = new StreamReader(filePath);
      var csv = new CsvHelper.CsvReader(reader, config);
      var records = csv.GetRecords<BggRecord>();

      return records;
    }
  }
}
