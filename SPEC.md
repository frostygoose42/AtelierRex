# Atelier Rex — Formal Specification

> *A craftsman's studio for digital restoration*

---

## Document Status

This is a living document. It will be updated as the project evolves and new requirements emerge. All changes are governed by the core architectural principle: **dependencies always flow inward.**

---

## Table of Contents

1. [Vision](#vision)
2. [Problem Statement](#problem-statement)
3. [Guiding Principles](#guiding-principles)
4. [License and Distribution](#license-and-distribution)
5. [Monetization Model](#monetization-model)
6. [Architecture Overview](#architecture-overview)
7. [Core Layer](#core-layer)
8. [Infrastructure Layer](#infrastructure-layer)
9. [Domain Layer](#domain-layer)
10. [Application Layer](#application-layer)
11. [MediaVault Module](#mediavault-module)
12. [Revive Module](#revive-module)
13. [Presentation Layer](#presentation-layer)
14. [Plugin System](#plugin-system)
15. [Theme System](#theme-system)
16. [Testing Strategy](#testing-strategy)
17. [Solution Structure](#solution-structure)
18. [Implementation Roadmap](#implementation-roadmap)

---

## Vision

Software is humanity's newest cultural heritage. Games, applications, and digital experiences represent entire eras of creative and technical achievement — yet they vanish silently as the platforms that ran them become obsolete. Unlike physical artifacts, they leave no ruins. They simply stop working.

Atelier Rex is a suite of tools built on a single conviction: that any file, in any format, from any era, can be read, understood, and given new life — if you have the right workshop.

**The full arc of the project:**

```
Preserve    — capture physical media before it is lost
Understand  — parse and interpret what was captured
Compare     — find patterns and relationships
Restore     — make it run again on modern devices
Modify      — enable creative reimagination
Share       — distribute the results to the world
```

Every tool in the workshop serves one or more of these six verbs.

---

## Problem Statement

The tools that exist today for working with legacy binary formats are narrow, abandoned, undocumented, and disconnected from modern software pipelines. Each solves one problem in isolation. None share infrastructure. None are extensible. None speak to each other. The practitioner is left bridging gaps manually, losing fidelity and understanding at every boundary.

Atelier Rex addresses this by providing a unified, extensible platform for:

- Reading and parsing binary files of any format
- Interpreting the meaning of what was parsed
- Comparing and analyzing across files and formats
- Manipulating and transforming content
- Exporting to modern targets
- Preserving physical media before it is lost
- Making legacy software run on modern devices
- Enabling community modification of preserved works

The immediate application is the restoration of Hell Cab (1993), a Macromedia Director game whose only surviving form is a binary artifact on a disc image. The broader application is any artifact worth preserving.

---

## Guiding Principles

1. **Generic before specific** — core infrastructure knows nothing about Director, RIFF, or any specific format
2. **Parse before interpret** — strict separation of structure from meaning
3. **Non-destructive** — original files are never modified
4. **Extensible** — new formats and tools added without modifying core
5. **Testable** — every component verifiable against known inputs and outputs
6. **Security and stability first** — never trust input; fail fast, fail loudly
7. **Accessibility first** — when fidelity and accessibility conflict, accessibility wins
8. **No paywalled functionality** — every feature is free; only aesthetics are monetized
9. **Evolutionary design** — the architecture supports everything that comes after it

### Architectural Principle — Inward Dependencies

The single inviolable rule: **dependencies always flow inward.** A layer may depend on layers beneath it, never above it. Everything else is negotiable as the project evolves.

### Architectural Principle — Evolutionary Design

Every module is a foundation, not a ceiling. New hardware interfaces, formats, platforms, and tools will be added as the project evolves. No design decision should foreclose any reasonable future capability.

---

## License and Distribution

**License:** Fair Source — source available, free for personal and non-commercial use, contributions welcome, redistribution and commercialization prohibited without written permission from the author.

**Distribution:**
- Source: GitHub
- Releases: GitHub Releases + dedicated website
- Premium content: Patreon
- Community themes: Free gallery on project website

---

## Monetization Model

Atelier Rex is free. Every feature, every tool, every capability — free, forever. No paywalled functionality. No crippled free tier.

**What is monetized (Patreon):**
- Theme Editor (quality of life tool, not functional)
- Premium built-in themes (cosmetic only)
- Supporter recognition in UI
- Priority support
- Early access to new features
- Name in credits

**Important:** Even if the Theme Editor is Patreon-gated, hand-crafted `.arxtheme` files are importable by anyone for free. The community can share themes freely; patrons get the visual tool to create them more easily.

---

## Architecture Overview

```
┌─────────────────────────────────────────┐
│           Presentation Layer            │
│         (UI + CLI frontends)            │
├─────────────────────────────────────────┤
│           Application Layer             │
│     (orchestration, project state)      │
├─────────────────────────────────────────┤
│            Domain Layer                 │
│   (analysis, comparison, query engine)  │
├─────────────────────────────────────────┤
│          Infrastructure Layer           │
│  (parsing, interpretation, export)      │
├─────────────────────────────────────────┤
│              Core Layer                 │
│   (abstractions, interfaces, models)    │
└─────────────────────────────────────────┘
```

**Satellite Modules** (depend on Application, feed into Presentation):
- **AtelierRex.MediaVault** — physical media preservation
- **AtelierRex.Revive** — multi-platform restoration and modification
- **AtelierRex.PluginForge** — plugin authoring and validation

---

## Core Layer

**Responsibility:** Define the language the entire system speaks. Zero dependencies on any other layer. Zero knowledge of any specific file format, UI framework, or export target.

**Rule:** If a class in Core imports anything from Infrastructure, Domain, or Presentation, the architecture has been violated.

### Core Abstractions

```csharp
// The atomic unit of all parsed data
public interface IChunk
{
    FourCC Tag { get; }
    long Offset { get; }
    long Size { get; }
    ReadOnlyMemory<byte> Raw { get; }
    IReadOnlyList<IChunk> Children { get; }
    bool IsKnown { get; }
}

// A parsed file as a navigable structure
public interface IFile
{
    string Path { get; }
    FileFormat Format { get; }
    IChunk Root { get; }
    IReadOnlyList<IChunk> Chunks { get; }
    FileMetadata Metadata { get; }
}

// Identifies what format a file is
public interface IFormatDetector
{
    bool CanDetect(ReadOnlySpan<byte> header);
    FileFormat Detect(ReadOnlySpan<byte> header);
}

// Parses a file into an IFile
public interface IFileParser
{
    bool CanParse(FileFormat format);
    Result<IFile> Parse(Stream stream, ParseOptions options);
}

// Interprets the meaning of a chunk's payload
public interface IChunkInterpreter
{
    bool CanInterpret(FourCC tag, FileFormat format);
    Result<IInterpretedChunk> Interpret(IChunk chunk);
}

// Exports to a target format
public interface IExporter
{
    string TargetFormat { get; }
    Result Export(IFile file, ExportOptions options);
}

// Compares two parsed structures
public interface IDiffer
{
    DiffResult Diff(IFile left, IFile right, DiffOptions options);
    DiffResult Diff(IChunk left, IChunk right, DiffOptions options);
}

// Queries across one or more files
public interface IQueryEngine
{
    QueryResult Query(IQuery query, IEnumerable<IFile> files);
}
```

### Core Value Types

```csharp
public readonly struct FourCC
{
    public string Value { get; }
    public static FourCC Parse(ReadOnlySpan<byte> bytes);
    public static FourCC Parse(string value);
}

public readonly struct FileFormat
{
    public string Name { get; }
    public string Family { get; }
    public Version Version { get; }
    public Endianness Endianness { get; }
}

public readonly struct FileMetadata
{
    public long FileSize { get; }
    public DateTime LastModified { get; }
    public string Hash { get; }
    public FileFormat Format { get; }
    public int ChunkCount { get; }
}

public readonly struct Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public AtelierRexError Error { get; }
    public static Result<T> Success(T value);
    public static Result<T> Failure(AtelierRexError error);
}

public readonly struct AtelierRexError
{
    public ErrorCode Code { get; }
    public string Message { get; }
    public string Context { get; }
    public Exception? InnerException { get; }
}
```

### Core Error Codes

```csharp
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
    PluginSecurityViolation
}
```

### Parse Options

```csharp
public record ParseOptions
{
    public bool GracefulDegradation { get; init; } = true;
    public int MaxDepth { get; init; } = 64;
    public long MaxFileSize { get; init; } = 1024 * 1024 * 512; // 512MB
    public bool IncludeRawBytes { get; init; } = true;
    public bool InterpretOnParse { get; init; } = false;
    public CancellationToken CancellationToken { get; init; }
}
```

---

## Infrastructure Layer

**Responsibility:** Implement Core interfaces using concrete technology. Knows about file systems, binary formats, and specific chunk types. Does not know about Domain, Application, or Presentation.

### Structure

```
AtelierRex.Infrastructure
├── IO
│   ├── FileSystem abstraction
│   ├── StreamPool
│   └── BinaryReader extensions
├── Parsing
│   ├── BinaryParser
│   ├── FormatRegistry
│   ├── ChunkRegistry
│   └── ParsePipeline
├── Formats
│   ├── Director
│   │   ├── DirectorFormatDetector
│   │   ├── DirectorFileParser
│   │   └── Chunks (MmapChunk, CastChunk, LscriptChunk, BitmapChunk, SoundChunk, ScoreChunk)
│   └── Riff
│       ├── RiffFormatDetector
│       ├── RiffFileParser
│       └── Chunks (FmtChunk, DataChunk, ListChunk)
├── Interpreters
│   ├── Director (LingoInterpreter, BitmapInterpreter, SoundInterpreter, ScoreInterpreter)
│   └── Riff (WaveInterpreter, AviInterpreter)
└── Export
    ├── GodotExporter
    ├── AssetExporter
    └── JsonExporter
```

### Binary Parser Safety Rules

- Every read is bounds-checked before execution
- Chunk size validated against remaining stream length
- Nesting depth enforced against MaxDepth option
- File size validated against MaxFileSize option
- All reads use `SequenceReader<byte>`
- No unsafe code blocks
- All errors produce typed `Result<T>` failures, never exceptions

### Built-in Registered Formats

```
Director Movie          .MMM .DIR
Director Protected      .DXR .CXT
Director Projector      .SKL
RIFF Wave Audio         .WAV
RIFF AVI Video          .AVI
Windows Bitmap          .BMP
Plain Text              .TXT .INI .WRI
```

### Director Chunk Types

```
Structural:  RIFX, imap, mmap, KEY*
Cast:        CAS*, CASt, STXT
Score:       VWSC
Lingo:       Lscr, Lnam, LctX
Media:       BITD, CLUT, snd, WAVE, PICT
Metadata:    VWFI, VWCF, DRCF
```

---

## Domain Layer

**Responsibility:** Business logic built on Core abstractions. Analysis, comparison, querying, and project modeling. No UI. No file system access beyond delegation.

### Structure

```
AtelierRex.Domain
├── Analysis
│   ├── AnalysisPipeline
│   ├── Analyzers (ChunkFrequency, FormatProfile, Dependency, Integrity)
│   └── Reports
├── Comparison
│   ├── Differ
│   ├── DiffResult / DiffNode
│   └── Strategies (Structural, Content, Semantic)
├── Query
│   ├── QueryEngine
│   ├── QueryBuilder (fluent API)
│   └── Expressions
├── Project
│   ├── AtelierProject
│   ├── ProjectFile
│   ├── ProjectFileTree
│   └── ProjectHistory
└── Knowledge
    ├── FormatKnowledgeBase
    ├── ChunkKnowledgeEntry
    └── KnowledgeConfidence (Confirmed, HighConfidence, Probable, Speculative)
```

### Diff Node Types

```csharp
public enum DiffNodeType
{
    Identical, Modified, Added, Removed, Moved, TypeChanged
}
```

### Three Diff Strategies

- **Structural** — chunk tree topology only, ignores payload, fast
- **Content** — raw payload bytes, byte for byte, comprehensive
- **Semantic** — interpreted meaning, understands what changed

### Query Builder Example

```csharp
// Find all Lingo scripts across all Hell Cab MMM files
var query = new QueryBuilder()
    .WithChunkType(FourCC.Parse("Lscr"))
    .InFormat(FileFormats.Director)
    .Build();
```

### Knowledge Confidence

```csharp
public enum KnowledgeConfidence
{
    Confirmed,      // verified against official documentation
    HighConfidence, // verified empirically against many files
    Probable,       // reasonable inference from limited data
    Speculative     // educated guess, needs verification
}
```

---

## Application Layer

**Responsibility:** Orchestrate Domain and Infrastructure. Bridge between raw capabilities and user intent. Manage session state, command history, and plugin lifecycle.

**Pattern:** CQRS — commands change state, queries read state. They never overlap.

### Structure

```
AtelierRex.Application
├── Commands (ICommand, CommandDispatcher, CommandHistory)
│   └── OpenFile, ParseFile, AnalyzeFile, DiffFiles, Query,
│       Export, CloseFile, OpenDirectory, OpenDiscImage,
│       OpenPhysicalDisc, CreateDiscImage, ParseProject,
│       AnalyzeProject, DiffDirectories
├── Queries (IApplicationQuery, QueryDispatcher)
│   └── GetProjectFiles, GetAnalysisResult, GetDiffResult, GetKnowledge
├── Services
│   ├── ProjectService
│   ├── ParseService (single + bulk)
│   ├── AnalysisService (single + bulk)
│   ├── DiffService (single + bulk)
│   ├── ExportService (single + bulk)
│   ├── DiscService (physical media)
│   └── PluginService
├── Events (EventBus)
│   └── FileOpened, FileParsed, AnalysisComplete, DiffComplete,
│       ExportComplete, DirectoryOpened, DiscImageMounted,
│       DiscInserted, DiscRemoved, BulkOperationStarted,
│       BulkOperationComplete
├── Session
│   ├── SessionState
│   └── SessionSettings
└── Plugins
    ├── PluginLoader
    ├── PluginRegistry
    ├── PluginSandbox
    ├── PluginAuditLog
    └── CapabilityEnforcer
```

### Bulk Operations

All services support bulk operations against directories, disc images, and file collections:

```csharp
public record DirectoryAddOptions
{
    public bool Recursive { get; init; } = true;
    public IReadOnlyList<string> IncludePatterns { get; init; }
    public IReadOnlyList<string> ExcludePatterns { get; init; }
    public bool ParseOnAdd { get; init; } = false;
    public DuplicateHandling DuplicateHandling { get; init; }
}

public record BulkOperationProgress
{
    public int TotalFiles { get; init; }
    public int ProcessedFiles { get; init; }
    public string CurrentFile { get; init; }
    public double PercentComplete { get; init; }
    public TimeSpan Elapsed { get; init; }
    public TimeSpan EstimatedRemaining { get; init; }
}
```

### Physical Disc Support

```csharp
public class DiscService
{
    public Result<IReadOnlyList<OpticalDrive>> GetDrives();
    public IObservable<DiscEvent> DriveEvents { get; }
    public Task<Result<DiscInfo>> ReadDiscInfoAsync(...);
    public Task<Result<BulkParseResult>> AddDiscAsync(...);
    public Task<Result<DiscImageResult>> CreateImageAsync(...);
}
```

### Supported Disc File Systems

```csharp
public enum DiscFileSystem
{
    ISO9660, Joliet, UDF, HFS, HFSPlus, Mixed, Unknown
}
```

Note: HFS/HFS+ support required for Mac-era hybrid discs.

### Session Settings

```csharp
public record SessionSettings
{
    public string ThemeId { get; init; }
    public string Language { get; init; }
    public ParseOptions DefaultParseOptions { get; init; }
    public bool AutoParseOnOpen { get; init; }
    public bool AutoAnalyzeOnParse { get; init; }
    public int MaxRecentFiles { get; init; }
    public string DefaultExportPath { get; init; }
}
```

---

## MediaVault Module

**Responsibility:** Preserve physical media in all forms. Image, catalogue, and feed parsed results into the core workshop.

### Supported Physical Media

**Optical (implemented first):**
- CD-ROM, CD-R/RW, CD Audio, CD Mixed Mode
- CD-i, Video CD, Photo CD, Enhanced CD
- DVD-ROM, DVD-R/RW/+R/+RW, DVD-RAM, DVD-Audio, DVD-Video
- Blu-ray ROM, Blu-ray R/RE
- HD-DVD (limited)
- Hybrid Mac/PC discs (HFS + ISO 9660)

**Magnetic (future milestone):**
- Floppy 3.5", 5.25", 8"
- Zip Drive, Jaz Drive, Tape

**Solid State (future milestone):**
- CompactFlash, SmartMedia, Memory Stick, SD Card

**Cartridge (future milestone):**
- Game Boy/GBC/GBA, SNES, NES, Sega Genesis, N64
- Compatible hardware readers: GB Operator, GBxCart RW, Retrode 2, INLretro

### Supported Image Formats

```csharp
public enum ImageFormat
{
    ISO,      // most compatible
    BIN_CUE,  // preserves track structure, best for games
    NRG,      // Nero legacy format
    MDF_MDS,  // Alcohol 120% legacy format
    IMG,      // raw sector image
    CDI,      // DiscJuggler
}
```

### Media Catalogue

Every imaged disc is catalogued with:
- Fingerprint (SHA256, SHA1, MD5)
- Volume metadata
- Track structure
- Imaging options used
- Verification result
- Optional match against Redump / TOSEC / No-Intro databases

### The MediaToParsePipeline

Bridge between MediaVault and core workshop:

```
Physical Disc / Image File
        ↓
MediaVault (image + catalogue)
        ↓
ParsePipeline (parse all files)
        ↓
AtelierProject (structured project)
```

### Structure

```
AtelierRex.MediaVault
├── Detection (DriveDetector, MediaTypeIdentifier, DriveCapabilityProbe)
├── Imaging (ImagingEngine, ImageVerifier, ErrorRecovery, Format Writers)
├── Catalogue (MediaCatalogue, CatalogueEntry, MediaFingerprint)
├── FileSystem (ISO9660, Joliet, UDF, HFS, HFSPlus, RedBookAudio, VideoTS)
├── Recovery (BadSectorHandler, ReadRetryStrategy, PartialImageRecovery)
├── Cartridge (CartridgeReaderRegistry, ROM format readers)
└── Pipeline (MediaToParsePipeline)
```

---

## Revive Module

**Responsibility:** Make preserved software run again — on any device, for any audience, faithful to the original or reimagined for a new era.

### Three Modes

**Preservation Mode:** Maximum fidelity, minimum intervention. Run the original software as close to original form as possible via compatibility layers.

**Restoration Mode:** Fully native modern experience. Extract everything from the original and rebuild to run natively on the target platform.

**Modification Mode:** ROM hack equivalent. Structured workspace for community modifications built on preserved/restored foundations.

### Fidelity Levels

```csharp
public enum FidelityLevel
{
    StrictOriginal,     // pixel perfect, no changes
    AccessibilityFirst, // necessary changes for modern play (default)
    Enhanced,           // quality of life improvements
    Reimagined          // creative reinterpretation
}
```

### Target Platform Matrix

```csharp
public enum TargetPlatform
{
    // Desktop
    Windows_x64, Windows_ARM64,
    macOS_x64, macOS_AppleSilicon,
    Linux_x64, Linux_ARM64,

    // Web
    WebAssembly, HTML5, PWA,

    // Mobile
    iOS, iPadOS, Android,

    // Console
    SteamDeck, NintendoSwitch, PlayStation5, XboxSeries,

    // Libretro (covers almost everything else)
    Libretro
}
```

### Revival Engines

```csharp
public interface IRevivalEngine
{
    string Name { get; }
    IReadOnlyList<TargetPlatform> SupportedPlatforms { get; }
    Task<Result<TranslatedLogic>> TranslateLogicAsync(...);
    Task<Result<EngineProject>> GenerateProjectAsync(...);
    Task<Result<BuildResult>> BuildAsync(...);
}
```

**Built-in engines:**
- **GodotEngine** — translates Lingo → GDScript/C#, generates Godot 4 project
- **HTML5Engine** — translates logic → JavaScript, broadest browser compatibility
- **LibretroEngine** — generates libretro core, runs on virtually every platform via RetroArch

### Distribution Package Formats

```
Desktop:  Windows Installer/Portable, macOS Bundle/DMG,
          Linux AppImage/Flatpak/Snap
Web:      WebBundle, PWA Manifest
Mobile:   iOS IPA, Android APK/AAB
Console:  Steam Deck Package, Nintendo Switch NSP
Libretro: Core ZIP
```

### ROM Hack Support

For cartridge-based games, Modification Mode produces standard patch formats:

```csharp
public enum ROMPatchFormat
{
    IPS, IPS32, BPS, UPS, XDELTA, VCDIFF, RUP
}
```

**ROM Hack Workspace tools:**
- Tile Editor, Sprite Editor, Map Editor
- Text Editor (translation/localization)
- Logic Patcher
- Patch Generator and Validator

### The Full Cartridge Pipeline

```
Physical Cartridge
        ↓
MediaVault (CartridgeReader hardware interface)
    — reads ROM via hardware reader
    — produces standard ROM image
    — catalogues with fingerprint
    — matches against No-Intro / TOSEC
        ↓
Core Workshop
    — parses ROM format
    — extracts assets and logic
        ↓
Revive — Preservation Mode
    — packages for modern emulators
    — libretro core output
        ↓
Revive — Modification Mode
    — ROM hack workspace
    — produces IPS/BPS/xdelta patch
    — patch applied to original ROM
```

### Revival Phases

```csharp
public enum RevivalPhase
{
    Analyzing, ExtractingAssets, MigratingAssets,
    TranslatingLogic, Assembling, Building, Packaging,
    Verifying, Complete
}
```

---

## Presentation Layer

**Responsibility:** Two independent frontends (UI + CLI) sharing one Application layer. No business logic. Views translate user intent into Application commands. Views react to Application events.

**Pattern:** MVVM throughout the UI. Every view has a corresponding ViewModel. Views contain zero logic. ViewModels contain zero UI code.

### UI Framework

**Avalonia UI** — chosen for:
- Truly cross-platform (Windows, macOS, Linux)
- Best-in-class theming and styling in .NET
- Strong community and active development

### Shell Layout

```
┌─────────────────────────────────────────────────────┐
│  TitleBar                                    [─][□][×]│
├─────────────────────────────────────────────────────┤
│  MenuBar: File  Edit  View  Tools  Help              │
├──┬──────────────────────────────────────────────────┤
│  │                                                    │
│A │         Primary Content Area                      │
│c │         (tabbed, splittable)                      │
│t │                                                    │
│i ├──────────────────────────────────────────────────┤
│v │         Secondary Panel                           │
│i │         (hex view, properties, output)            │
│t │                                                    │
│y ├──────────────────────────────────────────────────┤
│B │  StatusBar    [operation] [progress] [indicators] │
│a └──────────────────────────────────────────────────┤
│r │
└──┘
```

### Activity Bar

```
⬡  Project Explorer
🔍  Inspector
📊  Analysis
⇄  Comparison
⌕  Query
💿  MediaVault
⚡  Revive
📚  Knowledge Base
🔌  Plugins
⚙  Settings
```

### Key Custom Controls

- **HexEditor** — hex view with offset, bytes, and ASCII columns
- **ChunkTreeView** — navigable chunk hierarchy
- **BinaryViewer** — raw binary visualization
- **DiffViewer** — side-by-side diff with change indicators
- **GraphViewer** — dependency graph visualization
- **ProgressRing** — operation progress indicator

### CLI

```bash
# Examples
atelier parse HELL.MMM --output json
atelier list-chunks HELL.MMM --format tree
atelier diff ESBN.MMM ESBS.MMM --strategy structural
atelier query --type Lscr --in ./HCDATA/**/*.MMM
atelier image-disc D: --output ./images/hellcab.iso --verify
atelier restore HELL.MMM --engine godot --target windows
atelier parse ./HCDATA --recursive --format json
atelier catalogue list
atelier catalogue search "Hell Cab"
```

---

## Plugin System

**Responsibility:** Allow external extensibility without compromising security.

### What Can Be Extended (via plugins)

- Format detection and parsing
- Chunk interpretation
- Export targets
- Analysis operations
- Workshop tools

Note: **Themes are NOT plugins.** They are a separate system with different rules. See Theme System.

### Security Model

- Source code required for all plugins — no binary-only plugins
- Plugins compiled locally by Plugin Developer Tool
- Capability declaration required at install time
- Runtime enforcement of declared capabilities
- File system access via Atelier Rex APIs only — no direct System.IO
- No network access permitted
- No process spawning permitted
- Full audit log of all plugin operations

### Plugin Capabilities

```csharp
public enum PluginCapability
{
    ReadFiles,
    WriteFiles,
    AddUIPanel,
    RegisterFormat,
    RegisterInterpreter,
    RegisterExporter,
    RegisterAnalyzer
}
```

### Plugin Lifecycle

1. Author writes plugin using Plugin Developer Tool (PluginForge)
2. Source is made publicly available
3. User downloads source
4. PluginForge compiles and validates locally
5. User reviews and approves capability declarations
6. Plugin enabled and sandboxed
7. All operations logged for transparency

### PluginForge

A first-class workshop tool for authoring, validating, testing, and distributing plugins. Available to all users free of charge. Contains:

- Project templates for each plugin type
- Capability declaration editor
- Local compiler and validator
- Security scanner (detects prohibited API calls)
- Test harness
- Registry submission tool

---

## Theme System

**Responsibility:** Define the visual identity of Atelier Rex. Pure presentation data — no executable code, no security concerns.

**Design Philosophy:** Retro aesthetic with modern quality of life. The tool should feel like it belongs to the world it's restoring, while never fighting the user.

### Theme Tiers

**Free (shipped with Atelier Rex):**

| Theme | Description |
|-------|-------------|
| Phosphor | Classic green CRT terminal |
| Amber | Amber monochrome monitor |
| Midnight | Modern dark |
| Inkwell | Clean light |
| Win95 | Windows 95 design language |
| Platinum | Mac OS 8/9 design language |

**Patreon (supporter exclusive):**

| Theme | Description |
|-------|-------------|
| Director | Macromedia Director aesthetic |
| Trinitron | Early color CRT |
| Xerox | Early graphical workstation |

**Community:** `.arxtheme` format, open specification, importable by any user, shareable without restriction.

### Win95 Theme Specification

```
Background:         #C0C0C0
BackgroundSecondary:#FFFFFF
Accent:             #000080
AccentSecondary:    #1084D0
Font:               MS Sans Serif equivalent
Effects:            None (flat)
Special:            Beveled borders, title bar gradient,
                    chunky scrollbars, classic window chrome
```

### Platinum Theme Specification (Mac OS 8/9)

```
Background:         #CCCCCC
BackgroundSecondary:#FFFFFF
Accent:             #0000CC
Font:               Charcoal/Chicago equivalent
MonospaceFont:      Monaco equivalent
Effects:            Pinstripe on title bars, rounded corners
Special:            Apple menu bar at top
```

### The .arxtheme Format

An open, documented, human-readable JSON format. Any user can hand-craft a theme file without the Theme Editor. The Theme Editor (Patreon) is a convenience tool, not a gatekeeper.

### ITheme Interface

```csharp
public interface ITheme
{
    string Id { get; }
    string Name { get; }
    string Author { get; }
    ThemeTier Tier { get; }
    ThemePalette Palette { get; }
    ThemeTypography Typography { get; }
    ThemeEffects Effects { get; }
    ThemeDensity Density { get; }
}
```

---

## Testing Strategy

Security and stability are primary concerns. Tests are written alongside or before the code they verify.

### Testing Principles

1. Every public interface has tests
2. Every parser has corpus tests — known input produces known output
3. Every interpreter has round-trip tests
4. Every differ has symmetry tests
5. Every exporter has output validation
6. No regression without a test
7. Tests are documentation — names describe behavior in plain English

### Test Categories

**Unit Tests:** Single component in isolation, all dependencies mocked, milliseconds per test.

**Integration Tests:** Multiple components working together, real file system where appropriate, seconds per test.

**Corpus Tests:** Known binary files as fixed inputs, known outputs as fixed expectations. Hell Cab files are the primary corpus.

**Performance Tests:** Parser throughput benchmarks, memory consumption profiles.

### Security Testing Principles

1. Never trust input — all binary input treated as potentially malformed or malicious
2. Bounds checking everywhere — no unchecked array access in parsers
3. No arbitrary code execution — plugin system sandboxed
4. Immutable parsed output — parsed structures cannot be modified after construction
5. Explicit error handling — no silent failures
6. No external network calls — entirely offline, no telemetry

### Stability Testing Principles

1. Fail fast, fail loudly — clear error better than silent corruption
2. Graceful degradation — unrecognized chunk skipped and reported, not crashed
3. Idempotent operations — same operation twice produces same result
4. Atomic file operations — writes complete fully or not at all
5. Versioned data models — parsed output format is versioned

---

## Solution Structure

```
AtelierRex.sln
├── Core
│   └── AtelierRex.Core
├── Infrastructure
│   ├── AtelierRex.Infrastructure
│   ├── AtelierRex.Formats.Director
│   └── AtelierRex.Formats.Riff
├── Domain
│   └── AtelierRex.Domain
├── Application
│   └── AtelierRex.Application
├── MediaVault
│   └── AtelierRex.MediaVault
├── Revive
│   ├── AtelierRex.Revive.Core
│   ├── AtelierRex.Revive.Engines.Godot
│   ├── AtelierRex.Revive.Engines.HTML5
│   └── AtelierRex.Revive.Engines.Libretro
├── Export
│   ├── AtelierRex.Export.Godot
│   └── AtelierRex.Export.Assets
├── Presentation
│   ├── AtelierRex.UI
│   └── AtelierRex.CLI
├── PluginForge
│   └── AtelierRex.PluginForge
└── Tests
    ├── AtelierRex.Tests.Core
    ├── AtelierRex.Tests.Infrastructure
    ├── AtelierRex.Tests.Domain
    ├── AtelierRex.Tests.Application
    ├── AtelierRex.Tests.MediaVault
    ├── AtelierRex.Tests.Revive
    ├── AtelierRex.Tests.UI
    └── AtelierRex.Tests.Integration
```

---

## Implementation Roadmap

### v0.1 — Foundation

**Goal:** Parse a Hell Cab .MMM file and produce a structured chunk map.

- [ ] AtelierRex.Core — all interfaces and value types
- [ ] AtelierRex.Infrastructure — BinaryParser, FormatRegistry, ChunkRegistry
- [ ] AtelierRex.Formats.Director — basic chunk walking (no interpretation)
- [ ] AtelierRex.Tests.Core — full coverage
- [ ] AtelierRex.Tests.Infrastructure — corpus tests against Hell Cab files
- [ ] SPEC.md at solution root

**Done when:** Given any Hell Cab .MMM file, produce a complete structured map of every chunk — tag, offset, size, and raw payload.

### v0.2 — Understanding

**Goal:** Interpret what the chunks mean.

- [ ] Director chunk interpreters (Lingo, Bitmap, Sound, Score)
- [ ] FormatKnowledgeBase populated with Director chunk knowledge
- [ ] AtelierRex.Domain — Differ, QueryEngine
- [ ] CLI — parse, list-chunks, query commands

**Done when:** Given HELL.MMM, extract all Lingo scripts as readable source.

### v0.3 — Workshop

**Goal:** Full working desktop application.

- [ ] AtelierRex.Application — all services, commands, events
- [ ] AtelierRex.UI — shell, core views, hex editor, chunk tree
- [ ] Theme system — all six free built-in themes
- [ ] Project model — open, save, load

**Done when:** Open Hell Cab's HCDATA folder, browse all files, inspect any chunk, run queries.

### v0.4 — MediaVault

**Goal:** Physical media support.

- [ ] Optical drive detection
- [ ] Disc imaging (ISO, BIN/CUE)
- [ ] Media catalogue
- [ ] MediaToParsePipeline

**Done when:** Insert Hell Cab disc, image it, parse all files, catalogue it.

### v0.5 — Revive (Preservation)

**Goal:** Run Hell Cab on modern Windows via automated compatibility shims.

- [ ] CompatibilityAnalyzer
- [ ] ShimGenerator
- [ ] PreservationPackager (Windows target first)

**Done when:** One-click Hell Cab preservation package for modern Windows.

### v1.0 — Revive (Restoration)

**Goal:** Full Hell Cab rebuild in Godot 4.

- [ ] Asset extraction pipeline
- [ ] Lingo → GDScript translator
- [ ] Godot 4 project generator
- [ ] All platform targets

**Done when:** Hell Cab runs natively in Godot 4 on Windows, macOS, Linux, and Web.

### v1.x and beyond

- Plugin system and PluginForge
- ROM hack workspace
- Cartridge reader support
- Additional format support
- Additional revival engines
- Community features

---

## Current Project Context

**Primary restoration target:** Hell Cab (1993)

**File locations:**
- ISO extracted to: `C:\Users\thoma\Documents\ROM\Windows 3.x\Hell-Cab_Win-3x_EN_ISO\unzip\Hell Cab (1993)`
- Installed game: `C:\Users\thoma\Documents\DOSBOX\C\HELLCAB\HELLCAB.EXE`
- Game currently runs via winevdm/otvdm on modern Windows

**Game technical details:**
- Built in Macromedia Director (Windows 3.x version)
- Scenes in `.MMM` files in `HCDATA` folder
- Scene names: `JHELL`, `AMAIN`, `CATM`, `EESB`, `GROME`, `HWWI`, `IPREH`, `PA`, `QA`, `SFX`
- Sound assets are plain `.WAV` files (modern compatible as-is)
- Main executable is a Director Projector (`PROJECTR.SKL`)

**Next implementation step:** Set up the solution, implement AtelierRex.Core, implement the BinaryParser in AtelierRex.Infrastructure, and write corpus tests against the Hell Cab .MMM files to validate chunk walking.

---

*Atelier Rex — restoring what deserves to be remembered.*
