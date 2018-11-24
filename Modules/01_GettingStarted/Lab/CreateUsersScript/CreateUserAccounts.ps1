Clear-Host 

# update the next three lines with values from your Office 365 tenant
$classroomDomainName = "YOUR_TENANT_NAME"
$globalAdminAccountName = "YOUR_USER_NAME"
$globalAdminPassword = "YOUR_USER_NAME_PASSWORD"

$classroomDomain = $classroomDomainName + ".onMicrosoft.com"

$globalAdminAccount = $globalAdminAccountName + "@" + $classroomDomain 
$globalAdminSecurePassword = ConvertTo-SecureString -String $globalAdminPassword -AsPlainText -Force

$e5LcenseSku = $classroomDomainName + ":ENTERPRISEPREMIUM"


function New-User($firstName, $lastName, $alternateEmail) {

 $displayName = $firstName + " " + $lastName

 $firstNameClean = $firstName -replace " ", ""
 $firstNameClean = $firstNameClean -replace "'", ""
 
 $lastNameClean = $lastName -replace " ", ""
 $lastNameClean = $lastNameClean -replace "'", ""

 $mailNickname = $firstNameClean + $lastNameClean.SubString(0,1)
 $userPrincipalName =  $mailNickname + "@" + $classroomDomain

 $password = "pass@word1"
 $passwordProfile = New-Object -TypeName Microsoft.Open.AzureAD.Model.PasswordProfile
 $passwordProfile.Password = $password
 $passwordProfile.EnforceChangePasswordPolicy = $false
 $passwordProfile.ForceChangePasswordNextLogin = $false

 $secureStringPassword = ConvertTo-SecureString -String "pass@word1" -AsPlainText -Force
 
 # Create new user account for student 
 $newUser = New-AzureADUser `
                -DisplayName $displayName `
                -GivenName $firstName `
                -Surname $lastName `
                -MailNickName $mailNickname `
                -PasswordProfile $passwordProfile `
                -PasswordPolicies "DisablePasswordExpiration, DisableStrongPassword" `
                -UserPrincipalName $userPrincipalName `
                -UsageLocation "US" `
                -AccountEnabled $True


 $licenseSkuPartNumber = 'ENTERPRISEPREMIUM'
 $LicenseSku = Get-AzureADSubscribedSku | Where-Object {$_.SkuPartNumber -eq $licenseSkuPartNumber } 
 
 #Create the AssignedLicense object with the License and DisabledPlans earlier created
 $License = New-Object -TypeName Microsoft.Open.AzureAD.Model.AssignedLicense
 $License.SkuId = $LicenseSku.SkuId
 
 #Create the AssignedLicenses Object 
 $AssignedLicenses = New-Object -TypeName Microsoft.Open.AzureAD.Model.AssignedLicenses
 $AssignedLicenses.AddLicenses = $License
 $AssignedLicenses.RemoveLicenses = @()

 #Assign the license to the user
 Set-AzureADUserLicense -ObjectId $newUser.ObjectId -AssignedLicenses $AssignedLicenses

 #print out new user info to console
 $newUser | select GivenName, Surname, DisplayName, UserPrincipalName, ObjectId, UsageLocation

}

$credential = New-Object -TypeName System.Management.Automation.PSCredential `
                         -ArgumentList $globalAdminAccount, $globalAdminSecurePassword

$connect = Connect-AzureAD  -Credential $credential 

New-User "Bing" "Crosy" "bing@gamil.com"
