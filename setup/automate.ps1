$svc = "votabl2"

$t = azure mobile table list $svc | where { $_.Contains("votes") } | measure

if ($t.Count -eq 1) {
	azure mobile table delete -q $svc votes
}
	
azure mobile table create -p 'insert=public,update=admin,delete=admin,read=public' $svc votes
azure mobile script upload $svc table/votes.insert.js
azure mobile script upload $svc table/votes.read.js

#grab the existing application key
$origKeyLine = azure mobile show $svc | where { $_.Contains("applicationKey") }

#extract the app key
$reOrig = [regex] 'applicationKey (.*)'
$orig = $reOrig.Matches($origKeyLine).Groups[1].Value

# regenerate and grab the new app key
$newKeyLine = azure mobile key regenerate $svc application | where { $_.Contains("application key") }

#this regex matches the application key
$reNew = [regex] 'application key is (.*)'
$key = $reNew.Matches($newKeyLine).Groups[1].Value

$path = '..\client\votabl2\Votabl2\Models\MainViewModel.cs'

(Get-Content $path) | foreach { $_ -replace $orig, $key } | Set-Content $path
