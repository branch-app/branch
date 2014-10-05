<#
.SYNOPSIS
	Uploads files from an ASP.NET application project folder (Scripts\ and Content\) 
	into an Azure storage container.

.EXAMPLE
	.\UploadAssetsToCdn
#>

[CmdletBinding(SupportsShouldProcess = $true)]
param(
	# The storage account name
	[string]$StorageAccount = "branchapp",

	# The name of the storage container to copy files to.
	[string]$StorageContainer = "cdn"
)


function Get-ScriptDirectory
{
	$Invocation = (Get-Variable MyInvocation -Scope 1).Value
	Split-Path $Invocation.MyCommand.Path
}

# The script has been tested on Powershell 3.0
Set-StrictMode -Version 3

# Following modifies the Write-Verbose behavior to turn the messages on globally for this session
$VerbosePreference = "ContinueSilently"

Import-Module "C:\Program Files (x86)\Microsoft SDKs\Azure\PowerShell\ServiceManagement\Azure\Azure.psd1" -ErrorAction Stop

$ProjectPath = Get-ScriptDirectory

# Get a list of files from the project folder
$files = (ls -Path $ProjectPath\Content -File -Recurse) + (ls -Path $ProjectPath\Scripts -File -Recurse)

$context = New-AzureStorageContext `
	-StorageAccountName $StorageAccount `
	-StorageAccountKey (Get-AzureStorageKey $StorageAccount).Primary

if ($files -ne $null -and $files.Count -gt 0)
{
	# Create the storage container.
	$existingContainer = Get-AzureStorageContainer -Context $context | 
		Where-Object { $_.Name -like $StorageContainer }

	if (-not $existingContainer -and
		$PSCmdlet.ShouldProcess($StorageContainer, "Create Container"))
	{
		$newContainer = New-AzureStorageContainer `
							-Context $context `
							-Name $StorageContainer `
							-Permission Blob
		"Storage container '" + $newContainer.Name + "' created."
	}

	# Upload the files to storage container.
	$fileCount = $files.Count
	$time = [DateTime]::UtcNow
	if ($files.Count -gt 0)
	{
		foreach ($file in $files) 
		{
			if ([System.IO.Path]::GetExtension($file) -ne ".css" -and
				[System.IO.Path]::GetExtension($file) -ne ".png" -and
				[System.IO.Path]::GetExtension($file) -ne ".jpg" -and
				[System.IO.Path]::GetExtension($file) -ne ".jpeg" -and
				[System.IO.Path]::GetExtension($file) -ne ".js" -and
				[System.IO.Path]::GetExtension($file) -ne ".map") {
					continue;
				}

			$blobFileName = (Resolve-Path $file.FullName -Relative).Substring(2)
			$contentType = switch ([System.IO.Path]::GetExtension($file))
			{
				".css" {"text/css"}
				".map" {"text/css"}
				".png" {"image/png"}
				".jpg" {"image/jpeg"}
				".jpeg" {"image/jpeg"}
				".js" {"text/javascript"}
				default {"application/octet-stream"}
			}

			$hash = Get-FileHash $file.FullName -Algorithm MD5

			Set-AzureStorageBlobContent `
				-Container $StorageContainer `
				-Context $context `
				-File $file.FullName `
				-Blob $blobFileName `
				-Properties @{ContentType=$contentType; CacheControl="public, max-age=3600"} `
				-Force
		}
	}

	$duration = [DateTime]::UtcNow - $time

	"Uploaded " + $files.Count + " files to blob container '" + $StorageContainer + "'."
	"Total upload time: " + $duration.TotalMinutes + " minutes."
}
else
{
	Write-Warning "No files found."
}