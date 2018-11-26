Clear-Host

$userName = ""
$password = ""

$appDisplayName = "Power BI Day SPA"
$replyUrl = "https://localhost:44300"

$outputFile = "$PSScriptRoot\PowerBIDaySPA.txt"
$newline = "`r`n"
Write-Host "Writing info to $outputFile"


$securePassword = ConvertTo-SecureString –String $password –AsPlainText -Force
$credential = New-Object –TypeName System.Management.Automation.PSCredential `
                         –ArgumentList $userName, $securePassword

$authResult = Connect-AzureAD -Credential $credential

$tenantId = $authResult.TenantId.ToString()
$tenantDomain = $authResult.TenantDomain
$tenantDisplayName = (Get-AzureADTenantDetail).DisplayName

$userAccountId = $authResult.Account.Id
$user = Get-AzureADUser -ObjectId $userAccountId
$userDisplayName = $user.DisplayName

Write-Host "Registering new app $appDisplayName in $tenantDomain"

# create Azure AD Application
$aadApplication = New-AzureADApplication `
                        -DisplayName $appDisplayName `
                        -PublicClient $false `
                        -AvailableToOtherTenants $false `
                        -ReplyUrls @($replyUrl) `
                        -Homepage $replyUrl `
                        -Oauth2AllowImplicitFlow $true

# create applicaiton's service principal 
$appId = $aadApplication.AppId
$appObjectId = $aadApplication.ObjectId
$serviceServicePrincipal = New-AzureADServicePrincipal -AppId $appId

# assign current user as owner
Add-AzureADApplicationOwner -ObjectId $aadApplication.ObjectId -RefObjectId $user.ObjectId


# configure login permissions for Azure Graph API
$requiredResourcesAccess1 = New-Object System.Collections.Generic.List[Microsoft.Open.AzureAD.Model.RequiredResourceAccess]

# configure signin delegated permisssions for the Microsoft Graph API
$requiredAccess1 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.RequiredResourceAccess"
$requiredAccess1.ResourceAppId = "00000003-0000-0000-c000-000000000000"
$permissionSignIn = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" `
                               -ArgumentList "37f7f235-527c-4136-accd-4a02d197296e","Scope"

$requiredAccess1.ResourceAccess = $permissionSignIn

# configure permissions for Power BI Service API
$requiredResourcesAccess2 = New-Object System.Collections.Generic.List[Microsoft.Open.AzureAD.Model.RequiredResourceAccess]

# configure delegated permisssions for the Power BI Service API
$requiredAccess2 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.RequiredResourceAccess"
$requiredAccess2.ResourceAppId = "00000009-0000-0000-c000-000000000000"

# Dashboard.Read.All
$permission1 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" `
                          -ArgumentList "2448370f-f988-42cd-909c-6528efd67c1a","Scope"

# Content.Create
$permission2 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" `
                          -ArgumentList "f3076109-ca66-412a-be10-d4ee1be95d47","Scope"

# Dataset.Read.All
$permission3 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" `
                          -ArgumentList "7f33e027-4039-419b-938e-2f8ca153e68e","Scope"

# Workspace.Read.All
$permission4 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" `
                          -ArgumentList "b2f1b2fa-f35c-407c-979c-a858a808ba85","Scope"

# Group.Read.All
$permission5 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" `
                          -ArgumentList "47df08d3-85e6-4bd3-8c77-680fbe28162e","Scope"

# Report.ReadWrite.All
$permission6 = New-Object -TypeName "Microsoft.Open.AzureAD.Model.ResourceAccess" `
                          -ArgumentList "7504609f-c495-4c64-8542-686125a5a36f","Scope"

# add permissions to ResourceAccess list
$requiredAccess2.ResourceAccess = $permission1, $permission2, $permission3, $permission4, $permission5, $permission6
Set-AzureADApplication -ObjectId $appObjectId -RequiredResourceAccess @($requiredAccess1, $requiredAccess2)

Out-File -FilePath $outputFile -InputObject "--- Info for $appDisplayName ---"
Out-File -FilePath $outputFile -Append -InputObject "AppId: $appId"
Out-File -FilePath $outputFile -Append -InputObject "ReplyUrl: $replyUrl"
Out-File -FilePath $outputFile -Append -InputObject "Tenant: $tenantDomain"

Notepad $outputFile