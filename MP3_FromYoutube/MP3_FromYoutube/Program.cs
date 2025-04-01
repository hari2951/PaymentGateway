using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;

Console.WriteLine("Enter YouTube Video URL:");
var videoUrl = Console.ReadLine();

if (string.IsNullOrEmpty(videoUrl))
{
    Console.WriteLine("URL is empty. Exiting...");
    return;
}

try
{
    var youTube = YouTube.Default;
    var video = youTube.GetVideo(videoUrl);
    var outPutFile = "C:\\Users\\hari4\\Documents\\YouTubeToMP3";
    var filePath =Path.Combine(outPutFile, video.FullName);
    var outPutFilePath = Path.Combine(outPutFile, video.Title);



    System.IO.File.WriteAllBytes(filePath, video.GetBytes());

    var inputFile = new MediaFile { Filename = filePath };
    var outputFile = new MediaFile { Filename = $"{outPutFilePath}.mp3" };

    using (var engine = new Engine())
    {
        engine.GetMetadata(inputFile);
        engine.Convert(inputFile, outputFile);
    }

    Console.WriteLine($"Downloaded and converted to MP3: {outputFile.Filename}");
}
catch (Exception ex)
{
    Console.WriteLine("Error occurred: " + ex.Message);
}