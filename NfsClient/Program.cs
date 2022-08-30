using NFSLibrary;
using System.Net;
using System.Text.Json;

var sourceIpAddress = "172.24.131.49";
var rootDevice = "/";
var sourceFolderName = "nfsshare";
var LogFileName = "log.csv";

Stream? stream = new MemoryStream();
var nfs = new NFSClient(NFSClient.NFSVersion.v4);

nfs.Connect(IPAddress.Parse(sourceIpAddress));
List<string> devices = nfs.GetExportedDevices();
if (devices.Contains(rootDevice))
{
    var device = devices.First(x => x == rootDevice);
    nfs.MountDevice(device);
    var files = nfs.GetItemList(sourceFolderName);

    if (files.Contains(LogFileName))
        nfs.Read($"{sourceFolderName}\\{LogFileName}", ref stream);
    else
        stream = null;
    
    nfs.UnMountDevice();
}
nfs.Disconnect();

if(stream is not null)
{
    var result = ConvertLogStreamToLogList(stream);
    stream.Dispose();
    var jsonResult = JsonSerializer.Serialize(result);
    Console.WriteLine(jsonResult);
}

static List<LogLine> ConvertLogStreamToLogList(Stream stream)
{
    StreamReader streamReader = new(stream);
    var importingData = new List<LogLine>();
    string? line;
    stream.Position = 0;
    while ((line = streamReader.ReadLine()) != null)
    {
        var row = line.Split(',');

        importingData.Add(new LogLine
        {
            Field1 = row[0],
            Field2 = row[1],
            Field3 = row[2],
        });
    }
    return importingData;
}