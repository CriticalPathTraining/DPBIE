Clear-Host

$authResult = Connect-AzureAD

$microsoftGraphAppId = "00000003-0000-0000-c000-000000000000"
$microsoftGraph = Get-AzureADServicePrincipal -All $true | Where-Object {$_.AppId -eq $microsoftGraphAppId}

$outputFile = "$PSScriptRoot\MicrosoftGraphPermissions.txt"

"--- Microsoft Graph Delegated Permissions (Scopes)---" | Out-File -FilePath $outputFile
$microsoftGraph.Oauth2Permissions | Sort-Object Type, Value | Format-Table Type, Value, Id | Out-File -FilePath $outputFile -Append

"--- Microsoft Graph Application Permissions (AppRoles) ---" | Out-File -FilePath $outputFile -Append
$microsoftGraph.AppRoles | Sort-Object Type, Value | Format-Table Value, Id, DisplayName | Out-File -FilePath $outputFile -Append

Notepad $outputFile