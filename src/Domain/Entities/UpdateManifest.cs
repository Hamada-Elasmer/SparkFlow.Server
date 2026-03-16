using System.Collections.Generic;

namespace SparkFlow.Server.Domain.Entities;

public record UpdateManifest(
    string Channel,
    string LatestVersion,
    string MinSupportedVersion,
    List<UpdateFile> Files
);

public record UpdateFile(
    string Name,
    string Url,
    string? Sha256
);