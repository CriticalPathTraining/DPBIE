
$authResult = Connect-AzureAD


$powerBiServiceAppId = "00000009-0000-0000-c000-000000000000"
$powerBiService = Get-AzureADServicePrincipal | Where-Object {$_.AppId -eq $powerBiServiceAppId}

$outputFile = "$PSScriptRoot\PowerBiServicePermissions.txt"

"--- Power BI Service API Delegated Permissions (Scopes)---" | Out-File -FilePath $outputFile
$powerBiService.Oauth2Permissions | Sort-Object Type, Value | Format-Table Type, Value, Id | Out-File -FilePath $outputFile -Append

"--- Application Permissions (AppRoles) ---" | Out-File -FilePath $outputFile -Append
$powerBiService.AppRoles | Sort-Object Type, Value | Format-Table Value, Id, DisplayName | Out-File -FilePath $outputFile -Append

Notepad $outputFile