# voidcat-rename.ps1 — Microsoft.Msagl -> VoidCat.Agl across the CORE project.
# Only GraphLayout/MSAGL is renamed; viewers/samples reference the old names
# and will no longer build — we never build them.
$old = 'Microsoft.Msagl'
$new = 'VoidCat.Agl'
Get-ChildItem "GraphLayout/MSAGL" -Recurse -Include *.cs, *.csproj |
  ForEach-Object {
    $c = Get-Content $_.FullName -Raw
    if ($c.Contains($old)) {
      $c.Replace($old, $new) | Set-Content $_.FullName -NoNewline
    }
  }