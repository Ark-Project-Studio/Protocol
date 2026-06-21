#!/usr/bin/env python3
"""
Sync C# file namespaces with their directory structure under Protocol/.
Also updates using directives where the old-to-new mapping is unambiguous.

Examples:
    Protocol/Codec/IO/Packet.cs        -> namespace Protocol.Codec.IO;
    Protocol/Logger/Logger.cs          -> namespace Protocol.Logger;
    Protocol/Protocol.cs               -> namespace Protocol;

Usage:
    python scripts/sync_namespace.py --dry-run          # preview changes
    python scripts/sync_namespace.py                    # apply namespace fixes
    python scripts/sync_namespace.py --add-missing      # also add ns to files without one
"""

import argparse
import re
import sys
from pathlib import Path

# --- Regex patterns ----------------------------------------------------------

FILE_SCOPED_NS_RE = re.compile(
    r'^(namespace\s+)([.\w]+)(\s*;)',
    re.MULTILINE,
)
BLOCK_NS_RE = re.compile(
    r'^(namespace\s+)([.\w]+)(\s*\{)',
    re.MULTILINE,
)
USING_RE = re.compile(
    r'^(using\s+)([.\w]+)(\s*;)',
    re.MULTILINE,
)

# --- Helpers -----------------------------------------------------------------


def get_expected_namespace(file_path: Path, root_dir: Path) -> str:
    """Compute the expected namespace from the file's directory structure."""
    rel = file_path.relative_to(root_dir)
    parts = rel.parent.parts  # directory components, excluding filename
    if not parts:
        return "Protocol"
    return "Protocol." + ".".join(parts)


def find_namespace(content: str) -> tuple[str | None, str]:
    """
    Find the first namespace declaration in the file.
    Returns (namespace_name, style) where style is 'file-scoped', 'block', or ''.
    """
    m = FILE_SCOPED_NS_RE.search(content)
    if m:
        return m.group(2), 'file-scoped'
    m = BLOCK_NS_RE.search(content)
    if m:
        return m.group(2), 'block'
    return None, ''


def replace_namespace(content: str, old_ns: str, new_ns: str, style: str) -> str:
    """Replace the first namespace declaration with the new one."""
    if style == 'file-scoped':
        pattern = re.compile(
            r'^(namespace\s+)' + re.escape(old_ns) + r'(\s*;)', re.MULTILINE
        )
        return pattern.sub(r'\g<1>' + new_ns + r'\2', content, count=1)
    else:
        pattern = re.compile(
            r'^(namespace\s+)' + re.escape(old_ns) + r'(\s*\{)', re.MULTILINE
        )
        return pattern.sub(r'\g<1>' + new_ns + r'\2', content, count=1)


def add_namespace(content: str, ns: str) -> str:
    """
    Insert a file-scoped namespace declaration before the first top-level type.
    Tries to place it after the last using directive for a clean look.
    """
    last_using = -1
    for m in re.finditer(r'^(\s*using\s+[\w.]+\s*;)', content, re.MULTILINE):
        last_using = m.end()

    if last_using != -1:
        before = content[:last_using]
        after = content[last_using:].lstrip('\n')
        return before + f"\n\nnamespace {ns};\n\n" + after
    return f"namespace {ns};\n\n" + content


def is_excluded(cs_file: Path) -> bool:
    """Skip build artifacts and hidden folders."""
    return any(
        part.startswith(".") or part in ("bin", "obj", "packages")
        for part in cs_file.parts
    )


# --- Main ---------------------------------------------------------------------


