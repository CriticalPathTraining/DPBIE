$creds = Get-Credential -UserName "tedp@bizappsummit.onMicrosoft.com" -Message "Enter password"

$workspaceId = "672e0de4-1e94-45bc-a638-5d4e0d063aef"
$datasetId = "d60fcfb3-bb8d-4ab0-bfe8-ad58e5beb7aa"

$restRefreshUrl = "https://api.powerbi.com/v1.0/myorg/groups/" + $workspaceId + "/datatsets/" + $datasetId + "/refreshes/"

#Login-PowerBIServiceAccount -Credential $creds
#Connect-PowerBIServiceAccount -ServicePrincipal -Credential $creds
Login-PowerBIServiceAccount -ServicePrincipal -Credential $creds

Invoke-PowerBIRestMethod -Url $restRefreshUrl -Body "" -ContentType "application/json" -Method Post 
