namespace AtelierRex.Core;

public enum ErrorCode
{
    // Parser errors
    UnknownFormat,
    MalformedHeader,
    UnexpectedEndOfFile,
    InvalidChunkSize,
    ChunkBoundaryViolation,

    // Interpreter errors
    UnknownChunkType,
    IncompatibleVersion,
    CorruptPayload,

    // Operation errors
    FileNotFound,
    AccessDenied,
    OperationCancelled,

    // Plugin errors
    PluginLoadFailure,
    CapabilityViolation,
    PluginSecurityViolation,
}