def main():
    parser = argparse.ArgumentParser(
        description="Sync C# namespaces with directory structure.",
    )
    parser.add_argument(
        "--add-missing",
        action="store_true",
        help="Add namespace declarations to files that don't have one.",
    )
    parser.add_argument(
        "--dry-run",
        action="store_true",
        help="Print what would be changed without modifying files.",
    )
    parser.add_argument(
        "--dir",
        default="Protocol",
        help="Root directory to scan (default: Protocol).",
    )
    args = parser.parse_args()

    script_dir = Path(__file__).parent.resolve()
    project_root = script_dir.parent.resolve()
    target_dir = (project_root / args.dir).resolve()

    if not target_dir.exists():
        print(f"Error: target directory does not exist: {target_dir}", file=sys.stderr)
        sys.exit(1)

    # ------------------------------------------------------------------
    # Phase 1: collect namespace mappings
    # ------------------------------------------------------------------
    files_to_fix = []          # (Path, content, old_ns, expected_ns, style)
    ns_mapping: dict[str, set[str]] = {}   # old_ns -> {new_ns, ...}

    for cs_file in sorted(target_dir.rglob("*.cs")):
        if is_excluded(cs_file):
            continue

        expected_ns = get_expected_namespace(cs_file, target_dir)
        content = cs_file.read_text(encoding='utf-8')
        current_ns, style = find_namespace(content)

        if current_ns is not None and current_ns != expected_ns:
            files_to_fix.append((cs_file, content, current_ns, expected_ns, style))
            ns_mapping.setdefault(current_ns, set()).add(expected_ns)
        elif current_ns is None and args.add_missing:
            files_to_fix.append((cs_file, content, None, expected_ns, 'missing'))

    # ------------------------------------------------------------------
    # Phase 2: determine safe using replacements (one-to-one mappings)
    # ------------------------------------------------------------------
    safe_using_replacements: dict[str, str] = {}
    ambiguous_mappings: dict[str, set[str]] = {}

    for old_ns, new_nss in ns_mapping.items():
        if len(new_nss) == 1:
            safe_using_replacements[old_ns] = new_nss.pop()
        else:
            ambiguous_mappings[old_ns] = new_nss

    # ------------------------------------------------------------------
    # Phase 3: fix namespace declarations
    # ------------------------------------------------------------------
    namespace_changes: list[str] = []
    for cs_file, content, old_ns, expected_ns, style in files_to_fix:
        if style == 'missing':
            new_content = add_namespace(content, expected_ns)
            desc = f"[missing] -> {expected_ns}"
        else:
            new_content = replace_namespace(content, old_ns, expected_ns, style)
            desc = f"{old_ns} -> {expected_ns}"

        rel = cs_file.relative_to(project_root)
        if args.dry_run:
            print(f"[DRY-RUN] {rel}: would fix namespace ({desc})")
        else:
            cs_file.write_text(new_content, encoding='utf-8')
            print(f"{rel}: fixed namespace ({desc})")
        namespace_changes.append(str(rel))

    # ------------------------------------------------------------------
    # Phase 4: fix using directives (only for unambiguous mappings)
    # ------------------------------------------------------------------
    using_changes: list[str] = []
    if safe_using_replacements:
        # Sort by length descending to avoid partial replacements
        sorted_replacements = sorted(
            safe_using_replacements.items(), key=lambda x: len(x[0]), reverse=True
        )

        for cs_file in sorted(target_dir.rglob("*.cs")):
            if is_excluded(cs_file):
                continue

            content = cs_file.read_text(encoding='utf-8')
            new_content = content
            changed = False

            for old_ns, new_ns in sorted_replacements:
                pattern = re.compile(
                    r'^(using\s+)' + re.escape(old_ns) + r'(\s*;)', re.MULTILINE
                )
                if pattern.search(new_content):
                    new_content = pattern.sub(
                        r'\g<1>' + new_ns + r'\2', new_content
                    )
                    changed = True

            if changed:
                rel = cs_file.relative_to(project_root)
                if args.dry_run:
                    print(f"[DRY-RUN] {rel}: would update using directives")
                else:
                    cs_file.write_text(new_content, encoding='utf-8')
                    print(f"{rel}: updated using directives")
                using_changes.append(str(rel))

    # ------------------------------------------------------------------
    # Report
    # ------------------------------------------------------------------
    if ambiguous_mappings:
        print("\n[!] Ambiguous namespace mappings (skipped for using replacement):")
        for old_ns, new_nss in sorted(ambiguous_mappings.items()):
            print(f"    {old_ns} -> {', '.join(sorted(new_nss))}")

    print(
        f"\nDone. {'Would have' if args.dry_run else ''} modified "
        f"{len(namespace_changes)} namespace(s) and {len(using_changes)} using-directive file(s)."
    )


if __name__ == "__main__":
    main()
