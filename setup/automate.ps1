param([String]$svc='votabl2')

$t = azure mobile table list $svc | where { $_.Contains("votes") } | measure

if ($t.Count -eq 1) {
	azure mobile table delete -q $svc votes
}
	
azure mobile table create -p 'insert=public,update=admin,delete=admin,read=public' $svc votes

#check if dynamic schema is on, if not, turn it on
$config = azure mobile config list $svc | where { $_.Contains("dynamicSchemaEnabled true") } | measure

if ($config.Count -eq 0) {
	azure mobile config set $svc dynamicSchemaEnabled true
} 

#insert data before adding the token validation script
node data.js $svc
azure mobile data truncate -q votabl2 votes

azure mobile script upload $svc table/votes.insert.js
azure mobile script upload $svc table/votes.read.js

#grab the existing application key
$details = azure mobile show $svc
$masterLine = $details | where { $_.Contains("masterKey") }
$origKeyLine = $details | where { $_.Contains("applicationKey") }

#extract the app key
$reOrig = [regex] 'applicationKey (.*)'
$orig = $reOrig.Matches($origKeyLine).Groups[1].Value

# regenerate and grab the new app key
$newKeyLine = azure mobile key regenerate $svc application | where { $_.Contains("application key") }

#this regex matches the application key
$reNew = [regex] 'application key is (.*)'
$key = $reNew.Matches($newKeyLine).Groups[1].Value

$path = '..\client\votabl2\Votabl2\Models\MainViewModel.cs'
$app = '..\client\votabl2\Votabl2\App.xaml.cs'

#swap the masterkey in the main file so that folks can't see it
(Get-Content $path) | foreach { $_ -replace $orig, $key } | Set-Content $path
(Get-Content $app) | foreach { $_ -replace $orig, $key } | Set-Content $app

#disable dynamic schema for the remainder of the demonstration.
azure mobile config set votabl2 dynamicSchemaEnabled false