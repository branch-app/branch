Azure Deployment Guide
======
Note: These are notes to me, so I don't forget any steps on deployment.


----------


1. Update CDN assets
	1. Download [Azure Powershell](http://go.microsoft.com/?linkid=9811175&clcid=0x409).
	2. Run `Get-AzurePublishSettingsFile ` to download Azure Subscriptions. (First Time Affair).
	3. Run `Select-AzureSubscription -SubscriptionName "{branch-subscription-name}"`.
	4. Run `.\UploadAssetsToCdn.ps1 -StorageAccount "{storage-account}" -StorageContainer "cdn"`.
	5. Wait...

2. Publish the project to azure. Grab a cider as this takes 30 minutes (fuck).