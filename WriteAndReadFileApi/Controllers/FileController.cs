using System.Text;
using Microsoft.AspNetCore.Mvc;
using StreamReader = System.IO.StreamReader;

namespace WriteAndReadFileApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private const string Path = "/home/onenx/data.txt";

    [Route("write")]
    [HttpGet]
    public IActionResult WriteFile()
    {
        using (var sw = new StreamWriter(Path, false, Encoding.UTF8))
        {
            var rnd = new Random();
            foreach (var _ in Enumerable.Range(0, 1000000))
            {
                // TODO: rx.net?
                var line = Enumerable.Range(0, 27)
                    .Aggregate("", (current, _) => current + rnd.Next(1, 100) + " ");
                // TODO: async?
                sw.WriteLine(line);
            }
        }
        return Ok();
    }

    [Route("read")]
    [HttpGet]
    public ActionResult<int[][]> ReadFile()
    {
        ICollection<IEnumerable<int>> result = new List<IEnumerable<int>>();
        using var sr = new StreamReader(Path);
        //TODO async? rx.net?
        while (sr.ReadLine() is { } line)
        {
            var parts = line.Split(' ')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Convert.ToInt32(x));
            result.Add(parts);
        }

        return Ok(result);
    }
}
