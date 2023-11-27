$ExcludeDestination = @(
    "AKDEMIC.CORE",
    "AKDEMIC.ECONOMICMANAGEMENT.WJ1",
    "AKDEMIC.ENTITIES",
    "AKDEMIC.LAURASSIACONSOLE",
    "AKDEMIC.PAYSERVICE",
    "AKDEMIC.REPOSITORY",
    "AKDEMIC.SERVICE",
    "AKDEMIC.WEBAPI",
    "AKDEMIC.WEBSERVICE",
    "script",
    "shared"
)
$Exclude = @(
    "README.txt"
)
$Path = "..\shared\*"

Get-ChildItem -Directory -Path "..\**\" |
Where-Object { $_.Name -notin $ExcludeDestination } |
ForEach-Object {
    $Destination = $_.FullName
    
    Copy-Item -Destination $Destination -Exclude $Exclude -Force -Path $Path -Recurse -Verbose
}
