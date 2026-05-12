# VS Code Extensions Guide — Atelier Rex

A reference guide for all installed VS Code extensions, their purpose, and key shortcuts.

---

## Theme Extensions

### Synthwave '84
Retro neon theme inspired by 80s/early 90s aesthetics.

- **Activate:** Press `Ctrl+K Ctrl+T` and select Synthwave '84
- **Enable Neon Glow:** `Ctrl+Shift+P` → type "Synthwave" → select "Enable Neon Dreams" → restart VS Code
- Note: Glow effect will show a VS Code "corrupt installation" warning — this is expected and harmless

### One Dark Pro
Clean dark theme — recommended for daytime use.

- **Activate:** Press `Ctrl+K Ctrl+T` and select One Dark Pro

**Theme shortcut to remember: `Ctrl+K Ctrl+T`**

---

## C# Dev Kit (by Microsoft)
Core C# language support — IntelliSense, debugging, project management.

| Shortcut | Action |
|----------|--------|
| `F12` | Go to definition |
| `Shift+F12` | Find all references |
| `Ctrl+.` | Quick fix / suggestions |
| `Ctrl+Space` | Trigger IntelliSense manually |

- **Solution Explorer** appears in the sidebar — shows all projects like Visual Studio

---

## GitLens (by GitKraken)
Supercharged Git integration.

| Shortcut | Action |
|----------|--------|
| `Ctrl+Shift+G` | Open Source Control panel |

- **Blame annotations:** Hover over any line to see who wrote it, when, and the commit message
- **Branch indicator:** Click the branch name in the bottom status bar to switch branches
- Files show colored indicators: **M** = modified, **U** = untracked, **A** = added

---

## Git Graph (by mhutchie)
Visual commit history diagram.

- **Open:** Click **Git Graph** in the bottom status bar
- Click any commit node to see what changed
- Right-click a commit for: checkout, revert, cherry-pick options

---

## Error Lens (by Alexander)
Shows errors and warnings inline on the line where they occur.

- Works automatically — no configuration needed
- Red = errors, Yellow = warnings, Blue = info
- No need to hover over squiggly lines — messages always visible

---

## Todo Tree (by Gruntfuggly)
Collects all TODO/FIXME comments across the entire codebase.

- **Open:** Click the tree icon in the Activity Bar (left sidebar)
- **Usage:** Write special comments in code:

```csharp
// TODO: implement bounds checking
// FIXME: this crashes on empty input
// HACK: temporary workaround
// BUG: known issue here
```

All tagged comments appear automatically in the Todo Tree panel.

---

## indent-rainbow (by oderwat)
Colors each indentation level a different subtle color.

- Works automatically — no configuration needed
- Makes nested code much easier to read at a glance

---

## Bookmarks (by Alessandro Fragnani)
Mark lines and jump between them across files.

| Shortcut | Action |
|----------|--------|
| `Ctrl+Alt+K` | Toggle bookmark on current line |
| `Ctrl+Alt+J` | Jump to previous bookmark |
| `Ctrl+Alt+L` | Jump to next bookmark |

- **Open panel:** Click the bookmark icon in the Activity Bar to see all bookmarks across all files

---

## Path Intellisense (by Christian Kohler)
Autocompletes file paths as you type.

- Works automatically when typing a file path string in code
- No configuration needed

---

## Better Comments (by Aaron Bond)
Colorizes comments based on their first character.

```csharp
// ! important warning     — red
// ? question              — blue
// TODO: task              — orange
// * highlighted note      — green
//   regular comment       — gray
```

Works alongside Todo Tree — they complement each other.

---

## Markdown Preview Enhanced (by Yiyi Wang)
Renders markdown files beautifully inside VS Code.

| Shortcut | Action |
|----------|--------|
| `Ctrl+Shift+V` | Open rendered preview alongside source |

- Right-click any `.md` tab and select **Open Preview**
- Use this to read SPEC.md with full formatting

---

## Draw.io Integration (by Henning Dieterichs)
Create architecture diagrams directly inside VS Code.

- Create any file with `.drawio` extension — VS Code opens it as a visual diagram editor
- Suggested use: create `ARCHITECTURE.drawio` at solution root to visualize layer dependencies

---

## .NET Core Test Explorer (by formulahendry)
Visual test runner for xUnit tests.

| Shortcut | Action |
|----------|--------|
| `Ctrl+; Ctrl+A` | Run all tests |
| `Ctrl+; Ctrl+R` | Run tests in current file |

- **Open:** Click the flask/beaker icon in the Activity Bar
- Discovers tests automatically
- Green checkmark = passing, Red X = failing
- Run individual tests, entire test projects, or the full suite

---

## NuGet Gallery (by pcislo)
Browse and install NuGet packages (C# libraries) without command line.

- **Open:** `Ctrl+Shift+P` → type "NuGet" → select "Open NuGet Gallery"
- Search for any package, select the target project, click install

---

## GitHub Pull Requests (by GitHub)
Manage pull requests without leaving VS Code.

- **Open:** Click the GitHub icon in the Activity Bar
- Sign in with your GitHub account
- Review, comment on, and merge PRs directly in VS Code

---

## Avalonia for VS Code
IntelliSense and live preview for Avalonia UI files.

- Activates automatically when opening `.axaml` files
- Provides XAML autocomplete and layout preview
- Essential when working on the AtelierRex.UI project

---

## SonarQube for IDE (formerly SonarLint)
Static code analysis — catches bugs, code smells, and security issues as you type.

| Shortcut | Action |
|----------|--------|
| `Ctrl+Shift+M` | Open Problems panel |

- Works silently in the background
- Issues appear in the Problems panel at the bottom
- Click any issue to jump to the offending line
- Right-click an issue to see the rule explanation and suggested fix
- Most useful once substantial code exists to analyze

---

## Quick Reference — Most Used Shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl+K Ctrl+T` | Change theme |
| `Ctrl+Shift+P` | Command Palette (access any VS Code command) |
| `Ctrl+Shift+M` | Open Problems panel |
| `Ctrl+Shift+G` | Open Source Control |
| `Ctrl+Shift+V` | Open Markdown preview |
| `F12` | Go to definition |
| `Shift+F12` | Find all references |
| `Ctrl+.` | Quick fix |
| `Ctrl+Alt+K` | Toggle bookmark |
| `Ctrl+Alt+J/L` | Previous/next bookmark |
| `Ctrl+; Ctrl+A` | Run all tests |
| `Ctrl+` ` | Open integrated terminal |

---

*Place this file at the root of the AtelierRex solution alongside SPEC.md*
