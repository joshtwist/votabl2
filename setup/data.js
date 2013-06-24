var svc = process.argv[2];

var req = require('request');

req.post({
	url : 'https://' + svc + '.azure-mobile.net/tables/votes',
	body: JSON.stringify({ eventShareId : '', votablId : 0 }),
	headers: { 
		'content-type' : 'application/json'
	 }
}, function (error, response, body) {
	if (error || response.statusCode != 201) {
		console.error(error || response.statusCode + ' ' + body);
	}
});