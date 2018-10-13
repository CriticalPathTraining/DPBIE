$authResult = Connect-AzureAD

$powerBiServiceAppId = "00000009-0000-0000-c000-000000000000"
$powerBiService = Get-AzureADServicePrincipal | Where-Object {$_.AppId -eq $powerBiServiceAppId}

Get-AzureADOAuth2PermissionGrant | Where-Object {$_.ResourceId -eq $powerBiService.ObjectId} | Format-List